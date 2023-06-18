using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{

    public class AreaExit : MonoBehaviour, Interactable
    {
        [SerializeField] private Scene targetScene;
        [SerializeField] private int areaTransaciton;
        public int AreaTransaciton => areaTransaciton;
        public Scene TargetScence => targetScene;

        public virtual void Interact(PlayerController player)
        {
            player.SetTargetSecne(areaTransaciton);
            UIManager.Instance.TransitionBetweenLevels(() => SceneLoader.LoadScene(targetScene));

        }
    }
}
