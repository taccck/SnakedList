using UnityEngine;

[CreateAssetMenu(fileName = "Start Speed Up", menuName = "Skills/StartSpeedUp", order = 6)]
public class StartSpeedUpSkill : Skill
{
    public float decreaseTime = .1f;
    
    public override void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        TickManager.current.startTickTime -= decreaseTime;
    }
}