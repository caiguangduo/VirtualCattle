using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRCattle;

public class ToolsSaveTags : EditorWindow {

    private Transform tagParent;
    private Transform tag;

    private string id;
    private string tagname;
    private string nameCN="请输入中文名字";
    private string nameEN= "请输入英文名字";
    private float p_x;
    private float p_y;
    private float p_z;
    private float r_x;
    private float r_y;
    private float r_z;
    private float s_x;
    private float s_y;
    private float s_z;
    private string pid;

    [MenuItem("Tools/SaveTags")]
    static void CalledByBtSaveTags()
    {
        VRCattleDataBase.ConnectToDataBase();

        ToolsSaveTags instance = GetWindow<ToolsSaveTags>();
        instance.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("标记物体的父节点");
        tagParent = (Transform)EditorGUILayout.ObjectField(tagParent, typeof(Transform), true);
        GUILayout.Label("标记物体");
        tag = (Transform)EditorGUILayout.ObjectField(tag, typeof(Transform), true);

        nameCN = GUILayout.TextField(nameCN);
        nameEN = GUILayout.TextField(nameEN);

        if (GUILayout.Button("保存"))
        {
            Transform pp = tagParent.parent;
            id = pp.name + "\\" + tagParent.name+"\\"+tag.name;
            tagname = tag.name;
            p_x = tag.localPosition.x;
            p_y = tag.localPosition.y;
            p_z = tag.localPosition.z;
            Vector3 r = tag.localRotation.eulerAngles;
            r_x = r.x;
            r_y = r.y;
            r_z = r.z;
            s_x = tag.localScale.x;
            s_y = tag.localScale.y;
            s_z = tag.localScale.z;
            pid = pp.name + "\\" + tagParent.name;
            Debug.Log(id + "_" + tagname + "_" + nameCN + "_" + nameEN + "_" + p_x + "_" + p_y + "_" + p_z + "_" +r_x + "_" +r_y + "_" +r_z + "_" +s_x + "_" +s_y + "_" +s_z);
            Tag tagClass = new Tag(id,tagname, nameCN, nameEN, p_x, p_y, p_z,r_x,r_y,r_z,s_x,s_y,s_z,pid);

            List<Tag> allTags = VRCattleDataBase.GetAllTag();

            bool isUpdate = false;
            int count = allTags.Count;
            for(int i = 0; i < count; i++)
            {
                if (tagClass.ID == allTags[i].ID)
                {
                    isUpdate = true;
                    VRCattleDataBase.UpdateTag(tagClass);
                    Debug.Log("Update");
                    break;
                }
            }
            if (!isUpdate)
            {
                Debug.Log("Add");
                VRCattleDataBase.AddTag(tagClass);
            }
        }
    }
    private void OnDestroy()
    {
        VRCattleDataBase.CloseConnection();
    }
}
