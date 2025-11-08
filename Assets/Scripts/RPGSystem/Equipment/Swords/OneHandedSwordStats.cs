using RPGSystem.Item_Definitions;

namespace RPGSystem.Equipment.Swords
{
    public class OneHandedSwordStats :  WeaponStats
    {
        public WeaponTemplate weaponTemplate;
        
        private void Start()
        {
            itemTemplate = weaponTemplate;
            GenerateBaseWeaponStats(weaponTemplate);
        }
    }
}