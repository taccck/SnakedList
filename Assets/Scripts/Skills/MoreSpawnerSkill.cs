using UnityEngine;

[CreateAssetMenu(fileName = "More Spawner", menuName = "Skills/MoreSpawner", order = 2)]
public class MoreSpawnerSkill : Skill
{
    public int spawnersToAdd = 1;
    
    public override void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        for (int i = 0; i < spawnersToAdd; i++)
        { Instantiate(_spawner.gameObject, _parent); }
    }
}
