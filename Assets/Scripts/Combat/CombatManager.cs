using System;
using System.Collections.Generic;
using Pyrite.Combat;
using UnityEngine;

public class Turn
{
    public Queue<ICombatAction> queuedCombatActions;
    public List<ICombatAction> completedCombatActions;
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    private CombatState _currentCombatState;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    private void Start()
    {
        Combatinitiator.OnCombatStarted += BeginCombat;
    }

    private void BeginCombat()
    {
        // Begin combat
        _currentCombatState = CombatState.Setup;
        // TODO: Move Camera
        // Switch/Display Combat HUD
        // Planning stage
        // Carry-out queued actions
    }

    /// <summary>
    /// Called by the OnClick() event.
    /// </summary>
    /// <param name="actionName"></param>
    public void ActionPressed(String actionName)
    {
        Debug.Log($"Pressed Action: {actionName}");
    }
}
