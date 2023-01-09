using Shared;
using KnjiznicarDataModel;
using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel.Message;
using Network;
using Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MatchInstance
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Transform _playerOneContainer;
        [SerializeField] private Transform _playerTwoContainer;
        [SerializeField] private LetterGrid _letterGrid;
        private MatchType _matchType;
        private PlayerController _ownPlayer;
        private PlayerController _enemyPlayer;
        private MatchEndMessage _matchEndCache;

        public void SetupScene(MatchType matchType, PlayerMatchData playerData, string enemyName, int enemyHealth)
        {
            if (_ownPlayer != null) return;
            _matchType = matchType;
            if (matchType != MatchType.Pvp) ChatController.Instance.gameObject.SetActive(false);
            _ownPlayer = Instantiate(ScriptablesHolder.Instance.PlayerScriptable.GetPrefab(AssetType.InstanceLocalPlayer)
                , _playerOneContainer).GetComponent<PlayerController>();
            _ownPlayer.PlayerName = GlobalPlayerData.PlayerData.PlayerName;
            _ownPlayer.MaximumHealth = playerData.Health;
            _ownPlayer.CurrentHealth = playerData.Health;
            UIManager.Instance.SetPlayerHealth(playerData.Health, playerData.Health);

            _letterGrid.SetupLetters(playerData.LetterMatrix);

            switch (matchType)
            {
                case MatchType.Pvp:
                    _enemyPlayer = Instantiate(ScriptablesHolder.Instance.PlayerScriptable.GetPrefab(AssetType.InstanceOtherPlayer)
                        , _playerTwoContainer).GetComponent<PlayerController>();
                    break;
                case MatchType.Adventure:
                    _enemyPlayer = Instantiate(ScriptablesHolder.Instance.PlayerScriptable.GetPrefab(AssetType.InstanceAI)
                        , _playerTwoContainer).GetComponent<PlayerController>();
                    break;
                default:
                    Debug.Log($"Unsupported match type: {matchType}");
                    break;
            }
            _enemyPlayer.PlayerName = enemyName;
            _enemyPlayer.MaximumHealth = enemyHealth;
            _enemyPlayer.CurrentHealth = enemyHealth;
            UIManager.Instance.SetEnemyHealth(enemyHealth, enemyHealth);

            StartCoroutine(HeartBeatSender());
        }

        private IEnumerator HeartBeatSender()
        {
            HeartBeatMessage message = new HeartBeatMessage();

            while (true)
            {
                yield return new WaitForSecondsRealtime(120);
                ClientSend.SendTCPData(message, Client.OverworldServer);
            }
        }

        public void HandleTurnOver(TurnOverMessage data)
        {
            UIManager.Instance.HideWaitingForOpponent();
            UIManager.Instance.UpdateLetters(data.NewLetters);
            _matchEndCache = data.MatchEndData;
            _ownPlayer.CurrentHealth = Math.Clamp(_ownPlayer.CurrentHealth - data.EnemyDamage, 0, _ownPlayer.CurrentHealth);
            _enemyPlayer.CurrentHealth = Math.Clamp(_enemyPlayer.CurrentHealth - data.OwnDamage, 0, _enemyPlayer.CurrentHealth);

            (AnimationType, AnimationType) animationDuration = UIManager.Instance.SetDamageText(data.OwnWord, data.OwnDamage, data.EnemyWord, data.EnemyDamage);
            switch (animationDuration.Item1)
            {
                case AnimationType.Short:
                    StartCoroutine(ShortPlayerSquence(animationDuration.Item2));
                    break;
                case AnimationType.Medium:
                    StartCoroutine(MediumPlayerSquence(animationDuration.Item2));
                    break;
                case AnimationType.Long:
                    StartCoroutine(LongPlayerSequence(animationDuration.Item2));
                    break;
            }
        }

        private IEnumerator ShortPlayerSquence(AnimationType enemyAnimation)
        {
            _ownPlayer.PerformLigthAttack();
            UIManager.Instance.ShowPlayerDamage();
            yield return new WaitForSeconds(0.5f);
            _enemyPlayer.PerformStagger();
            yield return new WaitForSeconds(0.8f);

            UIManager.Instance.SetEnemyHealth(_enemyPlayer.CurrentHealth, _enemyPlayer.MaximumHealth);
            PlayEnemyAnimation(enemyAnimation);
        }

        private IEnumerator MediumPlayerSquence(AnimationType enemyAnimation)
        {
            _ownPlayer.PerformMediumAttack();
            UIManager.Instance.ShowPlayerDamage();
            yield return new WaitForSeconds(0.5f);
            _enemyPlayer.PerformStagger();
            yield return new WaitForSeconds(0.9f);

            UIManager.Instance.SetEnemyHealth(_enemyPlayer.CurrentHealth, _enemyPlayer.MaximumHealth);
            PlayEnemyAnimation(enemyAnimation);
        }

        private IEnumerator LongPlayerSequence(AnimationType enemyAnimation)
        {
            _ownPlayer.PerformHeavyAttack();
            UIManager.Instance.ShowPlayerDamage();
            yield return new WaitForSeconds(1.15f);
            _enemyPlayer.PerformStagger();
            yield return new WaitForSeconds(1.2f);

            UIManager.Instance.SetEnemyHealth(_enemyPlayer.CurrentHealth, _enemyPlayer.MaximumHealth);
            PlayEnemyAnimation(enemyAnimation);
        }

        private void PlayEnemyAnimation(AnimationType enemyAnimation)
        {
            switch (enemyAnimation)
            {
                case AnimationType.Short:
                    StartCoroutine(ShortEnemySquence());
                    break;
                case AnimationType.Medium:
                    StartCoroutine(MediumEnemySquence());
                    break;
                case AnimationType.Long:
                    StartCoroutine(LongEnemySequence());
                    break;
            }
        }

        private IEnumerator ShortEnemySquence()
        {
            _enemyPlayer.PerformLigthAttack();
            UIManager.Instance.ShowEnemyDamage();
            yield return new WaitForSeconds(0.5f);
            _ownPlayer.PerformStagger();
            yield return new WaitForSeconds(0.8f);

            UIManager.Instance.SetPlayerHealth(_ownPlayer.CurrentHealth, _ownPlayer.MaximumHealth);
            if (_matchEndCache != null)
            {
                yield return new WaitForSeconds(0.5f);
                GameEnd();
            }
            else UIManager.Instance.EnableSelection();
        }

        private IEnumerator MediumEnemySquence()
        {
            _enemyPlayer.PerformMediumAttack();
            UIManager.Instance.ShowEnemyDamage();
            yield return new WaitForSeconds(0.5f);
            _ownPlayer.PerformStagger();
            yield return new WaitForSeconds(0.9f);

            UIManager.Instance.SetPlayerHealth(_ownPlayer.CurrentHealth, _ownPlayer.MaximumHealth);
            if (_matchEndCache != null)
            {
                yield return new WaitForSeconds(0.5f);
                GameEnd();
            }
            else UIManager.Instance.EnableSelection();
        }

        private IEnumerator LongEnemySequence()
        {
            _enemyPlayer.PerformHeavyAttack();
            UIManager.Instance.ShowEnemyDamage();
            yield return new WaitForSeconds(1.15f);
            _ownPlayer.PerformStagger();
            yield return new WaitForSeconds(1.2f);

            UIManager.Instance.SetPlayerHealth(_ownPlayer.CurrentHealth, _ownPlayer.MaximumHealth);
            if (_matchEndCache != null)
            {
                yield return new WaitForSeconds(0.5f);
                GameEnd();
            }
            else UIManager.Instance.EnableSelection();
        }

        private void GameEnd()
        {
            if (_matchEndCache != null)
            {
                UIManager.Instance.ShowMatchEndView(_matchEndCache);
                if (_ownPlayer.CurrentHealth == 0)
                {
                    _ownPlayer.PerformDeath();
                    if (_enemyPlayer.CurrentHealth > 0) _enemyPlayer.PerformDance();
                }
                if (_enemyPlayer.CurrentHealth == 0)
                {
                    _enemyPlayer.PerformDeath();
                    if (_ownPlayer.CurrentHealth > 0) _ownPlayer.PerformDance();
                }
            }
        }
    }
}