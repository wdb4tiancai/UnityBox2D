namespace Box2DSharp.Dynamics
{
    /// 实现此类以提供碰撞过滤。 换句话说，您可以实施
    /// 如果您想更好地控制联系人创建，则使用此类。
    public sealed class DefaultContactFilter : IContactFilter
    {
        /// 如果应在这两个形状之间执行接触计算，则返回 true。
        /// @warning 出于性能原因，仅当 AABB 开始重叠时才会调用。
        public bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            var filterA = fixtureA.Filter;
            var filterB = fixtureB.Filter;

            if (filterA.GroupIndex == filterB.GroupIndex && filterA.GroupIndex != 0)
            {
                return filterA.GroupIndex > 0;
            }

            var collide = (filterA.MaskBits & filterB.CategoryBits) != 0
                       && (filterA.CategoryBits & filterB.MaskBits) != 0;
            return collide;
        }
    }
}