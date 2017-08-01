using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LocalMsg
{
    public enum MsgID
    {

        _UnKnow = 0, // 最小值            

        // 动态增加和减少处理函数
        I_AddMsgHandler,
        I_RemoveMsgHandler,

        // 网络引擎
        ConnectNet, // 连接网络
        DisconnectNet, // 断开网络
        NetStatus, // 网络连接还是断开的状态
        NetConnectStatus, // 网络连接状态文本
        SetNetConfig, // 设置网络配置
        NetConfigInfor, // 获得配置消息
        RequestDeviceData, // 订阅设备数据
        RequestJiguiData, //订阅机柜数据        

        // 初始化机房，用于机房切换
        InitRoomData,
        GetJiguiData, // =10 创建板卡UI后，重新获得板卡数据
        SelectJigui, // UI选中机柜后，用来控制当前灯，并闪亮

        // 设备状态改变
        DeviceStatus,
        BankaStatus,
        LightStatus,
        CXLightStatus,
        SwitchStatus,
        JiguiStatus,

        // 3d 设定消息
        Focus3DObject, // 将相机聚焦到选中物体
        DrawOutline, // 绘制轮廓线
        SetRoomShow, // 设置房间是否显示
        SetTrainShow, // 设置火车是否显示
        SetMetroShow,//设置地铁是否显示
        SetGameObjectShow, // 设置游戏对象是否显示
        SetRoomSize, // 设置房间尺寸
        SetColor, // 板卡，灯泡色彩
        SetMaterial, // 显示屏材质
        SetPosition, // 机柜位置 = 20
        SetRotation, // 开关文字
        LockCamera, // 锁定相机
        MoveCamera,
        RotationCamera180,
        AddLightObj,
        CXSetLight,


        // UI 设定
        PlaybackEnable, // 回放模式是否有效
        SetUIStatusText, // 设定UI设备和板卡状态的文字
        CreateUIDeviceList, // 创建UI设备列表
        CreateUIBankaList,// 创建UI板卡列表
        SelectUIDeviceList, // 选择UI设备列表上的机柜选项
                            // 添加日志
        AddLogText,

        // 单机数据
        LoadLocalData,
        OutputLocalData,

        //Message tester
        BuildSafeNetMessage,

        //
        SetMessageTestBtnState,

        CheckMessageFileExist,

        ExecuteMessage,

        SetCurrentRoomJG,

        MAX = 1000 // 最大值
    }
}
