using UnityEngine;

namespace Shared
{
    public class ChatController : Singleton<ChatController>
    {
        [SerializeField] private ChatMessage _chatMessagePrefab;
        [SerializeField] private Transform _messageContainer;

        public void ShowMessage(string message)
        {
            ChatMessage chatMessage = Instantiate(_chatMessagePrefab, _messageContainer);
            chatMessage.SetText(message);
        }
    }
}