using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRCattle
{
    public class VRCattleKeyBoardAssociate : MonoBehaviour
    {
        List<Node> list = new List<Node>();
        public Button btPrefab;
        List<Button> bts = new List<Button>();

        private void Start()
        {
            VRCattleKeyBoardManager.instance.OnInputChange += OnInputChanged;
        }
        void OnInputChanged(string str)
        {
            if (!gameObject.activeInHierarchy) return;
            if (!str.Equals(""))
            {
                if (VRCattleDataBase.instance)
                {
                    list = VRCattleDataBase.instance.GetNodeByAcronym(str);
                }
            }
            else
            {
                list.Clear();
            }
            UIRefresh();
        }
        void UIRefresh()
        {
            int count = list.Count > 6 ? 6 : list.Count;
            if (bts.Count != count)
            {
                if (bts.Count < count)
                {
                    int len = count - bts.Count;
                    for(int i = 0; i < len; i++)
                    {
                        CreateButton();
                    }
                }
                else
                {
                    int len = bts.Count - count;
                    for(int i = 0; i < len; i++)
                    {
                        DeleteButton();
                    }
                }
            }
            for(int i = 0; i < count; i++)
            {
                bts[i].GetComponent<VRCattleKeyAssociateItem>().node = list[i];
            }
        }
        void CreateButton()
        {
            Button bt = Instantiate(btPrefab) as Button;
            bt.transform.SetParent(transform);
            bt.transform.localPosition = Vector3.zero;
            bt.transform.localScale = Vector3.one;
            bt.transform.localRotation = Quaternion.identity;
            bt.GetComponent<EventTriggerListener>().onClick += OnButtonClicked;

            bts.Add(bt);
        }
        void DeleteButton()
        {
            Button bt = bts[bts.Count - 1];
            bts.RemoveAt(bts.Count - 1);
            DestroyImmediate(bt.gameObject);
        }
        void OnButtonClicked(GameObject go)
        {
            VRCattleKeyAssociateItem item = go.GetComponent<VRCattleKeyAssociateItem>();
            if (VRCattleManager.instance)
            {
                Transform t = item.node.ID.GetTransform();
                //VRCattleManager.instance.OnRayCast(t);
                VRCattleManager.instance.OnSelect(t,true);
            }
        }
    }
}
