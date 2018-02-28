using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRCattle
{
    public class VRCattleForPage02GridBt : MonoBehaviour
    {
        public Text nameText;
        public Text timeText;
        public RawImage image;

        public string Name
        {
            get { return nameText.text; }
            set { nameText.text = value; }
        }

        public string Time
        {
            get { return timeText.text; }
            set { timeText.text = value; }
        }

        public Texture Pic
        {
            get { return image.texture; }
            set { image.texture = value; }
        }
    }
}

