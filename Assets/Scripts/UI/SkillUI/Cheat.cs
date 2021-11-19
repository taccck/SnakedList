using UnityEngine;

public class Cheat : MonoBehaviour
{
    public void InfiniteScore()
    {
        PlayerScore.current.score = 999999;
    }
}
