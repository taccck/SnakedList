using UnityEngine;

[CreateAssetMenu(fileName = "Speed Increase Down", menuName = "Skills/SpeedIncreaseDown", order = 5)]
public class SpeedIncreaseDownSkill : Skill
{
    public float decreaseBy = .01f;
    
    public override void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        TickManager.current.speedUpPercent -= decreaseBy;
    }
}