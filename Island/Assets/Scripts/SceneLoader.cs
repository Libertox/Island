using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Island
{
    public static class SceneLoader
    {
        public static void LoadScene(Scene targetScene) => SceneManager.LoadScene(targetScene.ToString());

        public static string GetCurrentSceneName() => SceneManager.GetActiveScene().name;

        public static int GetCurrentSceneIndex() => SceneManager.GetActiveScene().buildIndex;

        public static bool IsHomeScence() => SceneManager.GetActiveScene().name == Scene.Home.ToString();

        public static bool IsMerchantScene() => SceneManager.GetActiveScene().name == Scene.Merchant.ToString();

    }

    public enum Scene
    {
        MainMenu,
        LoadingScene,
        Home,
        Cave_Level_1,
        Cave_Level_2,
        Cave_Level_3,
        Cave_Level_4,
        Cave_Level_5,
        Cave_Level_6,
        Cave_Level_7,
        Cave_Level_8,
        Cave_Level_9,
        Cave_Level_10,
        Cave_Level_11,
        Cave_Level_12,
        Cave_Level_13,
        Cave_Level_14,
        Cave_Level_15,
        Merchant,
    }
}
