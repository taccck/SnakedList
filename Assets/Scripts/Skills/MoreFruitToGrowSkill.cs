using UnityEngine;

[CreateAssetMenu(fileName = "More Fruit To Grow", menuName = "Skills/MoreFruitToGrow", order = 4)]
public class MoreFruitToGrowSkill : Skill
{
    public int moreToEat = 1;

    public override void SkillEffect(FruitSpawner _spawner, Transform _parent)
    {
        PlayerController.current.eatToGrow += moreToEat;
    }
}