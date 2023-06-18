using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public class PlayerAnimation : MonoBehaviour
    {
        private const string IS_WALKING = "isWalking";
        private const string MOVE_X = "moveX";
        private const string MOVE_Y = "moveY";
        private const string LAST_MOVE_X = "lastMoveX";
        private const string LAST_MOVE_Y = "lastMoveY";
        private const string ATTACK = "Attack";
        private const string IS_DEAD = "isDead";

        private Animator animator;

        [SerializeField] private PlayerController playerController;

        private void Awake() => animator = GetComponent<Animator>();

        private void LateUpdate()
        {
            animator.SetBool(IS_WALKING, playerController.IsWalking());
            animator.SetFloat(MOVE_X, playerController.MovementDirection.x);
            animator.SetFloat(MOVE_Y, playerController.MovementDirection.y);
            animator.SetFloat(LAST_MOVE_X, playerController.LastDirectionVector.x);
            animator.SetFloat(LAST_MOVE_Y, playerController.LastDirectionVector.y);
            animator.SetBool(ATTACK, playerController.IsAttacking);
            animator.SetBool(IS_DEAD, playerController.PlayerStats.IsDead);
        }

    }
}
