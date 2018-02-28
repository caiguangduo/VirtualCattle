using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRCattle
{
    public class VRCattleTupuBt : MonoBehaviour
    {
        private Sprite spriteMale;
        [SerializeField]
        private Sprite spriteFemale;
        private Image image;
        private Text text;
        private string titleMale;
        [SerializeField]
        private string titleFemale;
        
        private void Start()
        {
            image = GetComponent<Image>();
            spriteMale = image.sprite;
            text = transform.GetChild(0).GetComponent<Text>();
            titleMale = text.text;
            VRCattleUIManager.instance.onSexChange += ChangeBtOnSexChange;
        }

        void ChangeBtOnSexChange()
        {
            if (VRCattleUIManager.instance.sex == VRCattleUIManager.Sex.Male)
            {
                image.sprite = spriteMale;
                text.text = titleMale;
            }
            else
            {
                image.sprite = spriteFemale;
                text.text = titleFemale;
            }
        }
    }
}

