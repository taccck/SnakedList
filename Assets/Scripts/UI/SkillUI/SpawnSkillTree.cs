using System.Collections.Generic;
using UnityEngine;

public class SpawnSkillTree : MonoBehaviour
{
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Skill head;

    private Dictionary<string, RectTransform> spawned = new Dictionary<string, RectTransform>();

    private void Start()
    {
        SpawnSkill(head);
    }

    private void SpawnSkill(Skill _head)
    {
        Spawn(_head);

        Vector2 Spawn(Skill _skill)
        {
            if (spawned.ContainsKey(_skill.name))
            {
                RectTransform rectTrans = spawned[_skill.name];
                rectTrans.transform.SetAsLastSibling();
                return rectTrans.localPosition;
            } 

            GameObject newSkillGo = Instantiate(skillPrefab, transform);
            SkillUI skillUI = newSkillGo.GetComponent<SkillUI>();
            skillUI.skill = _skill;
            RectTransform skillRectTrans = newSkillGo.GetComponent<RectTransform>();
            skillRectTrans.localPosition = new Vector3(skillUI.skill.position.x, skillUI.skill.position.y);
            spawned.Add(_skill.name, skillRectTrans);

            Vector2 left = Vector2.zero;
            Vector2 right = Vector2.zero;

            if (_skill.left != null)
                left = Spawn(_skill.left);
            if (_skill.right != null)
                right = Spawn(_skill.right);
            skillUI.SetUI(left, right);

            return skillRectTrans.localPosition;
        }
    }
}