using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Two Infront", menuName = "Skills/Spawn2Infront", order = 3)]
public class Spawn2InfrontSkill : Skill
{
    public GameObject spawner;
    public override void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        Instantiate(spawner, _parent);
    }
}