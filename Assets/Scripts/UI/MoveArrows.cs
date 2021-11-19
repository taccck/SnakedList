using UnityEngine;
using UnityEngine.UI;

public class MoveArrows : MonoBehaviour
{
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private Image image;

    public void SetArrow(Vector2Int moveVector)
    {
        (int x, int y) moveDir = (moveVector.x, moveVector.y);
        
        switch (moveDir)
        {
            case (0,1):
                image.sprite = upSprite;
                return;
            
            case (0,-1):
                image.sprite = downSprite;
                return;
            
            case (-1,0):
                image.sprite = leftSprite;
                return;
            
            case (1,0):
                image.sprite = rightSprite;
                return;
        }
    }
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }
}
