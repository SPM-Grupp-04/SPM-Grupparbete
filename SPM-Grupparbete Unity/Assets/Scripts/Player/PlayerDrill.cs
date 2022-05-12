
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDrill : MonoBehaviour
{
    private PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private LayerMask igenoreMask;

    [SerializeField] private float overHeatAmount = 0;
    [SerializeField] private float overHeatIncreaseAmount = 0.5f;
    [SerializeField] private float overHeatDecreaseAmount = 1f;
    [SerializeField] private float coolDownTimerStart = 2f;

     private int drillLevel;
    [SerializeField] private int drillDamageOres = 1;
    [SerializeField] private int drillDamageMonsters = 1;

    private LineRenderer lr;

    private float timer = 0;
    private GameObject laserPoint;
    private GameObject drillPoint;
    private GameObject beamGO; 

    private bool isUsed;
    private bool canShoot = true;
    private bool isShooting;
    private bool isDrilling;

    private void Awake()
    {
        laserPoint = transform.Find("LaserPoint").gameObject;
        drillPoint = transform.Find("DrillPoint").gameObject;
        drillLevel = playerStatistics.drillLevel;
        lr = GetComponent<LineRenderer>();
        DrillDamage(drillLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed == false)
        {
            lr.enabled = false;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
        }
        if (timer == 0 && isUsed == false)
        {
            CoolDownDrill();
            if (overHeatAmount <= 0)
            {
                canShoot = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            ShootObject();
        } else if (isDrilling)
        {
            DrillObject();
        }
    }

    public void Shoot(bool state)
    {
        isShooting = state;
    }
    public void Drill(bool state)
    {
        isDrilling = true;
    }

    private void DrillObject()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 3) && hit.collider.gameObject.CompareTag("Rocks"))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            LaserBetweenPoints(transform.position, hit.point);
            hit.collider.gameObject.SendMessage("ReduceMaterialHP", drillDamageOres);
            return;
        }
        else
        {
            LaserBetweenPoints(transform.position, drillPoint.transform.position);
            return;
        }

    }


    void LaserBetweenPoints(Vector3 start, Vector3 end)
    {
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        //var offset = end - start;
        //var scale = new Vector3(width, offset.magnitude / 2.0f, width);
        //var position = start + (offset / 4.0f) ;
        //Destroy(beamGO);
        //beamGO = Instantiate(beamMaterial, position, transform.rotation);
        //beamGO.transform.up = offset;
        //beamGO.transform.localScale = scale;

    }

    private void ShootObject()
    {
        RaycastHit shootHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (overHeatAmount < 100 && canShoot)
        {
            if (Physics.Raycast(transform.position, fwd, out shootHit, 10f, igenoreMask))
            {
                Debug.DrawLine(transform.position, shootHit.point, Color.green);
                LaserBetweenPoints(transform.position, shootHit.point);
                if (shootHit.collider.gameObject.CompareTag("Enemy"))
                {
                    //shootHit.collider.gameObject.SendMessage("TakeDamage");
                    var takeDamge = new DamageDealt(shootHit.collider.gameObject,1);
                    EventSystem.current.FireEvent(takeDamge);
                }
                overHeatAmount += overHeatIncreaseAmount;
                return;
            }
            else
            {
                LaserBetweenPoints(transform.position, laserPoint.transform.position);
                overHeatAmount += overHeatIncreaseAmount;
                return;
            }
        }
        else if (overHeatAmount >= 100)
        {
            Destroy(beamGO);
            if (timer <= 0)
            {
                canShoot = false;
                timer = coolDownTimerStart;
            }
        }
    }

    public void DrillInUse(bool state)
    {
        isUsed = state;
        if(isUsed == false)
        {
            isDrilling = false;
            isShooting = false;
        }

    }

    private void CoolDownDrill()
    {
        if (overHeatAmount > 0)
        {
            overHeatAmount -= overHeatDecreaseAmount;
        }
    }

    public float GetOverheatAmount()
    {
        return overHeatAmount;
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
            case 0:
                drillDamageOres = 1;
                drillDamageMonsters = 1;
                break;
            case 1:
                drillDamageOres = 1;
                drillDamageMonsters = 1;
                break;
            case 2:
                drillDamageOres = 2;
                drillDamageMonsters = 2;
                break;
            case 3:
                drillDamageOres = 3;
                drillDamageMonsters = 3;
                break;

        }

    }
}
