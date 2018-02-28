using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRCattle
{
    public class VRCattleCCSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            VRCattleManager.instance.audioSourceUI.clip = VRCattleManager.instance.uiClickClip;
            VRCattleManager.instance.audioSourceUI.Play();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            VRCattleManager.instance.audioSourceUI.clip = VRCattleManager.instance.uiCoverClip;
            VRCattleManager.instance.audioSourceUI.Play();
        }
    }
}

