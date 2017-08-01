using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LocalMsg
{
    public class MsgHandlerMap
    {

        /// <summary>
        /// 消息映射表   消息ID - 引擎ID列表
        /// </summary>
        static Dictionary<int, List<EngineID>> m_msgMap = new Dictionary<int, List<EngineID>>();

        /// <summary>
        /// 添加消息映射项
        /// </summary>
        /// <param name="msgID">消息ID</param>
        /// <param name="engineID">引擎ID</param>
        public static void Add(int msgID, EngineID engineID)
        {
            lock (m_msgMap)
            {
                List<EngineID> elist;
                if (m_msgMap.TryGetValue(msgID, out elist))
                {
                    // 在引擎列表中传入查找比较委托
                    if (elist.Find(eid =>
                        eid == engineID) == 0)
                    {
                        elist.Add(engineID);
                    }
                }
                else
                {
                    elist = new List<EngineID>();
                    elist.Add(engineID);
                    m_msgMap.Add(msgID, elist);
                }
            }
        }

        /// <summary>
        /// 删除消息映射项
        /// </summary>
        /// <param name="msgID">消息ID</param>
        /// <param name="engineID">引擎ID</param>
        public static void Remove(int msgID, EngineID engineID)
        {
            lock (m_msgMap)
            {
                List<EngineID> elist;
                if (m_msgMap.TryGetValue(msgID, out elist))
                {
                    elist.Remove(engineID);
                    if (elist.Count == 0) m_msgMap.Remove(msgID);
                }
            }
        }


        /// <summary>
        /// 获取消息处理引擎列表
        /// </summary>
        /// <param name="msgID">消息ID</param>
        /// <returns>处理该消息的引擎ID列表</returns>
        public static List<EngineID> GetEngineIDList(int msgID)
        {
            lock (m_msgMap)
            {
                List<EngineID> oldList = null;
                // 拷贝复制一个新对象，防止多线程使用出现错误
                List<EngineID> newList = null;
                if (m_msgMap.TryGetValue(msgID, out oldList))
                {
                    newList = new List<EngineID>();
                    foreach (EngineID id in oldList)
                    {
                        newList.Add(id);
                    }
                }
                return newList;
            }
        }
    }
}
