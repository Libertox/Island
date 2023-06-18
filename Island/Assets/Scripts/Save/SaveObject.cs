using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.InteractObject;

namespace Island
{
    public class SaveObject : MonoBehaviour
    {
        private const string Interact_Item_Key = "/InteractItem";
        private const string Interact_Item_Count = "/InteractItemCount";

        private void Start()
        {
            SaveManager.Instance.OnSavedGame += SaveManager_OnSavedGame;
            SaveManager.Instance.OnSaveSceneObject += SaveManager_OnSavedGame;
        }
        private void OnDestroy()
        {
            SaveManager.Instance.OnSavedGame -= SaveManager_OnSavedGame;
            SaveManager.Instance.OnSaveSceneObject -= SaveManager_OnSavedGame;
        }

        private void SaveManager_OnSavedGame(object sender, System.EventArgs e) => Save();

        public void Save()
        {
            Object[] findobject = FindObjectsOfType<Object>();
            List<IWriteable> writableObject = new List<IWriteable>();
            for (int i = 0; i < findobject.Length; i++)
            {
                if (findobject[i] is IWriteable writeable)
                    writableObject.Add(writeable);
            }

            for (int i = 0; i < writableObject.Count; i++)
            {
                ObjectSerializable data = writableObject[i].CreateItemData();
                SaveManager.Save(data, Interact_Item_Key + i + SceneLoader.GetCurrentSceneName());

            }
            SaveManager.Save(writableObject.Count, Interact_Item_Count + SceneLoader.GetCurrentSceneName());
        }

        public void Load()
        {
            int count = SaveManager.Load<int>(Interact_Item_Count + SceneLoader.GetCurrentSceneName());

            for (int i = 0; i < count; i++)
            {
                ObjectSerializable data = SaveManager.Load<ObjectSerializable>(Interact_Item_Key + i + SceneLoader.GetCurrentSceneName());
                data.CreateObject(SaveManager.Instance);
            }

            TimeManager.Instance.ResetTime();
        }
    }
}
