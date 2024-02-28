using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Box2DSharp.Testbed.Unity
{
    public class BoxUi : MonoBehaviour
    {
        public void BackMain()
        {
            SceneManager.LoadScene("Main");
        }

        public void AddMonster()
        {
            GameBox2DManager.Instance.MaxMonster += 100;
            Debug.LogError($"怪物上限 {GameBox2DManager.Instance.MaxMonster}");
        }
    }
}
