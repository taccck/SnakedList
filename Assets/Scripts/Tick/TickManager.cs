using System;
using System.Collections;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public float startTickTime = .5f;
    [Range(0, 1)] public float speedUpPercent = .04f;
    public static TickManager current;
    public event Action OnMove;
    public event Action OnSpawnFruit;
    [NonSerialized] public bool running = true;

    private float currTickTime;

    public void SpeedUp()
    {
        currTickTime -= currTickTime * speedUpPercent;
    }

    private IEnumerator Tick()
    {
        yield return new WaitForEndOfFrame();
        while (running) //stops when player dies
        {
            if (OnMove != null) OnMove.Invoke();
            if (OnSpawnFruit != null) OnSpawnFruit.Invoke();
            yield return new WaitForSeconds(currTickTime);
        }
    }

    private void Start()
    {
        StartCoroutine(Tick());
    }

    private void Awake()
    {
        current = this;
        currTickTime = startTickTime;
    }
}