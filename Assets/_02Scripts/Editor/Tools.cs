using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class Tools : EditorWindow
{
    [MenuItem("Tools/AddMeshCollider")]
    static void AddMeshCollider()
    {
        Transform t = Selection.activeTransform;
        if (t != null)
        {
            MeshRenderer[] mr = t.GetComponentsInChildren<MeshRenderer>();
            int length = mr.Length;
            for(int i = 0; i < length; i++)
            {
                if (mr[i].GetComponent<MeshCollider>() == null)
                    mr[i].gameObject.AddComponent<MeshCollider>();
            }
        }
        else
        {
            Debug.Log("请选择一个物体");
        }
    }

    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }
    [MenuItem("Tools/ChangeMaterialFade")]
    static void ChangeMaterialFade()
    {
        Transform t = Selection.activeTransform;
        if (t != null)
        {
            MeshRenderer[] mr = t.GetComponentsInChildren<MeshRenderer>();
            int length = mr.Length;
            for(int i = 0; i < length; i++)
            {
                int lengthJ = mr[i].sharedMaterials.Length;
                for(int j = 0; j < lengthJ; j++)
                {
                    SetMaterialRenderingMode(mr[i].sharedMaterials[j], RenderingMode.Fade);
                }
            }
        }
        else
        {
            Debug.Log("请选择一个物体");
        }
    }
    [MenuItem("Tools/ChangeMaterialOpaque")]
    static void ChangeMaterialOpaque()
    {
        Transform t = Selection.activeTransform;
        if (t != null)
        {
            MeshRenderer[] mr = t.GetComponentsInChildren<MeshRenderer>();
            int lengthI = mr.Length;
            for(int i = 0; i < lengthI; i++)
            {
                int lengthJ = mr[i].sharedMaterials.Length;
                for(int j = 0; j < lengthJ; j++)
                {
                    SetMaterialRenderingMode(mr[i].sharedMaterials[j], RenderingMode.Opaque);
                }
            }
        }
        else
        {
            Debug.Log("请选择一个物体");
        }
    }
    private static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        material.SetFloat("_Mode", (float)renderingMode);
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case RenderingMode.Fade:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case RenderingMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }

    [MenuItem("Tools/SetKeyboardText")]
    static void SetKeyboardText()
    {
        Transform t = Selection.activeTransform;
        int count = t.childCount;
        for(int i = 0; i < count; i++)
        {
            Transform child = t.GetChild(i);
            string str = child.name.Split('_')[1];
            child.GetComponent<VRCattle.VRCattleKeyItem>().key = (KeyCode)System.Enum.Parse(typeof(KeyCode), str);
            if (str.StartsWith("Alpha"))
                str = str.Replace("Alpha", "");
            Text text = child.GetChild(0).GetComponent<Text>();
            text.text = str.ToLower();
            text.fontSize = 80;
        }
    }
}

public class SavePanel : EditorWindow
{
    private Transform root;
    [MenuItem("Tools/SavePanel")]
    static void CalledByBtSavePanel()
    {
        SavePanel instance = GetWindow<SavePanel>();
        instance.Show();
    }

    private void OnGUI()
    {
        root = (Transform)EditorGUILayout.ObjectField(root, typeof(Transform), true);

        if (GUILayout.Button("重置"))
        {
            int count = root.childCount;
            for(int i = 0; i < count; i++)
            {
                root.GetChild(i).gameObject.SetActive(true);
            }
            MeshRenderer[] mr = root.GetComponentsInChildren<MeshRenderer>(true);
            count = mr.Length;
            for (int i = 0; i < count; i++)
            {
                mr[i].gameObject.SetActive(true);
            }
        }

        if (GUILayout.Button("保存"))
        {
            if (root == null) return;
            string path = EditorUtility.SaveFilePanel("保存系统存档", Application.dataPath + "/Resources/Tupu", "存档", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                int count = root.childCount;
                bool[] bodySystemStates = new bool[count + 1];
                bodySystemStates[count] = true;
                for(int i = 0; i < count; i++)
                {
                    bodySystemStates[i] = root.GetChild(i).gameObject.activeSelf;
                }
                Transform cameraTrans = SceneView.lastActiveSceneView.camera.transform;
                Vector3 euler = cameraTrans.rotation.eulerAngles;
                euler.x = 0;
                euler.z = 0;
                VRCattle.VRCattleSaveXml xml = new VRCattle.VRCattleSaveXml(GetDisableArray(), bodySystemStates, cameraTrans.position, Quaternion.Euler(euler));
                File.WriteAllText(path, XmlTool.SerializeObject<VRCattle.VRCattleSaveXml>(xml, true));
                AssetDatabase.Refresh();
            }
        }
    }
    private List<string> GetDisableArray()
    {
        List<string> disables = new List<string>();
        MeshRenderer[] mr = root.GetComponentsInChildren<MeshRenderer>(true);
        int length = mr.Length;
        for (int i = 0; i < length; i++)
        {
            if (!mr[i].gameObject.activeSelf)
                disables.Add(VRCattle.VRCattleBusinessLogic.GetNodeIDByTransform(mr[i].transform));
        }
        return disables;
    }

    [MenuItem("Tools/AddMaleFemaleToTupu")]
    static void AddMaleFemaleToTupu()
    {
        string path = Application.dataPath + "/Resources/Tupu/";
        string malePath = path + "Male/";
        string femalePath = path + "Female/";

        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] infos = directory.GetFiles();
        VRCattle.VRCattleDifferent diff = FindObjectOfType<VRCattle.VRCattleDifferent>();
        int length = infos.Length;
        for (int i = 0; i < length; i++)
        {
            if (infos[i].Extension == ".meta")
                continue;
            StreamReader sr = infos[i].OpenText();
            string content = sr.ReadToEnd();
            sr.Dispose();
            sr.Close();
            VRCattle.VRCattleSaveXml male = XmlTool.DeserializeObject<VRCattle.VRCattleSaveXml>(content);
            VRCattle.VRCattleSaveXml female = XmlTool.DeserializeObject<VRCattle.VRCattleSaveXml>(content);
            male.isMale = true;
            female.isMale = false;
            diff.AddToVSX(ref male, true);
            diff.AddToVSX(ref female, false);

            if (!Directory.Exists(malePath))
            {
                Directory.CreateDirectory(malePath);
            }
            if (!Directory.Exists(femalePath))
            {
                Directory.CreateDirectory(femalePath);
            }
            File.WriteAllText(malePath + infos[i].Name, XmlTool.SerializeObject<VRCattle.VRCattleSaveXml>(male));
            File.WriteAllText(femalePath + infos[i].Name, XmlTool.SerializeObject<VRCattle.VRCattleSaveXml>(female));
        }
        AssetDatabase.Refresh();
    }
}

