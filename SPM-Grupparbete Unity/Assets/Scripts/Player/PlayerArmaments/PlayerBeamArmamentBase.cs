//Primary authors:
//Simon Canbäck
//
using EgilEventSystem;
using EgilScripts.DieEvents;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBeamArmamentBase : MonoBehaviour
{
    protected PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    protected ArmamentLevel ArmLevel => playerStatistics.armamentLevel;

    [Header("Audio settings")]
    [SerializeField] protected AudioClip sound;
    private AudioSource audioSource;
    public AudioClip Sound => sound;

    [Header("VFX settings")]
    [SerializeField] protected Material[] VFXMaterials;
    [SerializeField] protected ParticleSystem beamRing;
    [SerializeField] protected ParticleSystem beamEmission;
    protected GameObject VFXMaxRange;
    public LineRenderer VFXLineRenderer
    {
        get; protected set;
    }
    protected int MaterialIndex
    {
        get; set;
    }

    [Header("Armament properties")]
    //Unity doesn't allow key-value pairs in the inspector by default, so it has to be hacked in like this:
    [SerializeField] private Player.ArmamentLevelToFloatMap armamentLevelToDamageTable;
    [SerializeField] private Player.ArmamentLevelToFloatMap armamentLevelToRangeTable;
    protected Dictionary<ArmamentLevel, float> armamentLevelToDamageDictionary = new Dictionary<ArmamentLevel, float>();
    protected Dictionary<ArmamentLevel, float> armamentLevelToRangeDictionary = new Dictionary<ArmamentLevel, float>();
    [SerializeField, Tooltip("Layers the beam is allowed to collide with visually.")] protected LayerMask allowedCollisionWhenRenderingMask;
    [SerializeField, Tooltip("Layers the beam is allowed to interact with programmatically.")] protected LayerMask interactMask;

    protected abstract bool CanShoot
    {
        get;
    }

    public abstract bool IsActivated
    {
        get;
    }

    public virtual float Damage => armamentLevelToDamageDictionary[ArmLevel];
    public virtual float Range => armamentLevelToRangeDictionary[ArmLevel];

    private void Awake()
    {
        VFXLineRenderer = GetComponentInParent<LineRenderer>();

        foreach (var altfe in armamentLevelToDamageTable.ledger)
            armamentLevelToDamageDictionary[altfe.armLevel] = altfe.val;

        foreach (var altfe in armamentLevelToRangeTable.ledger)
            armamentLevelToRangeDictionary[altfe.armLevel] = altfe.val;

        VFXMaxRange = new GameObject("MaxRange");
        VFXMaxRange.transform.parent = transform;
        VFXMaxRange.transform.localPosition = new Vector3(0.0f, 0.0f, Range);
        MaterialIndex = 0;

        audioSource = GetComponentInParent<AudioSource>();
    }

    protected virtual void LaserBetweenPoints(Vector3 start, Vector3 end, int material)
    {
        VFXLineRenderer.material = VFXMaterials[material];

        VFXLineRenderer.enabled = true;
        VFXLineRenderer.SetPosition(0, start);
        VFXLineRenderer.SetPosition(1, end);
    }

    public virtual void Shoot()
    {
        RaycastHit shootHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (CanShoot)
        {
            if (!beamRing.isPlaying && !beamEmission.isPlaying)
            {
                beamRing.Play();
                beamEmission.Play();
            }

            Physics.Raycast(transform.position, fwd, out shootHit, Range, allowedCollisionWhenRenderingMask);

            Vector3 point2 = shootHit.collider == null ? VFXMaxRange.transform.position : shootHit.point;

            LaserBetweenPoints(transform.position, point2, MaterialIndex);

            if (shootHit.collider != null && Utility.LayerMaskExtensions.IsInLayerMask(shootHit.collider.gameObject, interactMask))
            {
                var takeDamage = new DamageDealt(shootHit.collider.gameObject, armamentLevelToDamageDictionary[ArmLevel]);
                EventSystem.current.FireEvent(takeDamage);
            }

            return;
        }
    }

    public virtual void ClearVFX()
    {
        beamEmission.Stop();
        beamEmission.Clear();
        beamRing.Stop();
        beamRing.Clear();
    }

    public void PlaySound()
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    public enum ArmamentLevel
    {
        LEVEL_0 = 0,
        LEVEL_1,
        LEVEL_2,
        LEVEL_3
    }
}
