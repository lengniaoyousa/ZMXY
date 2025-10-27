using UnityEngine;
using UnityEngine.Rendering;

public class Advanced2DFullScreenBlur : MonoBehaviour
{
    [Range(0, 10)]
    public int blurIterations = 3;
    
    [Range(0, 4)]
    public float blurSpread = 0.6f;
    
    public Shader blurShader;
    
    private Material blurMaterial;
    private CommandBuffer blurBuffer;
    private RenderTexture blurTexture;

    void OnEnable()
    {
        if (blurShader == null)
        {
            blurShader = Shader.Find("Hidden/FastBlur");
        }
        
        blurMaterial = new Material(blurShader);
        blurBuffer = new CommandBuffer();
        blurBuffer.name = "2D Blur";
        
        Camera.main.AddCommandBuffer(CameraEvent.AfterEverything, blurBuffer);
    }

    void OnDisable()
    {
        if (blurBuffer != null)
        {
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterEverything, blurBuffer);
            blurBuffer.Dispose();
        }
        
        if (blurMaterial != null)
        {
            Destroy(blurMaterial);
        }
        
        if (blurTexture != null)
        {
            RenderTexture.ReleaseTemporary(blurTexture);
        }
    }
    
    /// <summary>
    /// 全屏模糊
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    // void OnRenderImage(RenderTexture source, RenderTexture destination)
    // {
    //     if (blurMaterial == null || blurIterations == 0)
    //     {
    //         Graphics.Blit(source, destination);
    //         return;
    //     }
    //
    //     int rtW = source.width / 4;
    //     int rtH = source.height / 4;
    //     
    //     RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
    //     buffer.filterMode = FilterMode.Bilinear;
    //
    //     // 降采样
    //     Graphics.Blit(source, buffer);
    //
    //     for (int i = 0; i < blurIterations; i++)
    //     {
    //         blurMaterial.SetFloat("_BlurSize", 1.0f + i * blurSpread);
    //         
    //         RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
    //         
    //         // 垂直模糊
    //         Graphics.Blit(buffer, buffer2, blurMaterial, 0);
    //         RenderTexture.ReleaseTemporary(buffer);
    //         buffer = buffer2;
    //         
    //         buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
    //         
    //         // 水平模糊
    //         Graphics.Blit(buffer, buffer2, blurMaterial, 1);
    //         RenderTexture.ReleaseTemporary(buffer);
    //         buffer = buffer2;
    //     }
    //
    //     Graphics.Blit(buffer, destination);
    //     RenderTexture.ReleaseTemporary(buffer);
    // }
    // 修改OnRenderImage方法，只对特定物体的渲染纹理进行模糊
    // 这就是 OnRenderImage 方法！
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (blurMaterial == null || blurIterations == 0)
        {
            // 如果没有模糊材质或迭代次数为0，直接输出原图
            Graphics.Blit(source, destination);
            return;
        }

        int rtW = source.width / 4;  // 降采样到1/4分辨率
        int rtH = source.height / 4;
        
        RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
        buffer.filterMode = FilterMode.Bilinear;

        // 首先降采样
        Graphics.Blit(source, buffer);

        // 多次迭代模糊
        for (int i = 0; i < blurIterations; i++)
        {
            blurMaterial.SetFloat("_BlurSize", 1.0f + i * blurSpread);
            
            RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
            
            // 垂直模糊 Pass 0
            Graphics.Blit(buffer, buffer2, blurMaterial, 0);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
            
            buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
            
            // 水平模糊 Pass 1
            Graphics.Blit(buffer, buffer2, blurMaterial, 1);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }

        // 最终输出到屏幕
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}
