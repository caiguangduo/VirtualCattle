using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawArrow : MonoBehaviour
{
    public Camera cam;
    public delegate void VoidDelegate();
    public Color32 paintColor = Color.blue;
    public Transform p;
    public int lineWidth=12;

    private bool drawArrow = false;
    private VoidDelegate callback;

    private Vector2 originalPos;
    private float limitX;
    private float limitY;
    private float texWidth;
    private float texHeight;
    public enum Type
    {
        QQ,
        None
    }
    [SerializeField]
    private Type type = Type.None;
    private ArrowGraphic ag;

    private void Start()
    {
        texWidth = GetComponent<RectTransform>().sizeDelta.x;
        texHeight = GetComponent<RectTransform>().sizeDelta.y;
        limitX = lineWidth * 1.0f / texWidth;
        limitY = lineWidth * 1.0f / texHeight;
    }
    void Update()
    {
        //判断一下鼠标是否悬浮在UI上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (drawArrow)
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
        if (ag == null)
        {
            ag = new GameObject().AddComponent<ArrowGraphic>();
            ag.transform.SetParent(p);
            ag.transform.localScale = Vector3.one;
            ag.transform.localPosition = Vector3.zero;
            ag.transform.localRotation = Quaternion.identity;
            ag.texWidth = texWidth;
        }
        ag.SetPos(originalPos, originalPos, paintColor, type, texWidth, texHeight);
    }
    void OnMove()
    {
        Vector2 nowPos = cam.ScreenToViewportPoint(Input.mousePosition);
        nowPos.x = Mathf.Clamp(nowPos.x, limitX, 1 - limitX);
        nowPos.y = Mathf.Clamp(nowPos.y, limitY, 1 - limitY);
        ag.SetPos(originalPos, nowPos, paintColor, type, texWidth, texHeight);
    }
    void OnEnd()
    {
        Vector2 nowPos = cam.ScreenToViewportPoint(Input.mousePosition);
        nowPos.x = Mathf.Clamp(nowPos.x, limitX, 1 - limitX);
        nowPos.y = Mathf.Clamp(nowPos.y, limitY, 1 - limitY);
        ag.SetPos(originalPos, nowPos, paintColor, type, texWidth, texHeight);
        ag.gameObject.layer = 9;
        p.gameObject.layer = 9;
        if (callback != null)
        {
            callback();
        }
        Destroy(ag.gameObject);
    }

    public void Begin( Color color,VoidDelegate callback)
    {
        paintColor = color;
        this.callback = callback;
        drawArrow = true;
    }
    public void End()
    {
        drawArrow = false;
        callback = null;
    }
}
