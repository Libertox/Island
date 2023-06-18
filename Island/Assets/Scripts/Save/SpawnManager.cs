using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Island.Item;

namespace Island
{

    public class SpawnManager : MonoBehaviour
    {

        private const string LACK_SPACE = "Lack of space";
        private const string CANT_PUT = "I can't put it up";

        private SaveObject saveObject;

        [SerializeField] private bool newGame = true;
        [SerializeField] private List<GameObject> basicObject;

        private void Awake() => saveObject = GetComponent<SaveObject>();

        private void Start()
        {
            if (PlayerPrefs.HasKey(SceneLoader.GetCurrentSceneName()))
            {
                if (PlayerPrefs.GetInt(SceneLoader.GetCurrentSceneName()) == 1)
                    newGame = true;
                else
                    newGame = false;
            }

            if (newGame)
            {
                newGame = false;
                saveObject.Save();
            }
            else
            {
                for (int i = 0; i < basicObject.Count; i++)
                    Destroy(basicObject[i]);

                saveObject.Load();
            }

            if (newGame)
                PlayerPrefs.SetInt(SceneLoader.GetCurrentSceneName(), 1);
            else
                PlayerPrefs.SetInt(SceneLoader.GetCurrentSceneName(), 0);

            PlayerPrefs.Save();
        }


        public static bool SpawnInteractObject<T>(T gameObject, Vector2 position) where T : Object
        {
            if (!SceneLoader.IsHomeScence())
            {
                PlayerController.Instance.SpeechBubble.SetMessage(CANT_PUT);
                return false;
            }


            bool canPut = true;
            Collider2D[] collider2D = Physics2D.OverlapBoxAll(position, gameObject.GetComponent<BoxCollider2D>().size, 1);
            foreach (Collider2D colid in collider2D)
            {
                if (colid)
                {
                    if (colid.TryGetComponent(out Interactable interactable))
                    {
                        if (interactable is BaseItem)
                            canPut = true;
                        else
                            canPut = false;
                    }
                    else
                        canPut = false;
                }
                else
                    canPut = true;
            }
            if (canPut)
            {
                Instantiate(gameObject, position, Quaternion.identity);
            }
            else
                PlayerController.Instance.SpeechBubble.SetMessage(LACK_SPACE);

            return canPut;

        }
    }
}
