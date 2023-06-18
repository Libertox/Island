using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Island.InteractObject
{
    public class Tree : Resource
    {
        private const string ANIM_DIRECTION = "Direction";

        [SerializeField] private Animator animator;

        private Plant treeSeed;

        public void Cut()
        {
            health--;
            if (health < 0)
            {
                if (treeSeed != null)
                    Destroy(treeSeed.gameObject);

                animator.enabled = true;
                animator.SetFloat(ANIM_DIRECTION, PlayerController.Instance.LastDirectionVector.x);
                Instantiate(Effect, transform.position, Quaternion.identity);
                SpawnIngredients();
            }
        }
        public void SetTreeSeed(Plant plant) => treeSeed = plant;

    }
   
}