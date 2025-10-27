using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        private MapDataDataMgr _mapDataDataMgr;
        private LevelEnum mLevelEnum;

        public override void OnCreate()
        {
            base.OnCreate();
            Debug.Log("BattleWorld OnCreate");
        }

        public async void EnterLevel(LevelEnum levelEnum)
        {
            mLevelEnum = levelEnum;

            if (mLevelEnum == LevelEnum.HuaGuoShan)
            {
                //生成地图
                // MapCfg map = ZMAsset.LoadScriptableObject<MapCfg>(AssetPath.Battle_MapCfg+"/HuaGuoShan.asset");
                // //await ZMAsset.InstantiateObjectAsync(AssetPath.Battle_HuaGuoShan_Prefabs+"/HuaGuoShanScene.prefab", null);
                // await ZMAsset.InstantiateObjectAsync(map.MapPrefabPath,null);
                // _mapDataDataMgr = GetExitsDataMgr<MapDataDataMgr>();
                //
                // _mapDataDataMgr.LoadMapData(mLevelEnum);
                //
                // await ZMAsset.InstantiateObjectAsync(_mapDataDataMgr.currentMapCfg.MapPrefabPath,null);

                GetExitsLogicCtrl<MapDataLogicCtrl>().EnterLevel(levelEnum);

                GetExitsLogicCtrl<HeroLogicCtrl>().CreateHero();
                
                Camera.main.GetComponent<MainCameraController>().SetSunController(GetExitsLogicCtrl<HeroLogicCtrl>().GetSunWuKong());
                // AssetsRequest roleRequest =
                //     await ZMAsset.InstantiateObjectAsync(AssetPath.Battle_Prefabs + "/SunWuKong.prefab", null);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("BattleWorld OnDestroy");
        }
    }
}