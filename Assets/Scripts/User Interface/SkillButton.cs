using Combat;
using Pyrite.Combat;
using UnityEngine;

namespace User_Interface
{
    public class SkillButton : MonoBehaviour
    {
        public bool hasSkillAssigned = false;
        public CombatAction associatedAction;
        private Sprite _skillIcon;

        public void SetSkillIcon(Sprite icon)
        {
            _skillIcon = icon;
        }
    }
}