using System;
using EditorAttributes;
using UnityEngine;

namespace RPGSystem.Backend
{
    [Serializable]
    public class BaseWeaponTemplate
    {
        [DataTable(true)] public RpgManager.StatRange<int> physicalDamage;
        [DataTable(true)] public RpgManager.ElementalDamage elementalDamage;

        [HideInInspector] public float criticalDamageMultiplier;

        [DataTable(true)] public RpgManager.StatRange<float> criticalDamageChance;
    }
}