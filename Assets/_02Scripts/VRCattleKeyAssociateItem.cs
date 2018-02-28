using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRCattle
{
    public class VRCattleKeyAssociateItem : MonoBehaviour
    {
        [SerializeField]
        private Text text;
        private Node _node;
        public Node node
        {
            get { return _node; }
            set
            {
                _node = value;
                text.text = value.Name_CN;
            }
        }
    }
}

