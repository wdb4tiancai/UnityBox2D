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
        private GameWorld m_GameWorld;
        //所有者运行的Body集合
        private Dictionary<int, Body> m_BodyList;

        //初始化BodyMgr
        public void InitBodyMgr(GameWorld world)
        {
            m_GameWorld = world;
            m_BodyList = new Dictionary<int, Body>();
        }

        //删除BodyMgr
        public void DeleteBodyMgr()
        {
            foreach (Body boby in m_BodyList.Values)
            {
                m_GameWorld.World.DestroyBody(boby);
            }
            m_BodyList.Clear();
            GuidIndex = 0;
            m_GameWorld = null;
        }


        /// <summary>
        /// 创建的是一个建筑阻挡
        /// </summary>
        /// <param name="pos"></param>
        private void CreateBuildBody(System.Numerics.Vector2 pos, float width, float height, float angle)
        {
            if (m_GameWorld?.IsInit() != true)
            {
                return;
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
            Body body = m_GameWorld.World.CreateBody(bodyDef);

            //形状
            PolygonShape shape = new PolygonShape();

            //便捷方法创建一个矩形
            shape.SetAsBox(width, height);

            //夹具
            var fixtureDef = new FixtureDef
            {
                Shape = shape,//形状
                Density = GameWorldConstants.Density,//密度
                Friction = GameWorldConstants.Friction//摩擦系数
            };
            body.LinearDamping = GameWorldConstants.LinearDamping;//线性阻尼
            body.AngularDamping = GameWorldConstants.AngularDamping;//角阻尼
            //将boby和夹具相关联
            body.CreateFixture(fixtureDef);

            //添加body到集合
            m_BodyList.Add(GetGuid(), body);
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
