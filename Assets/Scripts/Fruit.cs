using UnityEngine;

public enum FruitType
{
    Cherry, Strawberry, Grape, Dekopon, Persimmon, Apple,
    Pear, Peach, Pineapple, Melon, Watermelon
}

public class Fruit : MonoBehaviour
{
    [SerializeField] public FruitSpawner fruitSpawner;
    
    public FruitType type;
    private bool merged = false;

    void Awake()
    {
        if (fruitSpawner == null)
            fruitSpawner = FindAnyObjectByType<FruitSpawner>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (merged) return;

        Fruit collidedFruit = collision.gameObject.GetComponent<Fruit>();
        if (collidedFruit != null && type == collidedFruit.type && !collidedFruit.merged)
        {
            merged = true;
            collidedFruit.merged = true;

            Destroy(gameObject);
            Destroy(collidedFruit.gameObject);

            int number = (int)type;
            Vector3 spawnPos = (transform.position + collidedFruit.transform.position) / 2f;
            fruitSpawner.SpawnMergedFruit(number + 1, spawnPos);
        }
    }
}
