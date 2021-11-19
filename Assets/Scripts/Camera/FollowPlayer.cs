using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField, Range(0f, .99f)] private float smoothness;

    private void Update()
    {
        Vector3? playerPosition = PlayerController.current?.SnakedList?.head?.transform.position; 
        if (playerPosition == null) return; //if player is destroyed
        
        Vector3 newPos = Vector2.Lerp(transform.position, 
            (Vector2) playerPosition, 1f - smoothness);
        newPos.z = -10;
        transform.position = newPos;
    }
}