using Box2DGame;
using Box2DSharp.Dynamics;
using UnityEngine;


public class HeroPhysicsAgent : MonoBehaviour
{

    public Rigidbody2D RigidBody2D { private set; get; }
    private float m_Speed;
    public void InitBoby()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
        m_Speed = 6;
    }

    public void DeleteBody()
    {
    }

    public void UpdatePos()
    {
        if (RigidBody2D == null)
        {
            return;
        }

        Vector3 monsterPos = transform.position;
        Vector3 moveVec = new Vector3(GamePhysicsManager.Instance.mousePosition.x, GamePhysicsManager.Instance.mousePosition.y, 0) - monsterPos;
        moveVec = moveVec.normalized;

        RigidBody2D.velocity = moveVec * m_Speed;
    }

}