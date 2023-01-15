using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    enum CollectibleType
    {
        Health,
        Ammo
    }

    [SerializeField] private CollectibleType type;
    [SerializeField] private int value;


    public void Collect(Player player)
    {
        switch (type)
        {
            case CollectibleType.Health:
                player.TakeDamage(-value);
                break;
            case CollectibleType.Ammo:
                Player.GetAmmo.Invoke();
                break;
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Player script = collision.gameObject.GetComponent<Player>();
        if(script)
        {
            Collect(script);
            Destroy(gameObject);
        }
    }
}
