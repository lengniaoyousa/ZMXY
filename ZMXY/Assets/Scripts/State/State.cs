using System;
using System.Collections.Generic;
using UnityEngine;

// 状态基类
public abstract class State
{
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}

// 泛型状态机
public class StateMachine<T> where T : State
{
    private T currentState;
    private Dictionary<Type, T> stateDictionary = new Dictionary<Type, T>();
    
    public void AddState(T state)
    {
        stateDictionary[state.GetType()] = state;
    }
    
    public void ChangeState<U>() where U : T
    {
        var newStateType = typeof(U);
        
        if (stateDictionary.ContainsKey(newStateType))
        {
            currentState?.Exit();
            currentState = stateDictionary[newStateType];
            currentState?.Enter();
        }
    }
    
    public void Update()
    {
        currentState?.Execute();
    }
    
    public Type GetCurrentStateType()
    {
        return currentState?.GetType();
    }
}