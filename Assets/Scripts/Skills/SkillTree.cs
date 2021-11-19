using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Skill snekHead;
    public Skill fruitHead;
    [SerializeField] private FruitSpawner fruitSpawner;
    [SerializeField] private Transform fruitSpawnerParent;
    
    private void Start()
    {
        ActivateSkill(snekHead);
        ActivateSkill(fruitHead);
    }

    private void ActivateSkill(Skill _skill)
    {
        if (_skill.locked) return;
        
        _skill.SkillEffect(fruitSpawner, fruitSpawnerParent);
        if (_skill.left != null)
        {
            ActivateSkill(_skill.left);
        }

        if (_skill.right != null)
        {
            ActivateSkill(_skill.right);
        }
    }
}
