using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public string skillName;
    public string desc;
    public int cost;

    //generated from skill tree graph
    [HideInInspector] public bool locked = true;
    [HideInInspector] public Skill left;
    [HideInInspector] public Skill right;
    [HideInInspector] public List<Skill> parent = new List<Skill>();
    [HideInInspector] public Vector2 position;

    public virtual void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        Debug.LogWarning("don't use this skill");
    }
}