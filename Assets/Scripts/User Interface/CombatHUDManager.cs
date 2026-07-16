using System.Collections.Generic;
using Combat;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace User_Interface
{
    public class CombatHUDManager : MonoBehaviour
    {
        [SerializeField] private List<SkillButton> skillButtons;
        private HorizontalLayoutGroup _skillsGridRootObject;

        [SerializeField] private GameObject actionBar;
        [SerializeField] private GameObject turnPanel;

        private void Awake()
        {
            Assert.IsNotEmpty(skillButtons, "There are no skill buttons assigned to the combat HUD manager!");
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
            Debug.Log($"CombatHUDManager received skill action: {skillAction?.Skill?.skillName}");

            var skillButton = GetNextEmptySkillButton();

            if (skillButton == null)
            {
                Debug.LogWarning("Could not add skill to hotbar because there are no empty skill buttons.");
                return;
            }

            skillButton.SetSkill(skillAction);
        }

        /// <summary>
        /// Attempts to return the next available empty skill button.
        /// </summary>
        /// <returns>A SkillButton if an unassigned one if found, null otherwise</returns>
        private SkillButton GetNextEmptySkillButton()
        {
            foreach (var skillButton in skillButtons)
                if (!skillButton.HasSkillAssigned)
                    return skillButton;
            return null;
        }
    }
}