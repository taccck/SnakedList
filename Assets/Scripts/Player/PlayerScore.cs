using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [NonSerialized] public static PlayerScore current;

    public int score;

    [SerializeField] private Skill snekHead;
    [SerializeField] private Skill fruitHead;


    private void Awake()
    {
        if (ThereCanOnlyBeOne()) return;
        LockAllSkill();
        current = this;
    }

    private bool ThereCanOnlyBeOne()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Score");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return true;
        }

        DontDestroyOnLoad(gameObject);
        return false;
    }

    private void LockAllSkill()
    {
        //unlocked skills aren't saved between play times
        Lock(snekHead);
        Lock(fruitHead);

        void Lock(Skill _skill)
        {
            _skill.locked = true;
            if (_skill.left != null)
                Lock(_skill.left);
            if (_skill.right != null)
                Lock(_skill.right);
        }
    }
}