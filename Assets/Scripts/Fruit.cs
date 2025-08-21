using Unity.VisualScripting;
using UnityEngine;

public enum FruitType
{
    Cherry, Strawberry, Grape, Dekopon, Persimmon, Apple,
    Pear, Peach, Pineapple, Melon, Watermelon
}

public class Fruit : MonoBehaviour
{
    [SerializeField] public FruitSpawner fruitSpawner;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    GameManager gameManager;

    public FruitType type;
    InputActions inputActions;
    Rigidbody2D rb;
    public bool isControlled = false;
    private bool merged = false;
    bool isDropped = false;
    bool gameOverTriggered = false;
    float defaultScore = 2f;

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Awake()
    {
        inputActions = new InputActions();
        gameManager = FindAnyObjectByType<GameManager>();
        if (fruitSpawner == null)
            fruitSpawner = FindAnyObjectByType<FruitSpawner>();

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!isControlled) return;

        Vector2 targetPos = inputActions.Player.Position.ReadValue<Vector2>();
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        MoveFruit(targetPos);
    }

    void MoveFruit(Vector2 targetPos)
    {
        if (!isDropped)
        {
            Vector2 newPos = new Vector2(targetPos.x, fruitSpawner.spawnPoint.transform.position.y);
            rb.MovePosition(newPos);
            rb.gravityScale = 0;
            DropFruit(rb);
        }
    }

    void DropFruit(Rigidbody2D rb)
    {
        if (!isDropped && inputActions.Player.Press.WasPressedThisFrame())
        {
            rb.gravityScale = 1;
            isDropped = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isControlled) return;

        if (collision.CompareTag("Game Over") && !gameOverTriggered)
        {
            gameOverTriggered = true;

            Fruit[] fruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
            foreach (var f in fruits)
            {
                f.isControlled = false;
                f.rb.linearVelocity = Vector2.zero;
                f.rb.gravityScale = 0;
            }

            gameManager.Invoke("GameOver", 0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameOverTriggered) return;
        if (isControlled && (collision.gameObject.CompareTag("Bottom Border") || collision.gameObject.CompareTag("Fruit")))
        {
            fruitSpawner.SpawnNextFruit();
            isControlled = false;
            rb.constraints = RigidbodyConstraints2D.None;
        }

        Fruit collidedFruit = collision.gameObject.GetComponent<Fruit>();
        if (collidedFruit != null && type == collidedFruit.type && !merged && !collidedFruit.merged)
        {
            if (GetInstanceID() < collidedFruit.GetInstanceID()) return;

            merged = true;
            collidedFruit.merged = true;

            int number = (int)type;
            Vector3 spawnPos = (transform.position + collidedFruit.transform.position) / 2f;
            if (number < 10)
            {
                fruitSpawner.SpawnMergedFruit(number + 1, spawnPos);
            }

            // add score
            gameManager.AddScore(defaultScore * (number + 1));

            // play sfx
            AudioSource.PlayClipAtPoint(audioClip, transform.position);

            Destroy(gameObject);
            Destroy(collidedFruit.gameObject);
        }
    }
}
