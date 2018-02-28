using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace VRCattle
{
    partial class VRCattleUIManager : MonoBehaviour
    {
        public static VRCattleUIManager instance;

        public bool[] bodyStates=new bool[13];

        public GameObject page01;//首页
        public GameObject page02;//存档加载
        public GameObject page03;//图谱认知
        public GameObject page04;//教程帮助
        public GameObject page05;//系统设置
        public GameObject page06;//场景UI页面

        #region Page01
        public Button page01Bt01;
        public Button page01Bt02;
        public Button page01Bt03;
        public Button page01Bt04;
        #endregion

        #region Page02
        public string Page02SaveBtPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VRCattle\\";
            }
        }
        VRCattleForPage02GridBt page02CurrentItem;
        public Button page02Bt01;
        public Button page02Bt02;
        public Button page02Bt03;
        public Button page02Bt04;
        public Button page02Bt05;
        public Button page02Bt06;
        public GameObject page02BtPrefab;
        public Transform page02Grid;
        Vector3 page02GridOriginPos = new Vector3(0, 735, 0);
        Vector3 page02GridMoveDis = new Vector3(0, 1470, 0);
        #endregion

        #region Page03
        public enum Sex
        {
            Male,
            Female
        }
        public Sex sex = Sex.Male;
        public delegate void OnSexChange();
        public OnSexChange onSexChange;
        public Button page03Bt01;
        public Button page03Bt02;
        public Button page03Bt03;
        public Button page03Bt04;
        public Button page03Bt05;
        public Button page03Bt06;
        public Button page03Grid13Bt01;
        public Button page03Grid13Bt02;
        public Button page03Grid13Bt03;
        public Button page03Grid13Bt04;
        public Button page03Grid13Bt05;
        public Button page03Grid13Bt06;
        public Button page03Grid13Bt07;
        public Button page03Grid13Bt08;
        public Button page03Grid13Bt09;
        public Button page03Grid13Bt10;
        public Button page03Grid13Bt11;
        public Button page03Grid13Bt12;
        private Button page03Grid13TempBt;
        public GameObject page03Grid01;
        public GameObject page03Grid02;
        public GameObject page03Grid03;
        public GameObject page03Grid04;
        public GameObject page03Grid05;
        public GameObject page03Grid06;
        public GameObject page03Grid07;
        public GameObject page03Grid08;
        public GameObject page03Grid09;
        public GameObject page03Grid10;
        public GameObject page03Grid11;
        public GameObject page03Grid12;
        private GameObject page03TempGrid;
        Vector3 page03GridOriginPos = new Vector3(0, 735, 0);
        Vector3 page03GridMoveDis = new Vector3(0, 1470, 0);
        #endregion

        #region Page04
        public Button page04Bt01;
        public Button page04Bt02;
        public Button page04Bt03;
        public Button page04Bt04;
        public Button page04Bt05;
        private Button page04CurBt;
        public Sprite page04SpriteJBCZ;
        public Sprite page04SpriteCJCZ;
        public Sprite page04SpriteSS;
        public Sprite page04SpriteBJ;
        public Image page04ImageCenter;
        #endregion

        #region Page05
        public Button page05Bt01;
        public Button page05Bt02;
        public GameObject page05Bt02Draw;
        public Button page05Bt03;
        public GameObject page05Bt03Draw;
        public Button page05Bt04;
        public GameObject page05Bt04Draw;
        public Button page05Bt05;
        public Button page05Bt06;
        public Scrollbar page05ScrollbarVolume;
        public Color page05ColorWhite = Color.white;
        public Color page05ColorBlack = Color.black;
        public enum Page05BgColorEnum
        {
            white,
            black,
            threeD
        }
        public Page05BgColorEnum page05BgColor = Page05BgColorEnum.white;
        #endregion

        #region Page06
        public Button page06Bt01;
        public Button page06Bt02;
        public Button page06Bt03;
        public Button page06Bt04;
        public Button page06Bt05;
        public Button page06Bt06;
        public Button page06Bt07;
        public Button page06Bt08;
        public Button page06Bt09;
        public Button page06Bt10;
        public Button page06Bt11;
        public Button page06Bt12;
        public Button page06Bt13;
        public Button page06Bt14;
        public Button page06Bt15;
        public Button page06Bt16;
        public Button page06Bt17;
        public Button page06Bt18;
        public Button page06Bt19;
        public Button page06Bt20;
        public Button page06Bt21;
        public Button page06Bt22;
        public Button page06Bt23;
        public Button page06Bt24;
        public Button page06Bt25;
        public Button page06Bt26;
        public Button page06Bt27;
        public Button page06Bt28;
        public Button page06Bt29;
        public Button page06Bt30;
        public Button page06Bt31;
        public Button page06Bt32;
        public Button page06Bt33;
        public Button page06Bt34;
        public Button page06Bt35;
        public Button page06Bt36;
        public Button page06Bt37;
        public Button page06Bt38;
        public Button page06Bt39;
        public Button page06Bt40;
        public Button page06Bt41;
        public Button page06Bt42;
        public Button page06Bt43;
        public Button page06Bt44;
        public Button page06Bt45;
        public Text page06NameCN;
        public Text page06NameEN;
        public Text page06Info;
        private bool isMulti = false;
        Toggle.ToggleEvent OnMultiModeChange = new Toggle.ToggleEvent();
        public bool IsMulti
        {
            get { return isMulti; }
            set
            {
                if (isMulti != value)
                {
                    isMulti = value;
                    if (OnMultiModeChange != null)
                        OnMultiModeChange.Invoke(value);
                }
            }
        }
        [HideInInspector]
        public string curSoundName;
        public Sprite page06Bt03SpriteN;
        public Sprite page06Bt03SpriteNA;
        public Sprite page06Bt03SpriteS;
        public Sprite page06Bt03SpriteSA;
        public GameObject page06ImageInfo;
        private bool page06Bt03State = false;
        public bool Page06Bt03State
        {
            get { return page06Bt03State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt03.GetComponent<Image>().sprite = page06Bt03SpriteS;
                    temp.pressedSprite = page06Bt03SpriteSA;
                }
                else
                {
                    page06Bt03.GetComponent<Image>().sprite = page06Bt03SpriteN;
                    temp.pressedSprite = page06Bt03SpriteNA;
                }
                page06Bt03.spriteState = temp;
                page06Bt03State = value;
                page06ImageInfo.SetActive(value);
            }
        }
        public Sprite page06Bt05SpriteN;
        public Sprite page06Bt05SpriteNA;
        public Sprite page06Bt05SpriteS;
        public Sprite page06Bt05SpriteSA;
        public GameObject page06ImageBiaoJiBts;
        public GameObject page06RawImageBiaoJi;
        private bool page06Bt05State = false;
        public bool Page06Bt05State
        {
            get { return page06Bt05State; }
            set
            {
                page06Bt05State = value;
                page06ImageBiaoJiBts.SetActive(value);
                page06RawImageBiaoJi.SetActive(value);
                VRCattleCameraControll.instance.isCanControll = !value;
                VRCattleObjectControll.instance.isCanControll = !value;
                VRCattleManager.instance.isCanRayCastRay = !value;
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt05.GetComponent<Image>().sprite = page06Bt05SpriteS;
                    temp.pressedSprite = page06Bt05SpriteSA;
                    Page06Bt08State = false;
                    Page06Bt03State = false;
                    VRCattleKeyBoardManager.instance.OnCancel();
                }
                else
                {
                    page06Bt05.GetComponent<Image>().sprite = page06Bt05SpriteN;
                    temp.pressedSprite = page06Bt05SpriteNA;
                    Page06Bt34State = -1;
                    Page06Bt35State = -1;
                    Page06Bt36State = -1;
                    page06Bt34ForDrawG.colorstate = VRCattleForDrawGraphicBts.ColorState.Blue;
                    page06Bt34ForDrawG.IsSelected = false;
                    page06Bt35ForDrawG.colorstate = VRCattleForDrawGraphicBts.ColorState.Blue;
                    page06Bt35ForDrawG.IsSelected = false;
                    page06Bt36ForDrawG.colorstate = VRCattleForDrawGraphicBts.ColorState.Blue;
                    page06Bt36ForDrawG.IsSelected = false;
                }
                page06Bt05.spriteState = temp;
            }
        }
        public Sprite page06Bt06SpriteN;
        public Sprite page06Bt06SpriteNA;
        public Sprite page06Bt06SpriteS;
        public Sprite page06Bt06SpriteSA;
        private bool page06Bt06State = false;
        public bool Page06Bt06State
        {
            get { return page06Bt06State; }
            set
            {
                if (value)
                {
                    if (Page06Bt07State)
                        Page06Bt07State = false;
                    page06Bt06.GetComponent<Image>().sprite = page06Bt06SpriteS;
                    SpriteState temp = new SpriteState();
                    temp.pressedSprite = page06Bt06SpriteSA;
                    page06Bt06.spriteState = temp;
                    VRCattleKeyBoardManager.instance.Call(ChaXunCallBack, "请输入查询首字母", true);
                }
                else
                {
                    //page06Bt06.GetComponent<Image>().sprite = page06Bt06SpriteN;
                    //temp.pressedSprite = page06Bt06SpriteNA;
                    VRCattleKeyBoardManager.instance.OnCancel();
                }
                page06Bt06State = value;
            }
        }
        public Sprite page06Bt07SpriteN;
        public Sprite page06Bt07SpriteNA;
        public Sprite page06Bt07SpriteS;
        public Sprite page06Bt07SpriteSA;
        private bool page06Bt07State = false;
        public bool Page06Bt07State
        {
            get { return page06Bt07State; }
            set
            {
                if (value)
                {
                    if (Page06Bt06State)
                        Page06Bt06State = false;
                    page06Bt07.GetComponent<Image>().sprite = page06Bt07SpriteS;
                    SpriteState temp = new SpriteState();
                    temp.pressedSprite = page06Bt07SpriteSA;
                    page06Bt07.spriteState = temp;
                    VRCattleKeyBoardManager.instance.Call(BaoCunCallBack, "请输入存档名称", false);
                }
                else
                {
                    //page06Bt07.GetComponent<Image>().sprite = page06Bt07SpriteN;
                    //temp.pressedSprite = page06Bt07SpriteNA;
                    VRCattleKeyBoardManager.instance.OnCancel();
                }
                page06Bt07State = value;
            }
        }
        public Sprite page06Bt08SpriteN;
        public Sprite page06Bt08SpriteNA;
        public Sprite page06Bt08SpriteS;
        public Sprite page06Bt08SpriteSA;
        public GameObject page06ChildBtList;
        private bool page06Bt08State = false;
        public bool Page06Bt08State
        {
            get { return page06Bt08State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt08.GetComponent<Image>().sprite = page06Bt08SpriteS;
                    temp.pressedSprite = page06Bt08SpriteSA;
                }
                else
                {
                    page06Bt08.GetComponent<Image>().sprite = page06Bt08SpriteN;
                    temp.pressedSprite = page06Bt08SpriteNA;
                }
                page06Bt08.spriteState = temp;
                page06Bt08State = value;
                page06ChildBtList.SetActive(value);
            }
        }
        public Sprite page06Bt20SpriteN;
        public Sprite page06Bt20SpriteNA;
        public Sprite page06Bt20SpriteS;
        public Sprite page06Bt20SpriteSA;
        private bool page06Bt20State = false;
        public bool Page06Bt20State
        {
            get { return page06Bt20State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt20.GetComponent<Image>().sprite = page06Bt20SpriteS;
                    temp.pressedSprite = page06Bt20SpriteSA;
                }
                else
                {
                    page06Bt20.GetComponent<Image>().sprite = page06Bt20SpriteN;
                    temp.pressedSprite = page06Bt20SpriteNA;
                }
                page06Bt20.spriteState = temp;
                page06Bt20State = value;
                VRCattleManager.instance.beiPi.SetActive(value);
                bodyStates[0] = value;
            }
        }
        public Sprite page06Bt21SpriteN;
        public Sprite page06Bt21SpriteNA;
        public Sprite page06Bt21SpriteS;
        public Sprite page06Bt21SpriteSA;
        private bool page06Bt21State = false;
        public bool Page06Bt21State
        {
            get { return page06Bt21State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt21.GetComponent<Image>().sprite = page06Bt21SpriteS;
                    temp.pressedSprite = page06Bt21SpriteSA;
                }
                else
                {
                    page06Bt21.GetComponent<Image>().sprite = page06Bt21SpriteN;
                    temp.pressedSprite = page06Bt21SpriteNA;
                }
                page06Bt21.spriteState = temp;
                page06Bt21State = value;
                VRCattleManager.instance.jiRou.SetActive(value);
                bodyStates[1] = value;
            }
        }
        public Sprite page06Bt22SpriteN;
        public Sprite page06Bt22SpriteNA;
        public Sprite page06Bt22SpriteS;
        public Sprite page06Bt22SpriteSA;
        private bool page06Bt22State = false;
        public bool Page06Bt22State
        {
            get { return page06Bt22State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt22.GetComponent<Image>().sprite = page06Bt22SpriteS;
                    temp.pressedSprite = page06Bt22SpriteSA;
                }
                else
                {
                    page06Bt22.GetComponent<Image>().sprite = page06Bt22SpriteN;
                    temp.pressedSprite = page06Bt22SpriteNA;
                }
                page06Bt22.spriteState = temp;
                page06Bt22State = value;
                VRCattleManager.instance.guGe.SetActive(value);
                bodyStates[2] = value;
            }
        }
        public Sprite page06Bt23SpriteN;
        public Sprite page06Bt23SpriteNA;
        public Sprite page06Bt23SpriteS;
        public Sprite page06Bt23SpriteSA;
        private bool page06Bt23State = false;
        public bool Page06Bt23State
        {
            get { return page06Bt23State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt23.GetComponent<Image>().sprite = page06Bt23SpriteS;
                    temp.pressedSprite = page06Bt23SpriteSA;
                }
                else
                {
                    page06Bt23.GetComponent<Image>().sprite = page06Bt23SpriteN;
                    temp.pressedSprite = page06Bt23SpriteNA;
                }
                page06Bt23.spriteState = temp;
                page06Bt23State = value;
                VRCattleManager.instance.xiaoHua.SetActive(value);
                bodyStates[3] = value;
            }
        }
        public Sprite page06Bt24SpriteN;
        public Sprite page06Bt24SpriteNA;
        public Sprite page06Bt24SpriteS;
        public Sprite page06Bt24SpriteSA;
        private bool page06Bt24State = false;
        public bool Page06Bt24State
        {
            get { return page06Bt24State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt24.GetComponent<Image>().sprite = page06Bt24SpriteS;
                    temp.pressedSprite = page06Bt24SpriteSA;
                }
                else
                {
                    page06Bt24.GetComponent<Image>().sprite = page06Bt24SpriteN;
                    temp.pressedSprite = page06Bt24SpriteNA;
                }
                page06Bt24.spriteState = temp;
                page06Bt24State = value;
                VRCattleManager.instance.huXi.SetActive(value);
                bodyStates[4] = value;
            }
        }
        public Sprite page06Bt25SpriteN;
        public Sprite page06Bt25SpriteNA;
        public Sprite page06Bt25SpriteS;
        public Sprite page06Bt25SpriteSA;
        private bool page06Bt25State = false;
        public bool Page06Bt25State
        {
            get { return page06Bt25State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt25.GetComponent<Image>().sprite = page06Bt25SpriteS;
                    temp.pressedSprite = page06Bt25SpriteSA;
                }
                else
                {
                    page06Bt25.GetComponent<Image>().sprite = page06Bt25SpriteN;
                    temp.pressedSprite = page06Bt25SpriteNA;
                }
                page06Bt25.spriteState = temp;
                page06Bt25State = value;
                VRCattleManager.instance.xunHuan.SetActive(value);
                bodyStates[5] = value;
            }
        }
        public Sprite page06Bt26SpriteN;
        public Sprite page06Bt26SpriteNA;
        public Sprite page06Bt26SpriteS;
        public Sprite page06Bt26SpriteSA;
        private bool page06Bt26State = false;
        public bool Page06Bt26State
        {
            get { return page06Bt26State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt26.GetComponent<Image>().sprite = page06Bt26SpriteS;
                    temp.pressedSprite = page06Bt26SpriteSA;
                }
                else
                {
                    page06Bt26.GetComponent<Image>().sprite = page06Bt26SpriteN;
                    temp.pressedSprite = page06Bt26SpriteNA;
                }
                page06Bt26.spriteState = temp;
                page06Bt26State = value;
                VRCattleManager.instance.shenJing.SetActive(value);
                bodyStates[6] = value;
            }
        }
        public Sprite page06Bt27SpriteN;
        public Sprite page06Bt27SpriteNA;
        public Sprite page06Bt27SpriteS;
        public Sprite page06Bt27SpriteSA;
        private bool page06Bt27State = false;
        public bool Page06Bt27State
        {
            get { return page06Bt27State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt27.GetComponent<Image>().sprite = page06Bt27SpriteS;
                    temp.pressedSprite = page06Bt27SpriteSA;
                }
                else
                {
                    page06Bt27.GetComponent<Image>().sprite = page06Bt27SpriteN;
                    temp.pressedSprite = page06Bt27SpriteNA;
                }
                page06Bt27.spriteState = temp;
                page06Bt27State = value;
                VRCattleManager.instance.linBa.SetActive(value);
                bodyStates[7] = value;
            }
        }
        public Sprite page06Bt28SpriteN;
        public Sprite page06Bt28SpriteNA;
        public Sprite page06Bt28SpriteS;
        public Sprite page06Bt28SpriteSA;
        private bool page06Bt28State = false;
        public bool Page06Bt28State
        {
            get { return page06Bt28State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt28.GetComponent<Image>().sprite = page06Bt28SpriteS;
                    temp.pressedSprite = page06Bt28SpriteSA;
                }
                else
                {
                    page06Bt28.GetComponent<Image>().sprite = page06Bt28SpriteN;
                    temp.pressedSprite = page06Bt28SpriteNA;
                }
                page06Bt28.spriteState = temp;
                page06Bt28State = value;
                VRCattleManager.instance.miNiao.SetActive(value);
                bodyStates[8] = value;
            }
        }
        public Sprite page06Bt29SpriteN;
        public Sprite page06Bt29SpriteNA;
        public Sprite page06Bt29SpriteS;
        public Sprite page06Bt29SpriteSA;
        private bool page06Bt29State = false;
        public bool Page06Bt29State
        {
            get { return page06Bt29State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt29.GetComponent<Image>().sprite = page06Bt29SpriteS;
                    temp.pressedSprite = page06Bt29SpriteSA;
                }
                else
                {
                    page06Bt29.GetComponent<Image>().sprite = page06Bt29SpriteN;
                    temp.pressedSprite = page06Bt29SpriteNA;
                }
                page06Bt29.spriteState = temp;
                page06Bt29State = value;
                VRCattleManager.instance.shengZhi.SetActive(value);
                bodyStates[9] = value;
            }
        }
        public Sprite page06Bt30SpriteN;
        public Sprite page06Bt30SpriteNA;
        public Sprite page06Bt30SpriteS;
        public Sprite page06Bt30SpriteSA;
        private bool page06Bt30State = false;
        public bool Page06Bt30State
        {
            get { return page06Bt30State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt30.GetComponent<Image>().sprite = page06Bt30SpriteS;
                    temp.pressedSprite = page06Bt30SpriteSA;
                }
                else
                {
                    page06Bt30.GetComponent<Image>().sprite = page06Bt30SpriteN;
                    temp.pressedSprite = page06Bt30SpriteNA;
                }
                page06Bt30.spriteState = temp;
                page06Bt30State = value;
                VRCattleManager.instance.ganJue.SetActive(value);
                bodyStates[10] = value;
            }
        }
        public Sprite page06Bt31SpriteN;
        public Sprite page06Bt31SpriteNA;
        public Sprite page06Bt31SpriteS;
        public Sprite page06Bt31SpriteSA;
        private bool page06Bt31State = false;
        public bool Page06Bt31State
        {
            get { return page06Bt31State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt31.GetComponent<Image>().sprite = page06Bt31SpriteS;
                    temp.pressedSprite = page06Bt31SpriteSA;
                }
                else
                {
                    page06Bt31.GetComponent<Image>().sprite = page06Bt31SpriteN;
                    temp.pressedSprite = page06Bt31SpriteNA;
                }
                page06Bt31.spriteState = temp;
                page06Bt31State = value;
                VRCattleManager.instance.neiFenMi.SetActive(value);
                bodyStates[11] = value;
            }
        }
        public GameObject page06LeftToolChildBts;
        public GameObject saveOkImage;
        public GameObject jieTuSaveOKImage;
        public GameObject page06KeyBoard;
        VRCattleForDrawGraphicBts page06Bt34ForDrawG;
        int page06Bt34State = -1;
        int Page06Bt34State
        {
            get { return page06Bt34State; }
            set
            {
                if (value == 3)
                    page06Bt34State = 0;
                else
                    page06Bt34State = value;
            }
        }
        VRCattleForDrawGraphicBts page06Bt35ForDrawG;
        int page06Bt35State = -1;
        int Page06Bt35State
        {
            get { return page06Bt35State; }
            set
            {
                if (value == 3)
                    page06Bt35State = 0;
                else
                    page06Bt35State = value;
            }
        }
        VRCattleForDrawGraphicBts page06Bt36ForDrawG;
        int page06Bt36State = -1;
        int Page06Bt36State
        {
            get { return page06Bt36State; }
            set
            {
                if (value == 3)
                    page06Bt36State = 0;
                else
                    page06Bt36State = value;
            }
        }
        public GameObject page06PanelExitMask;
        public Sprite page06Bt02SpriteN;
        public Sprite page06Bt02SpriteNA;
        public Sprite page06Bt02SpriteS;
        public Sprite page06Bt02SpriteSA;
        private bool page06Bt02State = false;
        public bool Page06Bt02State
        {
            get { return page06Bt02State; }
            set
            {
                SpriteState temp = new SpriteState();
                if (value)
                {
                    page06Bt02.GetComponent<Image>().sprite = page06Bt02SpriteS;
                    temp.pressedSprite = page06Bt02SpriteSA;
                    CreateAllTags();
                }
                else
                {
                    page06Bt02.GetComponent<Image>().sprite = page06Bt02SpriteN;
                    temp.pressedSprite = page06Bt02SpriteNA;
                    ClearTags();
                }
                page06Bt02.spriteState = temp;
                page06Bt02State = value;
            }
        }
        public Scrollbar page06Scrollbar;
        [HideInInspector]
        public bool isCanExecute = true;
        #endregion

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }
        private void Start()
        {
            CalledByStart();
            CalledByStartInitPage01();
            CalledByStartInitPage02();
            CalledByStartInitPage03();
            CalledByStartInitPage04();
            CalledByStartInitPage05();
            CalledByStartInitPage06();
        }
        void CalledByStart()
        {
            bodyStates = new bool[13];
        }
        void CalledByStartInitPage01()
        {
            AddBtListener(page01Bt01, ForPage01Bt01);
            AddBtListener(page01Bt02, ForPage01Bt02);
            AddBtListener(page01Bt03, ForPage01Bt03);
            AddBtListener(page01Bt04, ForPage01Bt04);
            page01.SetActive(true);
        }
        void CalledByStartInitPage02()
        {
            AddBtListener(page02Bt01, ForPage02Bt01);
            AddBtListener(page02Bt02, ForPage02Bt02);
            AddBtListener(page02Bt03, ForPage02Bt03);
            AddBtListener(page02Bt04, ForPage02Bt04);
            AddBtListener(page02Bt05, ForPage02Bt05);
            AddBtListener(page02Bt06, ForPage02Bt06);
            page02.SetActive(false);
        }
        void CalledByStartInitPage03()
        {
            AddBtListener(page03Bt01, ForPage03Bt01);
            AddBtListener(page03Bt02, ForPage03Bt02);
            AddBtListener(page03Bt03, ForPage03Bt03);
            AddBtListener(page03Grid13Bt01, ForPage03Grid13Bt01);
            AddBtListener(page03Grid13Bt02, ForPage03Grid13Bt02);
            AddBtListener(page03Grid13Bt03, ForPage03Grid13Bt03);
            AddBtListener(page03Grid13Bt04, ForPage03Grid13Bt04);
            AddBtListener(page03Grid13Bt05, ForPage03Grid13Bt05);
            AddBtListener(page03Grid13Bt06, ForPage03Grid13Bt06);
            AddBtListener(page03Grid13Bt07, ForPage03Grid13Bt07);
            AddBtListener(page03Grid13Bt08, ForPage03Grid13Bt08);
            AddBtListener(page03Grid13Bt09, ForPage03Grid13Bt09);
            AddBtListener(page03Grid13Bt10, ForPage03Grid13Bt10);
            AddBtListener(page03Grid13Bt11, ForPage03Grid13Bt11);
            AddBtListener(page03Grid13Bt12, ForPage03Grid13Bt12);
            AddBtListener(page03Bt04, ForPage03Bt04);
            AddBtListener(page03Bt05, ForPage03Bt05);
            AddBtListener(page03Bt06, ForPage03Bt06);

            page03Grid02.SetActive(false);
            page03Grid03.SetActive(false);
            page03Grid04.SetActive(false);
            page03Grid05.SetActive(false);
            page03Grid06.SetActive(false);
            page03Grid07.SetActive(false);
            page03Grid08.SetActive(false);
            page03Grid09.SetActive(false);
            page03Grid10.SetActive(false);
            page03Grid11.SetActive(false);
            page03Grid12.SetActive(false);

            ClosePage03();
        }
        void CalledByStartInitPage04()
        {
            AddBtListener(page04Bt01, ForPage04Bt01);
            AddBtListener(page04Bt02, ForPage04Bt02);
            AddBtListener(page04Bt03, ForPage04Bt03);
            AddBtListener(page04Bt04, ForPage04Bt04);
            AddBtListener(page04Bt05, ForPage04Bt05);
            ForPage04Bt02();
            page04.SetActive(false);
        }
        void CalledByStartInitPage05()
        {
            AddBtListener(page05Bt01, ForPage05Bt01);
            AddBtListener(page05Bt02, ForPage05Bt02);
            AddBtListener(page05Bt03, ForPage05Bt03);
            AddBtListener(page05Bt04, ForPage05Bt04);
            AddBtListener(page05Bt05, ForPage05Bt05);
            AddBtListener(page05Bt06, ForPage05Bt06);
            AddScrollbarListener(page05ScrollbarVolume, ForPage05ScrollbarVolume);
            VRCattleManager.instance.panelWhite.SetActive(false);
            VRCattleManager.instance.panelBlack.SetActive(false);
            VRCattleManager.instance.environment.SetActive(false);
            InitPage05();
            ClosePage05();
        }
        void CalledByStartInitPage06()
        {
            OnMultiModeChange.AddListener(VRCattleManager.instance.MultiModeChange);
            AddBtListener(page06Bt01, ForPage06Bt01);
            AddBtListener(page06Bt02, ForPage06Bt02);
            AddBtListener(page06Bt03, ForPage06Bt03);
            AddBtListener(page06Bt04, ForPage06Bt04);
            AddBtListener(page06Bt05, ForPage06Bt05);
            AddBtListener(page06Bt06, ForPage06Bt06);
            AddBtListener(page06Bt07, ForPage06Bt07);
            AddBtListener(page06Bt08, ForPage06Bt08);
            AddBtListener(page06Bt09, ForPage06Bt09);
            AddBtListener(page06Bt10, ForPage06Bt10);
            AddBtListener(page06Bt11, ForPage06Bt11);
            AddBtListener(page06Bt12, ForPage06Bt12);
            AddBtListener(page06Bt13, ForPage06Bt13);
            AddBtListener(page06Bt14, ForPage06Bt14);
            AddBtListener(page06Bt15, ForPage06Bt15);
            AddBtListener(page06Bt16, ForPage06Bt16);
            AddBtListener(page06Bt17, ForPage06Bt17);
            AddBtListener(page06Bt18, ForPage06Bt18);
            AddBtListener(page06Bt19, ForPage06Bt19);
            AddBtListener(page06Bt20, ForPage06Bt20);
            AddBtListener(page06Bt21, ForPage06Bt21);
            AddBtListener(page06Bt22, ForPage06Bt22);
            AddBtListener(page06Bt23, ForPage06Bt23);
            AddBtListener(page06Bt24, ForPage06Bt24);
            AddBtListener(page06Bt25, ForPage06Bt25);
            AddBtListener(page06Bt26, ForPage06Bt26);
            AddBtListener(page06Bt27, ForPage06Bt27);
            AddBtListener(page06Bt28, ForPage06Bt28);
            AddBtListener(page06Bt29, ForPage06Bt29);
            AddBtListener(page06Bt30, ForPage06Bt30);
            AddBtListener(page06Bt31, ForPage06Bt31);
            AddBtListener(page06Bt32, ForPage06Bt32);
            AddBtListener(page06Bt33, ForPage06Bt33);
            AddBtListener(page06Bt34, ForPage06Bt34);
            AddBtListener(page06Bt35, ForPage06Bt35);
            AddBtListener(page06Bt36, ForPage06Bt36);
            AddBtListener(page06Bt37, ForPage06Bt37);
            AddBtListener(page06Bt38, ForPage06Bt38);
            AddBtListener(page06Bt39, ForPage06Bt39);
            AddBtListener(page06Bt40, ForPage06Bt40);
            AddBtListener(page06Bt41, ForPage06Bt41);
            AddBtListener(page06Bt42, ForPage06Bt42);
            AddBtListener(page06Bt43, ForPage06Bt43);
            AddBtListener(page06Bt44, ForPage06Bt44);
            AddBtListener(page06Bt45, ForPage06Bt45);
            AddScrollbarListener(page06Scrollbar, ForPage06Scrollbar);

            page06Bt34ForDrawG = page06Bt34.GetComponent<VRCattleForDrawGraphicBts>();
            page06Bt35ForDrawG = page06Bt35.GetComponent<VRCattleForDrawGraphicBts>();
            page06Bt36ForDrawG = page06Bt36.GetComponent<VRCattleForDrawGraphicBts>();

            page06LeftToolChildBts.SetActive(false);
            page06ChildBtList.SetActive(false);
            page06ImageInfo.SetActive(false);
            page06RawImageBiaoJi.SetActive(false);
            page06ImageBiaoJiBts.SetActive(false);
            saveOkImage.SetActive(false);
            jieTuSaveOKImage.SetActive(false);
            page06PanelExitMask.SetActive(false);
            page06KeyBoard.SetActive(false);

            ForPage06Bt09();

            page06.SetActive(false);
            VRCattleManager.instance.root.SetActive(false);
        }
    }
}

