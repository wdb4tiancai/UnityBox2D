using Box2DSharp.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Box2DGame
{
    //碰撞定义
    public class GameWorldFilter
    {

        //标记分配
        private const ushort StaticMapLayer = 0x0001;//建筑
        private const ushort Hero = 0x0002;//玩家
        private const ushort AI = 0x0004;//小怪
        private const ushort BigAI = 0x0008;//大怪
        private const ushort FlyAI = 0x0010;//飞行怪
        private const ushort FlyPet = 0x0020;//飞行宠物
        private const ushort HeroBullet = 0x0040;//玩家子弹
        private const ushort AIBullet = 0x0080;//AI子弹
        private const ushort ALLBullet = 0x0100;//全局子弹
        private const ushort BattleProp = 0x0200;//掉落物
        private const ushort BossFence = 0x0400;//boss墙

        //0x0800
        //0x1000
        //0x2000
        //0x4000
        //0x8000

        //Filter对象
        public static Filter StaticMapLayerFilter;//建筑
        public static Filter HeroFilter;//玩家
        public static Filter AIFilter;//小怪
        public static Filter BigAIFilter;//大怪
        public static Filter FlyAIFilter;//飞行怪
        public static Filter FlyPetFilter;//飞行宠物
        public static Filter HeroBulletFilter;//玩家子弹
        public static Filter AIBulletFilter;//AI子弹
        public static Filter ALLBulletFilter;//全局子弹
        public static Filter BattlePropFilter;//掉落物
        public static Filter BossFenceFilter;//boss墙



        static GameWorldFilter()
        {
            short groupIndex = 1;
            //初始化建筑
            StaticMapLayerFilter = CreateFilter(StaticMapLayer, Hero | AI | BigAI | HeroBullet | AIBullet | ALLBullet, 0);
            //玩家
            HeroFilter = CreateFilter(Hero, BossFence | AIBullet | ALLBullet, 0);
            //小怪
            AIFilter = CreateFilter(AI, AI | BigAI | ALLBullet | HeroBullet, groupIndex);
            groupIndex++;
            //大怪
            BigAIFilter = CreateFilter(BigAI, BigAI | ALLBullet | HeroBullet, groupIndex);
            groupIndex++;
            //飞行怪
            FlyAIFilter = CreateFilter(FlyAI, AI | BigAI | HeroBullet, groupIndex);
            groupIndex++;
            //飞行宠物
            FlyPetFilter = CreateFilter(FlyPet, HeroBullet, -2);
            //玩家子弹
            HeroBulletFilter = CreateFilter(HeroBullet, AI | BigAI | FlyAI | FlyPet | StaticMapLayer, -1);
            //AI子弹
            AIBulletFilter = CreateFilter(AIBullet, FlyAI | StaticMapLayer | Hero, -1);
            //全局子弹
            ALLBulletFilter = CreateFilter(ALLBullet, AI | BigAI | StaticMapLayer | Hero, -1);
            //掉落物
            BattlePropFilter = CreateFilter(BattleProp, 0, -1);
            //boss墙
            BossFenceFilter = CreateFilter(BossFence, Hero, 0);
        }

        private static Filter CreateFilter(ushort categoryBits, ushort maskBits, short groupIndex)
        {
            Filter filter = new Filter();
            filter.CategoryBits = categoryBits;
            filter.MaskBits = maskBits;
            filter.GroupIndex = groupIndex;
            return filter;
        }
    }
}

//待验证
//在 Box2D 中，`groupIndex` 的工作方式如下：
//1. 如果两个物体的 `groupIndex` 均为零，或者至少一个 `groupIndex` 为零，则会检查它们的 `categoryBits` 和 `maskBits` 来确定碰撞行为。
//2. 如果两个物体的 `groupIndex` 均为正数且相等，则它们之间的碰撞会被忽略。
//3. 如果其中一个物体的 `groupIndex` 为正数，而另一个物体的 `groupIndex` 为负数，则它们之间的碰撞将始终发生。
//4. 如果两个物体的 `groupIndex` 不相等且其中至少一个为负数，则它们之间的碰撞将始终发生。
//总结起来，`groupIndex` 用于确定是否忽略碰撞，具体规则如下：
//- 如果两个物体的 `groupIndex` 相等且为正数，则碰撞将被忽略。
//- 如果其中一个物体的 `groupIndex` 为正数，而另一个物体的 `groupIndex` 为负数，则碰撞将始终发生。
//- 如果其中至少一个物体的 `groupIndex` 为负数，则碰撞将始终发生。
//- 如果两个物体的 `groupIndex` 不相等且其中至少一个为负数，则碰撞将始终发生。
