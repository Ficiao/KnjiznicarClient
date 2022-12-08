using UnityEngine;
using UnityEngine.EventSystems;

namespace Overworld
{
    public class DisableMouseMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            CameraController.Instance.DisableCameraMovement = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CameraController.Instance.DisableCameraMovement = false;
        }
    }
}