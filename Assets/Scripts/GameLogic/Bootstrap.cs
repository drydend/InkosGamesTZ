using UnityEngine;
using GameView;
using Utils;
using GameUI;
using TanksSystem;
using GameInput;
using CameraSystem;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameLogic
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private CoroutinePlayer _coroutinePlayer;
        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private CameraSizeFitter _cameraSizeFitter;

        [SerializeField]
        private Transform _viewParent;

        [SerializeField]
        private AudioSource _audioSource;

        [Header("Configs")]
        [SerializeField]
        private GameConfig _gameConfig;

        [Header("UI")]
        [SerializeField]
        private GameUIMenus _gameUIMenus;
        [SerializeField]
        private DecreasingCounterUI _playerHPCounterUI;
        [SerializeField]
        private DecreasingCounterUI _enemiesLeftCounterUI;
        [SerializeField]
        private ScoreCounterUI _scoreCounterUI;
        [SerializeField]
        private List<Button> _restartButtons;

        [Header("Factories")]
        [SerializeField]
        private CellFactory _cellFactory;
        [SerializeField]
        private EnemyTankFactory _enemyTankFactory;
        [SerializeField]
        private PlayerTankFactory _playerTankFactory;

        private Counter _enemyDeathCounter = new Counter();
        private Counter _playerDeathCounter = new Counter();
        private Counter _playerScoreCounter = new Counter();

        private Game _game;


        private void Start()
        {
            _game = CreateNewGame();
            _game.StartGame();

            InitializeUI();

            _cameraSizeFitter.SetSize(_gameConfig.GameFieldHeight);
        }

        private void InitializeUI()
        {
            foreach (var button in _restartButtons)
            {
                button.onClick.AddListener(() => _game.RestartGame());
            }

            _scoreCounterUI.Initialize(_playerScoreCounter);
            _playerHPCounterUI.Initialize(_gameConfig.PlayerHeals, _playerDeathCounter);
            _enemiesLeftCounterUI.Initialize(_gameConfig.AllEnemiesNumber, _enemyDeathCounter);
        }

        private Game CreateNewGame()
        {
            var enemyLogic = CreateEnemyLogic();
            var playerSpawner = new PlayerSpawner(_playerTankFactory, _playerInput);
            var mazeVisualizer = new MazeVisualizer(_cellFactory, _viewParent);

            var game = new Game(_gameConfig, mazeVisualizer, _coroutinePlayer, _gameUIMenus,
                enemyLogic, playerSpawner, _playerDeathCounter, _playerScoreCounter, _audioSource);

            game.InitializeState();

            return game;
        }

        private EnemiesLogic CreateEnemyLogic()
        {
            var spawner = new EnemySpawner(_enemyTankFactory);
            var logic = new EnemiesLogic(_gameConfig, spawner, _enemyDeathCounter, _coroutinePlayer, _playerScoreCounter);

            return logic;
        }
    }
}
