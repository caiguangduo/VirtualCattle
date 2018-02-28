using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRCattle;

public class ToolsDeleteTags : EditorWindow
{
    private Transform tagParent;

    [MenuItem("Tools/DeleteTags")]
    static void CalledByDeleteTags()
    {
        VRCattleDataBase.ConnectToDataBase();

        ToolsDeleteTags instance = GetWindow<ToolsDeleteTags>();
        instance.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("标记物体的父节点");
        tagParent = (Transform)EditorGUILayout.ObjectField(tagParent, typeof(Transform), true);

        if (GUILayout.Button("清空该物体下的标签"))
        {
            List<Tag> targetList = new List<Tag>();
            targetList = VRCattleDataBase.GetTagByPidStatic(tagParent.parent.name+"\\"+tagParent.name);
            Debug.Log(targetList.Count);
            for(int i = 0; i < targetList.Count; i++)
            {
                Debug.Log(targetList[i].NameCN + "   " + targetList[i].PID);
            }
            VRCattleDataBase.DeleteTags(targetList);
        }
    }

    private void OnDestroy()
    {
        VRCattleDataBase.CloseConnection();
    }
}
