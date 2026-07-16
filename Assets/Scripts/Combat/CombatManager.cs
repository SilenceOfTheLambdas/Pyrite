using UnityEngine;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;

        public CombatState CurrentCombatState { get; private set; }

        public GameObject currentTarget;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }else
                Instance = this;
        }

        private void Start()
        {
            CombatInitiator.OnCombatStarted += BeginCombat;
        }

        private void BeginCombat()
        {
            // Begin combat
            CurrentCombatState = CombatState.Chasing;
        }

        /// <summary>
        /// Called by the OnClick() event.
        /// </summary>
        /// <param name="actionToPerform"></param>
        public void ActionPressed(SkillAction actionToPerform)
        {
            if (!actionToPerform.CanPerform(GameObject.FindGameObjectWithTag("Player"), currentTarget)) return;
            actionToPerform.Execute(GameObject.FindGameObjectWithTag("Player"), currentTarget, () => { });
        }
    }
}