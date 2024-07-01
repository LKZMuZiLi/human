using System.Collections;
using UnityEngine;

namespace LKZ.Rolle
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RolleControl : MonoBehaviour
    {
        private static readonly int talk_AnimationID = Animator.StringToHash("talk");
        private static readonly int dance_AnimationID = Animator.StringToHash("dance");
        private static readonly int salute_AnimationID = Animator.StringToHash("salute");


        private AudioSource audioSource;

        public AudioSource AudioSource
        {
            get
            {
                return audioSource ??= GetComponent<AudioSource>();
            }
        }

        public bool AnimationRoot { get; set; }

        private Animator _animator;

        /// <summary>
        /// 行礼动画持续时间
        /// </summary>
        public float saluteDurationTime = 4.5f;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            if (!AnimationRoot)
                return;

            base.transform.localPosition += _animator.deltaPosition;
            base.transform.localRotation *= _animator.deltaRotation;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            SetLock(UnityEngine.Camera.main.transform.position);
        }

        internal void RolleEnable()
        {
            gameObject.SetActive(true);
        }

        internal void RolleDisable()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 触发行礼动画
        /// </summary>
        internal void TriggerSaluteAnimation()
        {
            if (!object.ReferenceEquals(null, TargetIKWeightCor))
                StopCoroutine(TargetIKWeightCor);
            TargetIKWeightCor = StartCoroutine(SetTargetIKWeight(0));
            _animator.SetTrigger(salute_AnimationID);
            TargetIKWeightCor = StartCoroutine(pp());
            IEnumerator pp()
            {
                yield return new WaitForSeconds(saluteDurationTime);
                StartCoroutine(SetTargetIKWeight(1));
            }
        }

        internal virtual void SetAnimationTalk(bool value)
        {
            if (!object.ReferenceEquals(null, TargetIKWeightCor))
                StopCoroutine(TargetIKWeightCor);
            TargetIKWeightCor = StartCoroutine(SetTargetIKWeight(1));
            _animator.SetBool(talk_AnimationID, value);
        }

        internal virtual void SetDanceAnimation(int value)
        {
            if (!object.ReferenceEquals(null, TargetIKWeightCor))
                StopCoroutine(TargetIKWeightCor);
            TargetIKWeightCor = StartCoroutine(SetTargetIKWeight(0));
            _animator.SetInteger(dance_AnimationID, value);
        }

        private float ik_Weight;
        Coroutine TargetIKWeightCor;
        internal void SetLock(Vector3 pos)
        {
            _animator.SetLookAtPosition(pos);
            _animator.SetLookAtWeight(ik_Weight, 0, ik_Weight, ik_Weight);
        }

        IEnumerator SetTargetIKWeight(float target)
        {
            float currentWeight = ik_Weight;
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * 2;
                ik_Weight = Mathf.Lerp(currentWeight, target, t);
                yield return null;
            }
        }
    }
}