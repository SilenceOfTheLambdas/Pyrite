using RPGSystem.Backend;
using Unity.VisualScripting;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance;

    public delegate void GiveItemToPlayer(ItemTemplate itemTemplate);

    void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        } else
            Destroy(Instance);
    }
}
