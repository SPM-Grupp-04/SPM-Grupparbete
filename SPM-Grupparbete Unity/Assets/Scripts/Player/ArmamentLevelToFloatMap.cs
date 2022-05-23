//nicked from https://gamedev.stackexchange.com/questions/74393/how-to-edit-key-value-pairs-like-a-dictionary-in-unitys-inspector/74408#74408
using System;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/ArmamentLevelToFloatMap")]
    public class ArmamentLevelToFloatMap : ScriptableObject
    {
        [Serializable]
        public class ArmamentLevelToFloatEntry
        {
            public PlayerBeamArmamentBase.ArmamentLevel armLevel;
            public float val;
        }

        public ArmamentLevelToFloatEntry[] ledger;
    }
}
