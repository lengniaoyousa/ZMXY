using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrailEffectManager : MonoBehaviour
{
    [Header("Ghost Trail Settings")]
    public float spawnInterval = 0.1f;
    public Color ghostColor = Color.white;
    public float fadeSpeed = 0.9f;
    public Material ghostMaterial;
    
    [Header("References")]
    public SpriteRenderer targetRenderer;
    
    private float timer;
    private List<GameObject> ghostPool = new List<GameObject>();
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > spawnInterval)
        {
            timer = 0;
            CreateGhost();
        }
        
        // 更新所有残影的透明度并移除完全透明的残影
        for (int i = ghostPool.Count - 1; i >= 0; i--)
        {
            GameObject ghost = ghostPool[i];
            SpriteRenderer ghostRenderer = ghost.GetComponent<SpriteRenderer>();
            Color color = ghostRenderer.color;
            color.a *= fadeSpeed;
            ghostRenderer.color = color;
            
            if (color.a < 0.05f)
            {
                ghostPool.RemoveAt(i);
                Destroy(ghost);
            }
        }
    }
    
    void CreateGhost()
    {
        GameObject ghost = new GameObject("Ghost");
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = transform.localScale;
        
        SpriteRenderer ghostRenderer = ghost.AddComponent<SpriteRenderer>();
        ghostRenderer.sprite = targetRenderer.sprite;
        ghostRenderer.material = ghostMaterial;
        ghostRenderer.color = ghostColor;
        ghostRenderer.sortingOrder = targetRenderer.sortingOrder - 1;
        
        ghostPool.Add(ghost);
    }
    
    public void ClearGhosts()
    {
        foreach (GameObject ghost in ghostPool)
        {
            Destroy(ghost);
        }
        ghostPool.Clear();
    }
}
