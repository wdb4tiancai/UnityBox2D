using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics.Contacts;

namespace Box2DSharp.Dynamics
{
    /// 实现这个类以获取接触信息。您可以使用这些结果进行声音和游戏逻辑等操作。
    /// 您还可以通过在时间步长后遍历接触列表来获取接触结果。然而，由于连续物理引擎会导致子步长，因此您可能会错过一些接触。
    /// 此外，您可能会在单个时间步长内收到同一个接触的多个回调。
    /// 您应该努力使您的回调函数高效，因为每个时间步长可能会有许多回调。
    /// 警告：您不能在这些回调中创建/销毁 Box2D 实体。
    public interface IContactListener
    {
        /// 当两个 Fixture 开始接触时调用。
        void BeginContact(Contact contact);

        /// 当两个 Fixture 停止接触时调用。
        void EndContact(Contact contact);

        /// 在接触更新之前调用。这允许您检查接触点和接触 manifold，并在需要时修改它们。
        /// 提供了旧的 manifold 的副本，以便您可以检测到更改。
        /// 注意：仅对唤醒的物体调用此方法。
        /// 注意：即使接触点的数量为零，也会调用此方法。
        /// 注意：传感器不会调用此方法。
        /// 注意：如果您将接触点的数量设置为零，则不会收到 EndContact 回调。但是，您可能会在下一步骤中收到 BeginContact 回调。
        void PreSolve(Contact contact, in Manifold oldManifold);

        /// 在接触更新之后调用。这让您可以检查求解器完成后的接触。
        /// 注意：接触 manifold 不包括时间点冲量，如果子步长很小，时间点冲量可以任意大。因此，冲量是以明确的方式提供的，而不是作为一个单独的数据结构。
        /// 注意：仅对正在接触、实心和唤醒的接触调用此方法。
        void PostSolve(Contact contact, in ContactImpulse impulse);
    }
}
