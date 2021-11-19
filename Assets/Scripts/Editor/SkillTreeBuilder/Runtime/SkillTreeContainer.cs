using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTreeContainer : ScriptableObject
{
    public List<NodeData> nodeData = new List<NodeData>();
    public List<LinkData> linkData = new List<LinkData>();
}
