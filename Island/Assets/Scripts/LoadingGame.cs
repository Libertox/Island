using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public class LoadingGame : MonoBehaviour
    {
        [SerializeField] private float waitToLoad;

        private float time;

        private void Update()
        {
            time += Time.deltaTime;
            if (time > waitToLoad)
                SceneLoader.LoadScene(Scene.Home);
        }
    }
}
