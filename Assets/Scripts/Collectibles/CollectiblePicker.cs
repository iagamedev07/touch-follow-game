using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePicker : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Collectible")
        {
            Collectibles collectible = col.gameObject.GetComponent<Collectibles>();
            if(collectible != null)
            {
                collectible.Collect();
            }
        }
    }
}
