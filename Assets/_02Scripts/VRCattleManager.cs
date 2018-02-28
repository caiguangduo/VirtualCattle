using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRCattle
{
    public class VRCattleManager : MonoBehaviour
    {

        public static VRCattleManager instance;

        public static Ray ray
        {
            get
            {
                return instance.mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            }
        }
        RaycastHit hit;

        public GameObject root;
        public GameObject beiPi;
        public GameObject jiRou;
        public GameObject guGe;
        public GameObject xiaoHua;
        public GameObject huXi;
        public GameObject xunHuan;
        public GameObject shenJing;
        public GameObject linBa;
        public GameObject miNiao;
        public GameObject shengZhi;
        public GameObject ganJue;
        public GameObject neiFenMi;

        public Transform mainCamera;

        public static List<Transform> allObjs=new List<Transform>();
        public static List<Transform> selectedObjs=new List<Transform>();
        public static List<Transform> disabledObjs=new List<Transform>();
        public static List<MeshRenderer> transparentObjs=new List<MeshRenderer>();
        public static float transparentValue = 0.4f;

        public static string currentLoaded;

        public AudioSource audioSourceUI;
        public AudioSource audioSourceSystem;
        private float volumeValue;
        public float VolumeValue
        {
            get { return volumeValue; }
            set
            {
                volumeValue = Mathf.Clamp01(value);
                audioSourceUI.volume = value;
                audioSourceSystem.volume = value;
            }
        }
        public AudioClip uiCoverClip;
        public AudioClip uiClickClip;

        public GameObject environment;
        public GameObject panelWhite;
        public GameObject panelBlack;

        [HideInInspector]
        public Vector3 centerPos;
        private List<Transform> loadedActivedObjs;
        public List<Transform> LoadedActivedObjs
        {
            get { return loadedActivedObjs; }
            set
            {
                loadedActivedObjs = value;
                centerPos = CalculateCenterPos(loadedActivedObjs);
            }
        }

        public bool isCanRayCastRay=true;

        public Transform tagPrefab;

        [HideInInspector]
        public Vector3 bodyOriginPos;
        [HideInInspector]
        public Quaternion bodyOriginRot;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;

            InitInAwake();
        }

        void InitInAwake()
        {
            bodyOriginPos = root.transform.position;
            bodyOriginRot = root.transform.rotation;
            allObjs = new List<Transform>();
            selectedObjs = new List<Transform>();
            disabledObjs = new List<Transform>();
            transparentObjs = new List<MeshRenderer>();
            Renderer[] r = root.GetComponentsInChildren<Renderer>();
            int length = r.Length;
            for(int i = 0; i < length; i++)
            {
                allObjs.Add(r[i].transform);
            }
        }

        public void ResetBodyTransform()
        {
            root.transform.position = bodyOriginPos;
            root.transform.rotation = bodyOriginRot;
        }

        public static void AddDisabledObj(Transform obj)
        {
            if (!disabledObjs.Contains(obj))
                disabledObjs.Add(obj);
        }
        public static void RemoveDisabledObj(Transform obj)
        {
            if (disabledObjs.Contains(obj))
                disabledObjs.Remove(obj);
        }
        public static void AddTransparentObj(MeshRenderer mr)
        {
            if (!transparentObjs.Contains(mr))
                transparentObjs.Add(mr);
        }
        public static void RemoveTransparentObj(MeshRenderer mr)
        {
            if (transparentObjs.Contains(mr))
                transparentObjs.Remove(mr);
        }

        public static bool PointOverUI
        {
            get
            {
                if (Input.mousePresent)
                    return EventSystem.current.IsPointerOverGameObject();
                else
                    return false;
            }
        }

        float t1 = 0, t2 = 0;
        string transTemp;
        private void Update()
        {
            if (isCanRayCastRay && Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 1 << 8))
                {
                    t2 = Time.realtimeSinceStartup;
                    if (t2 - t1 < 0.4f&&transTemp==hit.transform.name)
                    {
                        List<Transform> list = new List<Transform>();
                        list.Add(hit.transform);
                        Vector3 center = CalculateCenterPos(list);
                        VRCattleCameraControll.instance.isScrollbar = false;
                        VRCattleCameraControll.instance.Distance = 1.5f;
                        VRCattleCameraControll.instance.SetTargetPoint(center);
                        VRCattleCameraControll.instance.isNeedLerpCamera = true;
                        VRCattleObjectControll.instance.SetTargetPoint(center);
                    }
                    t1 = t2;
                    transTemp = hit.transform.name;
                    OnSelect(hit.transform, false);
                }
                else
                    OnSelect(null,false);
            }
        }
        public void OnSelect(Transform t,bool isKeyBoard)
        {
            if (t == null) return;
            if (selectedObjs.Contains(t))
            {
                if (!isKeyBoard)
                {
                    BlurOutlineCommandBuffer.Remove(t.GetComponent<Renderer>());
                    selectedObjs.Remove(t);
                    if (selectedObjs.Count == 0)
                        VRCattleUIManager.instance.page06LeftToolChildBts.SetActive(false);
                    ChangeInfoAndCursound(null);
                    if(VRCattleUIManager.instance.Page06Bt02State)
                        VRCattleUIManager.instance.RemoveTag(t);
                }
            }
            else
            {
                if (!VRCattleUIManager.instance.IsMulti && selectedObjs.Count > 0)
                    OnSelectClear();
                BlurOutlineCommandBuffer.Add(t.GetComponent<Renderer>());
                selectedObjs.Add(t);
                if (!VRCattleUIManager.instance.page06LeftToolChildBts.activeSelf)
                    VRCattleUIManager.instance.page06LeftToolChildBts.SetActive(true);
                Node node = VRCattleDataBase.instance.GetNodeByID(VRCattleBusinessLogic.GetNodeIDByTransform(t));
                ChangeInfoAndCursound(node);
                if(VRCattleUIManager.instance.Page06Bt02State)
                    VRCattleUIManager.instance.CreateTag(t);
            }
        }
        public void OnSelectClear()
        {
            int count = selectedObjs.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    BlurOutlineCommandBuffer.Remove(selectedObjs[i].GetComponent<Renderer>());
                }
                if (VRCattleUIManager.instance.Page06Bt02State)
                    VRCattleUIManager.instance.ClearTags();
                selectedObjs.Clear();
                if (VRCattleUIManager.instance.page06LeftToolChildBts.activeSelf)
                    VRCattleUIManager.instance.page06LeftToolChildBts.SetActive(false);
                ChangeInfoAndCursound(null);
            }
        }
        public void MultiModeChange(bool value)
        {
            if (!value && selectedObjs.Count > 1)
                OnSelectClear();
        }
        public void PlaySound(string path)
        {
            AudioClip clip = Resources.Load<AudioClip>("AudioClips/" + path);
            if (clip != null)
            {
                audioSourceSystem.clip = clip;
                audioSourceSystem.Play();
            }
            else
            {
                Debug.Log("AudioClips/" + path + " is not found");
            }
        }
        public void ChangeInformation(string nameCN,string nameEN,string description)
        {
            VRCattleUIManager.instance.page06NameCN.text = nameCN;
            VRCattleUIManager.instance.page06NameEN.text = nameEN;
            if (!string.IsNullOrEmpty(description))
                VRCattleUIManager.instance.page06Info.text = "<color=#577DBEFF><释></color><color=#F2F2F2FF>.</color>" + description;
            else
                VRCattleUIManager.instance.page06Info.text = description;
        }
        public void ChangeInfoAndCursound(Node node)
        {
            if (node != null)
            {
                VRCattleUIManager.instance.curSoundName = node.Sound;
                ChangeInformation(node.Name_CN, node.Name_EN, node.Description);
            }
            else
            {
                VRCattleUIManager.instance.curSoundName = "";
                ChangeInformation(null, null, null);
            }
        }
        Vector3 CalculateCenterPos(List<Transform> list)
        {
            Vector3 center = Vector3.zero;
            int count = list.Count;
            for(int i = 0; i < count; i++)
            {
                center += list[i].GetComponent<MeshRenderer>().bounds.center;
            }
            if (count > 0)
            {
                center = center / count;
                Bounds bounds = new Bounds(center, Vector3.zero);
                for(int i = 0; i < count; i++)
                {
                    bounds.Encapsulate(list[i].GetComponent<MeshRenderer>().bounds);
                }
                return bounds.center;
            }
            return root.transform.position;
        }

    }

    public static class ExtentionClass
    {
        public static Transform GetTransform(this string nodeID)
        {
            return VRCattleBusinessLogic.GetTransformByNodeID(nodeID);
        }
    }
}

