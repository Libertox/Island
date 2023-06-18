using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{
    public class Stone : Resource
    {
        public void Dig()
        {
            health--;
            if (health < 0)
            {
                Instantiate(Effect, transform.position, Quaternion.identity);
                SpawnIngredients();
                Destroy(gameObject);
            }
        }

    }
}
