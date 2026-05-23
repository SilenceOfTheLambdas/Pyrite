using UnityEngine;

namespace User_Interface
{
    public class SkillButton : MonoBehaviour
    {
        public bool hasSkillAssigned = false;
        private Sprite _skillIcon;

        public void SetSkillIcon(Sprite icon) => _skillIcon = icon;
    }
}