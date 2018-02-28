using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlurOutlineCommandBuffer : MonoBehaviour {
    private CommandBuffer _commandBuffer;
    private List<Renderer> targets = new List<Renderer>();
    public static List<BlurOutlineCommandBuffer> instances = new List<BlurOutlineCommandBuffer>();

    private Material _glowMat;
    private Material _blurMaterial;
    private Vector2 _blurTexelSize;

    private int _prePassRenderTexID;
    private int _blurPassRenderTexID;
    private int _tempRenderTexID;
    private int _blurSizeID;
    private int _glowColorID;

    #region AddByCai
    public static Color blurColor = Color.yellow;
    #endregion

    public static void Add(Renderer renderer)
    {
        if (renderer == null) return;
        for (int i = 0; i < instances.Count; i++)
        {
            instances[i].AddRenderer(renderer);
        }
    }

    public static void Remove(Renderer renderer)
    {
        for (int i = 0; i < instances.Count; i++)
        {
            instances[i].RemoveRenderer(renderer);
        }
    }

    public static void Clear()
    {
        for (int i = 0; i < instances.Count; i++)
        {
            instances[i].ClearRenderer();
        }
    }

    public void AddRenderer(Renderer renderer)
    {
        targets.Add(renderer);
    }

    public void RemoveRenderer(Renderer renderer)
    {
        targets.Remove(renderer);
    }

    public void ClearRenderer()
    {
        targets.Clear();
    }

    private void Awake()
    {
        instances.Add(this);
        _glowMat = new Material(Shader.Find("Hidden/GlowCmdShader"));
        _blurMaterial = new Material(Shader.Find("Hidden/Blur"));

        _prePassRenderTexID = Shader.PropertyToID("_GlowPrePassTex");
        _blurPassRenderTexID = Shader.PropertyToID("_GlowBlurredTex");
        _tempRenderTexID = Shader.PropertyToID("_TempTex0");
        _blurSizeID = Shader.PropertyToID("_BlurSize");
        _glowColorID = Shader.PropertyToID("_GlowColor");

        _commandBuffer = new CommandBuffer();
        _commandBuffer.name = "Glowing Objects Buffer"; // This name is visible in the Frame Debugger, so make it a descriptive!
        GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
    }

    private void RebuildCommandBuffer()
    {
        _commandBuffer.Clear();
        int antiAliasing = QualitySettings.antiAliasing;
        if (antiAliasing == 0) antiAliasing = 1;
        //Debug.Log(antiAliasing);
        _commandBuffer.GetTemporaryRT(_prePassRenderTexID, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, antiAliasing);
        _commandBuffer.SetRenderTarget(_prePassRenderTexID);
        _commandBuffer.ClearRenderTarget(true, true, Color.clear);
        //_commandBuffer.SetGlobalColor(_glowColorID, Color.yellow);
        #region AddByCai
        _commandBuffer.SetGlobalColor(_glowColorID, blurColor);
        #endregion
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].gameObject.activeInHierarchy)
                _commandBuffer.DrawRenderer(targets[i], _glowMat);
        }
        _commandBuffer.GetTemporaryRT(_blurPassRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, antiAliasing);
        _commandBuffer.GetTemporaryRT(_tempRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, antiAliasing);
        _commandBuffer.Blit(_prePassRenderTexID, _blurPassRenderTexID);

        _blurTexelSize = new Vector2(1.5f / (Screen.width >> 1), 1.5f / (Screen.height >> 1));
        _commandBuffer.SetGlobalVector(_blurSizeID, _blurTexelSize);

        for (int i = 0; i < 1; i++)
        {
            _commandBuffer.Blit(_blurPassRenderTexID, _tempRenderTexID, _blurMaterial, 0);
            _commandBuffer.Blit(_tempRenderTexID, _blurPassRenderTexID, _blurMaterial, 1);
        }
    }

    private void Update()
    {
        RebuildCommandBuffer();
    }

    private void OnDestroy()
    {
        GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
        _commandBuffer.Clear();
        _commandBuffer.Dispose();
        instances.Remove(this);
    }
}
