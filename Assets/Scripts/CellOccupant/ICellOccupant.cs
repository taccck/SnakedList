using UnityEngine;

public interface ICellOccupant
{   //this is an interface instead of a mono behavior class so snake nodes can inherit the node class
    public enum OccupantType
    {
        Player,
        Fruit,
        Obstacle
    }

    public OccupantType Type { get; }
    public GameObject ThisGameObject { get; }
}