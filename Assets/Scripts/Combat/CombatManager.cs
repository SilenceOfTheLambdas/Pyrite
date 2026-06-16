using System;
using System.Collections.Generic;
using Combat;
using Pyrite.Combat;
using UnityEngine;

public class Turn
{
    public Queue<CombatAction> queuedCombatActions;
    public List<CombatAction> completedCombatActions;
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public CombatState CurrentCombatState { get; private set; }

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
        CurrentCombatState = CombatState.Setup;
        // TODO: Move Camera

        // Planning stage
        // Carry-out queued actions
    }

    /// <summary>
    /// Called by the OnClick() event.
    /// </summary>
    /// <param name="actionName"></param>
    public void ActionPressed(string actionName)
    {
        Debug.Log($"Pressed Action: {actionName}");
    }
}