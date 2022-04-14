using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Max_Drill : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private LayerMask igenoreMask;

    [SerializeField] private float _overHeatAmount = 0;
    [SerializeField] private float overHeatIncreaseAmount = 0.5f;
    [SerializeField] private float overHeatDecreaseAmount = 1f;
    [SerializeField] private float coolDownTimerStart = 2f;

    [SerializeField] private int drillLevel = 1;
    [SerializeField] private int drillDamageOres = 1;
    [SerializeField] private int drillDamageMonsters = 1;


    private float _timer = 0;

    private GameObject _laserPoint;
    private GameObject _drillPoint;

    private GameObject _beamGO;

    private bool _isUsed;
    private bool _canShoot = true;
    private bool _isShooting;

    private void Awake()
    {
        _laserPoint = transform.Find("LaserPoint").gameObject;
        _drillPoint = transform.Find("DrillPoint").gameObject;
        DrillDamage(drillLevel);
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

            if (hit.collider.gameObject.GetComponent<Max_MinableOre>().GetRequierdWeaponLevel() <= drillLevel)
            {
                hit.collider.gameObject.SendMessage("ReduceMaterialHP", drillDamageOres);
            }

            return;

        }
        else
        {

            CreateCylinderBetweenPoints(transform.position, _drillPoint.transform.position, 0.25f, beamPrefab);
            return;

        }

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
                _overHeatAmount += overHeatIncreaseAmount;

                return;

            }
            else
            {

                CreateCylinderBetweenPoints(transform.position, _laserPoint.transform.position, 0.25f, laserPrefab);
                _overHeatAmount += overHeatIncreaseAmount;
                return;

            }
        }
        else if (_overHeatAmount >= 100)
        {
            Destroy(_beamGO);
            if (_timer <= 0)
            {
                _canShoot = false;
                _timer = coolDownTimerStart;

            }

        }

    }




    public void DrillInUse(bool state)
    {
        _isUsed = state;

    }

    private void CoolDownDrill()
    {
        if (_overHeatAmount > 0)
        {
            _overHeatAmount -= overHeatDecreaseAmount;

        }
    }

    public float GetOverheatAmount()
    {
        return _overHeatAmount;
    }

    public int GetDrillDamageOres()
    {
        return drillDamageOres;
    }

    public int GetDrillDamageMonsters()
    {
        return drillDamageMonsters;
    }

    private void DrillDamage(int drillLevel)
    {
        switch (drillLevel)
        {
            case 1:
                drillDamageOres = 1;
                drillDamageMonsters = 1;
                return;
        }

    }
}
