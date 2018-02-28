using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCattle
{
    public class VRCattleLookAtCam : MonoBehaviour
    {

        Transform cam = null;

        private void OnEnable()
        {
            if (cam == null)
            {
                if (VRCattleManager.instance.mainCamera != null)
                    cam = VRCattleManager.instance.mainCamera;
            }
        }

        private void Update()
        {
            if (cam != null)
                transform.LookAt(cam);
        }

    }
}

