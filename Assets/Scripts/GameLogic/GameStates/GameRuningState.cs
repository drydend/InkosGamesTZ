using GameLogic.GameStates.Args;
using GameUI;
using StateMachines;
using System.Linq.Expressions;
using Utils;

namespace GameLogic.GameStates
{
    public class GameRuningState : ParamBaseState<GameRuningStateArgs>
    {
        private StateMachine _stateMachine;
        private GameConfig _config;
        private PlayerSpawner _spawner;
        private readonly Counter _scoreCounter;
        private Counter _playerDeathCounter;
        private EnemiesLogic _enemiesLogic;
        private CoroutinePlayer _coroutinePlayer;
        private UIMenu _UIMenu;

        private GameRuningStateArgs _args;

        private GameCycle _gameCycle;

        public GameRuningState(StateMachine stateMachine, GameConfig config, PlayerSpawner spawner,Counter scoreCounter
            ,Counter playerDeathCounter, EnemiesLogic enemiesLogic, CoroutinePlayer coroutinePlayer, UIMenu UIMenu)
        {
            _stateMachine = stateMachine;
            _config = config;
            _spawner = spawner;
            _scoreCounter = scoreCounter;
            _playerDeathCounter = playerDeathCounter;
            _enemiesLogic = enemiesLogic;
            _coroutinePlayer = coroutinePlayer;
            _UIMenu = UIMenu;
        }

        public override void Enter()
        {
            _UIMenu.Open();

            if (_gameCycle != null)
            {
                _gameCycle.DestroyCycle();
            }

            _enemiesLogic.Update(_args.MazePresenter.Maze);
            _spawner.SetMaze(_args.MazePresenter.Maze);

            _gameCycle = new GameCycle(_config, _spawner, _args.MazePresenter.MazeView,
                _playerDeathCounter,_scoreCounter ,_enemiesLogic, _coroutinePlayer);

            _gameCycle.StartGame();

            _gameCycle.OnLose += OnPlayerLose;
            _gameCycle.OnWin += OnPlayerWin;
        }

        public override void Exit()
        {
            _UIMenu.Close();
        }

        public override void SetArgs(GameRuningStateArgs args)
        {
            _args = args;
        }

        private void OnPlayerWin()
        {
            _stateMachine.SwitchState<GameWinState>();
        }

        private void OnPlayerLose()
        {
            _stateMachine.SwitchState<GameLoseState>();
        }
    }
}
