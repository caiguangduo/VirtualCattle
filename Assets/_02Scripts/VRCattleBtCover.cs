using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace VRCattle
{
    public class VRCattleBtCover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float scaleFactor = 1.2f;
        public float scaleTime = 0.15f;
        bool isScaled = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(scaleFactor, scaleTime);
            isScaled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1, scaleTime);
            isScaled = false;
        }

        private void OnDisable()
        {
            UndoScale();
        }

        public void UndoScale()
        {
            if (isScaled)
                OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }
}

