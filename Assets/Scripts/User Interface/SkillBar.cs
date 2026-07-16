using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class SkillBar : MonoBehaviour
    {
        [SerializeField] private List<SkillButton> skillButtons;
        [SerializeField] private List<InputActionReference> slotActions;

        private void Awake()
        {
            AssignShortcutLabels();
        }

        private void OnEnable()
        {
            foreach (var slotAction in slotActions)
            {
                if (slotAction?.action == null) continue;
                slotAction.action.performed += OnSlotActionPerformed;
                slotAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (var slotAction in slotActions)
            {
                if (slotAction?.action == null) continue;
                slotAction.action.performed -= OnSlotActionPerformed;
                slotAction.action.Disable();
            }
        }

        public bool TryAddSkill(SkillAction skillAction)
        {
            var skillButton = GetNextEmptySkillButton();

            if (skillButton == null) return false;
            
            skillButton.SetSkill(skillAction);
            return true;
        }

        private void AssignShortcutLabels()
        {
            for (var i = 0; i < skillButtons.Count; i++)
            {
                if (skillButtons[i] == null) continue;
                
                if (i >= slotActions.Count || slotActions[i] == null || slotActions[i].action == null)
                {
                    skillButtons[i].SetShortcutKeyText("");
                    continue;
                }
                
                skillButtons[i].SetShortcutKeyText(slotActions[i].action.GetBindingDisplayString());
            }
        }

        private void OnSlotActionPerformed(InputAction.CallbackContext context)
        {
            var actionIndex = GetActionIndex(context.action);
            if (actionIndex < 0 || actionIndex >= skillButtons.Count) return;
            
            skillButtons[actionIndex].ActivateSkill();
        }

        private int GetActionIndex(InputAction action)
        {
            for (var i = 0; i < slotActions.Count; i++)
            {
                if (slotActions[i]?.action == action) return i;
            }

            return -1;
        }

        private SkillButton GetNextEmptySkillButton()
        {
            foreach (var skillButton in skillButtons)
            {
                if (skillButton != null && !skillButton.HasSkillAssigned)
                    return skillButton;
            }

            return null;
        }
    }
}
