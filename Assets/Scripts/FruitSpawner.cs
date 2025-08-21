using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fruitPrefabs;

    public void SpawnNextFruit()
    {
        // Instantiate();
    }

    public void SpawnMergedFruit(int mergedType, Vector3 spawnPos)
    {
        Instantiate(fruitPrefabs[mergedType], spawnPos, Quaternion.identity);
    }
}
