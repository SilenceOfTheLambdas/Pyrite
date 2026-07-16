using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Skills/New Skill")]
    public class Skill : ScriptableObject
    {
        [Header("Skill Information")] public string skillName;

        [Multiline] [TextArea] public string skillDescription;

        [AssetPreview(64, 64)] public Sprite skillIcon;

        public int manaCost = 1;

        /// <summary>
        /// Cooldown of the skill in seconds.
        /// </summary>
        public float cooldown = 2;

        /// <summary>
        /// The range the actor needs to be within to use this skill.
        /// </summary>
        public float range = 3;

        [DataTable(true, false)] public List<SkillDamage> availableSkillDamage;
    }

    [Serializable]
    public struct SkillDamage
    {
        public DamageType damageType;
        public RpgManager.StatRange<int> amount;
    }

    public enum DamageType
    {
        PHYSICAL,
        FIRE,
        COLD,
        LIGHTNING,
        POISON
    }
}