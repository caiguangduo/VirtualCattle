using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCattle
{
    internal class VRCattleUndoBase
    {
        public virtual void Do()
        {
        }
        public virtual void Undo()
        {
        }
    }

    internal class VRCattleDisableUndo : VRCattleUndoBase
    {
        GameObject[] obj;
        bool[] originState;
        int length = 0;

        public VRCattleDisableUndo(GameObject[] obj)
        {
            this.obj = obj;
            length = obj.Length;
            originState = new bool[length];
            for(int i = 0; i < length; i++)
            {
                originState[i] = obj[i].activeSelf;
            }
        }

        public override void Do()
        {
            base.Do();
            for(int i = 0; i < length; i++)
            {
                this.obj[i].SetActive(false);
                VRCattleManager.AddDisabledObj(this.obj[i].transform);
            }
        }

        public override void Undo()
        {
            base.Undo();
            for(int i = 0; i < length; i++)
            {
                obj[i].SetActive(originState[i]);
                VRCattleManager.RemoveDisabledObj(this.obj[i].transform);
            }
        }
    }

    internal class VRCattleTransparentUndo : VRCattleUndoBase
    {
        MeshRenderer[] mrs;
        float[] originTransValue;
        int length = 0;

        public VRCattleTransparentUndo(MeshRenderer[] mrs)
        {
            this.mrs = mrs;
            length = mrs.Length;
            originTransValue = new float[length];
            for(int i = 0; i < length; i++)
            {
                originTransValue[i] = mrs[i].material.color.a;
            }
        }

        public override void Do()
        {
            base.Do();
            for(int i = 0; i < length; i++)
            {
                Color color = this.mrs[i].material.color;
                color.a = VRCattleManager.transparentValue;
                this.mrs[i].material.color = color;
                VRCattleManager.AddTransparentObj(this.mrs[i]);
            }
        }

        public override void Undo()
        {
            base.Undo();
            for(int i = 0; i < length; i++)
            {
                Color color = this.mrs[i].material.color;
                color.a = originTransValue[i];
                this.mrs[i].material.color = color;
                VRCattleManager.RemoveTransparentObj(this.mrs[i]);
            }
        }
    }

    public class VRCattleUndo
    {
        private static Stack<VRCattleUndoBase> stack = new Stack<VRCattleUndoBase>();

        public static void DisableObj(GameObject[] obj)
        {
            Push(new VRCattleDisableUndo(obj));
        }

        public static void TransparentObj(MeshRenderer[] mr)
        {
            Push(new VRCattleTransparentUndo(mr));
        }

        private static void Push(VRCattleUndoBase undo)
        {
            undo.Do();
            stack.Push(undo);
            VRCattleUIManager.instance.SetUndoBtInteractableState(true);
        }

        public static void Pop()
        {
            if (stack.Count == 0) return;
            VRCattleUndoBase undo = stack.Pop();
            undo.Undo();
            if (stack.Count == 0)
                VRCattleUIManager.instance.SetUndoBtInteractableState(false);
        }

        public static void Clear()
        {
            stack.Clear();
            VRCattleUIManager.instance.SetUndoBtInteractableState(false);
        }
    }
}

