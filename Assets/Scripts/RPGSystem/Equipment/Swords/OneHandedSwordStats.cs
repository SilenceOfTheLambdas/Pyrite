using RPGSystem.Item_Definitions;
using UnityEngine;

namespace RPGSystem.Equipment.Swords
{
    public class OneHandedSwordStats :  WeaponStats
    {
        [SerializeField] private WeaponTemplate weaponTemplate;
        
        private void Start()
        {
            GenerateBaseWeaponStats(weaponTemplate);
        }
    }
}