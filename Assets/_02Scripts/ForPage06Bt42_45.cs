using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRCattle
{
    public class ForPage06Bt42_45 : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {
        public float moveSpeed = 0.025f;

        string btName;

        bool isNeedMoveLeft = false;
        bool isNeedMoveUp = false;
        bool isNeedMoveRight = false;
        bool isNeedMoveDown = false;

        private void Start()
        {
            btName = gameObject.name;
        }

        private void Update()
        {
            if (isNeedMoveLeft)
            {
                VRCattleCameraControll.instance.MoveCamera(moveSpeed, 0);
            }
            if (isNeedMoveUp)
            {
                VRCattleCameraControll.instance.MoveCamera(0, -moveSpeed);
            }
            if (isNeedMoveRight)
            {
                VRCattleCameraControll.instance.MoveCamera(-moveSpeed, 0);
            }
            if (isNeedMoveDown)
            {
                VRCattleCameraControll.instance.MoveCamera(0, moveSpeed);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (btName)
            {
                case "Bt42"://左
                    isNeedMoveLeft = true;
                    break;
                case "Bt43"://上
                    isNeedMoveUp = true;
                    break;
                case "Bt44"://右
                    isNeedMoveRight = true;
                    break;
                case "Bt45"://下
                    isNeedMoveDown = true;
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            switch (btName)
            {
                case "Bt42"://左
                    isNeedMoveLeft = false;
                    break;
                case "Bt43"://上
                    isNeedMoveUp = false;
                    break;
                case "Bt44"://右
                    isNeedMoveRight = false;
                    break;
                case "Bt45"://下
                    isNeedMoveDown = false;
                    break;
            }
        }
    }
}

