using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public class SavePlayerStats : MonoBehaviour
    {
        private const string PLAYER_STATS_KEY = "/PlayerStats";

        private PlayerController player;

        private void Awake() => player = GetComponent<PlayerController>();

        private void Start()
        {
            SaveManager.Instance.OnSavedGame += SaveManager_OnSavedGame;

            if (PlayerPrefs.HasKey(SceneLoader.GetCurrentSceneName()))
                Load();
            else
                Save();

        }

        private void SaveManager_OnSavedGame(object sender, System.EventArgs e) => Save();


        private void Save()
        {
            PlayerStatsSeralizable playerStatsSeralizable = new PlayerStatsSeralizable(player);
            SaveManager.Save(playerStatsSeralizable, PLAYER_STATS_KEY);
        }

        private void Load()
        {
            PlayerStatsSeralizable playerStatsSeralizable = SaveManager.Load<PlayerStatsSeralizable>(PLAYER_STATS_KEY);
            player.PlayerStats.UpdateStats(playerStatsSeralizable);
            player.transform.position = new Vector3(playerStatsSeralizable.position.x, playerStatsSeralizable.position.y, playerStatsSeralizable.position.z);
        }

    }
}
