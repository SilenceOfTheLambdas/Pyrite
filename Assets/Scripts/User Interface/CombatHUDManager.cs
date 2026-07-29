using System.Collections.Generic;
using Combat;
using NUnit.Framework;
using UnityEngine;

namespace User_Interface
{
    public class CombatHUDManager : MonoBehaviour
    {
        [SerializeField] private List<SkillBar> skillBars;
        [SerializeField] private GameObject actionBar;

        private void Awake()
        {
            Assert.IsNotEmpty(skillBars, "There are no skill bars assigned to the combat HUD manager!");
            Assert.IsNotNull(actionBar, "Action bar panel needs to be assigned to the combat HUD manager!");
        }

        private void Start()
        {
            CombatInitiator.OnCombatStarted += EnableCombatHUD;
            ActionsManager.OnCombatActionAdded += AddNewSkillToHotbar;
            // TODO: On Combat Ended
        }

        private void EnableCombatHUD()
        {
            actionBar.SetActive(true);
        }

        private void AddNewSkillToHotbar(SkillAction skillAction)
        {
            foreach (var skillBar in skillBars)
            {
                if (skillBar != null && skillBar.TryAddSkill(skillAction)) return;
            }
        }
    }
}