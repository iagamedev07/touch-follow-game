using System;
using UnityEngine;

public class CollectibleTwo : Collectibles
{
    public static event Action OnCollectibleTwoCollected;

    [SerializeField] private GameObject destroyParticle;
    public override void Collect()
    {
        Debug.Log("Picked Collectible : 2");

        if(destroyParticle != null)
            Instantiate(destroyParticle, transform.position, Quaternion.identity);

        if (OnCollectibleTwoCollected != null)
            OnCollectibleTwoCollected();

        Destroy(gameObject);
    }
}
