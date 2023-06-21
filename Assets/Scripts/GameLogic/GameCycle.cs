using GameView;
using MazeSystem;
using System;
using System.Collections;
using TanksSystem;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public class GameCycle
    {
        private GameConfig _gameConfig;

        private PlayerSpawner _playerSpawner;
        private Counter _playerDeathCounter;
        private Counter _scoreCounter;
        private EnemiesLogic _enemyLogic;
        private CoroutinePlayer _coroutinePlayer;
        private MazeView _mazeView;

        private PlayerTank _currentPlayerTank;

        private Coroutine _respawnCoroutine;

        public event Action OnWin;
        public event Action OnLose;

        public GameCycle(GameConfig config, PlayerSpawner spawner, MazeView mazeView,
            Counter playerDeathCounter,Counter scoreCounter ,EnemiesLogic enemiesLogic, CoroutinePlayer coroutinePlayer)
        {
            _gameConfig = config;
            _playerSpawner = spawner;
            _mazeView = mazeView;
            _playerDeathCounter = playerDeathCounter;
            _scoreCounter = scoreCounter;
            _enemyLogic = enemiesLogic;
            _coroutinePlayer = coroutinePlayer;
        }

        public void DestroyCycle()
        {
            if (_respawnCoroutine != null)
            {
                _coroutinePlayer.StopCoroutine(_respawnCoroutine);
            }

            _scoreCounter.Reset();
            _playerDeathCounter.Reset();

            _currentPlayerTank.OnDie -= OnPlayerDied;

            _enemyLogic.OnAllEnemiesDestroyed -= OnPlayerWin;
            _mazeView.PlayerBase.OnDestroyed -= OnBaseDestroyed;

            _currentPlayerTank?.DestroyTank();
            _currentPlayerTank = null;

            _enemyLogic.Reset();
        }

        public void StartGame()
        {
            SpawnPlayer();
            _enemyLogic.StartSpawningEnemies();

            _enemyLogic.OnAllEnemiesDestroyed += OnPlayerWin;
            _mazeView.PlayerBase.OnDestroyed += OnBaseDestroyed;
        }

        public void StopGame()
        {
            _currentPlayerTank?.Pause();
            _enemyLogic.StopAllEnemies();
        }

        private void OnPlayerWin()
        {
            OnWin?.Invoke();
        }

        private void OnPlayerDied()
        {
            _currentPlayerTank.OnDie -= OnPlayerDied;
            _currentPlayerTank.DestroyTank();
            
            _currentPlayerTank = null;

            _playerDeathCounter.Increase();

            if (_playerDeathCounter.Value == _gameConfig.PlayerHeals)
            {
                OnLose?.Invoke();
                _enemyLogic.StopAllEnemies();
                return;
            }

            SpawnPlayer(_gameConfig.PlayerRespawnTime);
        }

        private void SpawnPlayer(float delay = 0)
        {
            if (_respawnCoroutine != null)
            {
                _coroutinePlayer.StopCoroutine(_respawnCoroutine);
            }

            _respawnCoroutine = _coroutinePlayer.StartCoroutine(RespawnPlayerRoutine(delay));
        }

        private IEnumerator RespawnPlayerRoutine(float playerRespawnTime)
        {
            yield return new WaitForSeconds(playerRespawnTime);
            _currentPlayerTank = _playerSpawner.SpawnPlayerTank();
            _currentPlayerTank.OnDie += OnPlayerDied;
        }

        private void OnBaseDestroyed(CellView cellView)
        {
            OnLose?.Invoke();

            _enemyLogic.StopAllEnemies();

            cellView.OnDestroyed -= OnBaseDestroyed;
        }
    }
}