using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public struct DamageInfo
    {
        public GameObject Source;
        public GameObject Target;
        
        public float PhysicalDamage;
        public RpgManager.ElementalDamage ElementalDamage;
        
        public bool CanCrit;
        public float CritChance;
        public float CritMultiplier;

        public bool WasCritical;
        public float FinalDamage;
    }
}