using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalMsg
{
    /// <summary>
    /// 引擎管理器，用于启动注册到这里的引擎
    /// </summary>
    public class EngineManager
    {
        /// <summary>
        /// 引擎容器
        /// </summary>
        static Dictionary<EngineID, MsgEngine> gEngineMap = new Dictionary<EngineID, MsgEngine>();
        /// <summary>
        /// 将引擎添加到引擎容器
        /// </summary>
        /// <param name="id">引擎ID</param>
        /// <param name="engine">消息引擎对象</param>
        public static void Add(EngineID id, MsgEngine engine)
        {
            if (gEngineMap.ContainsKey(id) == false)
            {
                gEngineMap.Add(id, engine);
            }
        }


        /// <summary>
        /// 移出引擎
        /// </summary>
        /// <param name="id">引擎ID</param>
        public static void Remove(EngineID id)
        {
            if (gEngineMap.ContainsKey(id))
            {
                gEngineMap.Remove(id);
            }
        }

        /// <summary>
        /// 启动引擎
        /// </summary>
        public static void Start()
        {
            foreach (MsgEngine engine in gEngineMap.Values)
            {
                engine.StartWork();
            }
        }

        /// <summary>
        /// 停止引擎
        /// </summary>
        public static void Stop()
        {
            foreach (MsgEngine engine in gEngineMap.Values)
            {
                engine.StopWork();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="engineID">引擎ID</param>
        public static void SendMsg(Msg msg, EngineID engineID = EngineID.Unknow)
        {
            // 默认向消息中心发送
            if (engineID == EngineID.Unknow)
            {
                engineID = EngineID.MsgCenter;
            }

            MsgEngine engine = null;
            if (gEngineMap.TryGetValue(engineID, out engine))
            {
                engine.PutMsg(msg);
            }


        }
    }
}