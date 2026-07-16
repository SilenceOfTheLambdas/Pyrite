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
        [SerializeField] private GameObject turnPanel;

        private void Awake()
        {
            Assert.IsNotEmpty(skillBars, "There are no skill bars assigned to the combat HUD manager!");
            Assert.IsNotNull(actionBar, "Action bar panel needs to be assigned to the combat HUD manager!");
            Assert.IsNotNull(turnPanel, "Turns panel needs to be assigned to the combat hud manager!");
        }

        private void Start()
        {
            CombatInitiator.OnCombatStarted += EnableCombatHUD;
            ActionsManager.OnCombatActionAdded += AddNewSkillToHotbar;
            // TODO: On Combat Ended
        }

        public void EnableCombatHUD()
        {
            actionBar.SetActive(true);
        }

        public void DisableCombatHUD()
        {
            actionBar.SetActive(false);
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