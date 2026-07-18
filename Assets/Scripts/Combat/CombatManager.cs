using System;
using UnityEngine;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;

        public CombatState CurrentCombatState { get; private set; }

        public GameObject currentTarget;

        private GameObject _player;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            CombatInitiator.OnCombatStarted += BeginCombat;
        }

        private void OnDestroy()
        {
            CombatInitiator.OnCombatStarted -= BeginCombat;
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
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player");
            
            if (_player == null || currentTarget == null) return;
            
            Combatant targetCombatant = currentTarget.GetComponent<Combatant>();
            
            if (targetCombatant == null || !targetCombatant.IsAlive) return;
            
            if (!actionToPerform.CanPerform(GameObject.FindGameObjectWithTag("Player"), currentTarget)) return;
            
            actionToPerform.Execute(GameObject.FindGameObjectWithTag("Player"), currentTarget, () => { });
        }

        public void SetCurrentTarget(GameObject target)
        {
            currentTarget = target;
        }
        
        public void ClearCurrentTarget(GameObject target)
        {
            if (currentTarget == target)
                currentTarget = null;
        }
    }
}