using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LocalMsg
{
    /// <summary>
    /// 消息中心引擎，主要是用来转发消息
    /// </summary>
    public class MsgCenterEngine : MsgEngine
    {
        /// <summary>
        /// 消息中心引擎构造函数，设定引擎ID
        /// </summary>
        public MsgCenterEngine()
        {
            Register(EngineID.MsgCenter);
        }

        /// <summary>
        /// 消息处理函数，转发消息
        /// </summary>
        /// <param name="msg">消息</param>
        public override void MsgProc(Msg msg)
        {
            // 定向发送,只让某个引擎处理该消息
            if (msg.ReceiverEngineID != EngineID.Unknow)
            {
                EngineManager.SendMsg(msg, msg.ReceiverEngineID);
            }
            else // 广播
            {
                List<EngineID> engineIDs = MsgHandlerMap.GetEngineIDList(msg.ID);
                if (engineIDs != null)
                {
                    if (engineIDs.Count == 1)
                    {
                        EngineManager.SendMsg(msg, engineIDs[0]);
                    }
                    else // 广播对象大于1个的时候，消息必须采用复制形式发送，这样在各个处理引擎处理
                    // 完释放后，才能正确的释放回对象池
                    {
                        for (int i = 0; i < engineIDs.Count; ++i)
                        {
                            if (i == engineIDs.Count - 1) // 最后一个消息，使用本体发过去
                                EngineManager.SendMsg(msg, engineIDs[i]);
                            else
                                EngineManager.SendMsg(Msg.Create(msg), engineIDs[i]);
                        }
                    }
                }
                else
                {
                   
                }
            }
        }

    }
}
