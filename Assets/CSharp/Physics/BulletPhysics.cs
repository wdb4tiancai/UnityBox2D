using Box2DGame;
using Box2DSharp.Dynamics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public bool IsDelete { get; set; } = false;
    private float m_Time;
    public Rigidbody2D RigidBody2D { private set; get; }

    private Vector3 m_MoveV;
    private Action<GameObject, GameObject> m_ContactCallBack;
    public void InitBoby(Transform heroTransform, float x, float y, Action<GameObject, GameObject> contactCallBack)
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
        m_ContactCallBack = contactCallBack;
        Vector3 pos = heroTransform.position + new Vector3(x, y, 0);
        transform.position = pos;
        m_MoveV = ((pos - heroTransform.position).normalized) * 7;
    }

    static float CalculateAngle(float deltaX, float deltaY)
    {
        // 计算两点之间的角度
        double radians = Math.Atan2(deltaY, deltaX);

        // 将弧度转换为角度
        double degrees = radians * (180 / Math.PI);

        // 由于 Atan2 的范围是 (-π, π]，将角度转换为正值
        if (degrees < 0)
        {
            degrees += 360;
        }

        return (float)degrees;
    }


    public void UpdatePos()
    {
        if (RigidBody2D == null)
        {
            return;
        }
        m_Time += Time.deltaTime;
        if (m_Time >= 5)
        {
            IsDelete = true;
            return;
        }
        RigidBody2D.velocity = m_MoveV;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_ContactCallBack?.Invoke(gameObject, other.gameObject);
    }
}