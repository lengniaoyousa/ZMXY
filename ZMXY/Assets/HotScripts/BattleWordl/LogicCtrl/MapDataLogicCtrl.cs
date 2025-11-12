/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/10/11 19:42:00
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class MapDataLogicCtrl : ILogicBehaviour
    {
        private MapDataDataMgr mMapDataMgr;
        public void OnCreate()
        {
        }

        public async void EnterLevel(LevelEnum levelEnum)
        {
            mMapDataMgr = BattleWorld.GetExitsDataMgr<MapDataDataMgr>();
            
            mMapDataMgr.LoadMapData(levelEnum);
            
            Camera.main.GetComponent<MainCameraController>().SetCameraOffsetValue(mMapDataMgr.currentMapCfg.XiangJiWeiZhiYuZhi[0],mMapDataMgr.currentMapCfg.XiangJiWeiZhiYuZhi[1]);
            
            await ZMAsset.InstantiateObjectAsync(mMapDataMgr.currentMapCfg.MapPrefabPath,null);
        }

        public void OnDestroy()
        {
        }
    }
}