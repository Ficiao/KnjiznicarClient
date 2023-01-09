using TMPro;
using UnityEngine;

namespace Shared
{
    public class ChatMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _chatText;

        public void SetText(string message)
        {
            _chatText.text = message;
        }
    }
}