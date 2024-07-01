using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace LKZ.Rolle
{
    public sealed class BlendSpapesControl : MonoBehaviour
    {
        [Serializable]
        public class Struct
        {
            public int blendSpapeIndex;

            public float min = 0, max = 100;

            public float interval = 5f;

            public float speed = 1f;
             
            internal float SmoothStep(in float t)
            {
                return Mathf.SmoothStep(min, max, t);   
            }
        }

        [SerializeField]
        private Struct[] blendSpapesStruct;

        [SerializeField]
        private SkinnedMeshRenderer meshRenderer;

        private void OnEnable()
        {
            foreach (var item in blendSpapesStruct)
            {
                StartCoroutine(ControlClendSpapesCoroutine(item));
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        [BurstCompile] 
        private IEnumerator ControlClendSpapesCoroutine(Struct value)
        {
            float t = 0;
            WaitForSeconds waitFor = new WaitForSeconds(value.interval);
            while (true)
            {
                t = 0;

                while (t < 1f)
                {
                    t += Time.deltaTime * value.speed;
                    t = math.clamp(t, 0, 1); 
                    meshRenderer.SetBlendShapeWeight(value.blendSpapeIndex, value.SmoothStep(t));

                    yield return null;
                }

                yield return null;

                while (t > 0)
                {
                    t -= Time.deltaTime * value.speed;
                    t = math.clamp(t,0,1); 

                    meshRenderer.SetBlendShapeWeight(value.blendSpapeIndex, value.SmoothStep(t));
                    yield return null;
                }

                yield return waitFor;
            }
        }
    }
}
