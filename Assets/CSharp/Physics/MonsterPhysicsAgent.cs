using Box2DGame;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using UnityEngine;


public class MonsterPhysicsAgent : MonoBehaviour
{
    public bool IsDelete { get; set; } = false;
    Transform m_HeroTransform;

    private float m_Speed;
    public Rigidbody2D RigidBody2D { private set; get; }

    public void InitBoby(Transform heroTransform)
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
        m_Speed = Random.Range(2f, 4f);
        m_HeroTransform = heroTransform;
        transform.position = new Vector3(Random.Range(-60, 60), Random.Range(-30, 30), 0);
    }



    public void UpdatePos()
    {
        if (RigidBody2D == null)
        {
            return;
        }
        Vector3 moveVec = (m_HeroTransform.position - transform.position).normalized;
        RigidBody2D.velocity = moveVec * m_Speed;

    }
}