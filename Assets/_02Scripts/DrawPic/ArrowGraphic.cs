using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArrowGraphic : Graphic
{
    private Vector2 prePos = new Vector2(-500, -250);
    private Vector2 nowPos = new Vector2(500, 250);
    private float len = 0.015f;
    private float angle = 22.5f;
    public float texWidth = 1600;
    private DrawArrow.Type type;

    public void SetPos(Vector2 v1, Vector2 v2, Color color, DrawArrow.Type type, float texWidth, float texHeight)
    {
        v1.x = (v1.x - 0.5f) * texWidth;
        v2.x = (v2.x - 0.5f) * texWidth;
        v1.y = (v1.y - 0.5f) * texHeight;
        v2.y = (v2.y - 0.5f) * texHeight;
        this.color = color;
        prePos = v1;
        nowPos = v2;
        this.type = type;
    }

    void Update()
    {
        SetAllDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Color32 color32 = color;
        vh.Clear();

        Vector2 direction = nowPos - prePos;
        Vector2 bDirection = RotationMatrix(direction, 90).normalized;
        Vector2[] triangle = new Vector2[7];
        float RectLength = direction.magnitude - Mathf.Abs(texWidth * len * Mathf.Cos(angle * Mathf.Deg2Rad));
        Vector2 Add = direction.normalized * RectLength;

        triangle[0] = nowPos;
        triangle[1] = nowPos - texWidth * len * RotationMatrix(direction, angle).normalized;
        triangle[6] = nowPos - texWidth * len * RotationMatrix(direction, -angle).normalized;

        triangle[4] = prePos + len / 8f * texWidth * bDirection;
        triangle[3] = prePos - len / 8f * texWidth * bDirection;
        triangle[5] = triangle[4] + Add;
        triangle[2] = triangle[3] + Add;

        if (type == DrawArrow.Type.QQ)
        {
            triangle[3] = prePos + len / 40f * texWidth * bDirection;
            triangle[4] = prePos - len / 40f * texWidth * bDirection;
        }

        for (int i = 0; i < triangle.Length; i++)
        {
            vh.AddVert(triangle[i], color32, new Vector2(0f, 0f));
        }

        // 几何图形中的三角形
        vh.AddTriangle(0, 1, 6);
        vh.AddTriangle(2, 3, 4);
        vh.AddTriangle(4, 5, 2);
    }

    private Vector2 RotationMatrix(Vector2 v, float angle)
    {
        var x = v.x;
        var y = v.y;
        var sin = Mathf.Sin(Mathf.PI * angle / 180);
        var cos = Mathf.Cos(Mathf.PI * angle / 180);
        var newX = x * cos + y * sin;
        var newY = x * -sin + y * cos;
        return new Vector2((float)newX, (float)newY);
    }
}
