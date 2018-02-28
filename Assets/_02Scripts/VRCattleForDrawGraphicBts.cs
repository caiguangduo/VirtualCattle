using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRCattleForDrawGraphicBts : MonoBehaviour {
    public Sprite blueN;
    public Sprite blueS;
    public Sprite redN;
    public Sprite redS;
    public Sprite yellowN;
    public Sprite yellowS;

    public Color colorBlue = Color.blue;
    public Color colorRed = Color.red;
    public Color colorYellow = Color.yellow;

    public Image thisImage;

    public enum ColorState
    {
        Blue,Red,Yellow
    }
    public ColorState colorstate = ColorState.Blue;

    private bool isSelected = false;
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            if (!value)
            {
                switch (colorstate)
                {
                    case ColorState.Blue:
                        thisImage.sprite = blueN;
                        break;
                    case ColorState.Red:
                        thisImage.sprite = redN;
                        break;
                    case ColorState.Yellow:
                        thisImage.sprite = yellowN;
                        break;
                }
            }
            isSelected = value;
            
        }
    }

    private Color _color = Color.blue;
    public Color color
    {
        get
        {
            switch (colorstate)
            {
                case ColorState.Blue:
                    return colorBlue;
                case ColorState.Red:
                    return colorRed;
                case ColorState.Yellow:
                    return colorYellow;
            }
            return default(Color);
        }
        
    }

    public void SetBtState(int index)
    {
        switch (index)
        {
            case 0:
                colorstate = ColorState.Blue;
                thisImage.sprite = blueS;
                break;
            case 1:
                colorstate = ColorState.Red;
                thisImage.sprite = redS;
                break;
            case 2:
                colorstate = ColorState.Yellow;
                thisImage.sprite = yellowS;
                break;
        }
        if(!IsSelected)
            IsSelected = true;
    }
}
