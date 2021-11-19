using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private SkillTreeGraph targetGraph;
    private SkillTreeContainer containerCash;

    private List<Edge> Edges => targetGraph.edges.ToList();
    private List<SkillTreeNode> Nodes => targetGraph.nodes.ToList().Cast<SkillTreeNode>().ToList();

    public static GraphSaveUtility GetInstance(SkillTreeGraph _targetGraph)
    {
        return new GraphSaveUtility
        {
            targetGraph = _targetGraph
        };
    }

    public void SaveGraph(string _fileName)
    {
        if (!Edges.Any()) return;


        SkillTreeContainer container = ScriptableObject.CreateInstance<SkillTreeContainer>();

        foreach (SkillTreeNode n in Nodes.Where(_node => !_node.entryPoint))
        {
            container.nodeData.Add(new NodeData
            {
                guid = n.guid,
                position = n.GetPosition().position,
                skill = n.skill,
            });

            n.skill.parent = new List<Skill>();
            n.skill.position = n.GetPosition().position;
            EditorUtility.SetDirty(n.skill);
        }

        Edge[] connectedPorts = Edges.Where(_x => _x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            SkillTreeNode outputNode = connectedPorts[i].output.node as SkillTreeNode;
            SkillTreeNode inputNode = connectedPorts[i].input.node as SkillTreeNode;

            container.linkData.Add(new LinkData
            {
                baseNodeGuid = outputNode.guid,
                portName = connectedPorts[i].output.portName,
                targetNodeGuid = inputNode.guid
            });

            inputNode.skill.parent.Add(outputNode.skill);

            if (connectedPorts[i].output.portName == "Left")
            {
                outputNode.skill.left = inputNode.skill;
            }

            if (connectedPorts[i].output.portName == "Right")
            {
                outputNode.skill.right = inputNode.skill;
            }

            if (outputNode.skill != null)
                EditorUtility.SetDirty(outputNode.skill);
            if (inputNode.skill != null)
                EditorUtility.SetDirty(inputNode.skill);
        }


        AssetDatabase.CreateAsset(container,
            $"Assets/Scripts/Editor/SkillTreeBuilder/Runtime/Resources/{_fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string _fileName)
    {
        containerCash = Resources.Load<SkillTreeContainer>(_fileName);
        if (containerCash == null)
        {
            EditorUtility.DisplayDialog("File not found", "Target file doesn't exist ", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (int x = 0; x < Nodes.Count; x++)
        {
            List<LinkData> connections = containerCash.linkData.Where(_x => _x.baseNodeGuid == Nodes[x].guid).ToList();
            for (int y = 0; y < connections.Count; y++)
            {
                string targetNodeGuid = connections[y].targetNodeGuid;
                SkillTreeNode targetNode = Nodes.First(_x => _x.guid == targetNodeGuid);
                LinkNodes(Nodes[x].outputContainer[y].Q<Port>(), (Port) targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(containerCash.nodeData.First(_x => _x.guid == targetNodeGuid).position,
                    targetGraph.nodeSize));
            }
        }
    }

    private void LinkNodes(Port _output, Port _input)
    {
        var tempEdge = new Edge
        {
            output = _output,
            input = _input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        targetGraph.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach (NodeData nodeData in containerCash.nodeData)
        {
            SkillTreeNode tempNode = targetGraph.CreateNode("New Skill");
            tempNode.guid = nodeData.guid;
            tempNode.skill = nodeData.skill;
            ObjectField skillField = (ObjectField) tempNode.inputContainer.ElementAt(1);
            skillField.SetValueWithoutNotify(nodeData.skill);
            targetGraph.AddElement(tempNode);
        }
    }

    private void ClearGraph()
    {
        Nodes.Find(_x => _x.entryPoint).guid = containerCash.linkData[0].baseNodeGuid;
        foreach (SkillTreeNode node in Nodes)
        {
            if (node.entryPoint) continue;
            Edges.Where(_x => _x.input.node == node).ToList().ForEach(_edge => targetGraph.RemoveElement(_edge));

            targetGraph.RemoveElement(node);
        }
    }
}