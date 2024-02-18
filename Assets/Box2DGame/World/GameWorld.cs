using Box2DSharp.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box2DGame
{
    public class GameWorld
    {
        //世界对象
        public World World { get; private set; } = null;
        //对象管理器
        public BodyMgr BodyMgr { get; private set; } = null;

        //初始化世界
        public void InitWorld()
        {
            //初始化世界对象
            World = new World(new System.Numerics.Vector2(0f, 0f));
            World.AllowSleep = true;//允许世界休眠
            World.WarmStarting = true;//热启动
            World.ContinuousPhysics = true;//启用连续碰撞
            World.SubStepping = true;//启用单步连续物理。
            //初始化对象管理器
            BodyMgr = new BodyMgr();
            BodyMgr.InitBodyMgr(this);
        }

        //删除世界
        public void DeleteWorld()
        {
            if (World == null)
            {
                return;
            }
            if (BodyMgr != null)
            {
                BodyMgr.DeleteBodyMgr();
                BodyMgr = null;
            }
            World = null;
        }

        //更新世界
        public void UpdateWorld()
        {
            World.Step(1.0f / 10.0f, 3, 3);
        }

        //是否初始化
        public bool IsInit()
        {
            return World != null;
        }

    }
}

