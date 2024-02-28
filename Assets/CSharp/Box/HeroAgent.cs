using Box2DGame;
using Box2DSharp.Dynamics;
using UnityEngine;


public class HeroAgent : MonoBehaviour
{
    BodyMgr m_BodyMgr;
    Body m_Body;
    int m_Guid;
    private float m_Speed;
    public void InitBoby()
    {
        m_Speed = 6;
        m_BodyMgr = GameBox2DManager.instance.GameWorld.BodyMgr;
        m_Guid = m_BodyMgr.CreateHeroBody(new System.Numerics.Vector2(0, 0), 0.5f, 0);
        m_Body = m_BodyMgr.GetBodyByGuid(m_Guid);
    }

    public void DeleteBody()
    {
        m_BodyMgr.DeleteBodyByGuid(m_Guid);
    }

    public void UpdatePos()
    {
        if (m_Body == null)
        {
            return;
        }

        Vector3 monsterPos = transform.position;
        Vector3 moveVec = new Vector3(GameBox2DManager.Instance.mousePosition.x, GameBox2DManager.Instance.mousePosition.y, 0) - monsterPos;
        moveVec = moveVec.normalized;


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