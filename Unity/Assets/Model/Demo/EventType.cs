using System;
using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct ChangePosition
        {
            public Unit Unit;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }

        public struct PingChange
        {
            public Scene ZoneScene;
            public long Ping;
        }
        
        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }
        
        public struct AfterCreateLoginScene
        {
            public Scene LoginScene;
        }

        public struct AppStartInitFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginFinish
        {
            public Scene ZoneScene;
        }

        public struct LoadingBegin
        {
            public Scene Scene;
        }

        public struct LoadingFinish
        {
            public Scene Scene;
        }

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }
        
        public struct MoveStart
        {
            public Unit Unit;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        public struct DataUpdate
        {
            public DataType DataType;
        }

        /// <summary>
        /// 红点节点值改变
        /// </summary>
        public struct ReddotNodeValueChange
        {
            public string ReddotNodePath;
            public int NewValue;
        }

        /// <summary>
        /// 红点节点数量改变
        /// </summary>
        public struct ReddotNodeNumChange
        {
            
        }

        /// <summary>
        /// 服务器传送Unit完成后
        /// </summary>
        public struct AfterServerTransferUnit
        {
            public int TransferMapIndex;
        }
        
    }
}