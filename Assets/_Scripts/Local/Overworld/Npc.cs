using UnityEngine;

namespace Overworld
{
    public abstract class Npc : MonoBehaviour
    {
        private Transform _camera;
        private Vector3 _rotation;

        public int NpcId;

        private void Start()
        {
            _camera = Camera.main.transform;
            _rotation = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);
            transform.Rotate(0, 180, 0);
            _rotation.y = transform.localEulerAngles.y;
            transform.localEulerAngles = _rotation;
        }

        public abstract void Interact();

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() is UserController) GameManager.Instance.Player.SetNpcToInteract(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerController>() is UserController) GameManager.Instance.Player.RemoveNpcInteract();
        }
    }
}
