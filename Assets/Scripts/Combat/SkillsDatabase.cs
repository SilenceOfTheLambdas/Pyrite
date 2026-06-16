using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// A databse containing instances of Scriptable Objects pertaining to the various skills available in the game.
    /// </summary>
    public class SkillsDatabase : MonoBehaviour
    {
        public static SkillsDatabase Instance;
        public List<Skill> skills;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
            else 
                Destroy(this);
        }

        /// <summary>
        /// Attempts to find and return a Skill with a given name.
        /// </summary>
        /// <param name="skillName">The name of the skill to look for, case-insensitive</param>
        /// <returns>Skill of one of found, null otherwise</returns>
        public Skill GetSkillByName(string skillName)
        {
            foreach (var skill in skills)
            {
                return string.Equals(skill.name, skillName, StringComparison.CurrentCultureIgnoreCase)
                    ? skill
                    : null;
            }
            return null;
        }
    }
}