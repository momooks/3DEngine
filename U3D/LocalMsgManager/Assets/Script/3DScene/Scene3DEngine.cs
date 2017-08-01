using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LocalMsg
{
    public class Scene3DEngine : MsgEngine
    {
        private void Awake()
        {
            Register(EngineID.Scene3D);


            AddMsgHandler((int)MsgID.MoveCamera, MoveCamera);
        }

        public void StartWork()
        {
            m_workFlag = true;
        }


        public void StopWork()
        {
            m_workFlag = false;
        }


        private void Update()
        {
            base.Work();
        }

        void MoveCamera(Msg msg)
        {
            float xx = msg.GetValue<float>("value");
            Debug.Log("MoveCamera!!! " + xx);
        }

    }
}
