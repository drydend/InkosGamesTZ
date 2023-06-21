using UnityEngine;

namespace CameraSystem
{
    public class CameraSizeFitter : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        public void SetSize(float height)
        {   
            var resolution = Screen.width / (float)Screen.height;
            _camera.orthographicSize = height / resolution;
        }
    }
}
