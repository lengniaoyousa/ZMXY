using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoxColliderGizmo : MonoBehaviour
{
    private BoxCollider2D  boxCollider2D;
    
    private Color gizmoColor = new Color(1, 0, 0, 1);
    
    private void Start()
    {
        boxCollider2D =  GetComponent<BoxCollider2D>();
    }

    private void OnDrawGizmos()
    {

        if (!gameObject.activeInHierarchy) return;
        
        boxCollider2D =  GetComponent<BoxCollider2D>();
        
        if (boxCollider2D == null) return;
        
        Gizmos.color = gizmoColor;
        DrawBox2DGizmo(boxCollider2D);
    }
    
    private void DrawBox2DGizmo(BoxCollider2D collider)
    {

        Vector2 offset = collider.offset;
        Vector2 size = collider.size;
        
        // 计算世界坐标
        Vector3 worldCenter = transform.TransformPoint(offset);
        Vector3 worldSize = new Vector3(size.x * transform.lossyScale.x, 
            size.y * transform.lossyScale.y, 
            0.1f);  // Z轴给一个很小的厚度
        
        // 应用变换
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(worldCenter, transform.rotation, Vector3.one);
        
        // 绘制填充和线框
        //Gizmos.DrawCube(Vector3.zero, worldSize);
        
        //Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
        Gizmos.DrawWireCube(Vector3.zero, worldSize);
        
        // 恢复矩阵
        Gizmos.matrix = originalMatrix;
    }
}
