using System.Collections;
using UnityEngine;



// Just a Quick script to spawn collectibles randomly
public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject collectibleAPrefab;
    [SerializeField] private GameObject collectibleBPrefab;
    [SerializeField] private int maxCollectibles;
    [SerializeField] private float spawnGap = 4f;
    [SerializeField] private Vector2 spawnAreaLimitsX;
    [SerializeField] private Vector2 spawnAreaLimitsZ;
    private int currentCollectiblesSpawned;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        StartCoroutine(SpawnCollectible());  //starts the Spawning    
    }

    private void OnEnable()
    {
        CollectibleOne.OnCollectibleOneCollected += RemoveCollectible;
        CollectibleTwo.OnCollectibleTwoCollected += RemoveCollectible;
    }

    private void OnDisable()
    {
        CollectibleOne.OnCollectibleOneCollected -= RemoveCollectible;
        CollectibleTwo.OnCollectibleTwoCollected -= RemoveCollectible;
    }

    private IEnumerator SpawnCollectible()
    {
        if(currentCollectiblesSpawned < maxCollectibles)
        {
            Vector3 pos = GetRandomScreenPosition();
            if(Random.value < 0.5f)
                Instantiate(collectibleAPrefab, pos, Quaternion.identity);
            else
                Instantiate(collectibleBPrefab, pos, Quaternion.identity);

            currentCollectiblesSpawned++;
        }

        yield return new WaitForSeconds(spawnGap);
        StartCoroutine(SpawnCollectible());
    }

    //Gets the random position inside the game view
    Vector3 GetRandomScreenPosition()
    {
        float xPos = Random.Range(spawnAreaLimitsX.x, spawnAreaLimitsX.y);
        float zPos = Random.Range(spawnAreaLimitsZ.x, spawnAreaLimitsZ.y);
        Vector3 spawnPos = new Vector3(xPos, 0.5f, zPos);
        return spawnPos;
    }

    //removes the collectible if destroyed
    void RemoveCollectible()
    {
        currentCollectiblesSpawned--;
    }


    
}
