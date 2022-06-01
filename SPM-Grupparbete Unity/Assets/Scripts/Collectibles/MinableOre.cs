using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MinableOre : DestroyableObjectBase
{
    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    
    [SerializeField] int oreMaterialHP = 10;
    [SerializeField] int oreRequierdWeaponLevel = 1;
    [SerializeField] GameObject ore;
    [SerializeField] GameObject uiHP;
    [SerializeField] [Range(1, 3)] private int collecitbleCrystals = 1;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ParticleSystem wallParticles;
    
    private Vector3 startPos;
    private float timer;
    private Vector3 randomPos;
    
    private float time = 0.2f;
    private float distance = 0.05f;
    private float delayBetweenShakes = 0f;


    UI_ObjectHP uiObjectHp;

    private void Start()
    {
        requiredWeaponLevel = oreRequierdWeaponLevel;
        materialHP = oreMaterialHP;
        uiObjectHp = uiHP.GetComponent<UI_ObjectHP>();
        startPos = transform.position;

    }

    public override void ReduceMaterialHP(int amount)
    {
        if (playerStatistics.drillLevel >= requiredWeaponLevel)
        {
            materialHP -= amount;
            uiObjectHp.ObjectTakeDamage(amount);
            if (materialHP <= 0)
            {
                DestoryObject();
            }
        }
        else
        {
            Begin();
        }
    }

    public override int GetRequiredWeaponLevel()
    {
        return requiredWeaponLevel;
    }


    private void DestoryObject()
    {

        //audioManager.PlayCrystalSound();
        
        for (int i = 0; i < collecitbleCrystals; i++)
        {
            Instantiate(ore, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    public int GetOreMaterialHP()
    {
        return oreMaterialHP;
    }
    
    private void OnValidate()
    {
        if (delayBetweenShakes > time)
            delayBetweenShakes = time;
    }
 
    public void Begin()
    {
        StopAllCoroutines();
        wallParticles.Play();
        StartCoroutine(Shake());
    }
 
    private IEnumerator Shake()
    {
        timer = 0f;
        
        while (timer < time)
        {
            timer += Time.deltaTime;
 
            randomPos = startPos + (Random.insideUnitSphere * distance);
 
            transform.position = randomPos;
            
 
            if (delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }
        //wallParticles.Stop();
        transform.position = startPos;
    }

}
