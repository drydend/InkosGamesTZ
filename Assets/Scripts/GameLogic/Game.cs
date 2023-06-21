using GameLogic.GameStates;
using GameLogic.GameStates.Args;
using GameUI;
using GameView;
using StateMachines;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public class Game
    {
        private GameConfig _gameConfig;
        private MazeVisualizer _mazeVisualizer;
        private CoroutinePlayer _coroutinePlayer;
        private GameUIMenus _gameUIMenus;

        private EnemiesLogic _enemiesLogic;
        private PlayerSpawner _playerSpawner;
        private AudioSource _audioSource;

        private Counter _playerDeathCounter;
        private Counter _scoreCounter;
        private StateMachine _stateMachine;

        public Game(GameConfig gameConfig, MazeVisualizer mazeVisualizer, CoroutinePlayer coroutinePlayer,
            GameUIMenus gameUIMenus, EnemiesLogic enemiesLogic, PlayerSpawner playerSpawner, Counter playerDeathCounter,
            Counter scoreCounter, AudioSource audioSource)
        {
            _gameConfig = gameConfig;
            _mazeVisualizer = mazeVisualizer;
            _coroutinePlayer = coroutinePlayer;
            _gameUIMenus = gameUIMenus;
            _enemiesLogic = enemiesLogic;
            _playerSpawner = playerSpawner;
            _playerDeathCounter = playerDeathCounter;
            _scoreCounter = scoreCounter;
            _audioSource = audioSource;
        }

        public void InitializeState()
        {
            var states = new Dictionary<Type, BaseState>();

            _stateMachine = new StateMachine(states);

            states.Add(typeof(GameLoadingState), new GameLoadingState(_stateMachine, _mazeVisualizer, _gameUIMenus.LoadingScreen));
            states.Add(typeof(GameRuningState), new GameRuningState(_stateMachine, _gameConfig, _playerSpawner,
                _playerDeathCounter, _scoreCounter, _enemiesLogic, _coroutinePlayer, _gameUIMenus.RuningGameUI));
            states.Add(typeof(GameWinState), new GameWinState(_gameUIMenus.WinScreen, _gameConfig.WinSound, _audioSource));
            states.Add(typeof(GameLoseState), new GameLoseState(_gameUIMenus.LoseScreen, _gameConfig.LoseSound, _audioSource));
        }

        public void StartGame()
        {
            var args = new GameLoadingArgs(_gameConfig.GameFieldWidth, _gameConfig.GameFieldHeight);
            _stateMachine.SwithcStateWithParam<GameLoadingState, GameLoadingArgs>(args);
        }

        public void RestartGame()
        {
            var args = new GameLoadingArgs(_gameConfig.GameFieldWidth, _gameConfig.GameFieldHeight);
            _stateMachine.SwithcStateWithParam<GameLoadingState, GameLoadingArgs>(args);
        }
    }
}
