using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EllipseGraphic : Graphic
{
    private Vector3 prePos = new Vector3(-500, -250, 0);
    private Vector3 nowPos = new Vector3(500, 250, 0);
    private Vector3 center;
    private float a;
    private float b;
    private float Factor = 0.003f;
    private float Radius = 1f;
    private int Segments = 80;

    public void SetPos(Vector2 v1,Vector2 v2,Color color,float texWidth,float texHeight)
    {
        v1.x = (v1.x - 0.5f) * texWidth;
        v2.x = (v2.x - 0.5f) * texWidth;
        v1.y = (v1.y - 0.5f) * texHeight;
        v2.y = (v2.y - 0.5f) * texHeight;
        this.color = color;
        if (v1.x < v2.x)
        {
            prePos.x = v1.x;
            nowPos.x = v2.x;
        }
        else
        {
            prePos.x = v2.x;
            nowPos.x = v1.x;
        }

        if (v1.y < v2.y)
        {
            prePos.y = v1.y;
            nowPos.y = v2.y;
        }
        else
        {
            prePos.y = v2.y;
            nowPos.y = v1.y;
        }
        Radius = Factor * texWidth;
        center = (prePos + nowPos) / 2f;
        a = (nowPos.x - prePos.x) / 2f;
        b = (nowPos.y - prePos.y) / 2f;
    }
    private void Update()
    {
        SetAllDirty();
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Color32 color32 = color;
        vh.Clear();
        float currAngle = Mathf.PI;
        int vertCount = 2 * (Segments * 1 + 1);
        float deltaAngle = 2 * currAngle / Segments;
        Vector3[] vertices = new Vector3[vertCount];
        for (int i = 0; i < vertCount; i += 2, currAngle -= deltaAngle)
        {
            float cosA = Mathf.Cos(currAngle);
            float sinA = Mathf.Sin(currAngle);
            vertices[i] = center + new Vector3(cosA * (a - Radius), sinA * (b - Radius), 0);
            vertices[i + 1] = center + new Vector3(cosA * a, sinA * b, 0);
            vh.AddVert(vertices[i], color32, Vector2.zero);
            vh.AddVert(vertices[i + 1], color32, Vector2.zero);
        }

        int len = 3 * (vertCount - 2);
        for (int i = 0, j = 0; i < len; i += 6, j += 2)
        {
            vh.AddTriangle(j + 1, j + 2, j);
            vh.AddTriangle(j + 1, j + 3, j + 2);
        }
    }
}
