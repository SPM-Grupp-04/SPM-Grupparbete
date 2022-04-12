using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Max_Drill : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private LayerMask igenoreMask;
    private GameObject _laserPoint;

    private GameObject _beamGO;
    private float _overHeatAmount = 0;
    private float _timer = 0;
    private bool _isUsed;
    private bool _canShoot = true;



    private void Awake()
    {
        _laserPoint = transform.Find("LaserPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_beamGO != null && _isUsed == false)
        {
            Destroy(_beamGO);
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _timer = 0;
            }
        }
        if (_timer == 0 && _beamGO == null)
        {
            CoolDownDrill();
            if (_overHeatAmount <= 0)
            {
                _canShoot = true;
            }
        }

    }

    public void DrillObject()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, 3) && hit.collider.gameObject.CompareTag("Rocks"))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);

            CreateCylinderBetweenPoints(transform.position, hit.point, 0.25f, beamPrefab);

            hit.collider.gameObject.SendMessage("ReduceMaterialHP", 1);

            return;

        }

        Destroy(_beamGO);

    }

    void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width, GameObject beamMaterial)
    {
        var offset = end - start;
        var scale = new Vector3(width, offset.magnitude / 2.0f, width);
        var position = start + (offset / 2.0f);
        Destroy(_beamGO);
        _beamGO = Instantiate(beamMaterial, position, Quaternion.identity);
        _beamGO.transform.up = offset;
        _beamGO.transform.localScale = scale;

    }

    public void Shoot()
    {
        RaycastHit shootHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);


        if (_overHeatAmount < 100 && _canShoot)
        {
            if (Physics.Raycast(transform.position, fwd, out shootHit, 10f, igenoreMask))
            {
                Debug.DrawLine(transform.position, shootHit.point, Color.green);

                CreateCylinderBetweenPoints(transform.position, shootHit.point, 0.25f, laserPrefab);
                if (shootHit.collider.gameObject.CompareTag("Enemy"))
                {
                    shootHit.collider.gameObject.SendMessage("TakeDamage");

                }
                _overHeatAmount += 0.5f;

                return;

            }
            else
            {

                CreateCylinderBetweenPoints(transform.position, _laserPoint.transform.position, 0.25f, laserPrefab);
                _overHeatAmount += 0.5f;
                return;

            }
        }
        else if(_overHeatAmount >= 100)
        {
            Destroy(_beamGO);
            if (_timer <= 0)
            {
                _canShoot = false;
                _timer = 2;
            }
            
        }
    }

    public void DrillInUse(bool state)
    {
        _isUsed = state;

    }

    private void CoolDownDrill()
    {
        if(_overHeatAmount > 0)
        {
            _overHeatAmount -= 0.5f;

        }
    }
}
