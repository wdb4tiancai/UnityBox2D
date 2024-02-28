using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box2DGame
{
    public class GameWorld : IContactListener
    {
        //世界对象
        public World World { get; private set; } = null;
        //对象管理器
        public BodyMgr BodyMgr { get; private set; } = null;
        //更新间隔
        private float m_UpdateInterval = 1.0f / 30.0f;
        //当前更新时间
        private float m_CurUpdateTime = 0.0f;

        //碰撞事件
        //子弹 被攻击对象
        private Action<int, int> m_ContactCallBack;

        //初始化世界
        public void InitWorld(Action<int, int> contactCallBack)
        {
            m_ContactCallBack = contactCallBack;
            //初始化世界对象
            World = new World(new System.Numerics.Vector2(0f, 0f));
            World.AllowSleep = true;//允许世界休眠
            World.WarmStarting = true;//热启动
            World.ContinuousPhysics = true;//启用连续碰撞
            World.SubStepping = true;//启用单步连续物理。
            World.SetContactListener(this);

            //初始化对象管理器
            BodyMgr = new BodyMgr();
            BodyMgr.InitBodyMgr(World);
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
            m_CurUpdateTime += Time.deltaTime;
            if (m_CurUpdateTime >= m_UpdateInterval)
            {
                m_CurUpdateTime -= m_UpdateInterval;
                World.Step(m_UpdateInterval, 3, 3);
            }

        }

        //是否初始化
        public bool IsInit()
        {
            return World != null;
        }

        public void BeginContact(Contact contact)
        {
            if (contact == null)
            {
                return;
            }
            if (!contact.FixtureA.Body.IsBullet && !contact.FixtureB.Body.IsBullet)
            {
                return;
            }
            if (contact.FixtureA.Body.IsBullet)
            {
                m_ContactCallBack?.Invoke(BodyMgr.GetGuidByBody(contact.FixtureA.Body), BodyMgr.GetGuidByBody(contact.FixtureB.Body));
            }
            else
            {
                m_ContactCallBack?.Invoke(BodyMgr.GetGuidByBody(contact.FixtureB.Body), BodyMgr.GetGuidByBody(contact.FixtureA.Body));
            }
        }

        public void EndContact(Contact contact)
        {
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
        }
    }
}

