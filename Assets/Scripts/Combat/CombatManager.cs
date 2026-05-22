using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    // Queue of combat actions that are due to be performed
    // List of combat actions succesfully performed
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

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
}
