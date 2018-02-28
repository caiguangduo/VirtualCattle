using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCattle
{
    public class VRCattleObjectControll : MonoBehaviour
    {
        public static VRCattleObjectControll instance;

        [HideInInspector]
        public bool isCanControll = true;
        [HideInInspector]
        public Vector3 targetPoint = new Vector3(0, 0, 0);
        
        public float xRotateSpeed = 100;
        public float yRotateSpeed = 100;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }

        public void SetTargetPoint(Vector3 targetPoint)
        {
            this.targetPoint = targetPoint;
        }

        private void Update()
        {
            if (!isCanControll) return;
            if (VRCattleCameraControll.instance.isNeedLerpCamera) return;
            if (Input.GetMouseButton(0))
            {
                float xRotateAngle = Input.GetAxis("Mouse X") * xRotateSpeed * Time.deltaTime;
                float yRotateAngle = -Input.GetAxis("Mouse Y") * yRotateSpeed * Time.deltaTime;
                RotateAroundTargetPoint(xRotateAngle,yRotateAngle);
            }
        }

        public void RotateAroundTargetPoint(float xAngle,float yAngle)
        {
            transform.RotateAround(targetPoint, Vector3.up, -xAngle);
            transform.RotateAround(targetPoint, Vector3.right, -yAngle);
        }
    }
}

