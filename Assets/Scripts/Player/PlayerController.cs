using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : SnakeController
{
    [NonSerialized] public int score;
    [NonSerialized] public static PlayerController current;
    [NonSerialized] public int eatToGrow = 1;

    [SerializeField] private MoveArrows moveArrows;
    [SerializeField] private float deathTime = 1f;

    private int currEaten;

    public override void Add()
    {
        score++;
        currEaten++;
        TickManager.current.SpeedUp();

        if (currEaten < eatToGrow) return;
        currEaten = 0;
        base.Add();
    }

    public override void Die()
    {
        base.Die();
        PlayerScore.current.score += score;
        TickManager.current.running = false;
        StartCoroutine(DeathWait());
    }

    private IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(deathTime);
        SceneManager.LoadScene("GameOverScreen");
    }

    private void OnMove(InputValue _value)
    {
        Vector2 input = _value.Get<Vector2>();
        Vector2Int inputVector = new Vector2Int((int) input.x, (int) input.y);
        if (inputVector == Vector2.zero || inputVector == lastMoved * -1) return;
        moveVector = inputVector;
        moveArrows.SetArrow(moveVector);
    }

    private void Awake()
    {
        current = this;
    }
}