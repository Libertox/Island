using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Island.Item;

namespace Island
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string SAVES_PATH = "/saves";

        public event EventHandler OnSavedGame;
        public event EventHandler OnSaveSceneObject;

        [SerializeField] private ItemsListSO itemsList;
        [SerializeField] private PrefabListSO prefabList;

        public PrefabListSO PrefabList => prefabList;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void SaveGame() => OnSavedGame?.Invoke(this, EventArgs.Empty);

        public void SaveSceneObject() => OnSaveSceneObject?.Invoke(this, EventArgs.Empty);

        public static void Save<T>(T obj, string key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SAVES_PATH;
            Directory.CreateDirectory(path);

            FileStream file = new FileStream(path + key, FileMode.OpenOrCreate);

            binaryFormatter.Serialize(file, obj);

            file.Close();

        }

        public static T Load<T>(string key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SAVES_PATH;

            FileStream file = new FileStream(path + key, FileMode.Open);

            T obj = default;

            if (File.Exists(path + key))
            {
                obj = (T)binaryFormatter.Deserialize(file);

                file.Close();
            }

            return obj;
        }

        public ItemInstance CreateLoadedItem(ItemInstance loadedItem)
        {
            if (loadedItem?.itemSOName != null && loadedItem.itemSOName != "")
            {
                if (loadedItem is BackpackInstance backpackInstance)
                    return CreateLoadedBackpack(backpackInstance);
                else
                    return new ItemInstance(FindItemSO(loadedItem.itemSOName), loadedItem.strength);
            }
            else
                return null;
        }

        public BackpackInstance CreateLoadedBackpack(BackpackInstance loadedBackpack)
        {
            if (loadedBackpack?.itemSOName != null)
            {
                for (int j = 0; j < loadedBackpack.content.Length; j++)
                    loadedBackpack.content[j] = CreateLoadedItem(loadedBackpack.content[j]);

                return new BackpackInstance(FindItemSO(loadedBackpack.itemSOName), loadedBackpack.strength, loadedBackpack.content, loadedBackpack.amount);
            }
            return null;

        }

        public ItemSO FindItemSO(string name)
        {
            foreach (ItemSO itemSO in itemsList.itemsList)
            {
                if (itemSO.name == name)
                    return itemSO;
            }
            return null;
        }
    }
}
