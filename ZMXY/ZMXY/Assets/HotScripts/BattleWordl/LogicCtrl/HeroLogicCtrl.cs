/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/10/11 20:35:45
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class HeroLogicCtrl : ILogicBehaviour
    {
        private SunController mPlayer;
        
        public void OnCreate()
        {
        }

        public async void CreateHero()
        {
            AssetsRequest roleRequest =
                await ZMAsset.InstantiateObjectAsync(AssetPath.Battle_Prefabs + "/SunWuKong.prefab", null);

            mPlayer = roleRequest.obj.gameObject.GetComponent<SunController>();
        }

        public SunController GetSunWuKong()
        {
            return mPlayer;
        }

        public void OnDestroy()
        {
        }
    }
}