using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace MazeSystem
{
    public class Maze
    {
        public readonly int Width;
        public readonly int Height;

        private List<Vector2Int> _emptyCells;
        private Vector2Int? _playerBasePos;

        private MazeCellState[,] _cells;

        private Vector2Int? PlayerBasePos
        {
            get
            {
                if (_playerBasePos == null)
                {
                    CachePlayerBasePosition();
                }

                return _playerBasePos;
            }
        }

        private List<Vector2Int> EmptyCells
        {
            get
            {
                if (_emptyCells == null)
                {
                    CacheEmptyCells();
                }

                return _emptyCells;
            }
        }

        public Maze(int width, int heigh)
        {
            Width = width;
            Height = heigh;
            _cells = new MazeCellState[width, heigh];
        }

        public void CacheData()
        {
            CacheEmptyCells();
            CachePlayerBasePosition();
        }

        public MazeCellState this[int x, int y]
        {
            get
            {
                return _cells[x, y];
            }
            set
            {
                _cells[x, y] = value;
            }
        }

        public Vector2 GetWorldPos(Vector2 position)
        {
            return new Vector2(position.x - (float)Width / 2, position.y - (float)Height / 2);
        }

        public Vector2 GetRandomEmptyPosition()
        {
            var randomCell = EmptyCells.GetRandom();
            return new Vector2(randomCell.x, randomCell.y);
        }

        public Vector2 GetPlayerSpawnPos()
        {
            var position = GetNearestCellOfType(PlayerBasePos.Value, MazeCellState.Empty);

            return new Vector2(position.x - (float)Width / 2, position.y - (float)Height / 2);
        }

        public Path GetPathToPlayerBase(Vector2Int position)
        {
            var startPos = position;

            var path = new Queue<Vector2Int>();
            var hashSet = new HashSet<Vector2Int>();
            var moveDirection = new Dictionary<Vector2Int, Vector2Int>();

            Vector2Int destination;

            hashSet.Add(position);
            path.Enqueue(position);

            destination = GetNearestCellOfType(PlayerBasePos.Value, MazeCellState.Empty);

            while (path.Count > 0)
            {
                position = path.Dequeue();
                hashSet.Add(position);

                if (position == destination)
                {
                    if (!IsAccessible(PlayerBasePos.Value))
                    {
                        Vector2 direction = PlayerBasePos.Value - destination;
                        direction.Normalize();
                        moveDirection[destination + new Vector2Int((int)direction.x, (int)direction.y)] = destination;
                        destination = destination + new Vector2Int((int)direction.x, (int)direction.y);
                    }

                    return GetPathWorld(moveDirection, destination, startPos);
                }

                var nearbyCells = GetNeabyEmptyCells(position);

                foreach (var item in nearbyCells)
                {
                    if (hashSet.Contains(item))
                    {
                        continue;
                    }

                    path.Enqueue(item);
                    moveDirection[item] = position;
                }
            }

            throw new Exception("There is no avaible path to destination in maze");
        }

        private void CacheEmptyCells()
        {
            _emptyCells = new List<Vector2Int>();

            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                for (int y = 0; y < _cells.GetLength(1); y++)
                {
                    if (_cells[x, y] == MazeCellState.Empty)
                    {
                        _emptyCells.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        private Path GetPathWorld(Dictionary<Vector2Int, Vector2Int> moveDirection, Vector2Int origin, Vector2Int destination)
        {
            var path = new List<Vector2>();

            var current = origin;
            path.Add(new Vector2(origin.x - (float)Width / 2, origin.y - (float)Height / 2));

            while (current != destination)
            {
                current = moveDirection[current];
                path.Add(new Vector2(current.x - (float)Width / 2, current.y - (float)Height / 2));
            }

            path.Reverse();

            return new Path(path);
        }

        private List<Vector2Int> GetNeabyCells(Vector2Int position)
        {
            var cells = new List<Vector2Int>();

            if (position.x > 0)
            {
                cells.Add(new Vector2Int(position.x - 1, position.y));
            }
            if (position.x < Width - 1)
            {
                cells.Add(new Vector2Int(position.x + 1, position.y));
            }
            if (position.y > 0)
            {
                cells.Add(new Vector2Int(position.x, position.y - 1));
            }
            if (position.y < Height - 1)
            {
                cells.Add(new Vector2Int(position.x, position.y + 1));
            }

            return cells;
        }

        private List<Vector2Int> GetNeabyEmptyCells(Vector2Int position)
        {
            var nearbyCells = GetNeabyCells(position);
            var emptyCells = nearbyCells.Where(pos => _cells[pos.x, pos.y] == MazeCellState.Empty).ToList();

            return emptyCells;
        }

        private void CachePlayerBasePosition()
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                for (int y = 0; y < _cells.GetLength(1); y++)
                {
                    if (_cells[x, y] == MazeCellState.PlayerBase)
                    {
                        _playerBasePos = new Vector2Int(x, y);
                        return;
                    }
                }
            }

            throw new Exception("There is no player base in maze");
        }

        private bool IsAccessible(Vector2Int position)
        {
            if (position.x > 0 && _cells[position.x - 1, position.y] == MazeCellState.Empty)
            {
                return true;
            }
            if (position.x < Width - 1 && _cells[position.x + 1, position.y] == MazeCellState.Empty)
            {
                return true;
            }
            if (position.y > 0 && _cells[position.x, position.y - 1] == MazeCellState.Empty)
            {
                return true;
            }
            if (position.y < Height - 1 && _cells[position.x, position.y + 1] == MazeCellState.Empty)
            {
                return true;
            }

            return false;
        }

        private Vector2Int GetNearestCellOfType(Vector2Int startPosition, MazeCellState type)
        {
            Vector2Int basePos = startPosition;

            Vector2Int position;

            var cells = new Queue<Vector2Int>();
            var hashSet = new HashSet<Vector2Int>();

            cells.Enqueue(basePos);

            while (cells.Count > 0)
            {
                position = cells.Dequeue();
                hashSet.Add(position);

                if (_cells[position.x, position.y] == type)
                {
                    return position;
                }

                var nearbyCells = GetNeabyCells(position);

                foreach (var cell in nearbyCells)
                {
                    if (hashSet.Contains(cell))
                    {
                        continue;
                    }

                    hashSet.Add((Vector2Int)cell);
                    cells.Enqueue(cell);
                }
            }

            throw new Exception("There is no empy space for player to spawn");
        }
    }
}
