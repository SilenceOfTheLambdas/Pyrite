using RPGSystem.Item_Definitions;

namespace RPGSystem.Equipment.Swords
{
    public class OneHandedSwordStats : WeaponStats
    {
        private void Start()
        {
            GenerateBaseWeaponStats(itemTemplate as WeaponTemplate);
        }
    }
}