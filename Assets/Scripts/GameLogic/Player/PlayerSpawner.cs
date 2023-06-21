using GameInput;
using MazeSystem;
using TanksSystem;

namespace GameLogic
{
    public class PlayerSpawner
    {
        private PlayerTankFactory _factory;
        private Maze _maze;
        private PlayerInput _input;

        public PlayerSpawner(PlayerTankFactory factory, PlayerInput input)
        {
            _factory = factory;
            _input = input;
        }

        public void SetMaze(Maze maze) 
        {
            _maze = maze;
        }

        public PlayerTank SpawnPlayerTank()
        {
            var instance = _factory.GetPlayerTank();
            instance.SetInput(_input);

            instance.transform.position = _maze.GetPlayerSpawnPos();

            return instance;
        }

    }
}