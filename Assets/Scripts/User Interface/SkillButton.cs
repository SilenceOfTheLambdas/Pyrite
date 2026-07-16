using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace User_Interface
{
    public class SkillButton : MonoBehaviour
    {
        public bool HasSkillAssigned => _associatedAction != null;
        
        [SerializeField] private Image slotIcon;
        [SerializeField] private Image skillCooldownTimerIcon;
        [SerializeField] private TextMeshProUGUI skillCooldownTimerText;
        [SerializeField] private TextMeshProUGUI shortcutKeyText;
        
        private SkillAction _associatedAction;
        private Sprite _skillIcon;

        private void Awake()
        {
            Assert.IsNotNull(skillCooldownTimerText, "Skill Cooldown Timer Text child component has not been assigned!");
            Assert.IsNotNull(shortcutKeyText, "Shortcut Key Text child component has not been assigned!");
            Assert.IsNotNull(slotIcon, "Slot Icon child component has not been assigned!");
            Assert.IsNotNull(skillCooldownTimerIcon, "Skill Cooldown Timer Icon child component has not been assigned!");
        }

        public void SetShortcutKeyText(string shortcutText)
        {
            shortcutKeyText.SetText(shortcutText);
        }
        
        public void ActivateSkill()
        {
            if (!HasSkillAssigned) return;
            if (CombatManager.Instance == null) return;
            CombatManager.Instance.ActionPressed(_associatedAction);
        }

        private void Update()
        {
            if (!HasSkillAssigned) return;
            
            if (_associatedAction.IsOnCooldown)
            {
                skillCooldownTimerIcon.color = new Color(0.13f, 0.13f, 0.13f);
                skillCooldownTimerText.SetText($"{_associatedAction.CurrentCooldown:0.0}");
                skillCooldownTimerIcon.fillAmount = (_associatedAction.CurrentCooldown / _associatedAction.Skill.cooldown);
            }
            else
            {
                skillCooldownTimerText.SetText("");
                skillCooldownTimerIcon.color = Color.white;
                skillCooldownTimerIcon.fillAmount = 1;
            }
        }

        public void SetSkill(SkillAction skillAction)
        {
            if (skillAction == null)
            {
                Debug.LogError("SkillAction is null!");
                ClearSkill();
                return;
            }
            
            _associatedAction = skillAction;
            _skillIcon = skillAction.Skill.skillIcon;
            
            skillCooldownTimerIcon.sprite = _skillIcon;
            slotIcon.sprite = _skillIcon;
            
            slotIcon.color = Color.white;
            skillCooldownTimerIcon.color = Color.white;

            skillCooldownTimerText.SetText("");
            skillCooldownTimerIcon.fillAmount = 1;
        }

        public void ClearSkill()
        {
            _associatedAction = null;
            _skillIcon = null;
            
            slotIcon.sprite = null;
            slotIcon.color = new Color(0, 0, 0, 0);
            
            skillCooldownTimerIcon.color = new Color(0, 0, 0, 0);
            
            skillCooldownTimerText.SetText("");
            skillCooldownTimerIcon.fillAmount = 0;
        }
    }
}