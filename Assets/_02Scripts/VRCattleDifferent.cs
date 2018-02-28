using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCattle
{
    public class VRCattleDifferent : MonoBehaviour
    {
        public static VRCattleDifferent instance;
        public bool isMale = true;

        public MeshRenderer[] beiPiMale;
        public MeshRenderer[] beiPiFemale;
        public MeshRenderer[] guGeMale;
        public MeshRenderer[] jiRouMale;
        public MeshRenderer[] linBaMale;
        public MeshRenderer[] miNiaoMale;
        public MeshRenderer[] miNiaoFemale;
        public MeshRenderer[] neiFenMiMale;
        public MeshRenderer[] shengZhiMale;
        public MeshRenderer[] shengZhiFemale;
        public MeshRenderer[] shenJingFemale;
        public MeshRenderer[] xunHuanFemale;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }

        public void DisableObjs(bool isMale)
        {
            this.isMale = isMale;
            int length = beiPiMale.Length;
            for(int i = 0; i < length; i++)
            {
                beiPiMale[i].gameObject.SetActive(isMale);
            }
            length = guGeMale.Length;
            for(int i = 0; i < length; i++)
            {
                guGeMale[i].gameObject.SetActive(isMale);
            }
            length = jiRouMale.Length;
            for(int i = 0; i < length; i++)
            {
                jiRouMale[i].gameObject.SetActive(isMale);
            }
            length = linBaMale.Length;
            for(int i = 0; i < length; i++)
            {
                linBaMale[i].gameObject.SetActive(isMale);
            }
            length = miNiaoMale.Length;
            for(int i = 0; i < length; i++)
            {
                miNiaoMale[i].gameObject.SetActive(isMale);
            }
            length = neiFenMiMale.Length;
            for(int i = 0; i < length; i++)
            {
                neiFenMiMale[i].gameObject.SetActive(isMale);
            }
            length = shengZhiMale.Length;
            for(int i = 0; i < length; i++)
            {
                shengZhiMale[i].gameObject.SetActive(isMale);
            }
            length = beiPiFemale.Length;
            for(int i = 0; i < length; i++)
            {
                beiPiFemale[i].gameObject.SetActive(!isMale);
            }
            length = miNiaoFemale.Length;
            for(int i = 0; i < length; i++)
            {
                miNiaoFemale[i].gameObject.SetActive(!isMale);
            }
            length = shengZhiFemale.Length;
            for(int i = 0; i < length; i++)
            {
                shengZhiFemale[i].gameObject.SetActive(!isMale);
            }
            length = shenJingFemale.Length;
            for(int i = 0; i < length; i++)
            {
                shenJingFemale[i].gameObject.SetActive(!isMale);
            }
            length = xunHuanFemale.Length;
            for(int i = 0; i < length; i++)
            {
                xunHuanFemale[i].gameObject.SetActive(!isMale);
            }
        }

        public void AddToVSX(ref VRCattleSaveXml vsx,bool isMale)
        {
            int length = 0;
            if (!isMale)
            {
                length = beiPiMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(beiPiMale[i].transform));
                }
                length = guGeMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(guGeMale[i].transform));
                }
                length = jiRouMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(jiRouMale[i].transform));
                }
                length = linBaMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(linBaMale[i].transform));
                }
                length = miNiaoMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(miNiaoMale[i].transform));
                }
                length = neiFenMiMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(neiFenMiMale[i].transform));
                }
                length = shengZhiMale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(shengZhiMale[i].transform));
                }
            }
            else
            {
                length = beiPiFemale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(beiPiFemale[i].transform));
                }
                length = miNiaoFemale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(miNiaoFemale[i].transform));
                }
                length = shengZhiFemale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(shengZhiFemale[i].transform));
                }
                length = shenJingFemale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(shenJingFemale[i].transform));
                }
                length = xunHuanFemale.Length;
                for(int i = 0; i < length; i++)
                {
                    vsx.disable.Add(VRCattleBusinessLogic.GetNodeIDByTransform(xunHuanFemale[i].transform));
                }
            }
        }

        public static void DisableObjsStatic(bool isMale)
        {
            if (instance != null)
                instance.DisableObjs(isMale);
        }
    }
}

