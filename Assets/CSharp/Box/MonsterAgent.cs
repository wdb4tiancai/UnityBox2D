using Box2DGame;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using UnityEngine;


public class MonsterAgent : MonoBehaviour
{
    public bool IsDelete { get; set; } = false;
    public int Guid { get; private set; } = 0;
    Transform m_HeroTransform;
    BodyMgr m_BodyMgr;
    Body m_Body;
    private float m_Speed;
    public void InitBoby(Transform heroTransform)
    {
        m_Speed = Random.Range(2f, 4f);
        m_HeroTransform = heroTransform;
        m_BodyMgr = GameBox2DManager.instance.GameWorld.BodyMgr;
        Guid = m_BodyMgr.CreateMonsterBody(new System.Numerics.Vector2(Random.Range(-60, 60), Random.Range(-30, 30)), 0, 0.5f, 0);
        m_Body = m_BodyMgr.GetBodyByGuid(Guid);
    }

    public void DeleteBody()
    {
        m_BodyMgr.DeleteBodyByGuid(Guid);
    }

    public void UpdatePos()
    {
        if (m_Body == null)
        {
            return;
        }
        Vector3 moveVec = (m_HeroTransform.position - transform.position).normalized;
        m_Body.SetLinearVelocity(new System.Numerics.Vector2(moveVec.x, moveVec.y) * m_Speed);

    }

    public void UpdateShowPosition()
    {
        if (m_Body == null)
        {
            return;
        }
        System.Numerics.Vector2 pos = m_Body.GetPosition();
        transform.position = new UnityEngine.Vector3(pos.X, pos.Y);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_Body.GetAngle()));
    }
}