using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCattle
{
    public class VRCattleCameraControll : MonoBehaviour
    {
        public static VRCattleCameraControll instance;

        [HideInInspector]
        public bool isCanControll = true;

        private Vector3 targetPoint = new Vector3(0, 0, 0);
        public Vector3 TargetPoint
        {
            get { return targetPoint; }
            set
            {
                GameObject temp = new GameObject();
                temp.transform.localPosition = value;
                targetPoint = temp.transform.position;
                //Vector3 targetPointTemp = temp.transform.position;
                //targetPoint = new Vector3(targetPointTemp.x, targetPointTemp.y, targetPointTemp.z - Distance);
                tempTargetPoint = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z - Distance);
                Destroy(temp);
            }
        }
        private Vector3 tempTargetPoint;

        [SerializeField]
        private float distance = 10;
        public float Distance
        {
            get { return distance; }
            set
            {
                distance = Mathf.Clamp(value, minDistance, maxDistance);
                if (!isScrollbar)
                {
                    Debug.Log("002");
                    VRCattleUIManager.instance.isCanExecute = false;
                    VRCattleUIManager.instance.page06Scrollbar.value = 1 - ((distance-minDistance) / (maxDistance - minDistance));
                }
            }
        }
        [HideInInspector]
        public bool isScrollbar = false;

        public float minDistance = 1;
        public float maxDistance = 11;
        public float disChangeSpeed = 100;

        [HideInInspector]
        public bool isNeedLerpCamera = false;

        public float xAxisMoveSpeed = 5;
        public float yAxisMoveSpeed = 5;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }
        public void SetTargetPoint(Vector3 targetPoint)
        {
            TargetPoint = targetPoint;
        }
        private void Update()
        {
            if (!isCanControll) return;
            if (!isNeedLerpCamera && Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float disDelta = Input.GetAxis("Mouse ScrollWheel") * disChangeSpeed * Time.deltaTime;
                if (isScrollbar)
                    isScrollbar = false;
                Distance -= disDelta;
                ChangeDistance();
            }
            if (isNeedLerpCamera)
            {
                ChangeDistanceLerp();
            }
            if (!isNeedLerpCamera && Input.GetMouseButton(2))
            {
                float xMoveDistance = -Input.GetAxis("Mouse X") * xAxisMoveSpeed * Time.deltaTime;
                float yMoveDistance = -Input.GetAxis("Mouse Y") * yAxisMoveSpeed * Time.deltaTime;
                MoveCamera(xMoveDistance, yMoveDistance);
            }
        }
        public void ChangeDistance()
        {
            //transform.position = new Vector3(TargetPoint.x,TargetPoint.y, TargetPoint.z-Distance);
            transform.position = new Vector3(transform.position.x, transform.position.y, TargetPoint.z - Distance);
        }

        public void MoveToTargetPoint()
        {
            transform.position = tempTargetPoint;
        }
        public void ChangeDistanceLerp()
        {
            transform.position = Vector3.Lerp(transform.position, tempTargetPoint, Time.deltaTime *1.5f);
            if (Vector3.Distance(transform.position, tempTargetPoint) < 0.005f)
            {
                transform.position = tempTargetPoint;
                isNeedLerpCamera = false;
            }
        }

        public void MoveCamera(float x, float y)
        {
            transform.Translate(new Vector3(x, y, 0), Space.World);
        }
    }
}

