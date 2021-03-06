using System;
using UnityEngine;
using CairoEngine;
using CairoEngine.MachineLearning;

namespace CairoEngine.MachineLearning.Sensers.Children
{
    public class GridUnit : MonoBehaviour
    {
        public GameObject collidingObject = null;

        void OnCollisionEnter(Collision collision)
        {
            collidingObject = collision.gameObject;
        }
    }
}
