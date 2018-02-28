using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawCircle : MonoBehaviour
{
    public Camera cam;
    public delegate void VoidDelegate();
    public Color32 paintColor=Color.blue;
    public Transform p;
    public int lineWidth=12;

    private bool drawCircle = false;
    private VoidDelegate callback;

    private Vector2 originalPos;
    private float limitX;
    private float limitY;
    private float texWidth;
    private float texHeight;
    private EllipseGraphic eg;

    private void Start()
    {
        texWidth = GetComponent<RectTransform>().sizeDelta.x;
        texHeight = GetComponent<RectTransform>().sizeDelta.y;
        limitX = lineWidth * 0.5f / texWidth;
        limitY = lineWidth * 0.5f / texHeight;
    }
    private void Update()
    {
        //判断一下鼠标是否悬浮在UI上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (drawCircle)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnBegan();
            }
            if (Input.GetMouseButton(0))
            {
                OnMove();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnEnd();
            }
        }
    }
    void OnBegan()
    {
        originalPos = cam.ScreenToViewportPoint(Input.mousePosition);
        originalPos.x = Mathf.Clamp(originalPos.x, limitX, 1 - limitX);
        originalPos.y = Mathf.Clamp(originalPos.y, limitY, 1 - limitY);
        if (eg == null)
        {
            eg = new GameObject().AddComponent<EllipseGraphic>();
            eg.transform.SetParent(p);
            eg.transform.localScale = Vector3.one;
            eg.transform.localRotation = Quaternion.identity;
            eg.transform.localPosition = Vector3.zero;
        }
        eg.SetPos(originalPos, originalPos, paintColor, texWidth, texHeight);
    }
    void OnMove()
    {
        Vector2 nowPos = cam.ScreenToViewportPoint(Input.mousePosition);
        nowPos.x = Mathf.Clamp(nowPos.x, limitX, 1 - limitX);
        nowPos.y = Mathf.Clamp(nowPos.y, limitY, 1 - limitY);
        eg.SetPos(originalPos, nowPos, paintColor, texWidth, texHeight);
    }
    void OnEnd()
    {
        Vector2 nowPos = cam.ScreenToViewportPoint(Input.mousePosition);
        nowPos.x = Mathf.Clamp(nowPos.x, limitX, 1 - limitX);
        nowPos.y = Mathf.Clamp(nowPos.y, limitY, 1 - limitY);
        eg.SetPos(originalPos, nowPos, paintColor, texWidth, texHeight);
        eg.gameObject.layer = 9;
        p.gameObject.layer = 9;
        if (callback != null)
        {
            callback();
        }
        Destroy(eg.gameObject);
    }

    public void Begin(Color color,VoidDelegate callback)
    {
        paintColor = color;
        drawCircle = true;
        this.callback = callback;
    }
    public void End()
    {
        drawCircle = false;
        callback = null;
    }
}
