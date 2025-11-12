using UnityEngine;
using UnityEngine.Rendering;

public class Advanced2DBlur : MonoBehaviour
{
    [Range(0, 10)]
    public int blurIterations = 3;
    
    [Range(0, 4)]
    public float blurSpread = 0.6f;
    
    public Shader blurShader;
    public LayerMask blurLayerMask = -1; // 选择要模糊的层
    
    private Material blurMaterial;
    private CommandBuffer blurBuffer;
    private RenderTexture blurRenderTexture;
    private Camera mainCamera;
    
    [Header("测试模式")]
    [Tooltip("只测试垂直模糊效果")]
    public bool testVerticalOnly = false;
    [Tooltip("只测试水平模糊效果")]
    public bool testHorizontalOnly = false;
    [Tooltip("测试模糊效果")]
    public bool testBoth = true;

    void OnEnable()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (blurShader == null)
        {
            blurShader = Shader.Find("Hidden/FastBlur");
        }
        
        blurMaterial = new Material(blurShader);
        
        // 创建渲染纹理
        CreateBlurRenderTexture();
        
        // 设置命令缓冲区
        SetupCommandBuffer();
    }

    void OnDisable()
    {
        if (blurBuffer != null)
        {
            mainCamera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, blurBuffer);
            blurBuffer.Dispose();
        }
        
        if (blurMaterial != null)
        {
            Destroy(blurMaterial);
        }
        
        if (blurRenderTexture != null)
        {
            RenderTexture.ReleaseTemporary(blurRenderTexture);
        }
    }

    void OnPreRender()
    {
        // 确保渲染纹理尺寸与屏幕匹配
        if (blurRenderTexture == null || blurRenderTexture.width != Screen.width || blurRenderTexture.height != Screen.height)
        {
            CreateBlurRenderTexture();
            SetupCommandBuffer();
        }
    }

    void CreateBlurRenderTexture()
    {
        if (blurRenderTexture != null)
        {
            RenderTexture.ReleaseTemporary(blurRenderTexture);
        }
        
        blurRenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        blurRenderTexture.filterMode = FilterMode.Bilinear;
        blurRenderTexture.Create();
    }

    void SetupCommandBuffer()
    {
        if (blurBuffer != null)
        {
            mainCamera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, blurBuffer);
            blurBuffer.Dispose();
        }

        blurBuffer = new CommandBuffer();
        blurBuffer.name = "Specific Object Blur";

        // 设置渲染目标到模糊纹理
        blurBuffer.SetRenderTarget(blurRenderTexture);
        blurBuffer.ClearRenderTarget(true, true, Color.clear);

        // 渲染特定层的物体到模糊纹理
        blurBuffer.DrawRendererList(GetRenderersInLayer(blurLayerMask));

        // 应用命令缓冲区
        mainCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, blurBuffer);
    }

    RendererList GetRenderersInLayer(LayerMask layerMask)
    {
        // 获取指定层中的所有渲染器
        var renderers = FindObjectsOfType<Renderer>();
        var validRenderers = new System.Collections.Generic.List<Renderer>();

        foreach (var renderer in renderers)
        {
            if ((layerMask.value & (1 << renderer.gameObject.layer)) != 0)
            {
                validRenderers.Add(renderer);
            }
        }

        // 这里需要更复杂的逻辑来创建RendererList，实际项目中可能需要使用不同的方法
        // 简化版本：使用Graphics.DrawRenderers或其他方法
        return default;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (blurMaterial == null || blurIterations == 0 || blurRenderTexture == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // 对模糊纹理应用模糊效果
        RenderTexture blurredTexture = ApplyBlurToTexture(blurRenderTexture);

        // 将模糊后的物体合成到原场景中
        blurMaterial.SetTexture("_MainTex", blurredTexture);
        blurMaterial.SetTexture("_BlurTex", blurredTexture);
        Graphics.Blit(source, destination, blurMaterial); // 使用第三个pass进行合成

        // 清理临时纹理
        if (blurredTexture != blurRenderTexture)
        {
            RenderTexture.ReleaseTemporary(blurredTexture);
        }
    }

    RenderTexture ApplyBlurToTexture(RenderTexture source)
    {
        int rtW = source.width / 4;
        int rtH = source.height / 4;
        
        RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
        buffer.filterMode = FilterMode.Bilinear;

        // 降采样
        Graphics.Blit(source, buffer);

        for (int i = 0; i < blurIterations; i++)
        {
            blurMaterial.SetFloat("_BlurSize", 1.0f + i * blurSpread);
        
            RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
        
            if (testVerticalOnly)
            {
                // 只进行垂直模糊
                Graphics.Blit(buffer, buffer2, blurMaterial, 0);
            }
            else if (testHorizontalOnly)
            {
                // 只进行水平模糊
                Graphics.Blit(buffer, buffer2, blurMaterial, 1);
            }
            else if (testBoth)
            {
                // 先垂直后水平模糊
                RenderTexture temp = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer, temp, blurMaterial, 0); // 垂直
                Graphics.Blit(temp, buffer2, blurMaterial, 1); // 水平
                RenderTexture.ReleaseTemporary(temp);
            }
        
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }

        return buffer;
    }

    // 动态添加/移除物体到模糊层
    public void AddObjectToBlur(GameObject obj)
    {
        obj.layer = GetFirstLayerInMask(blurLayerMask);
        SetupCommandBuffer(); // 重新设置命令缓冲区
    }

    public void RemoveObjectFromBlur(GameObject obj)
    {
        // 将物体移出模糊层
        obj.layer = 0;
        SetupCommandBuffer();
    }

    public int GetFirstLayerInMask(LayerMask mask)
    {
        int layerNumber = 0;
        int maskValue = mask.value;
        
        while (maskValue > 0)
        {
            if ((maskValue & 1) != 0)
                return layerNumber;
            
            maskValue >>= 1;
            layerNumber++;
        }
        
        return 0;
    }
}
