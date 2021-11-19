using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] protected Sprite[] fruitSprites;
    
    private static int ticksToSpawn = 5;
    private int currentTicks;
    
    public static int TicksToSpawn
    {
        get => ticksToSpawn;
        set
        {
            if (value < 0)
                value = 0;
            ticksToSpawn = value;
        } 
    }

    protected virtual void Spawn()
    {
        Level.GridCell cell = Level.current.RandomEmptyCell();
        if (cell == null) return;

        GameObject newFruit = new GameObject
        {
            name = "Fruit",
            transform =
            {
                parent = transform,
                position = cell.position
            }
        };

        SpriteRenderer spriteRenderer = newFruit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
        spriteRenderer.sortingOrder = 5;

        Fruit fruit = newFruit.AddComponent<Fruit>();
        fruit.cell = cell;
        cell.occupant = fruit;
    }

    private void SpawnFruit()
    {
        currentTicks++;

        if (currentTicks >= ticksToSpawn)
        {
            currentTicks = 0;

            Spawn();
        }
    }

    private void Start()
    {
        TickManager.current.OnSpawnFruit += SpawnFruit;
    }
}