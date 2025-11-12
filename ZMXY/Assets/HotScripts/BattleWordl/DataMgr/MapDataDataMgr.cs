/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2025/10/11 19:26:19
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class MapDataDataMgr : IDataBehaviour
    {
        public MapCfg currentMapCfg = null;
        public void OnCreate()
        {
            Debug.Log("MapDataDataMgr OnCreate");
            //currentMapCfg = ZMAsset.LoadScriptableObject<MapCfg>(AssetPath.Battle_MapCfg+"/HuaGuoShan.asset");
        }

        public void LoadMapData(LevelEnum levelEnum)
        {
            currentMapCfg = ZMAsset.LoadScriptableObject<MapCfg>(AssetPath.Battle_MapCfg+$"/{levelEnum}.asset");
        }

        public void OnDestroy()
        {
            Debug.Log("MapDataDataMgr OnDestroy");
        }
    }
}