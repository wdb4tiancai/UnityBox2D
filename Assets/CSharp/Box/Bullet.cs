using Box2DGame;
using Box2DSharp.Dynamics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool IsDelete { get; set; } = false;
    public int Guid { get; private set; } = 0;
    Transform m_HeroTransform;
    BodyMgr m_BodyMgr;
    Body m_Body;
    private float m_Time;

    private System.Numerics.Vector2 m_MoveV;
    public void InitBoby(Transform heroTransform, float x, float y)
    {
        m_HeroTransform = heroTransform;
        m_BodyMgr = GameBox2DManager.instance.GameWorld.BodyMgr;

        Vector3 pos = m_HeroTransform.position + new Vector3(x, y, 0);
        Vector3 movev = (pos - m_HeroTransform.position).normalized;
        m_MoveV = new System.Numerics.Vector2(movev.x, movev.y) * 7;
        Guid = m_BodyMgr.CreateRoundBulletBody(new System.Numerics.Vector2(pos.x, pos.y), 0, 0.5f,
            0);
        m_Body = m_BodyMgr.GetBodyByGuid(Guid);
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
        m_Time += Time.deltaTime;
        if (m_Time >= 5)
        {
            IsDelete = true;
            return;
        }
        m_Body.SetLinearVelocity(m_MoveV);
    }

    public void UpdateShowPosition()
    {
        if (m_Body == null)
        {
            return;
        }
        if (m_Time >= 5)
        {
            return;
        }
        System.Numerics.Vector2 pos = m_Body.GetPosition();
        transform.position = new UnityEngine.Vector3(pos.X, pos.Y);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_Body.GetAngle()));
    }
}