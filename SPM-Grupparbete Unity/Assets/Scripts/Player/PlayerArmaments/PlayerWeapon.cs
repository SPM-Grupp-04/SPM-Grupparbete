//Primary authors:
//Simon Canbäck
//
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : PlayerBeamArmamentBase
{
    [Header("Laser properties")]
    [SerializeField] private float currentHeat = 0.0f;
    [SerializeField] private float overheatThreshold = 100.0f;
    [SerializeField] private float weaponHeatingRate = 0.5f;
    [SerializeField, Tooltip("Cooling rate while weapon is used normally.")]
    private float weaponCoolingRate = 1.0f;
    [SerializeField, Tooltip("Cooling rate while weapon is overheated.")]
    private float weaponOverheatCoolingRate = 1.0f;
    [SerializeField] private float weaponCooldownTimerStart = 2.0f;

    public float WeaponCurrentHeat => currentHeat;
    public float OverheatThreshold => overheatThreshold;
    public float WeaponHeatingRate => weaponHeatingRate;
    public float WeaponCoolingRate => weaponCoolingRate;
    public float WeaponOverheatCoolingRate => weaponOverheatCoolingRate;
    public float WeaponCooldownTimerStart => weaponCooldownTimerStart;
    public bool IsOverheated => currentHeat >= overheatThreshold;

    //change this if we add stuff like stuns, weapon misfires, etc.
    protected override bool CanShoot
    {
        get => currentHeat < overheatThreshold;
    }

    public override bool IsActivated => GetComponentInParent<PlayerController>().IsShooting;

    [Header("Disco settings")]
    [SerializeField] private float discoDelayTimer = 1.0f;
    private bool isDisco = false;
    float nextDiscoColourTime;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextDiscoColourTime && isDisco == true)
        {
            VFXLineRenderer.material.color = new Color(
                Random.Range(0, 256),
                Random.Range(0, 256),
                Random.Range(0, 256)
                );

            nextDiscoColourTime = Time.time + discoDelayTimer;
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        currentHeat += weaponHeatingRate;
    }

    public void CoolWeapon()
    {
        currentHeat -= weaponCoolingRate;
        currentHeat = currentHeat < 0.0f ? 0.0f : currentHeat;
    }
}
