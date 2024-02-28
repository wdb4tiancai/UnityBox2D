using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Box2DSharp.Testbed.Unity
{
    public class MainUi : MonoBehaviour
    {
        public void Box()
        {
            SceneManager.LoadScene("Box2d");
        }

        public void Physics()
        {
            SceneManager.LoadScene("Physics");
        }
    }
}
