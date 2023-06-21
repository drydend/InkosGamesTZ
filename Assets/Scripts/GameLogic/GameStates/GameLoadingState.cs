using GameLogic.GameStates.Args;
using GameUI;
using GameView;
using MazeSystem;
using StateMachines;
using System.Collections;
using UnityEngine;
using Utils;

namespace GameLogic.GameStates
{
    public class GameLoadingState : ParamBaseState<GameLoadingArgs>
    {
        private StateMachine _stateMachine;
        private MazeVisualizer _mazeVisualizer;
        private UIMenu _loadingScreen;

        private MazePresenter _mazePresenter;

        private GameLoadingArgs _args;

        public GameLoadingState(StateMachine stateMachine, MazeVisualizer mazeVisualizer,
             UIMenu loadingScreen)
        {
            _stateMachine = stateMachine;
            _mazeVisualizer = mazeVisualizer;
            _loadingScreen = loadingScreen;
        }

        public override void Enter()
        {
            LoadGame();
        }

        public override void Exit()
        {
        }

        public override void SetArgs(GameLoadingArgs args)
        {
            _args = args;
        }

        private void LoadGame()
        {
            _loadingScreen.Open();

            _mazePresenter?.Destroy();
            var maze = new MazeGenerator().GetMaze(_args.FieldWidth, _args.FieldHeight);
            var mazeView = _mazeVisualizer.CreateView(maze);

            _mazePresenter = new MazePresenter(maze, mazeView);
            _mazePresenter.Initialize();

            _loadingScreen.Close();

            var args = new GameRuningStateArgs(_mazePresenter);
            _stateMachine.SwithcStateWithParam<GameRuningState, GameRuningStateArgs>(args);
        }
    }
}
