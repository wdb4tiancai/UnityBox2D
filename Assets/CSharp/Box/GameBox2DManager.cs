
using Box2DGame;
using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBox2DManager : MonoBehaviour
{

    public UnityEngine.Vector2 mousePosition;

    public GameWorld GameWorld;
    public GameObject HeroPrefab;
    public GameObject MonsterPrefab;
    public GameObject BulletPrefab;

    private List<MonsterAgent> m_MonsterAgents;
    private List<Bullet> m_Bullets;
    private HeroAgent m_HeroAgent;

    public static GameBox2DManager instance;

    private float m_CreateBulletTime = 0;
    public int MaxMonster = 200;

    public static GameBox2DManager Instance
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
        GameWorld = new GameWorld();
        GameWorld.InitWorld(ContactCallBack);
        m_MonsterAgents = new List<MonsterAgent>();
        m_Bullets = new List<Bullet>();
        GameObject heroObj = GameObject.Instantiate(HeroPrefab);
        m_HeroAgent = heroObj.GetComponent<HeroAgent>();
        m_HeroAgent.InitBoby();
    }

    private void ContactCallBack(int bulletId, int monsterId)
    {
        for (int i = 0; i < m_Bullets.Count; i++)
        {
            if (m_Bullets[i].Guid == bulletId)
            {
                m_Bullets[i].IsDelete = true;
            }
        }
        for (int i = 0; i < m_MonsterAgents.Count; i++)
        {
            if (m_MonsterAgents[i].Guid == monsterId)
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
                m_Bullets[i].DeleteBody();
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
                m_MonsterAgents[i].DeleteBody();
                GameObject.Destroy(m_MonsterAgents[i].gameObject);
                m_MonsterAgents.RemoveAt(i);
            }
            else
            {
                m_MonsterAgents[i].UpdatePos();
            }
        }

        GameWorld.UpdateWorld();
        m_HeroAgent.UpdateShowPosition();
        for (int i = 0; i < m_MonsterAgents.Count; i++)
        {
            m_MonsterAgents[i].UpdateShowPosition();
        }
        for (int i = 0; i < m_Bullets.Count; i++)
        {
            m_Bullets[i].UpdateShowPosition();
        }
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
        MonsterAgent monsterAgent = monsterObj.GetComponent<MonsterAgent>();
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
            Bullet bulletAgent = bulletObj.GetComponent<Bullet>();
            bulletAgent.InitBoby(m_HeroAgent.transform, (float)x, (float)y);
            m_Bullets.Add(bulletAgent);

        }

    }
    // 将角度转换为弧度
    static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}