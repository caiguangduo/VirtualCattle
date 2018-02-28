using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCattle
{
    public class VRCattleBusinessLogic : MonoBehaviour
    {
        public static VRCattleBusinessLogic instance;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }

        public void Disable()
        {
            int count = VRCattleManager.selectedObjs.Count;
            if (count > 0)
            {
                GameObject[] objs = new GameObject[count];
                for(int i = 0; i < count; i++)
                {
                    Transform t = VRCattleManager.selectedObjs[i];
                    objs[i] = t.gameObject;
                    BlurOutlineCommandBuffer.Remove(t.GetComponent<Renderer>());
                }
                VRCattleManager.selectedObjs.Clear();
                VRCattleUndo.DisableObj(objs);
            }
        }

        public void DisableOther()
        {
            List<GameObject> list = new List<GameObject>();
            bool isNeed = false;
            int count = VRCattleManager.allObjs.Count;
            if (VRCattleManager.selectedObjs.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Transform t = VRCattleManager.allObjs[i];
                    if (!VRCattleManager.selectedObjs.Contains(t))
                    {
                        if (t.gameObject.activeSelf && !isNeed)
                            isNeed = true;
                        list.Add(t.gameObject);
                    }
                }
                if (isNeed)
                    VRCattleUndo.DisableObj(list.ToArray());
            }
        }

        void ResetDisable()
        {
            int count = VRCattleManager.disabledObjs.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    VRCattleManager.disabledObjs[i].gameObject.SetActive(true);
                }
                VRCattleManager.disabledObjs.Clear();
            }
        }

        public void Transparent()
        {
            List<MeshRenderer> list = new List<MeshRenderer>();
            bool isNeed = false;
            int count = VRCattleManager.selectedObjs.Count;
            if (count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    MeshRenderer mr = VRCattleManager.selectedObjs[i].GetComponent<MeshRenderer>();
                    if (mr.material.color.a != VRCattleManager.transparentValue)
                        isNeed = true;
                    list.Add(mr);
                }
                if (isNeed)
                {
                    VRCattleUndo.TransparentObj(list.ToArray());
                }
            }
        }

        public void TransparentOther()
        {
            List<MeshRenderer> list = new List<MeshRenderer>();
            bool isNeed = false;
            int count = VRCattleManager.allObjs.Count;
            if (VRCattleManager.selectedObjs.Count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    Transform t = VRCattleManager.allObjs[i];
                    if(!VRCattleManager.selectedObjs.Contains(t))
                    {
                        MeshRenderer mr = t.GetComponent<MeshRenderer>();
                        if (mr.material.color.a != VRCattleManager.transparentValue)
                            isNeed = true;
                        list.Add(mr);
                    }
                }
                if (isNeed)
                    VRCattleUndo.TransparentObj(list.ToArray());
            }
        }

        void ResetTransparent()
        {
            int count = VRCattleManager.transparentObjs.Count;
            if (count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    Renderer rendererTemp = VRCattleManager.transparentObjs[i].GetComponent<Renderer>();
                    Color colorTemp = rendererTemp.material.color;
                    colorTemp.a = 1;
                    rendererTemp.material.color = colorTemp;
                }
                VRCattleManager.transparentObjs.Clear();
            }
        }

        public void Undo()
        {
            VRCattleUndo.Pop();
        }

        public void ShowAll()
        {
            VRCattleManager.instance.OnSelectClear();
            ResetDisable();
            ResetTransparent();
            VRCattleUndo.Clear();
        }

        public void ResetAll()
        {
            VRCattleManager.instance.OnSelectClear();
            ResetDisable();
            ResetTransparent();
            VRCattleUndo.Clear();
        }

        Vector3 GetCenter()
        {
            List<Transform> list = new List<Transform>();
            int count = VRCattleManager.allObjs.Count;
            for (int i = 0; i < count; i++)
            {
                if (VRCattleManager.allObjs[i].gameObject.activeInHierarchy)
                {
                    list.Add(VRCattleManager.allObjs[i]);
                }
            }
            VRCattleManager.instance.LoadedActivedObjs = list;
            return VRCattleManager.instance.centerPos;
        }

        public static void LoadFromSave(string content,bool transparent = true,bool isNeedHandleCamera=true)
        {
            VRCattleManager.currentLoaded = content;
            VRCattleSaveXml saveXml = XmlTool.DeserializeObject<VRCattleSaveXml>(content);

            VRCattleDifferent.DisableObjsStatic(saveXml.isMale);

            int count = saveXml.disable.Count;
            for(int i = 0; i < count; i++)
            {
                string str = saveXml.disable[i];
                Transform t = GetTransformByNodeID(str);
                if (t == null)
                    continue;
                VRCattleManager.disabledObjs.Add(t);
                t.gameObject.SetActive(false);
            }

            VRCattleUIManager.instance.Page06SetBodyAndBt20_31State(saveXml.bodyStates);

            if (transparent)
            {
                count = saveXml.transparent.Count;
                for(int i = 0; i < count; i++)
                {
                    Transform t = GetTransformByNodeID(saveXml.transparent[i]);
                    if (t == null)
                        continue;
                    MeshRenderer mr = t.GetComponent<MeshRenderer>();
                    VRCattleManager.transparentObjs.Add(mr);
                    Color color = mr.material.color;
                    color.a = VRCattleManager.transparentValue;
                    mr.material.color = color;
                }
            }

            if (isNeedHandleCamera)
            {
                VRCattleManager.instance.ResetBodyTransform();
                Vector3 center = instance.GetCenter();

                VRCattleCameraControll.instance.isScrollbar = false;
                VRCattleCameraControll.instance.Distance = 3.5f;
                VRCattleCameraControll.instance.SetTargetPoint(center);
                //VRCattleCameraControll.instance.ChangeDistance();
                VRCattleCameraControll.instance.MoveToTargetPoint();

                VRCattleObjectControll.instance.SetTargetPoint(center);
            }
        }

        public static string GetNodeIDByTransform(Transform t,string baseString = "")
        {
            if (t.parent == null)
            {
                return baseString;
            }
            else
            {
                if (baseString == "")
                    baseString = t.name + baseString;
                else
                    baseString = t.name + "\\" + baseString;
                return GetNodeIDByTransform(t.parent, baseString);
            }
        }
        public static Transform GetTransformByNodeID(string nodeID)
        {
            string str = nodeID.Replace('\\', '/');
            return VRCattleManager.instance.root.transform.Find(str);
        }
    }

    public class VRCattleSaveXml
    {
        public List<string> disable = new List<string>();
        public List<string> transparent = new List<string>();
        public bool isMale = true;
        public bool[] bodyStates = new bool[13];
        public Quaternion cameraRot = Quaternion.Euler(0, 0, 0);
        public Vector3 cameraPos = Vector3.zero;

        public VRCattleSaveXml()
        {
            if (VRCattleUIManager.instance)
            {
                List<Transform> list = new List<Transform>();
                list = VRCattleManager.disabledObjs;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    this.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(list[i]));
                }

                List<MeshRenderer> temp = VRCattleManager.transparentObjs;
                count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    this.transparent.Add(VRCattleBusinessLogic.GetNodeIDByTransform(temp[i].transform));
                }

                this.bodyStates = VRCattleUIManager.instance.bodyStates;

                this.cameraPos = VRCattleManager.instance.mainCamera.position;
                this.cameraRot = VRCattleManager.instance.mainCamera.rotation;
            }
        }
        public VRCattleSaveXml(List<string> disable,bool[] bodyStates,Vector3 cameraPos,Quaternion cameraRot)
        {
            this.disable = disable;
            this.bodyStates = bodyStates;
            this.cameraPos = cameraPos;
            this.cameraRot = cameraRot;
        }
    }
}

