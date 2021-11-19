using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI score;
    
    private void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        score.text = PlayerController.current.score.ToString();
    }
}
