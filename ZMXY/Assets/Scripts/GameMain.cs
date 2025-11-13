using System;
using Hall;
using UnityEngine;
using ZM.ZMAsset;

public class GameMain : MonoBehaviour
{
    private void Awake()
    {
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();
    }

    private void Start()
    {
        WorldManager.CreateWorld<HallWorld>();
    }

    private void Update()
    {
        LogicTimerManager.Instance.OnLogicTimerUpdate();
        ActionController.Instance.OnActionControllerUpdate();
    }
}
