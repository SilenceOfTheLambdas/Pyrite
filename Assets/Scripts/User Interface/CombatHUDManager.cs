using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace User_Interface
{
    public class CombatHUDManager : MonoBehaviour
    {
        [SerializeField] private List<SkillButton> skillButtons;
        private readonly GameObject _skillsGridRootObject;

        private void Awake()
        {
            gameObject.GetComponentInChildren<HorizontalLayoutGroup>();
            Assert.IsNotNull(_skillsGridRootObject, "Could not find GridLayoutGrid child game object.");
            Assert.IsNotEmpty(skillButtons, "There are no skill buttons assigned to the combat HUD manager!");
        }
    }
}