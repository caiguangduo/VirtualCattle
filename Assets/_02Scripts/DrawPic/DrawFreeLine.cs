using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawFreeLine : MonoBehaviour
{
    public Camera cam;
    public delegate void VoidDelegate();

    private bool drawline = false;
    private VoidDelegate callback;

    private bool textureNeedsUpdate = false;
    private Vector2 pixelUV;
    private Vector2 pixelUVOld;
    private bool wentOutside = false;
    private bool connectBrushStokes = true;
    public int brushSize=24;
    public float brushSizeFactor = 0.002f;
    public Color32 paintColor = Color.blue;

    private void Awake()
    {
        brushSize = Mathf.CeilToInt(Screen.width * brushSizeFactor);
    }

    private void Update()
    {
        //判断一下鼠标是否悬浮在UI上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (drawline)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnBegin();
            }
            if (Input.GetMouseButton(0))
            {
                OnMove();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnEnd();
            }
            UpdateTexture();
        }
    }
    void OnBegin()
    {
        Vector2 pos = cam.ScreenToViewportPoint(Input.mousePosition);
        //Push(TestBt.instance.pixels);
        pixelUVOld = new Vector2(pos.x * DrawPicManager.instance.tex.width, pos.y * DrawPicManager.instance.tex.height);
        pixelUV = pixelUVOld;
    }
    void OnMove()
    {
        Vector2 pos = cam.ScreenToViewportPoint(Input.mousePosition);
        pixelUVOld = pixelUV;
        pixelUV = pos;
        pixelUV.x *= DrawPicManager.instance.tex.width;
        pixelUV.y *= DrawPicManager.instance.tex.height;
        if (wentOutside)
        {
            pixelUVOld = pixelUV;
            wentOutside = false;
        }
        DrawCircle((int)pixelUV.x, (int)pixelUV.y);
        textureNeedsUpdate = true;

        if (connectBrushStokes && Vector2.Distance(pixelUV, pixelUVOld) > brushSize)
        {
            DrawStraghtLine(pixelUVOld, pixelUV);
            pixelUVOld = pixelUV;
            textureNeedsUpdate = true;
        }
    }
    void OnEnd()
    {
        if (callback != null)
            callback();
    }
    void UpdateTexture()
    {
        if (textureNeedsUpdate)
        {
            textureNeedsUpdate = false;
            DrawPicManager.instance.tex.LoadRawTextureData(DrawPicManager.instance.pixels);
            DrawPicManager.instance.tex.Apply(false);
        }
    }
    void DrawCircle(int x,int y)
    {
        int pixel = 0;
        //int lineWidth = brushSize / 4;
        // draw fast circle: 
        int r2 = brushSize * brushSize; // 1/4 个 正方形
        int area = r2 << 2; // 完整正方形
        int rr = brushSize << 1; // 正方形边长
        for (int i = 0; i < area; i++) //遍历整个正方形
        {
            int tx = (i % rr) - brushSize; // 取X轴坐标
            int ty = (i / rr) - brushSize; // 取Y轴坐标
            if (tx * tx + ty * ty < r2) //勾股定理确定该坐标是否在BrushSize为半径的圆上
            {
                if (x + tx < 0 || y + ty < 0 || x + tx >= DrawPicManager.instance.tex.width || y + ty >= DrawPicManager.instance.tex.height) continue; // 确定该坐标加上Texture2D上的偏移量是否还在Texture2D的范围内


                pixel = (DrawPicManager.instance.tex.width * (y + ty) + x + tx) * 4; //计算加上偏移量后在Texture2D一维坐标的位置

                DrawPicManager.instance.pixels[pixel] = paintColor.r;
                DrawPicManager.instance.pixels[pixel + 1] = paintColor.g;
                DrawPicManager.instance.pixels[pixel + 2] = paintColor.b;
                DrawPicManager.instance.pixels[pixel + 3] = paintColor.a;
            } // if in circle
        } // for area
    }
    void DrawStraghtLine(Vector2 start,Vector2 end)
    {
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistance = (int)(brushSize >> 1); // divide by 2, you might want to set mindistance to smaller value, to avoid gaps between brushes when moving fast
        int pixelCount = 0;
        int e2;
        while (loop)
        {
            pixelCount++;
            if (pixelCount > minDistance)
            {
                pixelCount = 0;
                DrawCircle(x0, y0);
            }
            if ((x0 == x1) && (y0 == y1)) loop = false;
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
    }

    public void Begin(Color color,VoidDelegate callback)
    {
        paintColor = color;
        drawline = true;
        this.callback = callback;
    }
    public void End()
    {
        drawline = false;
        callback = null;
    }
}
