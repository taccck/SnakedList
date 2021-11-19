using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillTreeGraph : GraphView
{
    public readonly Vector2 nodeSize = new Vector2(150, 200);

    public SkillTreeGraph()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        GridBackground background = new GridBackground();
        Insert(0, background);
        background.StretchToParentSize();


        AddElement(GenerateEntryPointNode());
    }

    public SkillTreeNode CreateNode(string _nodeName)
    {
        SkillTreeNode node = new SkillTreeNode
        {
            title = _nodeName,
            guid = Guid.NewGuid().ToString()
        };
        Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);

        Port generatedPort1 = GeneratePort(node, Direction.Output);
        generatedPort1.portName = "Right";
        node.outputContainer.Add(generatedPort1);

        Port generatedPort2 = GeneratePort(node, Direction.Output);
        generatedPort2.portName = "Left";
        node.outputContainer.Add(generatedPort2);

        ObjectField skillField = new ObjectField
        {
            objectType = typeof(Skill)
        };
        skillField.MarkDirtyRepaint();
        skillField.RegisterValueChangedCallback(_evt => node.skill = (Skill) _evt.newValue);
        node.inputContainer.Add(skillField);

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(Vector2.zero, nodeSize));
        AddElement(node);
        return node;
    }

    public override List<Port> GetCompatiblePorts(Port _startPort, NodeAdapter _nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach((_port) =>
        {
            if (_startPort != _port && _startPort.node != _port.node)
            {
                compatiblePorts.Add(_port);
            }
        });

        return compatiblePorts;
    }

    private Port GeneratePort(SkillTreeNode _node, Direction _portDirection,
        Port.Capacity _capacity = Port.Capacity.Single)
    {
        return _node.InstantiatePort(Orientation.Horizontal, _portDirection, _capacity, typeof(float));
    }

    private SkillTreeNode GenerateEntryPointNode()
    {
        SkillTreeNode node = new SkillTreeNode
        {
            title = "Start",
            guid = Guid.NewGuid().ToString(),
            entryPoint = true
        };

        Port generatedPort1 = GeneratePort(node, Direction.Output);
        generatedPort1.portName = "First";
        node.outputContainer.Add(generatedPort1);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }
}