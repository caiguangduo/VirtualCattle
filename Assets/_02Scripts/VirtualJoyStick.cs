using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float maxDis = 80;
    Vector3 originPos;
    Vector3 lastInputPos = Vector3.zero;
    Vector3 virtualInputPos = Vector3.zero;

    bool isCanRotateCattle = false;
    public float rotateFactor = 0.01f;

    private void Start()
    {
        originPos = Vector3.zero ;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastInputPos = Input.mousePosition;
        virtualInputPos = Vector3.zero;
        VRCattle.VRCattleObjectControll.instance.isCanControll = false;
        isCanRotateCattle = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        virtualInputPos += (Input.mousePosition - lastInputPos);
        if (Vector3.Distance(originPos, virtualInputPos) <= maxDis)
        {
            transform.localPosition = virtualInputPos;
        }
        else
        {
            Vector3 moveDirection = virtualInputPos - originPos;
            transform.localPosition = originPos + moveDirection.normalized * maxDis;
        }
        lastInputPos = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = originPos;
        isCanRotateCattle = false;
        VRCattle.VRCattleObjectControll.instance.isCanControll = true;
    }

    private void Update()
    {
        if (isCanRotateCattle)
        {
            VRCattle.VRCattleObjectControll.instance.RotateAroundTargetPoint(transform.localPosition.x*rotateFactor,-transform.localPosition.y*rotateFactor);
        }
    }
}
