using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRCattle
{
    public class VRCattleKeyBoardManager : MonoBehaviour
    {
        public static VRCattleKeyBoardManager instance;
        public delegate void StringDelegate(string content);
        public StringDelegate OnInputChange;
        public StringDelegate CallBack;
        public Text inputText;
        private string tip = "Please Input Your Word...";
        public Button okBt;
        public Button cancelBt;
        public Button backspaceBt;
        public Transform keyParent;
        private bool isOK = false;

        public GameObject associateBg;
        //public GameObject associateGroup;

        private string content = "";
        public string Content
        {
            get { return content; }
            set
            {
                if (value.Length > 25)
                {
                    value = value.Substring(0, 25);
                }
                if (content != value)
                {
                    if (OnInputChange != null)
                        OnInputChange(value);
                }
                content = value;
                Color color = inputText.color;
                if (string.IsNullOrEmpty(value))
                {
                    inputText.text = tip;
                    color.a = 0.5f;
                    inputText.color = color;
                }
                else
                {
                    inputText.text = value;
                    color.a = 1.0f;
                    inputText.color = color;
                }
            }
        }

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
            Init();
        }
        private void Init()
        {
            Content = "";
            okBt.onClick.AddListener(OnOK);
            cancelBt.onClick.AddListener(OnCancel);
            backspaceBt.onClick.AddListener(OnBackSpace);
            int countChild = keyParent.childCount;
            for(int i = 0; i < countChild; i++)
            {
                keyParent.GetChild(i).GetComponent<EventTriggerListener>().onClick += OnKeyClicked;
            }
        }

        private void OnGUI()
        {
            if (Input.anyKeyDown)
            {
                Event e = Event.current;
                if (e.isKey)
                {
                    if (e.keyCode == KeyCode.None) return;
                    if (e.keyCode == KeyCode.Backspace)
                    {
                        OnBackSpace();
                        return;
                    }
                    int value = (int)e.keyCode;
                    if (value >= 97 && value <= 122)
                    {
                        char c = (char)(e.keyCode - KeyCode.A + 'a');
                        Content += new string(c, 1);
                        return;
                    }
                    if (value >= 48 && value <= 57)
                    {
                        string temp1 = (value - 48).ToString();
                        Content += temp1;
                        return;
                    }
                    if (value >= 256 && value <= 265)
                    {
                        string temp2 = (value - 256).ToString();
                        Content += temp2;
                        return;
                    }
                }
            }
        }

        void OnOK()
        {
            isOK = true;
            Close();
        }
        public void OnCancel()
        {
            isOK = false;
            Close();
        }
        public void Close()
        {
            if (CallBack != null)
            {
                if (isOK)
                    CallBack(Content);
                else
                    CallBack(null);
                CallBack = null;
            }
            Content = "";
            isOK = false;

            gameObject.SetActive(false);
        }
        public void Call(StringDelegate callback,string tip,bool associate = false)
        {
            gameObject.SetActive(true);
            if (isOK)
                isOK = false;
            if (this.CallBack != null)
                this.CallBack = null;
            if (Content != null)
                Content = "";
            this.CallBack = callback;
            associateBg.SetActive(associate);
            //associateGroup.SetActive(associate);
            if (!string.IsNullOrEmpty(tip))
            {
                this.tip = tip;
                inputText.text = tip;
            }
        }
        void OnBackSpace()
        {
            if (Content.Length > 0)
            {
                Content = Content.Substring(0, Content.Length - 1);
            }
        }
        void OnKeyClicked(GameObject go)
        {
            VRCattleKeyItem item = go.GetComponent<VRCattleKeyItem>();
            int keyValue = (int)item.key;
            if (keyValue >= 48 && keyValue <= 57)
            {
                string temp = (keyValue - 48).ToString();
                Content += temp;
            }
            else
            {
                char c = (char)(item.key - KeyCode.A + 'a');
                Content += new string(c, 1);
            }
        }
    }
}

