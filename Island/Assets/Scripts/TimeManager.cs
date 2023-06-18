using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        private float homeTime;
        private float merchantTime;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!SceneLoader.IsHomeScence())
                homeTime += Time.deltaTime;
            if (!SceneLoader.IsMerchantScene())
                merchantTime += Time.deltaTime;
        }

        public float GetExtraTime()
        {
            if (SceneLoader.IsHomeScence())
                return homeTime;
            if (SceneLoader.IsMerchantScene())
                return merchantTime;

            return 0;
        }

        public void ResetTime()
        {
            if (SceneLoader.IsHomeScence())
                homeTime = 0;
            if (SceneLoader.IsMerchantScene())
                merchantTime = 0;
        }
    }
}
