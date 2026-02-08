using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerRpgController : MonoBehaviour
    {
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            // Setting player starting stats.
            CurrentPlayerLevel = 1;
            CurrentPlayerExp = 0;
            CurrentPlayerHealth = 100;
            PlayerMaxHealth = 100;
        }

        #region Variables

        public int CurrentPlayerLevel { get; private set; }
        public int CurrentPlayerExp { get; private set; }
        public int CurrentPlayerHealth { get; private set; }
        public int PlayerMaxHealth { get; private set; }
        [field: SerializeField] public PlayerStats CurrentPlayerAttributes { get; private set; }

        public static PlayerRpgController Instance { get; private set; }

        #endregion

        [Serializable]
        public struct PlayerStats
        {
            public int strength;
            public int intelligence;
            public int dexterity;
            public int vitality;
            public int magic;
            public int luck;
        }
    }
}