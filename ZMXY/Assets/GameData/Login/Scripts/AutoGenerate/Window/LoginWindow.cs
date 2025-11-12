/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/10/6 10:14:49
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine;

namespace ZM.UI
{
    public class LoginWindow : WindowBase
    {
        public LoginWindowDataComponent dataCompt;

        #region 生命周期函数

        //调用机制与Mono Awake一致
        public override void OnAwake()
        {
            dataCompt = gameObject.GetComponent<LoginWindowDataComponent>();
            dataCompt.InitComponent(this);
            base.OnAwake();
        }

        //物体显示时执行
        public override void OnShow()
        {
            base.OnShow();
        }

        //物体隐藏时执行
        public override void OnHide()
        {
            base.OnHide();
        }

        //物体销毁时执行
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        #endregion

        #region API Function

        #endregion

        #region UI组件事件

        public void OnOnePlayerGameButtonClick()
        {
            dataCompt.YouXiMoShiGameObject.SetActive(false);
            dataCompt.XinYouXiOrJiXuGameObject.SetActive(true);
        }

        public void OnTwoPlayerGameButtonClick()
        {
            //JsonConvert
        }

        public void OnChengJiuFangJianButtonClick()
        {
        }

        public void OnYouXiShangDianButtonClick()
        {
        }

        public void OnYouXiTuiJianButtonClick()
        {
        }

        public void OnGuanYuWoMenButtonClick()
        {
        }

        public void OnXinDeKaiShiButtonClick()
        {
            dataCompt.XinYouXiOrJiXuGameObject.SetActive(false);
            dataCompt.JueSeGameObject.SetActive(true);
        }

        public void OnJiXuYouXiButtonClick()
        {
        }

        public void OnQuXiaoYouXiButtonClick()
        {
        }

        public void OnSunWuKongButtonClick()
        {
            string _savePath = Path.Combine(Application.persistentDataPath, "SaveData", "game_save.json");
            Debug.Log("游戏存档位置:"+_savePath);
            GameData.InitSunWuKongData();
            GameData.SaveGame();
            GameData.LoadGame();

            UIModule.Instance.HideWindow<LoginWindow>();
            UIModule.Instance.PopUpWindow<QiMoWangWindow>();
        }

        public void OnTangShengButtonClick()
        {
        }

        public void OnQuXiaoJueSeButtonClick()
        {
        }

        #endregion
    }
}