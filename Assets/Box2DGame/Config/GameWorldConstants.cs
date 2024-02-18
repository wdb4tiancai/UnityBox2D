using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Box2DGame
{

    //常量统一定义
    public class GameWorldConstants
    {
        //摩擦系数，通常在[0, 1] 范围内。
        public const float Friction = 0f;

        //密度，通常以 kg/m^2 为单位。
        //如果你想测试旋转角度，官方示例中推荐的Density为1，这会导致如果你添加力过小的话，物体发生旋转的角度可以忽略不计。
        //你需要更改这方面的内容。
        //（可以设置一个Density不为零的物体到非常远的地方，或者不放入到字典中，之后其余的物体Density=0。
        //官方手册中说明至少要有一个物体Density不为零否则会导致奇怪的计算出现）
        public const float Density = 1f;

        //线性阻尼用于模拟物体在移动过程中由于外部摩擦和空气阻力等因素而逐渐减速的效果。
        //如果将 LinearDamping 设置为 0，则物体在运动时将不会受到任何额外的减速影响。
        //当 LinearDamping 的值大于 0 时，物体的速度将以一个衰减的速率减少，这有助于使物体在模拟中更接近真实世界中的运动行为。
        public const float LinearDamping = 0.0f;

        //角阻尼用于模拟物体在旋转运动时由于摩擦和空气阻力等因素而逐渐减速的效果。
        //如果将 angularDamping 设置为 0，则物体在旋转时不会受到任何额外的减速影响。
        //当 angularDamping 的值大于 0 时，物体的角速度将以一个衰减的速率减少。
        public const float AngularDamping = 0.0f;
    }
}
