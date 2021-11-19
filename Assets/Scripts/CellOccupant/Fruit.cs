using UnityEngine;

public class Fruit : MonoBehaviour, ICellOccupant
{
    public Level.GridCell cell;
    public ICellOccupant.OccupantType Type => ICellOccupant.OccupantType.Fruit;
    public GameObject ThisGameObject => gameObject;

    public void Move(Level.GridCell _cell)
    {
        if (cell != null)
            cell.occupant = null;
        cell = _cell;
        cell.occupant = this;
        transform.position = cell.position;
    }
}
