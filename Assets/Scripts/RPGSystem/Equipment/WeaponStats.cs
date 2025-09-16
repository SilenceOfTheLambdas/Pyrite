using System;
using System.Collections.Generic;
using RPGSystem.Backend;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// The generated stats of a weapon equipment type.
    /// </summary>
    [Serializable]
    public abstract class WeaponStats : BaseItemInfo
    {
        /// <summary>
        /// The generated physical damage of this weapon.
        /// </summary>
        public int physicalDamage;
        
        /// <summary>
        /// The generated attack speed.
        /// </summary>
        public int attackSpeed;
        
        /// <summary>
        /// The generated attack range.
        /// </summary>
        public float attackRange;
        
        /// <summary>
        /// A generated list of affixes that apply to this weapon, this may be empty.
        /// </summary>
        public List<ItemTemplate.Affix> generatedAffixes;
    }
}