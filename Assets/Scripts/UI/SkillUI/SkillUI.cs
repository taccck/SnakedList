using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [NonSerialized] public Skill skill;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private UILineRenderer rightLine;
    [SerializeField] private UILineRenderer leftLine;
    
    private RectTransform rectTransform;

    public void Unlock()
    {
        if (!skill.locked) return;

        foreach (Skill parent in skill.parent)
        {
            if (parent == null) continue;
            if (parent.locked) return;
        }

        if (PlayerScore.current.score < skill.cost) return;
        PlayerScore.current.score -= skill.cost;
        
        skill.locked = false;
        SetColor();
    }

    public void SetUI(Vector2 _leftChildPos, Vector2 _rightChildPos)
    {
        SetColor();
        if (_leftChildPos != Vector2.zero)
            _leftChildPos -= (Vector2) rectTransform.localPosition;
        if (_rightChildPos != Vector2.zero)
            _rightChildPos -= (Vector2) rectTransform.localPosition;
        SetLineRenderer(_leftChildPos, _rightChildPos);
        nameText.text = skill.skillName;
        descText.text = skill.desc;
        costText.text = $"Cost: {skill.cost}";
    }
    
    private void SetColor()
    {
        image.color = skill.locked ? Color.grey : Color.white;
        Color lineColor = skill.locked ? Color.black : Color.gray;
        leftLine.color = lineColor;
        rightLine.color = lineColor;
    }

    private void SetLineRenderer(Vector2 _leftChildPos, Vector2 _rightChildPos)
    {
        leftLine.endPos = _leftChildPos;
        leftLine.SetAllDirty();
        rightLine.endPos = _rightChildPos;
        rightLine.SetAllDirty();
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}