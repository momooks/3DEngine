using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalMsg
{
    /// <summary>
    /// 引擎ID定义
    /// </summary>
    public enum EngineID
    {
        Unknow,    // 未知对象，可用于默认参数 
        MsgCenter,  // 消息中心
        Logical,    // 业务引擎
        Net,        // 网络引擎
        UI,         // UI 管理
        Scene3D,    // 3D 管理 
        U3DMainRun,  // U3D 所有代码入口对象，用于杂项管理
        MessageTest
    }
}
