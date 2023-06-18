using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Enemy
{
    public class SpiderAnimation : MonoBehaviour
    {
        private const string DIR_X = "DirX";
        private const string DIR_Y = "DirY";
        private const string LAST_X = "Last_X";
        private const string LAST_Y = "Last_Y";
        private const string IS_MOVE = "IsMove";
        private const string IS_ATTACK = "IsAttacking";

        private Animator animator;
        [SerializeField] private Spider spider;

        private void Awake() => animator = GetComponent<Animator>();
        private void LateUpdate()
        {
            animator.SetFloat(DIR_X, spider.Direction.x);
            animator.SetFloat(DIR_Y, spider.Direction.y);
            animator.SetFloat(LAST_X, spider.LastDirection.x);
            animator.SetFloat(LAST_Y, spider.LastDirection.y);
            animator.SetBool(IS_MOVE, spider.IsWalking);
            animator.SetBool(IS_ATTACK, spider.IsAttacking);
        }

    }
}
