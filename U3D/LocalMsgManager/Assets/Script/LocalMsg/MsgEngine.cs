using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LocalMsg
{
    public class MsgEngine : ThreadTask
    {
        /// <summary>
        /// 引擎在一次循环中处理消息的个数
        /// </summary>
        public int ProMsgNum = -1;

        /// <summary>
        /// 引擎ID
        /// </summary>
        [HideInInspector]
        public EngineID ID { get; set; }

        /// <summary>
        /// 消息队列，先进先出
        /// </summary>
        protected Queue<Msg> m_msgQueue = new Queue<Msg>();

        /// <summary>
        /// 消息处理映射表
        /// </summary>
        Dictionary<int, MsgHandler> m_msgHandlerMap = new Dictionary<int, MsgHandler>();

        /// <summary>
        /// 临时链表对象
        /// </summary>
        List<Msg> m_proMsgList = new List<Msg>();

        /// <summary>
        /// 将消息放入池中
        /// </summary>
        /// <param name="msg">消息</param>
        public void PutMsg(Msg msg)
        {
            lock (m_msgQueue)
            {
                m_msgQueue.Enqueue(msg);
            }
        }

        /// <summary>
        /// 将消息列表放入池中
        /// </summary>
        /// <param name="msgList">消息列表</param>
        public void PutMsgList(List<Msg> msgList)
        {
            lock (m_msgQueue)
            {
                foreach (Msg msg in msgList)
                {
                    m_msgQueue.Enqueue(msg);
                }
            }
        }


        /// <summary>
        /// 向引擎管理对象添加引擎对象
        /// </summary>
        public void Register(EngineID id)
        {
            ID = id;
            EngineManager.Add(ID, this);
        }

        /// <summary>
        /// 启动消息处理线程
        /// </summary>
        public override void StartWork()
        {
            base.StartWork();
        }

        /// <summary>
        /// 停止消息处理线程
        /// </summary>
        public override void StopWork()
        {
            lock (m_msgQueue)
            { ////清除消息
                m_msgQueue.Clear();
            }
            base.StopWork();
        }


        /// <summary>
        /// 消息处理函数
        /// </summary>
        /// <param name="msg">消息</param>
        public virtual void MsgProc(Msg msg)
        { 
            MsgHandler msgHandler;
            if (m_msgHandlerMap.TryGetValue(msg.ID, out msgHandler))
            {
                msgHandler(msg);
            }
            Msg.Destory(msg); // 释放消息
        }

        /// <summary>
        /// 消息处理线程函数
        /// </summary>
        /// <param name="p">流逝的毫秒数</param>
        public override void Work()
        {
            // 引擎停止工作，不再处理消息
            if (m_workFlag == false) return;

            // 复制消息出来，防止线程竞争队列资源
            lock (m_msgQueue)
            {
                if (ProMsgNum == -1)
                {
                    while (m_msgQueue.Count > 0)
                    {
                        Msg msg = m_msgQueue.Dequeue();
                        m_proMsgList.Add(msg);
                    }
                }
                else
                {
                    for (int i = 0; i < ProMsgNum; ++i)
                    {
                        if (m_msgQueue.Count > 0)
                        {
                            Msg msg = m_msgQueue.Dequeue();
                            m_proMsgList.Add(msg);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            foreach (Msg msg in m_proMsgList)
            {
                MsgProc(msg);
            }

            m_proMsgList.Clear();
        }


        /// <summary>
        /// 添加消息处理函数
        /// </summary>
        /// <param name="msgID">消息ID</param>
        /// <param name="msgProc">消息处理函数</param>
        public void AddMsgHandler(int msgID, MsgHandler msgProc)
        {
            // Debug.Log("添加消息ID: " + msgID);
            if (msgProc == null) return;
            /*foreach(KeyValuePair<int, MsgHandler> kvp in m_msgHandlerMap)
            {
                int key = kvp.Key;
                Debug.Log("消息：" + key);
            }*/

            if (m_msgHandlerMap.ContainsKey(msgID) == false)
            {
                // 注册消息处理函数
                m_msgHandlerMap.Add(msgID, msgProc);
                // 注入消息路由表
                MsgHandlerMap.Add(msgID, ID);
            }
            else
            {
  
            }
        }


        /// <summary>
        /// 删除消息处理函数
        /// </summary>
        /// <param name="msgID">消息ID</param>
        public void RemoveMsgHandler(int msgID)
        {
            m_msgHandlerMap.Remove(msgID);
            MsgHandlerMap.Remove(msgID, ID);
        }
    }
}
