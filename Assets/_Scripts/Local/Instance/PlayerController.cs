using UnityEngine;
using TMPro;

namespace MatchInstance
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] private Animator _playerAnimator;

        public int MaximumHealth;
        public int CurrentHealth;

        public string PlayerName
        {
            get => _playerNameText.text;
            set => _playerNameText.text = value;
        }

        public void PerformLigthAttack() => _playerAnimator.SetTrigger("LightAttack");
        public void PerformMediumAttack() => _playerAnimator.SetTrigger("MediumAttack");
        public void PerformHeavyAttack() => _playerAnimator.SetTrigger("HeavyAttack");
        public void PerformStagger() => _playerAnimator.SetTrigger("Stagger");
        public void PerformDeath() => _playerAnimator.SetTrigger("Death");
        public void PerformDance() => _playerAnimator.SetTrigger("Dance");
    }
}