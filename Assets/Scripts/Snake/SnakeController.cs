using System;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public Sprite head;
    public Sprite body;
    public Sprite tail;
    public SnakedList<SnakeNode> SnakedList { get; private set; }

    [NonSerialized] public Vector2Int moveVector = Vector2Int.up;
    protected Vector2Int lastMoved = Vector2Int.up;

    public virtual void Add() => SpawnSnakeNode();
    private SnakeNode HeadNode => SnakedList.head.value;
    private SnakeNode TailNode => SnakedList.tail.value;

    public virtual void Die()
    {
        foreach (SnakeNode node in SnakedList)
        {
            Destroy(node.gameObject);
        }

        SnakedList = null;
    }

    private void SpawnSnakeNode()
    {
        Level.GridCell cell = CellToSpawnAt();
        if (cell.occupant != null) return;

        GameObject newObject = new GameObject
        {
            transform =
            {
                position = cell.position,
                parent = transform
            }
        };

        SnakeNode newNode = newObject.AddComponent<SnakeNode>();
        newNode.occupying = cell;
        cell.occupant = newNode;
        newNode.controller = this;

        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;

        SnakeNode lastNode = SnakedList.Add(newNode)?.value;
        if (SnakedList.Count == 1)
        {
            spriteRenderer.sprite = head;
            newObject.name = "Head";
            return;
        }

        if (SnakedList.Count > 2)
        {
            lastNode.GetComponent<SpriteRenderer>().sprite = body;
            lastNode.gameObject.name = "Body";
        }
        newObject.name = "Tail";
        spriteRenderer.sprite = tail;
    }


    private Level.GridCell CellToSpawnAt()
    {
        Level.GridCell cell;
        if (SnakedList.tail == null) //add to the middle if no snake nodes are in the list
            cell = Level.current.LevelGrid[
                Level.current.LevelGrid.GetLength(0) / 2,
                Level.current.LevelGrid.GetLength(1) / 2];
        else if (TailNode.cellToAddNodeAt == null) //add to one cell below the tail if it doesn't have a cell to add at 
            cell = Level.current.LevelGrid[TailNode.occupying.index.x,
                TailNode.occupying.index.y - 1];
        else
            cell = TailNode.cellToAddNodeAt;

        return cell;
    }

    private void Move()
    {
        if (SnakedList.head == null) return;
        lastMoved = moveVector;
        Vector2Int moveTo = HeadNode.occupying.index + moveVector;
        HeadNode.Move(moveTo);
    }

    private void Start()
    {
        SnakedList = new SnakedList<SnakeNode>();
        TickManager.current.OnMove += Move;
        SpawnSnakeNode();
        SpawnSnakeNode();
        SpawnSnakeNode();
    }
}