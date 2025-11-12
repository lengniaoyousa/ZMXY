using System.Collections;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 0.1f;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);
    
    private float timer;
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if(timer > spawnInterval)
        {
            timer = 0;
            CreateGhost();
        }
    }
    
    void CreateGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        ghostSR.sprite = GetComponent<SpriteRenderer>().sprite;
        ghostSR.color = ghostColor;
        
        StartCoroutine(FadeOut(ghostSR, 0.5f));
    }
    
    IEnumerator FadeOut(SpriteRenderer sr, float duration)
    {
        float elapsed = 0f;
        Color startColor = sr.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            sr.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        
        Destroy(sr.gameObject);
    }
}