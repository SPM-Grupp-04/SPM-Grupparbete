
using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PlayerDrill : MonoBehaviour
{
    private PlayerStatistics playerStatistics = PlayerStatistics.Instance;

    [SerializeField] private LayerMask igenoreMask;

    
    [SerializeField] private float overHeatAmount = 0;
    [SerializeField] private float overHeatIncreaseAmount = 0.5f;
    [SerializeField] private float overHeatDecreaseAmount = 1f;
    [SerializeField] private float coolDownTimerStart = 2f;
    private int drillDamageOres = 1;
    private int drillDamageMonsters = 1;
    
    
    //[SerializeField] Material lrMaterial;
    private LineRenderer lr;
    [SerializeField] private Material[] beamMaterials;
    
    //particlesytems
    [SerializeField] private ParticleSystem laserRing;
    [SerializeField] private ParticleSystem laserEmission;
    [SerializeField] private ParticleSystem drillRing;
    [SerializeField] private ParticleSystem drillEmission;
    [SerializeField] private ParticleSystem overheatEmission;

    
    [SerializeField] private VisualEffect laserHit;
    [SerializeField] private VisualEffect drillHit;
    
    [SerializeField] private float drillDistance = 3;
    [SerializeField] private float laserDistance = 10;
    
    private GameObject laserPoint;
    private GameObject drillPoint;

    private float timer = 0;
    private bool canShoot = true;
    
    private bool isUsed;
    private bool isShooting;
    private bool isDrilling;

    private bool coolDownLaser;

    private bool isDisco = false;
    Color c1 = Color.white;
    Color c2;
    int randomColour1;
    int randomColour2;
    int randomColour3;
    float nextColour;
    [SerializeField] private float delayTimer = 1;
    
    public bool CanShoot
    {
        get { return canShoot; }
    }
    

    private void Awake()
    {
        laserPoint = transform.Find("LaserPoint").gameObject;
        drillPoint = transform.Find("DrillPoint").gameObject;
        laserPoint.transform.localPosition = new Vector3(0,0.75f,laserDistance);
        lr = GetComponent<LineRenderer>();
        DrillDamage();
        WeaponLevel();
        laserHit.transform.parent = null;
        drillHit.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
        }
        
        if(Time.time >= nextColour && isDisco == true)
        {
            randomColour1 = Random.Range(0, 255);
            randomColour2 = Random.Range(0, 255);
            randomColour3 = Random.Range(0, 255);
            //lrMaterial.color = new Color(randomColour1, randomColour2, randomColour3);
            nextColour = Time.time + delayTimer;
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
        
        if (timer == 0 && (isShooting == false || canShoot == false))
        {
            CoolDownDrill();
            if (overHeatAmount <= 0)
            {
                canShoot = true;
            }
        }
    }
    
    private void DrillObject()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        if (drillRing.isPlaying == false && drillEmission.isPlaying == false)
        {
            drillRing.Play();
            drillEmission.Play();
        }
        if (Physics.Raycast(transform.position, fwd, out hit, 3, igenoreMask))
        {
            drillHit.enabled = true;
            drillHit.transform.position = hit.point;
            drillHit.Play();
            LaserBetweenPoints(transform.position, hit.point, 1);
            if (hit.collider.gameObject.CompareTag("Rocks"))
            {
                hit.collider.gameObject.SendMessage("ReduceMaterialHP", drillDamageOres);
            }
            return;
        }
        drillHit.enabled = false;
        LaserBetweenPoints(transform.position, drillPoint.transform.position, 1);
    }
    
    private void ShootObject()
    {
        RaycastHit shootHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (overHeatAmount <= 100 && canShoot && isShooting)
        {
            if (laserRing.isPlaying == false && laserEmission.isPlaying == false)
            {
                laserRing.Play();
                laserEmission.Play();
            }

            if (Physics.Raycast(transform.position, fwd, out shootHit, 10f, igenoreMask))
            {
                laserHit.enabled = true;
                laserHit.transform.position = shootHit.point;
                laserHit.Play();
                LaserBetweenPoints(transform.position, shootHit.point, 2);
                if (shootHit.collider.gameObject.CompareTag("Enemy"))
                {

                    var takeDamge = new DealDamageEventInfo(shootHit.collider.gameObject,1);
                    EventSystem.current.FireEvent(takeDamge);
                }
                overHeatAmount += overHeatIncreaseAmount;
                return;
            }

            laserHit.enabled = false;
            LaserBetweenPoints(transform.position, laserPoint.transform.position, 2); 
            overHeatAmount += overHeatIncreaseAmount;
        }
        else
        {
            if (timer <= 0 && canShoot)
            {
                lr.enabled = false;
                canShoot = false;
                timer = coolDownTimerStart;
                StopLaserParticles();
            }
            if(overheatEmission.isPlaying == false)
            overheatEmission.Play();
        }
    }
    
    void LaserBetweenPoints(Vector3 start, Vector3 end, int material)
    {
        if (material == 1)
        {
            lr.material = beamMaterials[0];
        }
        else if(material == 2)
        {
            lr.material = beamMaterials[1];
        }
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void DrillInUse(bool state)
    {
        isUsed = state;
        if(isUsed == false)
        {
            isDrilling = false;
            isShooting = false;
            
            StopLaserParticles();
            StopDrillParticles();
        }

    }
    
    public void Shoot(bool state)
    {
        isShooting = state;
        if (!isShooting)
        {
            StopLaserParticles();
            overheatEmission.Stop();
            overheatEmission.Clear();
        }
    }

    private void StopLaserParticles()
    {
        if (!isUsed)
        {
            lr.enabled = false;
        }
        laserEmission.Stop();
        laserEmission.Clear();
        laserRing.Stop();
        laserRing.Clear();
        laserHit.Stop();
        laserHit.enabled = false;
    }

    private void StopDrillParticles()
    {
        if (!isUsed)
        {
            lr.enabled = false;
        }
        drillEmission.Stop();
        drillEmission.Clear();
        drillRing.Stop();
        drillRing.Clear();
        drillHit.Stop();
        drillHit.enabled = false;
    }

    public void Drill(bool state)
    {
        isDrilling = state;
        if (!isDrilling)
        {
            StopDrillParticles();
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
    
    private void DrillDamage()
    {
        int drillLevel = playerStatistics.drillLevel;
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
            default:
                drillDamageOres = 3;
                drillDamageMonsters = 3;
                break;
        }
    }

    private void WeaponLevel()
    {
        int level = playerStatistics.weaponLevel;
        switch (level)
        {
            case 1:
                laserDistance = 10f;
                laserPoint.transform.localPosition = new Vector3(0,0.75f,laserDistance);
                break;
            case 2:
                laserDistance = 15f;
                laserPoint.transform.localPosition = new Vector3(0,0.75f,laserDistance);
                break;
            case 3:
                laserDistance = 20f;
                laserPoint.transform.localPosition = new Vector3(0,0.75f,laserDistance);
                break;
            default:
                laserDistance = 20f;
                laserPoint.transform.localPosition = new Vector3(0,0.75f,laserDistance);
                break;
        }
    }

    public void SetWeaponLevel()
    {
        WeaponLevel();
    }

    public void IncreaseOverheatAmount(float increaseAmount)
    {
        overHeatAmount += increaseAmount;
    }
}
