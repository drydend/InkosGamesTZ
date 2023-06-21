using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MazeSystem
{
    public class MazeGenerator
    {
        private readonly MazeCellState[,] _playerBase = new MazeCellState[5, 5]
        {
            {MazeCellState.Empty,MazeCellState.Empty,MazeCellState.Empty,MazeCellState.Empty,MazeCellState.Empty},
            {MazeCellState.Empty,MazeCellState.BreakableWall,MazeCellState.BreakableWall,MazeCellState.BreakableWall,MazeCellState.Empty },
            {MazeCellState.Empty,MazeCellState.BreakableWall,MazeCellState.PlayerBase,MazeCellState.BreakableWall,MazeCellState.Empty },
            {MazeCellState.Empty,MazeCellState.BreakableWall,MazeCellState.BreakableWall,MazeCellState.BreakableWall,MazeCellState.Empty },
            {MazeCellState.Empty, MazeCellState.Empty, MazeCellState.Empty, MazeCellState.Empty, MazeCellState.Empty }
        };

        public void AddPlayerBaseAt(int baseX, int baseY, Maze maze)
        {
            int shiftX = -_playerBase.GetLength(0) / 2;
            int shiftY = -_playerBase.GetLength(1) / 2;

            for (int x = 0; x < _playerBase.GetLength(0); x++, shiftX++)
            {
                for (int y = 0; y < _playerBase.GetLength(1); y++, shiftY++)
                {
                    TrySetValueAt(maze, baseX + shiftX, baseY + shiftY, _playerBase[x, y]);
                }

                shiftY = -_playerBase.GetLength(1) / 2;
            }
        }

        public Maze GetMaze(int width, int height)
        {
            var grid = GetRanodmGrid(width, height);
            var walls = new List<Vector2Int>();

            var currentCell = grid[0, 0];
            currentCell.IsVisited = true;
            currentCell.CellState = MazeCellState.Empty;

            var neighbourWalls = GetWallsAt(0, 0, grid);
            walls.Add(neighbourWalls);

            while (walls.Count > 0)
            {
                int wallIndex;
                int neighbourCellCount = 0;

                var wall = walls.GetRandom(out wallIndex);
                walls.Remove(wall);

                neighbourCellCount = CalculateneighbourCellCount(wall.x, wall.y, grid);

                if (neighbourCellCount == 1)
                {
                    currentCell = grid[wall.x, wall.y];
                    currentCell.IsVisited = true;
                    currentCell.CellState = MazeCellState.Empty;

                    var currentCellWalls = GetWallsAt(wall.x, wall.y, grid);
                    walls.Add(currentCellWalls);
                }
            }

            return GetMaze(grid);
        }

        private void TrySetValueAt(Maze maze, int x, int y, MazeCellState value)
        {
            if (x < 0 || y < 0 || x >= maze.Width || y >= maze.Height)
            {
                return;
            }

            maze[x, y] = value;
        }

        private Maze GetMaze(GridCell[,] grid)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);

            var maze = new Maze(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    maze[x, y] = grid[x, y].CellState;
                }
            }

            AddPlayerBaseAt(Random.Range(0, width), Random.Range(0, height), maze);
            maze.CacheData();
            return maze;
        }

        private int CalculateneighbourCellCount(int x, int y, GridCell[,] grid)
        {
            var result = 0;

            if (y > 0 && grid[x, y - 1].CellState == MazeCellState.Empty)
            {
                result++;
            }
            if (y < grid.GetLength(1) - 1 && grid[x, y + 1].CellState == MazeCellState.Empty)
            {
                result++;
            }
            if (x > 0 && grid[x - 1, y].CellState == MazeCellState.Empty)
            {
                result++;
            }
            if (x < grid.GetLength(0) - 1 && grid[x + 1, y].CellState == MazeCellState.Empty)
            {
                result++;
            }

            return result;
        }

        private List<Vector2Int> GetWallsAt(int x, int y, GridCell[,] grid)
        {
            var walls = new List<Vector2Int>();

            if (TryGetWallAt(x, y + 1, grid))
            {
                walls.Add(new Vector2Int(x, y + 1));
            }
            if (TryGetWallAt(x + 1, y, grid))
            {
                walls.Add(new Vector2Int(x + 1, y));
            }
            if (TryGetWallAt(x, y - 1, grid))
            {
                walls.Add(new Vector2Int(x, y - 1));
            }
            if (TryGetWallAt(x - 1, y, grid))
            {
                walls.Add(new Vector2Int(x - 1, y));
            }

            return walls;
        }

        private bool TryGetWallAt(int x, int y, GridCell[,] grid)
        {
            if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1))
            {
                return false;
            }

            if (grid[x, y].IsVisited)
            {
                return false;
            }


            grid[x, y].IsVisited = true;
            return grid[x, y].CellState != MazeCellState.Empty;
        }

        private GridCell[,] GetRanodmGrid(int width, int height)
        {
            var grid = new GridCell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var wallType = Random.Range(0, 2) > 0 ? MazeCellState.BreakableWall : MazeCellState.UnbreakableWall;
                    grid[x, y] = new GridCell(wallType, false);
                }
            }

            return grid;
        }

        private class GridCell
        {
            public MazeCellState CellState;
            public bool IsVisited;

            public GridCell(MazeCellState mazeCellState, bool isVisited)
            {
                CellState = mazeCellState;
                IsVisited = isVisited;
            }

        }
    }
}
