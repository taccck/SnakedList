using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class SkillTreeEditor : EditorWindow
{
    private SkillTreeGraph graphView;
    private string fileName = "New Skill Tree";

    [MenuItem("Tools/Skill Tree")]
    public static void OpenEditor()
    {
        SkillTreeEditor window = GetWindow<SkillTreeEditor>();
        window.titleContent = new GUIContent("Skill Tree Graph");
    }

    private void OnEnable()
    {
        ConstructGraph();
        GenerateToolbar();
        GenerateMiniMap();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraph()
    {
        graphView = new SkillTreeGraph
        {
            name = "Skill Tree"
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();
        Button nodeCreateButt = new Button(() => { graphView.CreateNode("New Skill"); });
        nodeCreateButt.text = "Create Node";
        toolbar.Add(nodeCreateButt);

        TextField fileNameTextField = new TextField("File Name");
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(_evt => fileName = _evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) {text = "Save Data"});
        toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});


        rootVisualElement.Add(toolbar);
    }

    private void GenerateMiniMap()
    {
        MiniMap miniMap = new MiniMap {anchored = true};
        miniMap.SetPosition(new Rect(10, 30, 200, 140));
        graphView.Add(miniMap);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name", "Enter a valid file name", "OK");
            return;
        }

        GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(graphView);
        if (save)
            saveUtility.SaveGraph(fileName);
        else
            saveUtility.LoadGraph(fileName);
    }
}