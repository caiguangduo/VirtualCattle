using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRCattle;

namespace VRCattle
{
    public class VRCattleMoveCamera : MonoBehaviour
    {
        public static VRCattleMoveCamera instance;
        public bool canControll = true;

        public Transform targetTrans;
        public Vector3 targetPos;
        public enum Mode
        {
            Point, Transform
        }
        public Mode mode = Mode.Transform;

        public float yMinLimit = -90, yMaxLimit = 90;
        public float xRotSpeed = 200, yRotSpeed = 200;
        public float mYSpeed = 20;
        public float mDisSpeed = 200;
        public float dis = 10, minDis = 2, maxDis = 15;
        public float x = 0, y = 0;
        private bool flag = false;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }
        public void SetTarget(Vector3 targetPos)
        {
            mode = Mode.Point;
            this.targetPos = targetPos;
        }
        public void SetTarget(Transform targetTrans)
        {
            mode = Mode.Transform;
            this.targetTrans = targetTrans;
        }
        public void SetPosRot(Vector3 pos,Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
        private void Update()
        {
            if (!canControll) return;
            if (targetTrans && !targetTrans.gameObject.activeInHierarchy)
                return;
            if (Input.GetMouseButtonUp(0))
                flag = false;
            if (VRCattleManager.PointOverUI && !flag)
                return;
            if (Input.GetMouseButtonDown(0))
                flag = true;
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xRotSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * yRotSpeed * Time.deltaTime;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
            if(!flag && (Input.GetAxis("Mouse ScrollWheel") != 0))
            {
                float axis = Input.GetAxis("Mouse ScrollWheel") * mDisSpeed * Time.deltaTime;
                dis -= axis;
                //dis = Mathf.Clamp(dis, minDis, maxDis);
            }
            if (!flag && Input.GetMouseButton(2))
            {
                this.targetPos += new Vector3(-Input.GetAxis("Mouse X") * mYSpeed * Time.deltaTime, -Input.GetAxis("Mouse Y") * mYSpeed * Time.deltaTime, 0f);
                //this.targetPos += new Vector3(0.0f, -Input.GetAxis("Mouse Y") * mYSpeed * Time.deltaTime, 0f);
            }
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(y, x, 0.0f);
            dis = Mathf.Clamp(dis, minDis, maxDis);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -dis);
            if (mode == Mode.Transform)
                transform.position = transform.rotation * disVector + targetTrans.position;
            else
            {
                transform.position = transform.rotation * disVector + targetPos;
            }

        }

        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}

