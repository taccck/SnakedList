using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : Graphic
{
    [HideInInspector] public Vector2 endPos;
    
    [SerializeField] private float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper _vh)
    {
        _vh.Clear();
        DrawVerticesForPoint(Vector2.zero);
        DrawVerticesForPoint(endPos);

        _vh.AddTriangle(0, 1, 2);
        _vh.AddTriangle(3, 2, 1);

        void DrawVerticesForPoint(Vector2 _point)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            vertex.position = new Vector3(-thickness / 2, 0);
            vertex.position += (Vector3) _point;
            _vh.AddVert(vertex);

            vertex.position = new Vector3(thickness / 2, 0);
            vertex.position += (Vector3) _point;
            _vh.AddVert(vertex);
        }
    }
}