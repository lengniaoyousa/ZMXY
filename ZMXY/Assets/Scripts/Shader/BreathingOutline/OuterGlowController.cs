using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OuterGlowController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header("Outer Glow Settings")] public Color glowColor = Color.cyan;
    [Range(0, 10)] public float glowWidth = 3f;
    [Range(0, 5)] public float glowIntensity = 0f;

    private Material material;
    private Image image;
    
    [Header("PingPong数值控制")]
    public float minValue = 0f;
    public float maxValue = 5f;
    public float speed = 1f;
    
    private float timer = 0f;

    private bool isPlay = true;

    void Start()
    {
        glowIntensity = 0;
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("Image component not found!");
            return;
        }

        // 检查Shader是否存在
        Shader shader = Shader.Find("lcl/shader2D/OuterGlow");
        if (shader == null)
        {
            Debug.LogError("Shader 'lcl/shader2D/OuterGlow' not found!");

            // 尝试查找其他可能的Shader名称
            shader = Shader.Find("Sprites/Default");
            if (shader != null)
            {
                Debug.Log("Using default sprite shader instead");
            }
            else
            {
                return;
            }
        }

        // 创建材质实例
        material = new Material(shader);

        // 复制原始材质的纹理和颜色
        if (image.material != null)
        {
            material.mainTexture = image.material.mainTexture;
        }
        else
        {
            material.mainTexture = image.sprite?.texture;
        }

        // 应用材质到Image
        image.material = material;

        Debug.Log("Material created: " + material.name);
        Debug.Log("Shader: " + material.shader.name);

        UpdateMaterialProperties();
    }

    void UpdateMaterialProperties()
    {
        if (material != null)
        {
            // 设置Shader属性
            material.SetColor("_GlowColor", glowColor);
            material.SetFloat("_GlowWidth", glowWidth);
            material.SetFloat("_GlowIntensity", glowIntensity);

            // 对于UI Image，需要强制重绘
            ForceImageUpdate();
        }
        else
        {
            Debug.LogWarning("Material is null!");
        }
    }

    void ForceImageUpdate()
    {
        if (image != null)
        {
            // 方法1：修改颜色触发更新
            Color originalColor = image.color;
            image.color = originalColor * 0.999f;
            image.color = originalColor;

            // 方法2：通过CanvasRenderer
            image.canvasRenderer.SetMaterial(material, null);

            // 方法3：设置脏标记
            image.SetAllDirty();
        }
    }

    void Update()
    {
        // 在Update中测试实时更新
        if (isPlay)
        {
            timer += Time.deltaTime * speed;
        
            // Mathf.PingPong会在0到(maxValue-minValue)之间来回变化
            glowIntensity = Mathf.PingPong(timer, maxValue - minValue) + minValue;

            UpdateMaterialProperties();
        }
    }

    void OnDestroy()
    {
        if (material != null)
            DestroyImmediate(material);
    }

    void OnValidate()
    {
        // 在编辑模式下也尝试更新
        if (material != null)
        {
            UpdateMaterialProperties();
        }
    }

    // 公共方法供其他脚本调用
    public void SetGlowProperties(Color color, float width, float intensity)
    {
        glowColor = color;
        glowWidth = width;
        glowIntensity = intensity;
        UpdateMaterialProperties();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPlay = false;
        glowWidth = 0;
        glowIntensity = 0;
        UpdateMaterialProperties();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glowWidth = 3;
        glowIntensity = 0;
        isPlay = true;
        UpdateMaterialProperties();

    }
}