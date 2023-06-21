using MazeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TanksSystem;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public class EnemiesLogic
    {
        private GameConfig _config;
        private EnemySpawner _spawner;
        private Counter _enemyDeathCounter;
        private Counter _scoreCounter;

        private CoroutinePlayer _coroutinePlayer;
        
        private int _createdEnemiesNumber = 0;
        private int _enemiesNumber = 0;
        private float _lastEnemySpawnTime;
        private List<EnemyTank> _allEnemies = new List<EnemyTank>();

        private Coroutine _spawnCoroutine;

        public event Action OnAllEnemiesDestroyed;

        public EnemiesLogic(GameConfig gameConfig, EnemySpawner enemySpawner, Counter enemyDeathCounter
            , CoroutinePlayer coroutinePlayer, Counter scoreCounter)
        {
            _config = gameConfig;
            _spawner = enemySpawner;
            _enemyDeathCounter = enemyDeathCounter;
            _coroutinePlayer = coroutinePlayer;
            _scoreCounter = scoreCounter;
        }

        public void Update(Maze maze)
        {
            _spawner.SetMaze(maze);
        }

        public void StartSpawningEnemies()
        {
            if(_spawnCoroutine != null)
            {
                _coroutinePlayer.StopCoroutine(_spawnCoroutine);
            }

           _spawnCoroutine = _coroutinePlayer.StartCoroutine(SpawnCoroutine());
        }

        public void UnpauseAllEnemies()
        {
            foreach (var enemy in _allEnemies)
            {
                enemy.Unpause();
            }
        }

        public void StopAllEnemies()
        {
            foreach (var enemy in _allEnemies)
            {
                enemy.Pause();
            }
        }

        public void Reset()
        {
            if (_spawnCoroutine != null)
            {
                _coroutinePlayer.StopCoroutine(_spawnCoroutine);
            }

            _createdEnemiesNumber = 0;
            _lastEnemySpawnTime = 0;
            _enemiesNumber = 0;
            _enemyDeathCounter.Reset();

            foreach (var enemy in _allEnemies)
            {
                enemy.DestroyTank();
            }

            _allEnemies.Clear();
        }

        private IEnumerator SpawnCoroutine()
        {
            while (_createdEnemiesNumber != _config.AllEnemiesNumber) 
            { 
                if(_enemiesNumber < _config.MaxEnemiesInGame && _lastEnemySpawnTime > _config.EnemySpawnTime)
                {
                    var enemy = _spawner.SpawnRandomEnemy();
                    _enemiesNumber++;
                    _createdEnemiesNumber++;
                    _allEnemies.Add(enemy);
                    
                    enemy.OnDie += OnEnemyDie;
                    _lastEnemySpawnTime = 0;
                }

                _lastEnemySpawnTime += Time.deltaTime;
                yield return null;   
            }
        }

        private void OnEnemyDie(EnemyTank enemy) 
        {
            _allEnemies.Remove(enemy);
            _scoreCounter.Increase(enemy.ScoreForKill);
            _enemyDeathCounter.Increase();
            _enemiesNumber--;

            if(_createdEnemiesNumber == _config.AllEnemiesNumber && _enemiesNumber == 0) 
            {
                OnAllEnemiesDestroyed?.Invoke();
            }
        }
    }
}
