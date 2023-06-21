using MazeSystem;
using TanksSystem;
using UnityEngine;

namespace GameLogic
{
    public class EnemySpawner
    {
        private EnemyTankFactory _enemyTankFactory;
        private Maze _maze;

        public EnemySpawner(EnemyTankFactory enemyTankFactory)
        {
            _enemyTankFactory = enemyTankFactory;
        }

        public void SetMaze(Maze maze) 
        {
            _maze = maze;
        }

        public EnemyTank SpawnRandomEnemy()
        {
            var position = _maze.GetRandomEmptyPosition();
            var path = _maze.GetPathToPlayerBase(new Vector2Int((int)position.x, (int)position.y));

            var enemy = _enemyTankFactory.CreateRandomTank();
            enemy.SetPath(path);
            enemy.transform.position = _maze.GetWorldPos(position);

            return enemy;
        }
    }
}