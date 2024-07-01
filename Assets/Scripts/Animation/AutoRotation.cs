using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.Animation
{
    public sealed class AutoRotation : MonoBehaviour
    {
        public float speed;
        void Update()
        {
            base.transform.eulerAngles += new Vector3(0, 0, speed) * Time.deltaTime;
        }
    }
}
