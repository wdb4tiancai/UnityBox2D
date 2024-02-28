using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using System.Collections.Generic;
namespace Box2DGame
{
    /// <summary>
    /// 碰撞对象管理器
    /// 
    /// </summary>
    public class BodyMgr
    {
        //Body唯一id
        private int GuidIndex = 0;
        //碰撞世界对象
        private World m_World;
        //所有者运行的Body集合
        private Dictionary<int, Body> m_IdToBodyList;
        private Dictionary<Body, int> m_BodyToIdList;

        //初始化BodyMgr
        public void InitBodyMgr(World world)
        {
            m_World = world;
            m_IdToBodyList = new Dictionary<int, Body>();
            m_BodyToIdList = new Dictionary<Body, int>();
        }

        //删除BodyMgr
        public void DeleteBodyMgr()
        {
            foreach (Body boby in m_IdToBodyList.Values)
            {
                m_World.DestroyBody(boby);
            }
            m_IdToBodyList.Clear();
            m_BodyToIdList.Clear();
            GuidIndex = 0;
            m_World = null;
        }

        //根据id获得Body
        public Body GetBodyByGuid(int guid)
        {
            m_IdToBodyList.TryGetValue(guid, out Body body);
            return body;
        }

        //根据Body获得id
        public int GetGuidByBody(Body body)
        {
            int guid = 0;
            m_BodyToIdList.TryGetValue(body, out guid);
            return guid;
        }

        public void DeleteBodyByGuid(int guid)
        {
            if (m_IdToBodyList.TryGetValue(guid, out Body body))
            {
                m_IdToBodyList.Remove(guid);
                m_BodyToIdList.Remove(body);
                m_World.DestroyBody(body);
            }
        }


        /// <summary>
        /// 创建一个建筑阻挡
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public int CreateBuildBody(System.Numerics.Vector2 pos, float width, float height, float angle)
        {
            if (m_World == null)
            {
                return -1;
            }

            //Body对象
            BodyDef bodyDef = new BodyDef();
            //静态的
            bodyDef.BodyType = BodyType.StaticBody;
            //位置
            bodyDef.Position = pos;
            //角度
            bodyDef.Angle = angle;

            //物体工厂来创建
            Body body = m_World.CreateBody(bodyDef);

            //形状
            PolygonShape shape = new PolygonShape();

            //便捷方法创建一个矩形
            shape.SetAsBox(width, height);

            //夹具
            var fixtureDef = new FixtureDef
            {
                Shape = shape,//形状
                Density = GameWorldConstants.Density,//密度
                Friction = GameWorldConstants.Friction,//摩擦系数
                Filter = GameWorldFilter.StaticMapLayerFilter//碰撞遮罩
            };
            body.LinearDamping = GameWorldConstants.LinearDamping;//线性阻尼
            body.AngularDamping = GameWorldConstants.AngularDamping;//角阻尼
            //将boby和夹具相关联
            body.CreateFixture(fixtureDef);

            int guid = GetGuid();
            //添加body到集合
            m_IdToBodyList.Add(guid, body);
            m_BodyToIdList.Add(body, guid);
            return guid;
        }


        /// <summary>
        /// 创建一个角色
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="radius">半径</param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public int CreateHeroBody(System.Numerics.Vector2 pos, float radius, float angle)
        {
            if (m_World == null)
            {
                return -1;
            }

            //Body对象
            BodyDef bodyDef = new BodyDef();
            //静态的
            bodyDef.BodyType = BodyType.DynamicBody;
            //位置
            bodyDef.Position = pos;
            //角度
            bodyDef.Angle = angle;

            //物体工厂来创建
            Body body = m_World.CreateBody(bodyDef);

            //形状
            CircleShape shape = new CircleShape();

            //便捷方法创建一个圆
            shape.Radius = radius;

            //夹具
            var fixtureDef = new FixtureDef
            {
                Shape = shape,//形状
                Density = GameWorldConstants.Density,//密度
                Friction = GameWorldConstants.Friction,//摩擦系数
                Filter = GameWorldFilter.HeroFilter//碰撞遮罩
            };
            body.LinearDamping = GameWorldConstants.LinearDamping;//线性阻尼
            body.AngularDamping = GameWorldConstants.AngularDamping;//角阻尼
            //将boby和夹具相关联
            body.CreateFixture(fixtureDef);

            int guid = GetGuid();
            //添加body到集合
            m_IdToBodyList.Add(guid, body);
            m_BodyToIdList.Add(body, guid);
            return guid;
        }

        /// <summary>
        /// 创建一个怪
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="aiType">0小怪 1飞行怪 2big怪 3飞行宠物</param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public int CreateMonsterBody(System.Numerics.Vector2 pos, int aiType, float radius, float angle)
        {
            if (m_World == null)
            {
                return -1;
            }

            //Body对象
            BodyDef bodyDef = new BodyDef();
            //静态的
            bodyDef.BodyType = BodyType.DynamicBody;
            //位置
            bodyDef.Position = pos;
            //角度
            bodyDef.Angle = angle;

            //物体工厂来创建
            Body body = m_World.CreateBody(bodyDef);

            //形状
            CircleShape shape = new CircleShape();

            //便捷方法创建一个圆
            shape.Radius = radius;

            Filter friction;
            if (aiType == 1)
            {
                friction = GameWorldFilter.FlyAIFilter;
            }
            else if (aiType == 2)
            {
                friction = GameWorldFilter.BigAIFilter;
            }
            else if (aiType == 3)
            {
                friction = GameWorldFilter.FlyPetFilter;
            }
            else
            {
                friction = GameWorldFilter.AIFilter;
            }
            //夹具
            var fixtureDef = new FixtureDef
            {
                Shape = shape,//形状
                Density = GameWorldConstants.Density,//密度
                Friction = GameWorldConstants.Friction,//摩擦系数
                Filter = friction//碰撞遮罩
            };
            body.LinearDamping = GameWorldConstants.LinearDamping;//线性阻尼
            body.AngularDamping = GameWorldConstants.AngularDamping;//角阻尼
            //将boby和夹具相关联
            body.CreateFixture(fixtureDef);

            int guid = GetGuid();
            //添加body到集合
            m_IdToBodyList.Add(guid, body);
            m_BodyToIdList.Add(body, guid);
            return guid;
        }


        /// <summary>
        /// 创建一个圆形子弹
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="buttleSource">0玩家 1AI子弹 2全局</param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public int CreateRoundBulletBody(System.Numerics.Vector2 pos, int buttleSource, float radius, float angle)
        {
            if (m_World == null)
            {
                return -1;
            }

            //Body对象
            BodyDef bodyDef = new BodyDef();
            //静态的
            bodyDef.BodyType = BodyType.DynamicBody;
            //位置
            bodyDef.Position = pos;
            //角度
            bodyDef.Angle = angle;

            //物体工厂来创建
            Body body = m_World.CreateBody(bodyDef);
            body.IsBullet = true;
            //形状
            CircleShape shape = new CircleShape();

            //便捷方法创建一个圆
            shape.Radius = radius;

            Filter friction;
            if (buttleSource == 0)
            {
                friction = GameWorldFilter.HeroBulletFilter;
            }
            else if (buttleSource == 1)
            {
                friction = GameWorldFilter.AIBulletFilter;
            }
            else
            {
                friction = GameWorldFilter.ALLBulletFilter;
            }
            //夹具
            var fixtureDef = new FixtureDef
            {
                Shape = shape,//形状
                Density = GameWorldConstants.Density,//密度
                Friction = GameWorldConstants.Friction,//摩擦系数
                Filter = friction//碰撞遮罩
            };
            body.LinearDamping = GameWorldConstants.LinearDamping;//线性阻尼
            body.AngularDamping = GameWorldConstants.AngularDamping;//角阻尼
            //将boby和夹具相关联
            Fixture fixture = body.CreateFixture(fixtureDef);

            // 将夹具设置为触发器
            fixture.IsSensor = true;

            int guid = GetGuid();
            //添加body到集合
            m_IdToBodyList.Add(guid, body);
            m_BodyToIdList.Add(body, guid);
            return guid;
        }


        /// <summary>
        /// 创建一个掉落物
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public int CreateBattlePropBody(System.Numerics.Vector2 pos, float radius, float angle)
        {
            if (m_World == null)
            {
                return -1;
            }

            //Body对象
            BodyDef bodyDef = new BodyDef();
            //静态的
            bodyDef.BodyType = BodyType.DynamicBody;
            //位置
            bodyDef.Position = pos;
            //角度
            bodyDef.Angle = angle;

            //物体工厂来创建
            Body body = m_World.CreateBody(bodyDef);

            //形状
            CircleShape shape = new CircleShape();

            //便捷方法创建一个圆
            shape.Radius = radius;

            //夹具
            var fixtureDef = new FixtureDef
            {
                Shape = shape,//形状
                Density = GameWorldConstants.Density,//密度
                Friction = GameWorldConstants.Friction,//摩擦系数
                Filter = GameWorldFilter.BattlePropFilter//碰撞遮罩
            };
            body.LinearDamping = GameWorldConstants.LinearDamping;//线性阻尼
            body.AngularDamping = GameWorldConstants.AngularDamping;//角阻尼
            //将boby和夹具相关联
            body.CreateFixture(fixtureDef);

            int guid = GetGuid();
            //添加body到集合
            m_IdToBodyList.Add(guid, body);
            m_BodyToIdList.Add(body, guid);
            return guid;
        }


        //获得唯一id
        private int GetGuid()
        {
            GuidIndex++;
            return GuidIndex;
        }


    }
}
///BodyDef、Body、Shape、Fixture 它们之间的关系？
///在 Box2D 中，`BodyDef`、`Body`、`Shape` 和 `Fixture` 之间有着密切的关系，它们一起协同工作来定义和模拟物体的行为和形状。
///1. `BodyDef`（刚体定义）：`BodyDef` 是用来定义物体的属性和初始状态的。它包含了诸如物体的初始位置、速度、角度、类型（静态、动态或者运动学）、是否允许休眠等信息。在 Box2D 中，通过创建 `BodyDef` 对象并设置相应的属性，然后将其传递给世界（`b2World`）来创建一个物体。
///2. `Body`（刚体）：`Body` 是物体的抽象表示，它代表了物体在物理仿真中的实体。`Body` 包含了物体的运动状态（位置、速度、角度、角速度等），并负责物体的碰撞检测和响应。通过将 `BodyDef` 对象传递给世界创建函数，可以创建一个物体，并返回对应的 `Body` 对象。
///3. `Shape`（形状）：`Shape` 用来定义物体的几何形状。在 Box2D 中，形状可以是多边形、圆形、边界等。`Shape` 是一个抽象类，具体的形状类如 `PolygonShape`、`CircleShape` 等都是继承自它的。在创建物体时，需要为物体添加一个或多个形状，以定义物体的外观和碰撞区域。
///4. `Fixture`（夹具）：`Fixture` 是用来将形状与物体关联起来的组件。每个 `Fixture` 对象包含一个形状（如 `Shape` 对象）以及一些物理属性（如密度、摩擦系数、恢复系数等）。通过将形状添加到物体上，可以使用 `Fixture` 来模拟物体的碰撞、摩擦和弹性等行为。在 Box2D 中，通过将 `Fixture` 添加到 `Body` 上来为物体添加碰撞区域和物理属性。
///因此，这四个组件共同构成了 Box2D 中物体的定义和模拟过程。`BodyDef` 定义了物体的基本属性，`Body` 是物体的实体表示，`Shape` 用来定义物体的形状，而 `Fixture` 则将形状与物体关联起来，并定义了形状的物理属性。
///
