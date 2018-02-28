using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawPicManager : MonoBehaviour {

    public static DrawPicManager instance;
    public Camera mainCamera;
    public Camera drawCamera;

    public const int canvasWidth=3840;
    public const int canvasHeight=2160;

    [HideInInspector]
    public Texture2D tex;
    public RawImage image;
    [HideInInspector]
    public byte[] pixels;
    private byte[] clearPixels;
    [HideInInspector]
    public byte[] pixelsUndo;
    
    private Stack<int> stack = new Stack<int>();
    private string directoryPath;

    public DrawFreeLine drawline;
    public DrawCircle drawcircle;
    public DrawArrow drawarrow;

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
        drawline = GetComponent<DrawFreeLine>();
        drawcircle = GetComponent<DrawCircle>();
        drawarrow = GetComponent<DrawArrow>();

        tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        image.texture = tex;

        pixels = new byte[Screen.width * Screen.height * 4];
        pixelsUndo = new byte[Screen.width * Screen.height * 4];
        clearPixels = new byte[Screen.width * Screen.height * 4];
        ClearDrawed();

        if (Application.isEditor)
            directoryPath = Application.persistentDataPath + "/../undos/";
        else
            directoryPath = Application.persistentDataPath + "/undos/";
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }

    public void GetSaveDrawPic(ref Texture2D tex,int width=canvasWidth,int height = canvasHeight)
    {
        if (instance == null) return;
        int antiAliasing = QualitySettings.antiAliasing;
        if (antiAliasing == 0) antiAliasing = 1;

        RenderTexture RT = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear, antiAliasing);
        Graphics.Blit(tex, RT);
        drawCamera.targetTexture = RT;
        drawCamera.Render();
        drawCamera.targetTexture = null;
        RenderTexture.active = RT;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(RT);
        tex.Apply(false);
    }

    string MyPicturesPath
    {
        get
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures) + "\\VRCattle\\";
        }
    }
    int PictureIndex
    {
        get
        {
            FileInfo[] files = new DirectoryInfo(MyPicturesPath).GetFiles();
            int index = 0;
            for(int i = 0; i < files.Length; i++)
            {
                int temp = int.Parse(files[i].Name.Split('.')[0]);
                if (temp > index)
                {
                    index = temp;
                }
            }
            return index + 1;
        }
    }
    Texture2D RenderMainCamera(int width=canvasWidth,int height=canvasHeight,int cutWidth=canvasWidth,int cutHeight = canvasHeight)
    {
        RenderTexture rt = new RenderTexture(width, height, 0);
        mainCamera.targetTexture = rt;
        mainCamera.Render();

        RenderTexture.active = rt;
        Texture2D screenShot=new Texture2D(cutWidth,cutHeight,TextureFormat.RGBA32,false,true);
        screenShot.ReadPixels(new Rect((width - cutWidth) / 2, (height - cutHeight) / 2, cutWidth, cutHeight), 0, 0);
        screenShot.Apply(false);

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);

        return screenShot;
    }
    public void SavePicture()
    {
        if (!Directory.Exists(MyPicturesPath))
            Directory.CreateDirectory(MyPicturesPath);
        Texture2D back = RenderMainCamera(tex.width, tex.height, tex.width, tex.height);
        byte[] bytes = back.EncodeToPNG();
        File.WriteAllBytes(string.Format("{0}{1}.png", MyPicturesPath, PictureIndex), bytes);
    }

    bool IsUndoEmpty
    {
        get { return stack.Count == 0; }
    }
    public void UndoDrawed()
    {
        if (IsUndoEmpty)
            return;
        if (stack.Count == 1)
            VRCattle.VRCattleUIManager.instance.SetPage06Bt37Interactable(false);
        string path = directoryPath + stack.Pop();
        byte[] bytes = File.ReadAllBytes(path);
        File.Delete(path);
        System.Array.Copy(bytes, pixels, bytes.Length);
        System.Array.Copy(pixels, 0, pixelsUndo, 0, pixels.Length);
        tex.LoadRawTextureData(bytes);
        tex.Apply(false);
    }

    void ClearStack()
    {
        while (stack.Count > 0)
        {
            string path = directoryPath + stack.Pop();
            System.IO.File.Delete(path);
        }
    }
    public void ClearDrawed()
    {
        VRCattle.VRCattleUIManager.instance.SetPage06Bt37Interactable(false);

        System.Array.Copy(clearPixels, 0, pixels, 0, clearPixels.Length);
        System.Array.Copy(pixels,0, pixelsUndo,0, pixels.Length);
        tex.LoadRawTextureData(clearPixels);
        tex.Apply(false);
        ClearStack();
    }

    void Push(byte[] bytes)
    {
        if (VRCattle.VRCattleUIManager.instance.page06Bt37.interactable == false)
        {
            VRCattle.VRCattleUIManager.instance.SetPage06Bt37Interactable(true);
        }
        stack.Push(stack.Count + 1);
        File.WriteAllBytes(directoryPath + stack.Count, bytes);
    }
    public void DrawByMesh()
    {
        Push(pixelsUndo);
        GetSaveDrawPic(ref tex, tex.width, tex.height);
        pixels = tex.GetRawTextureData();
        System.Array.Copy(pixels, 0,pixelsUndo,0, pixels.Length);
    }

    private void OnDisable()
    {
        ClearDrawed();
    }


    public void DrawFreeLine(Color color)
    {
        drawcircle.End();
        drawarrow.End();
        drawline.Begin(color,DrawByMesh);
    }
    public void DrawCircle(Color color)
    {
        drawline.End();
        drawarrow.End();
        drawcircle.Begin(color,DrawByMesh);
    }
    public void DrawArrow(Color color)
    {
        drawline.End();
        drawcircle.End();
        drawarrow.Begin(color,DrawByMesh);
    }
    public void CheXiao()
    {
        UndoDrawed();
    }
    public void QingChu()
    {
        ClearDrawed();
    }
    public void BaoCun()
    {
        SavePicture();
    }
}
