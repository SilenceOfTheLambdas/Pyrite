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
        private SkillAction _associatedAction;
        private Sprite _skillIcon;
        [SerializeField] private Image slotIcon;

        private void Awake()
        {
            Assert.IsNotNull(slotIcon, "Slot Icon child component has not been assigned!");
        }

        public void ActivateSkill()
        {
            CombatManager.Instance.ActionPressed(_associatedAction);
        }
        
        public void SetSkill(SkillAction skillAction)
        {
            _associatedAction = skillAction;
            hasSkillAssigned = true;
            _skillIcon = skillAction.Skill.skillIcon;
            slotIcon.sprite = _skillIcon;
            slotIcon.color = Color.white;
        }

        public void ClearSkill()
        {
            _associatedAction = null;
            hasSkillAssigned = false;
            _skillIcon = null;
            slotIcon.sprite = null;
            slotIcon.color = new Color(0, 0, 0, 0);
        }
    }
}