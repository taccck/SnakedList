using UnityEngine;

public class Obstacle : MonoBehaviour, ICellOccupant
{
    public static Obstacle current;
    
    public ICellOccupant.OccupantType Type => ICellOccupant.OccupantType.Obstacle;
    public GameObject ThisGameObject => gameObject;
    
    private void Awake()
    {
        current = this;
    }
}
