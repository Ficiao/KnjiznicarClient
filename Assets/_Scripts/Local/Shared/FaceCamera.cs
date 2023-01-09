using UnityEngine;
using TMPro;
using System.Collections;

namespace Shared
{
    public class FaceCamera : MonoBehaviour
    {
        [SerializeField] private bool _continuous;
        private Transform _camera;

        private void Awake() 
        {
            _camera = Camera.main.transform;
            StartCoroutine(LookAtCamera());
        }

        private IEnumerator LookAtCamera()
        {
            do
            {
                transform.LookAt(_camera);
                transform.Rotate(0, 180, 0);
                yield return null;
            } while (_continuous);
        }
    }
}