using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Box2DSharp.Testbed.Unity
{
    public class PhysicsUi : MonoBehaviour
    {
        public void BackMain()
        {
            SceneManager.LoadScene("Main");
        }

        public void AddMonster()
        {
            GamePhysicsManager.Instance.MaxMonster += 100;
            Debug.LogError($"怪物上限 {GamePhysicsManager.Instance.MaxMonster}");
        }
    }
}
