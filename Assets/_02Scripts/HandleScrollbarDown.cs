using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VRCattle;

public class HandleScrollbarDown : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        VRCattleUIManager.instance.isCanExecute = true;
    }
}
