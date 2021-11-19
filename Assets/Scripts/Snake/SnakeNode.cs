using System;
using UnityEngine;

public class SnakeNode : SnakedList<SnakeNode>.Node, ICellOccupant
{   //inherits the node class to move with recursion through the next reference
    public Level.GridCell cellToAddNodeAt;
    [NonSerialized] public SnakeController controller;
    public ICellOccupant.OccupantType Type => ICellOccupant.OccupantType.Player;
    public GameObject ThisGameObject => gameObject;

    public Level.GridCell occupying;
    
    public void Move(Vector2Int _moveTo) 
    {   //should only be called for the list head, since the other nodes move through recursion
        Level.GridCell moveToCell = CheckCell(_moveTo);
        if (moveToCell == null) return;

        Move(moveToCell);
    }

    private Level.GridCell CheckCell(Vector2Int _toCheck)
    {
        if (!Level.current.WithinGrid(_toCheck))
        {
            controller.Die();
            return null;
        }

        Level.GridCell moveToCell = Level.current.LevelGrid[_toCheck.x, _toCheck.y];
        
        if (moveToCell.occupant != null)
        {
            switch (moveToCell.occupant.Type)
            {
                case ICellOccupant.OccupantType.Fruit:
                    controller.Add();
                    Destroy(moveToCell.occupant.ThisGameObject);
                    break;
                case ICellOccupant.OccupantType.Player:
                    controller.Die();
                    return null;
                case ICellOccupant.OccupantType.Obstacle:
                    controller.Die();
                    return null;
            }
        }

        return moveToCell;
    }

    private void Move(Level.GridCell _moveTo)
    {
        Level.GridCell lastCell = occupying;
        occupying.occupant = null;
        occupying = _moveTo;
        occupying.occupant = this;
        transform.position = occupying.position;

        if (next != null)
        {   //move the snake through recursion
            next.value.Move(lastCell);
            return;
        }
        cellToAddNodeAt = lastCell; //set where to add new nodes at the tails last position
    }

    private void Awake()
    {
        value = this;
    }
}