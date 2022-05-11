
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
    
    
    //[SerializeField] Material lrMaterial;
    private LineRenderer lr;
    [SerializeField] private Material[] beamMaterials;
    
    //particlesytems
    [SerializeField] private ParticleSystem laserParticles;
    [SerializeField] private ParticleSystem drillParticles;


    [SerializeField] private float drillDistance = 3;
    [SerializeField] private float laserDistance = 10;

    
    
    
    
    private float timer = 0;
    private GameObject laserPoint;
    private GameObject drillPoint;
    private GameObject beamGO; 

    private bool isUsed;
    private bool canShoot = true;
    private bool isShooting;
    private bool isDrilling;

    private bool isDisco = false;
    Color c1 = Color.white;
    Color c2;
    int randomColour1;
    int randomColour2;
    int randomColour3;
    float nextColour;
    [SerializeField] private float delayTimer = 1;
    

    private void Awake()
    {
       
        laserPoint = transform.Find("LaserPoint").gameObject;
        drillPoint = transform.Find("DrillPoint").gameObject;
        drillPoint.transform.localPosition = new Vector3(0,0.75f,drillDistance);
        laserPoint.transform.localPosition = new Vector3(0,0.75f,laserDistance);
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
        
        if (timer == 0 && isUsed == false)
        {
            CoolDownDrill();
            if (overHeatAmount <= 0)
            {
                canShoot = true;
            }
        }
    }

    public void Shoot(bool state)
    {
        isShooting = state;
    }
    public void Drill(bool state)
    {
        isDrilling = state;
    }

    private void DrillObject()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 3) && hit.collider.gameObject.CompareTag("Rocks"))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            LaserBetweenPoints(transform.position, hit.point, 1);
           /*
            if (drillParticles.isPlaying == false)
            {
                drillParticles.Play();
            }
            
            */
            hit.collider.gameObject.SendMessage("ReduceMaterialHP", drillDamageOres);
            return;
        }
        else
        {
            LaserBetweenPoints(transform.position, drillPoint.transform.position, 1);
            return;
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
                LaserBetweenPoints(transform.position, shootHit.point, 2);
                if (laserParticles.isPlaying == false)
                {
                    laserParticles.Play();
                }
                if (shootHit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy getting hit");
                    //shootHit.collider.gameObject.SendMessage("TakeDamage");
                    var takeDamge = new DealDamageEventInfo(shootHit.collider.gameObject,1);
                    EventSystem.current.FireEvent(takeDamge);
                }
                overHeatAmount += overHeatIncreaseAmount;
                return;
            }
            else
            {
                LaserBetweenPoints(transform.position, laserPoint.transform.position, 2);
                overHeatAmount += overHeatIncreaseAmount;
                return;
            }
        }
        else if (overHeatAmount >= 100)
        {
            Destroy(beamGO);
            if (timer <= 0)
            {
                lr.enabled = false;
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
            drillParticles.Stop();
            laserParticles.Stop();
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
            default:
                drillDamageOres = 3;
                drillDamageMonsters = 3;
                break;
                

        }

    }
}
