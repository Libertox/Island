using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public interface IDamageable
    {
        public void TakeDamage(int damage);
    }
}