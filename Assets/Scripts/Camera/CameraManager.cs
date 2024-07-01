using LKZ.Commands.Camera;
using LKZ.DependencyInject;
using LKZ.Rolle;
using LKZ.TypeEventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LKZ.Camera
{
    public sealed class CameraManager : MonoBehaviour
    {
        [Serializable]
        public sealed class PosStruct
        {
            public Vector3 Pos;
            public Vector3 Rotation;

            [HideInInspector]
            internal Quaternion quaternion;
        }

        [SerializeField]
        private PosStruct[] PosStructs;

        [SerializeField]
        private float moveSpeed = 1f;

        private Transform camera_tf;

        [Inject]
        private IRegisterCommand RegisterCommand { get; set; }

        [Inject]
        private IRollePosition RollePosition { get; set; }

        private int posIndex = 0;

        private Coroutine moveCoroutine;

        private Vector3 cameraPosOffset;

        private bool isFollow;
        private void Start()
        {
            RegisterCommand.Register<SwitchCameraCommand>(SwitchCameraCommandCallback);
            camera_tf = transform.GetChild(0);

            for (int i = 0; i < PosStructs.Length; i++)
            {
                PosStructs[i].quaternion = Quaternion.Euler(PosStructs[i].Rotation);
            }

            camera_tf.localPosition = PosStructs[0].Pos;
            camera_tf.localRotation = PosStructs[0].quaternion;

            cameraPosOffset = camera_tf.position - RollePosition.RolleShowPosition;
            isFollow = true;
        }

        private void LateUpdate()
        {
            if (isFollow)
            {
                var currentPos= camera_tf.position;
                camera_tf.position= Vector3.Slerp(currentPos, RollePosition.Position + cameraPosOffset, Time.deltaTime*2);
            }
        }

        private void SwitchCameraCommandCallback(SwitchCameraCommand obj)
        {
            if (!object.ReferenceEquals(null, moveCoroutine))
                StopCoroutine(moveCoroutine);

            posIndex = ++posIndex % PosStructs.Length;
            moveCoroutine = StartCoroutine(MoveCameraCoroutine(PosStructs[posIndex].Pos, PosStructs[posIndex].quaternion));
        }

        private IEnumerator MoveCameraCoroutine(Vector3 targetPos, Quaternion quat)
        {
            isFollow = false;

            float t = 0;
            var currentPos = camera_tf.localPosition;
            var currentQu = camera_tf.localRotation;

            while (t < 1f)
            {
                t += Time.deltaTime * this.moveSpeed;

                t = Mathf.Clamp01(t);

                camera_tf.localPosition = currentPos = Vector3.Slerp(currentPos, targetPos, t);
                camera_tf.localRotation = currentQu = Quaternion.Slerp(currentQu, quat, t);
                yield return null;
            }
            moveCoroutine = null;

            cameraPosOffset = camera_tf.position - RollePosition.RolleShowPosition;
            isFollow = true;

        }
    }
}
