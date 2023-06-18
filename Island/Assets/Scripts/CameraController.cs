using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{

    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;

        private void Awake() => virtualCamera = GetComponent<CinemachineVirtualCamera>();

        private void Start() => virtualCamera.Follow = PlayerController.Instance.transform;

    }
}
