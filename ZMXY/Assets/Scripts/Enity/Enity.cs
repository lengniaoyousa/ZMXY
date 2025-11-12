using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enity : MonoBehaviour
{
    [HideInInspector] public StateMachine<State> stateMachine;  // 状态机实例
    [HideInInspector] public Animator animator; // 动画控制器
    [HideInInspector] public Rigidbody2D rigidbody2D;
    
    /// <summary>
    /// 面朝向
    /// </summary>
     protected int mMianChaoXiang = -1;

    public virtual void Start()
    {
        //获取组件引用
        animator = GetComponentInChildren<Animator>();
        
        //刚体
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        //创建状态机实例
        stateMachine = new StateMachine<State>();
    }

    // public virtual void Idel()
    // {
    //     
    // }
    
    public virtual int GetMianChaoXiang()
    {
        return mMianChaoXiang;
    }
}
