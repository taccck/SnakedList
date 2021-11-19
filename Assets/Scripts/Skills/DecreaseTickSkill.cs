using UnityEngine;

[CreateAssetMenu(fileName = "Decrease Tick", menuName = "Skills/DecreaseTick", order = 1)]
public class DecreaseTickSkill : Skill
{
    public int decreaseSpawnTicksBy = 1;
    
    public override void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        FruitSpawner.TicksToSpawn -= decreaseSpawnTicksBy;
    }
}
