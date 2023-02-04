using System;
using UnityEngine;

public class CollectibleOne : Collectibles
{
    public static event Action OnCollectibleOneCollected;

    [SerializeField] private GameObject destroyParticle;
    public override void Collect()
    {
        Debug.Log("Picked Collectible : 1");

        if (destroyParticle != null)
            Instantiate(destroyParticle, transform.position, Quaternion.identity);

        if (OnCollectibleOneCollected != null)
            OnCollectibleOneCollected();

        Destroy(gameObject);
    }
}
