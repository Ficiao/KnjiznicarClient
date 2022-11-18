using UnityEngine;

namespace Assets._Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        public string Username;

        public void Move(Vector3 position)
        {
            transform.position = position;
        }
    }
}
