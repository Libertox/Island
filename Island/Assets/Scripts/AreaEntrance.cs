using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.InteractObject
{

    public class AreaEntrance : MonoBehaviour
    {
        [SerializeField] private AreaExit areaExit;

        private void Start()
        {
            if (PlayerController.Instance.GetTargetScene() == areaExit.AreaTransaciton)
                PlayerController.Instance.transform.position = transform.position;
        }
    }
}
