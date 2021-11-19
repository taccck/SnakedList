using UnityEngine;

public class TwoInFrontSpawner : FruitSpawner
{
    protected override void Spawn()
    {
        if (PlayerController.current.SnakedList == null) return;
        
        Vector2Int dir = PlayerController.current.SnakedList.head.value.occupying.index +
                         PlayerController.current.moveVector;
        if (!Level.current.WithinGrid(dir)) return;

        Level.GridCell cell = Level.current.LevelGrid[dir.x, dir.y];
        if (cell.occupant != null) return;

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

        fruit.Move(cell);
    }
}