using System;
using Combat;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace User_Interface
{
    public class SkillButton : MonoBehaviour
    {
        public bool hasSkillAssigned = false;
        public CombatAction AssociatedAction;
        private Sprite _skillIcon;
        [SerializeField] private Image slotIcon;

        private void Awake()
        {
            Assert.IsNotNull(slotIcon, "Slot Icon child component has not been assgigned!");
        }

        public void SetSkill(CombatAction combatAction)
        {
            AssociatedAction = combatAction;
            hasSkillAssigned = true;
            _skillIcon = combatAction.Skill.skillIcon;
            slotIcon.sprite = _skillIcon;
            slotIcon.color = Color.white;
        }

        public void ClearSkill()
        {
            AssociatedAction = null;
            hasSkillAssigned = false;
            _skillIcon = null;
            slotIcon.sprite = null;
            slotIcon.color = new Color(0, 0, 0, 0);
        }
    }
}