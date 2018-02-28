using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

namespace VRCattle
{
    partial class VRCattleUIManager : MonoBehaviour
    {
        public void AddBtListener(Button bt, UnityAction action)
        {
            bt.onClick.AddListener(action);
        }
        public void AddScrollbarListener(Scrollbar sb, UnityAction<float> action)
        {
            sb.onValueChanged.AddListener(action);
        }

        public void SetUndoBtInteractableState(bool state)
        {
            page06Bt12.interactable = state;
        }

        #region Page01Methods
        public void OpenPage01()
        {
            page01.SetActive(true);
        }
        public void ClosePage01()
        {
            page01.SetActive(false);
        }
        public void ForPage01Bt01()
        {
            ClosePage01();
            OpenPage02();
        }
        public void ForPage01Bt02()
        {
            ClosePage01();
            OpenPage03();
        }
        public void ForPage01Bt03()
        {
            ClosePage01();
            OpenPage04();
        }
        public void ForPage01Bt04()
        {
            ClosePage01();
            OpenPage05();
        }
        #endregion

        #region Pge02Methods
        public void OpenPage02()
        {
            page02.SetActive(true);
            ForPage02FindSave();
            ForPage02Bt04();
        }
        public void ClosePage02()
        {
            ForPage02RemoveAll();
            page02.SetActive(false);
        }
        void ForPage02FindSave()
        {
            DirectoryInfo info = new DirectoryInfo(Page02SaveBtPath);

            if (info.Exists)
            {
                DirectoryInfo[] childs = info.GetDirectories();
                int count = childs.Length;
                for (int i = 0; i < count; i++)
                {
                    byte[] bytes = File.ReadAllBytes(childs[i].FullName + "\\icon.png");
                    Texture2D tex = new Texture2D(1, 1);
                    tex.LoadImage(bytes);

                    string name = childs[i].Name;
                    string time = childs[i].CreationTime.ToString("yy-MM-dd HH:mm:ss");
                    ForPage02CreateSave(name, time, tex);
                }
            }
        }
        void ForPage02CreateSave(string name, string time, Texture2D tex)
        {
            GameObject go = Instantiate<GameObject>(page02BtPrefab);
            Transform bt = go.transform;
            bt.SetParent(page02Grid);
            bt.localPosition = Vector3.zero;
            bt.localRotation = Quaternion.identity;
            bt.localScale = Vector3.one;

            VRCattleForPage02GridBt item = bt.GetComponent<VRCattleForPage02GridBt>();
            item.Name = name;
            item.Time = time;
            item.Pic = tex;

            bt.GetComponent<EventTriggerListener>().onClick += OnPage02GridBtClick;
        }
        public void OnPage02GridBtClick(GameObject go)
        {
            page02CurrentItem = go.GetComponent<VRCattleForPage02GridBt>();
        }
        void ForPage02RemoveAll()
        {
            int count = page02Grid.childCount;
            for (int i = 0; i < count; i++)
            {
                ForPage02RemoveAt(i);
            }
        }
        void ForPage02RemoveAt(int index)
        {
            if (page02Grid.childCount > index)
                Destroy(page02Grid.GetChild(index).gameObject);
        }
        void ForPage02RemoveAt(VRCattleForPage02GridBt item)
        {
            File.Delete(string.Format("{0}\\{1}\\data.vhsx", Page02SaveBtPath, item.Name));
            File.Delete(string.Format("{0}\\{1}\\icon.png", Page02SaveBtPath, item.Name));
            Directory.Delete(string.Format("{0}\\{1}", Page02SaveBtPath, item.Name));
            Destroy(item.gameObject);
        }
        public void ForPage02Bt01()
        {
            ClosePage02();
            OpenPage01();
        }
        public void ForPage02Bt02()
        {
            if (page02CurrentItem != null)
            {
                ClosePage02();
                OpenPage06();
                VRCattleBusinessLogic.LoadFromSave(File.ReadAllText(Page02SaveBtPath + "\\" + page02CurrentItem.Name + "\\data.vhsx"));
            }
        }
        public void ForPage02Bt03()
        {
            if (page02CurrentItem != null)
            {
                ForPage02RemoveAt(page02CurrentItem);
                page02CurrentItem = null;
            }
        }
        public void ForPage02Bt04()
        {
            if (page02Grid.localPosition.y != page02GridOriginPos.y)
                page02Grid.localPosition = page02GridOriginPos;
        }
        public void ForPage02Bt05()
        {
            if (page02Grid.localPosition.y != page02GridOriginPos.y)
                page02Grid.localPosition -= page02GridMoveDis;
        }
        public void ForPage02Bt06()
        {
            if (page02Grid.localPosition != CalcPageBottomPos())
                page02Grid.localPosition += page02GridMoveDis;
        }
        Vector3 CalcPageBottomPos()
        {
            Vector3 bottomPos = Vector3.zero;
            int count = 0;
            count = Mathf.CeilToInt(page02Grid.childCount / 10.0f);
            bottomPos = (count - 1) * page02GridMoveDis + page02GridOriginPos;
            return bottomPos;
        }
        #endregion

        #region Page03Methods
        public void OpenPage03()
        {
            page03.SetActive(true);
        }
        public void ClosePage03()
        {
            ForPage03Bt02();
            ForPage03Grid13Bt01();
            page03.SetActive(false);
        }
        public void ForPage03Bt01()
        {
            ClosePage03();
            OpenPage01();
        }
        public void ForPage03Bt02()
        {
            CalledByPage03Bt0203(Sex.Male, true);
        }
        public void ForPage03Bt03()
        {
            CalledByPage03Bt0203(Sex.Female, false);
        }
        void CalledByPage03Bt0203(Sex sex, bool isMale)
        {
            this.sex = sex;
            if (onSexChange != null)
                onSexChange();
            page03Bt02.interactable = !isMale;
            page03Bt03.interactable = isMale;
        }
        public void ForTuPuLoadBts(string str)
        {
            VRCattleBusinessLogic.instance.ResetAll();
            ClosePage03();
            OpenPage06();

            TextAsset asset = Resources.Load("Tupu/" + sex.ToString() + "/" + str) as TextAsset;
            if (asset == null) return;
            VRCattleBusinessLogic.LoadFromSave(asset.text);
            Resources.UnloadAsset(asset);
        }
        public void ForPage03Grid13Bt01()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt01, page03Grid01);
        }
        public void ForPage03Grid13Bt02()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt02, page03Grid02);
        }
        public void ForPage03Grid13Bt03()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt03, page03Grid03);
        }
        public void ForPage03Grid13Bt04()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt04, page03Grid04);
        }
        public void ForPage03Grid13Bt05()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt05, page03Grid05);
        }
        public void ForPage03Grid13Bt06()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt06, page03Grid06);
        }
        public void ForPage03Grid13Bt07()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt07, page03Grid07);
        }
        public void ForPage03Grid13Bt08()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt08, page03Grid08);
        }
        public void ForPage03Grid13Bt09()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt09, page03Grid09);
        }
        public void ForPage03Grid13Bt10()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt10, page03Grid10);
        }
        public void ForPage03Grid13Bt11()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt11, page03Grid11);
        }
        public void ForPage03Grid13Bt12()
        {
            CalledByPage03Grid13Bts(page03Grid13Bt12, page03Grid12);
        }
        void CalledByPage03Grid13Bts(Button bt, GameObject grid)
        {
            if (page03Grid13TempBt != null)
            {
                page03Grid13TempBt.interactable = true;
                ExecuteEvents.Execute(page03Grid13TempBt.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            }
            if (page03TempGrid != null)
            {
                page03TempGrid.SetActive(false);
            }

            page03Grid13TempBt = bt;
            bt.interactable = false;

            page03TempGrid = grid;
            grid.SetActive(true);
            ForPage03Bt04();

        }
        public void ForPage03Bt04()
        {
            Transform t = page03TempGrid.transform.GetChild(0);
            if (t != null)
            {
                if (t.localPosition.y != page03GridOriginPos.y)
                    t.localPosition = page03GridOriginPos;
            }
        }
        public void ForPage03Bt05()
        {
            Transform t = page03TempGrid.transform.GetChild(0);
            if (t != null)
            {
                if (t.localPosition.y != page03GridOriginPos.y)
                    t.localPosition -= page03GridMoveDis;
            }
        }
        public void ForPage03Bt06()
        {
            Transform t = page03TempGrid.transform.GetChild(0);
            if (t != null)
            {
                int count = Mathf.CeilToInt(t.childCount / 10.0f);
                Vector3 bottomPos = (count - 1) * page03GridMoveDis + page03GridOriginPos;
                if (t.localPosition.y != bottomPos.y)
                {
                    t.localPosition += page03GridMoveDis;
                }
            }
        }
        #endregion

        #region Page04Methods
        public void OpenPage04()
        {
            page04.SetActive(true);
        }
        public void ClosePage04()
        {
            ForPage04Bt02();
            page04.SetActive(false);
        }
        public void ForPage04Bt01()
        {
            ClosePage04();
            OpenPage01();
        }
        public void ForPage04Bt02()
        {
            CalledByPage04Bt02_05(page04Bt02, page04SpriteJBCZ);
        }
        public void ForPage04Bt03()
        {
            CalledByPage04Bt02_05(page04Bt03, page04SpriteCJCZ);
        }
        public void ForPage04Bt04()
        {
            CalledByPage04Bt02_05(page04Bt04, page04SpriteSS);
        }
        public void ForPage04Bt05()
        {
            CalledByPage04Bt02_05(page04Bt05, page04SpriteBJ);
        }
        void CalledByPage04Bt02_05(Button bt,Sprite sp)
        {
            if (page04CurBt != null)
            {
                page04CurBt.interactable = true;
                ExecuteEvents.Execute(page04CurBt.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            }
            page04CurBt = bt;
            bt.interactable = false;
            page04ImageCenter.sprite = sp;
        }
        #endregion

        #region Page05Methods
        public void OpenPage05()
        {
            page05.SetActive(true);
        }
        public void ClosePage05()
        {
            page05.SetActive(false);
            PlayerPrefs.SetInt("BgColor", (int)page05BgColor);
            PlayerPrefs.SetFloat("Volume", page05ScrollbarVolume.value);
        }
        public void ForPage05Bt01()
        {
            ClosePage05();
            OpenPage01();
        }
        void InitPage05()
        {
            page05BgColor = (Page05BgColorEnum)PlayerPrefs.GetInt("BgColor", 0);
            switch (page05BgColor)
            {
                case Page05BgColorEnum.white:
                    ForPage05Bt02();
                    break;
                case Page05BgColorEnum.black:
                    ForPage05Bt03();
                    break;
                case Page05BgColorEnum.threeD:
                    ForPage05Bt04();
                    break;
            }

            VRCattleManager.instance.VolumeValue = PlayerPrefs.GetFloat("Volume", 0.5f);
            page05ScrollbarVolume.value = VRCattleManager.instance.VolumeValue;
        }
        void SetBgColorDraw(bool isWhite,bool isBlack,bool is3D)
        {
            page05Bt02Draw.SetActive(isWhite);
            page05Bt03Draw.SetActive(isBlack);
            page05Bt04Draw.SetActive(is3D);
        }
        void SetEnvironment(bool isWhite,bool isBlack,bool is3D)
        {
            if (VRCattleManager.instance.panelWhite.activeSelf != isWhite)
                VRCattleManager.instance.panelWhite.SetActive(isWhite);
            if (VRCattleManager.instance.panelBlack.activeSelf != isBlack)
                VRCattleManager.instance.panelBlack.SetActive(isBlack);
            if (VRCattleManager.instance.environment.activeSelf != is3D)
                VRCattleManager.instance.environment.SetActive(is3D);
        }
        void SetCamera(CameraClearFlags ccf,Color c)
        {
            Camera mainCamera = VRCattleManager.instance.mainCamera.GetComponent<Camera>();
            mainCamera.clearFlags = ccf;
            if (page05BgColor != Page05BgColorEnum.threeD)
                mainCamera.backgroundColor = c;
        }
        public void ForPage05Bt02()
        {
            page05BgColor = Page05BgColorEnum.white;
            SetBgColorDraw(true, false, false);
            SetCamera(CameraClearFlags.SolidColor, page05ColorWhite);
            SetEnvironment(true, false, false);
        }
        public void ForPage05Bt03()
        {
            page05BgColor = Page05BgColorEnum.black;
            SetBgColorDraw(false, true, false);
            SetCamera(CameraClearFlags.SolidColor, page05ColorBlack);
            SetEnvironment(false, true, false);
        }
        public void ForPage05Bt04()
        {
            page05BgColor = Page05BgColorEnum.threeD;
            SetBgColorDraw(false, false, true);
            SetCamera(CameraClearFlags.Skybox, default(Color));
            SetEnvironment(false, false, true);
        }
        public void ForPage05Bt05()
        {
            Debug.Log("联系我们");
        }
        public void ForPage05Bt06()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }
        public void ForPage05ScrollbarVolume(float value)
        {
            VRCattleManager.instance.VolumeValue = page05ScrollbarVolume.value;
        }
        #endregion

        #region Page06Methods
        public void OpenPage06()
        {
            VRCattleManager.instance.root.SetActive(true);
            page06.SetActive(true);
        }
        public void ClosePage06()
        {
            if (Page06Bt02State)
                Page06Bt02State = false;

            if (Page06Bt06State || Page06Bt07State)
                VRCattleKeyBoardManager.instance.OnCancel();

            if (Page06Bt08State)
                Page06Bt08State = false;

            if (Page06Bt03State)
                Page06Bt03State = false;

            if (Page06Bt05State)
                Page06Bt05State = false;

            ForPage06Bt09();

            VRCattleBusinessLogic.instance.ResetAll();

            page06.SetActive(false);

            VRCattleManager.instance.root.SetActive(false);
        }
        public void ForPage06Bt01()
        {
            ClosePage06();
            OpenPage03();
        }
        public void ForPage06Bt02()
        {
            Page06Bt02State = !Page06Bt02State;
        }
        public void CreateTag(Transform target)
        {
            string pid = target.parent.name + "\\" + target.name;
            List<Tag> listTag = VRCattleDataBase.instance.GetTagByPID(pid);
            if (listTag == null) return;
            int count = listTag.Count;
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Transform tag = Instantiate(VRCattleManager.instance.tagPrefab);

                    Text textNameCN = tag.GetChild(3).GetChild(1).GetComponent<Text>();
                    Text textNameEN = tag.GetChild(3).GetChild(2).GetComponent<Text>();
                    textNameCN.text = listTag[i].NameCN;
                    textNameEN.text = listTag[i].NameEN;

                    tag.name = listTag[i].TagName;
                    tag.SetParent(target);
                    Vector3 pos = new Vector3(listTag[i].P_X, listTag[i].P_Y, listTag[i].P_Z);
                    Vector3 euler = new Vector3(listTag[i].R_X, listTag[i].R_Y, listTag[i].R_Z);
                    Vector3 scale = new Vector3(listTag[i].S_X, listTag[i].S_Y, listTag[i].S_Z);
                    tag.localPosition = pos;
                    tag.localRotation = Quaternion.Euler(euler);
                    tag.localScale = scale;
                }
            }
        }
        public void CreateAllTags()
        {
            int count = VRCattleManager.selectedObjs.Count;
            for(int i = 0; i < count; i++)
            {
                CreateTag(VRCattleManager.selectedObjs[i]);
            }
        }
        public void RemoveTag(Transform target)
        {
            int count = target.childCount;
            List<GameObject> temp = new List<GameObject>();
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    temp.Add(target.GetChild(i).gameObject);
                }
                for(int i = 0; i < count; i++)
                {
                    Destroy(temp[i]);
                }
            }
        }
        public void ClearTags()
        {
            int countSelect = VRCattleManager.selectedObjs.Count;
            if (countSelect != 0)
            {
                for(int i = 0; i < countSelect; i++)
                {
                    RemoveTag(VRCattleManager.selectedObjs[i]);
                }
            }
        }
        public void ForPage06Bt03()
        {
            Page06Bt03State = !Page06Bt03State;
        }
        public void ForPage06Bt04()
        {
            VRCattleManager.instance.PlaySound(curSoundName);
        }
        public void ForPage06Bt05()
        {
            Page06Bt05State = !Page06Bt05State;
        }
        public void ForPage06Bt06()
        {
            //查询
            if (!Page06Bt06State && !Page06Bt07State)
            {
                if (Page06Bt03State)
                    Page06Bt03State = false;
                if (Page06Bt08State)
                    Page06Bt08State = false;
            }
            Page06Bt06State = !Page06Bt06State;
        }
        void ChaXunCallBack(string content)
        {
            page06Bt06.GetComponent<Image>().sprite = page06Bt06SpriteN;
            SpriteState temp = new SpriteState();
            temp.pressedSprite = page06Bt06SpriteNA;
            page06Bt06.spriteState = temp;
            page06Bt06State = false;
        }
        public void ForPage06Bt07()
        {
            //保存
            if(!Page06Bt06State&&!Page06Bt07State)
            {
                if (Page06Bt03State)
                    Page06Bt03State = false;
                if (Page06Bt08State)
                    Page06Bt08State = false;
            }
            Page06Bt07State = !Page06Bt07State;
        }
        void BaoCunCallBack(string content)
        {
            page06Bt07.GetComponent<Image>().sprite = page06Bt07SpriteN;
            SpriteState temp = new SpriteState();
            temp.pressedSprite = page06Bt07SpriteNA;
            page06Bt07.spriteState = temp;
            page06Bt07State = false;

            Debug.Log("001");

            if (string.IsNullOrEmpty(content)) return;

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\VRCattle\\";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            if(System.IO.Directory.Exists(path+content))
            {
                System.IO.File.Delete(string.Format("{0}\\{1}\\data.vhsx", path, content));
                System.IO.File.Delete(string.Format("{0}\\{1}\\icon.png", path, content));
            }
            else
            {
                System.IO.Directory.CreateDirectory(string.Format("{0}\\{1}", path, content));
            }
            System.IO.File.WriteAllText(string.Format("{0}\\{1}\\data.vhsx", path, content), XmlTool.SerializeObject<VRCattleSaveXml>(new VRCattleSaveXml(), false));

            StartCoroutine(CaptureAndSavePic(path,content));
        }
        IEnumerator ShowSaveOKIEnumerator;
        IEnumerator ShowSaveOkImage(float wait)
        {
            saveOkImage.SetActive(true);
            yield return new WaitForSeconds(wait);
            saveOkImage.SetActive(false);
        }

        IEnumerator CaptureAndSavePic(string path,string content)
        {
            yield return new WaitForEndOfFrame();
            Texture2D tex = CaptureCamera(VRCattleManager.instance.mainCamera.GetComponent<Camera>(), new Rect(Screen.width * 0.0f, Screen.height * 0.0f, Screen.width * 1.0f, Screen.height * 1.0f));
            System.IO.File.WriteAllBytes(string.Format("{0}\\{1}\\icon.png", path, content), tex.EncodeToPNG());

            if (ShowSaveOKIEnumerator != null)
            {
                StopCoroutine(ShowSaveOKIEnumerator);
                ShowSaveOKIEnumerator = null;
            }
            ShowSaveOKIEnumerator = ShowSaveOkImage(1.5f);
            StartCoroutine(ShowSaveOKIEnumerator);
        }

        Texture2D CaptureCamera(Camera camera,Rect rect)
        {
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            camera.targetTexture = rt;
            camera.Render();

            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D(Mathf.CeilToInt(Screen.width*874.8f/1600f), Mathf.CeilToInt(Screen.height*762f/900f), TextureFormat.RGBA32, false);
            Rect newRect = new Rect(Screen.width / 2 - (Mathf.CeilToInt(Screen.width * 874.8f / 1600f))/2.0f, Screen.height / 2.0f - (Mathf.CeilToInt(Screen.height * 762f / 900f))/2.0f, Screen.width * 874.8f / 1600f, Screen.height * 762f / 900f);
            screenShot.ReadPixels(newRect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rt);

            return screenShot;
        }
        public void ForPage06Bt08()
        {
            Page06Bt08State = !Page06Bt08State;
        }
        public void ForPage06Bt09()
        {
            IsMulti = false;
            page06Bt09.interactable = false;
            page06Bt10.interactable = true;
            ExecuteEvents.Execute(page06Bt10.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
        }
        public void ForPage06Bt10()
        {
            IsMulti = true;
            page06Bt09.interactable = true;
            page06Bt10.interactable = false;
            ExecuteEvents.Execute(page06Bt09.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
        }
        public void ForPage06Bt11()
        {
            VRCattleManager.instance.OnSelectClear();
        }
        public void ForPage06Bt12()
        {
            VRCattleBusinessLogic.instance.Undo();
        }
        public void ForPage06Bt13()
        {
            VRCattleBusinessLogic.instance.ResetAll();
            VRCattleBusinessLogic.LoadFromSave(VRCattleManager.currentLoaded);
        }
        public void ForPage06Bt14()
        {
            VRCattleBusinessLogic.instance.ShowAll();
            VRCattleBusinessLogic.LoadFromSave(VRCattleManager.currentLoaded,true,false);
        }
        public void ForPage06Bt15()
        {
            VRCattleBusinessLogic.instance.Transparent();
        }
        public void ForPage06Bt16()
        {
            VRCattleBusinessLogic.instance.Disable();
        }
        public void ForPage06Bt17()
        {
            VRCattleBusinessLogic.instance.TransparentOther();
        }
        public void ForPage06Bt18()
        {
            VRCattleBusinessLogic.instance.DisableOther();
        }
        public void ForPage06Bt19()
        {
            Page06Bt08State = false;
        }
        public void Page06SetBodyAndBt20_31State(bool[] state)
        {
            int length = state.Length;
            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        Page06Bt20State = state[i];
                        break;
                    case 1:
                        Page06Bt21State = state[i];
                        break;
                    case 2:
                        Page06Bt22State = state[i];
                        break;
                    case 3:
                        Page06Bt23State = state[i];
                        break;
                    case 4:
                        Page06Bt24State = state[i];
                        break;
                    case 5:
                        Page06Bt25State = state[i];
                        break;
                    case 6:
                        Page06Bt26State = state[i];
                        break;
                    case 7:
                        Page06Bt27State = state[i];
                        break;
                    case 8:
                        Page06Bt28State = state[i];
                        break;
                    case 9:
                        Page06Bt29State = state[i];
                        break;
                    case 10:
                        Page06Bt30State = state[i];
                        break;
                    case 11:
                        Page06Bt31State = state[i];
                        break;
                    case 12:
                        VRCattleManager.instance.root.SetActive(state[i]);
                        bodyStates[i] = state[i];
                        break;
                }
            }
        }
        public void ForPage06Bt20()
        {
            Page06Bt20State = !Page06Bt20State;
        }
        public void ForPage06Bt21()
        {
            Page06Bt21State = !Page06Bt21State;
        }
        public void ForPage06Bt22()
        {
            Page06Bt22State = !Page06Bt22State;
        }
        public void ForPage06Bt23()
        {
            Page06Bt23State = !Page06Bt23State;
        }
        public void ForPage06Bt24()
        {
            Page06Bt24State = !Page06Bt24State;
        }
        public void ForPage06Bt25()
        {
            Page06Bt25State = !Page06Bt25State;
        }
        public void ForPage06Bt26()
        {
            Page06Bt26State = !Page06Bt26State;
        }
        public void ForPage06Bt27()
        {
            Page06Bt27State = !Page06Bt27State;
        }
        public void ForPage06Bt28()
        {
            Page06Bt28State = !Page06Bt28State;
        }
        public void ForPage06Bt29()
        {
            Page06Bt29State = !Page06Bt29State;
        }
        public void ForPage06Bt30()
        {
            Page06Bt30State = !Page06Bt30State;
        }
        public void ForPage06Bt31()
        {
            Page06Bt31State = !Page06Bt31State;
        }
        public void ForPage06Bt32()
        {
            Page06Bt03State = false;
        }
        public void ForPage06Bt33()
        {
            Page06Bt05State = false;
        }
        public void ForPage06Bt34()
        {
            page06Bt35ForDrawG.IsSelected = false;
            page06Bt36ForDrawG.IsSelected = false;

            Page06Bt34State += 1;
            page06Bt34ForDrawG.SetBtState(Page06Bt34State);
            Color color = page06Bt34ForDrawG.color;
            DrawPicManager.instance.DrawFreeLine(color);
        }
        public void ForPage06Bt35()
        {
            page06Bt34ForDrawG.IsSelected = false;
            page06Bt36ForDrawG.IsSelected = false;

            Page06Bt35State += 1;
            page06Bt35ForDrawG.SetBtState(Page06Bt35State);
            Color color = page06Bt35ForDrawG.color;
            DrawPicManager.instance.DrawCircle(color);
        }
        public void ForPage06Bt36()
        {
            page06Bt34ForDrawG.IsSelected = false;
            page06Bt35ForDrawG.IsSelected = false;

            Page06Bt36State += 1;
            page06Bt36ForDrawG.SetBtState(Page06Bt36State);
            Color color = page06Bt36ForDrawG.color;
            DrawPicManager.instance.DrawArrow(color);
        }
        public void ForPage06Bt37()
        {
            DrawPicManager.instance.CheXiao();
        }
        public void ForPage06Bt38()
        {
            DrawPicManager.instance.QingChu();
        }
        public void ForPage06Bt39()
        {
            DrawPicManager.instance.BaoCun();
            StartCoroutine(ShowJieTuOKImage());
        }
        IEnumerator ShowJieTuOKImage()
        {
            jieTuSaveOKImage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            jieTuSaveOKImage.SetActive(false);
        }
        public void ForPage06Bt40()
        {

        }
        public void ForPage06Bt41()
        {

        }
        public void ForPage06Bt42()
        {

        }
        public void ForPage06Bt43()
        {

        }
        public void ForPage06Bt44()
        {

        }
        public void ForPage06Bt45()
        {

        }
        public void ForPage06Scrollbar(float value)
        {
            if (!isCanExecute) return;
            Debug.Log("001:"+value);
            VRCattleCameraControll.instance.isScrollbar = true;
            VRCattleCameraControll.instance.Distance = (1 - value) * (VRCattleCameraControll.instance.maxDistance - VRCattleCameraControll.instance.minDistance)+VRCattleCameraControll.instance.minDistance;
            VRCattleCameraControll.instance.ChangeDistance();
        }
        public void SetPage06Bt37Interactable(bool isActive)
        {
            page06Bt37.interactable = isActive;
            if (isActive)
            {
                ExecuteEvents.Execute(page06Bt37.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            }
        }
        #endregion
    }
}

