
using Box2DGame;
using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePhysicsManager : MonoBehaviour
{

    public UnityEngine.Vector2 mousePosition;
    public GameObject HeroPrefab;
    public GameObject MonsterPrefab;
    public GameObject BulletPrefab;

    private List<MonsterPhysicsAgent> m_MonsterAgents;
    private List<BulletPhysics> m_Bullets;
    private HeroPhysicsAgent m_HeroAgent;

    public static GamePhysicsManager instance;

    private float m_CreateBulletTime = 0;

    public int MaxMonster = 200;

    public static GamePhysicsManager Instance
    {
        get { return instance; }
    }



    void Awake()
    {
        MaxMonster = 200;
        instance = this;
    }

    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        m_MonsterAgents = new List<MonsterPhysicsAgent>();
        m_Bullets = new List<BulletPhysics>();
        GameObject heroObj = GameObject.Instantiate(HeroPrefab);
        m_HeroAgent = heroObj.GetComponent<HeroPhysicsAgent>();
        m_HeroAgent.InitBoby();
    }

    private void ContactCallBack(GameObject bulletobj, GameObject monsterObj)
    {
        for (int i = 0; i < m_Bullets.Count; i++)
        {
            if (m_Bullets[i].gameObject == bulletobj)
            {
                m_Bullets[i].IsDelete = true;
            }
        }
        for (int i = 0; i < m_MonsterAgents.Count; i++)
        {
            if (m_MonsterAgents[i].gameObject == monsterObj)
            {
                m_MonsterAgents[i].IsDelete = true;
            }
        }
    }

    void Update()
    {

        // 发射射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 检测射线是否击中了地面
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 如果击中了地面，打印击中点的世界坐标
            mousePosition.x = hit.point.x;
            mousePosition.y = hit.point.y;
            // 这里可以执行其他逻辑，比如在点击位置生成一个物体等等

        }

        m_HeroAgent.UpdatePos();
        for (int i = m_Bullets.Count - 1; i >= 0; i--)
        {
            if (m_Bullets[i].IsDelete)
            {
                GameObject.Destroy(m_Bullets[i].gameObject);
                m_Bullets.RemoveAt(i);
            }
            else
            {
                m_Bullets[i].UpdatePos();
            }
        }

        for (int i = m_MonsterAgents.Count - 1; i >= 0; i--)
        {
            if (m_MonsterAgents[i].IsDelete)
            {
                GameObject.Destroy(m_MonsterAgents[i].gameObject);
                m_MonsterAgents.RemoveAt(i);
            }
            else
            {
                m_MonsterAgents[i].UpdatePos();
            }
        }


        Physics2D.Simulate(Time.deltaTime);

        if (m_MonsterAgents.Count < MaxMonster)
        {
            CreateMonster();
        }
        m_CreateBulletTime += Time.deltaTime;
        if (m_CreateBulletTime > 1)
        {
            m_CreateBulletTime = 0;
            CreateBullet();
        }
    }

    private void CreateMonster()
    {
        GameObject monsterObj = GameObject.Instantiate(MonsterPrefab);
        MonsterPhysicsAgent monsterAgent = monsterObj.GetComponent<MonsterPhysicsAgent>();
        monsterAgent.InitBoby(m_HeroAgent.transform);
        m_MonsterAgents.Add(monsterAgent);
    }
    private void CreateBullet()
    {
        int numPoints = 50;
        double radius = 0.2;

        for (int i = 0; i < numPoints; i++)
        {
            // 计算角度
            double angle = i * (360.0 / numPoints);

            // 将极坐标转换为笛卡尔坐标
            double x = radius * Math.Cos(DegreesToRadians(angle));
            double y = radius * Math.Sin(DegreesToRadians(angle));
            GameObject bulletObj = GameObject.Instantiate(BulletPrefab);
            BulletPhysics bulletAgent = bulletObj.GetComponent<BulletPhysics>();
            bulletAgent.InitBoby(m_HeroAgent.transform, (float)x, (float)y, ContactCallBack);
            m_Bullets.Add(bulletAgent);

        }

    }
    // 将角度转换为弧度
    static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}