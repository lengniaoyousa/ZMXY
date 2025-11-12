using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UI;

namespace Hall
{
    public class HallWorld : World
    {
        public override void OnCreate()
        {
            base.OnCreate();
            Debug.Log("HallWorld OnCreate");
            UIModule.Instance.PopUpWindow<LoginWindow>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("HallWorld OnDestroy");
        }
    }
}

