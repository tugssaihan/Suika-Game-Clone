using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fruitPrefabs;
    [SerializeField] public GameObject spawnPoint;

    void Start()
    {
        SpawnNextFruit();
    }

    public void SpawnNextFruit()
    {
        int randomIndex = Random.Range(0, 5);
        var fruit = Instantiate(fruitPrefabs[randomIndex], spawnPoint.transform.position, Quaternion.identity);
        fruit.GetComponent<Fruit>().isControlled = true;
    }

    public void SpawnMergedFruit(int mergedType, Vector3 spawnPos)
    {
        Instantiate(fruitPrefabs[mergedType], spawnPos, Quaternion.identity);
    }
}