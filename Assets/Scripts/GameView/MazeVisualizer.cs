using MazeSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameView
{
    public class MazeVisualizer
    {
        private CellFactory _cellFactory;
        private Transform _viewParent;

        public MazeVisualizer(CellFactory factory, Transform viewParent)
        {
            _cellFactory = factory;
            _viewParent = viewParent;
        }

        public MazeView CreateView(Maze maze)
        {
            return CreateView(maze, Vector2.zero, _viewParent);
        }

        public MazeView CreateView(Maze maze, Vector2 centerWorldPos, Transform parent)
        {
            CellView playerBase = null;
            var views = new Dictionary<CellView, Vector2Int>();

            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    if (maze[x, y] == MazeCellState.Empty)
                    {
                        continue;
                    }

                    var view = CreateCellViewAt(centerWorldPos.x + x - (float)maze.Width / 2, centerWorldPos.y + y - (float)maze.Height / 2,
                        maze[x, y], parent);
                    views.Add(view, new Vector2Int(x, y));

                    if (maze[x, y] == MazeCellState.PlayerBase)
                    {
                        playerBase = view;
                    }
                }
            }

            var borders = CreateBorders(maze, centerWorldPos);

            return new MazeView(views, playerBase, borders);
        }

        private List<CellView> CreateBorders(Maze maze, Vector2 centerWorldPos)
        {
            var borders = new List<CellView>();

            var cornerBorder = _cellFactory.GetViewBorder();
            var borderPosition = new Vector2(centerWorldPos.x - (float)maze.Width / 2 - 1, centerWorldPos.y - (float)maze.Height / 2 - 1);
            cornerBorder.transform.position = borderPosition;
            borders.Add(cornerBorder);

            for (float x = -(float)maze.Width / 2; x <= (float)maze.Width / 2; x++)
            {
                var upBorderPosition = new Vector2(centerWorldPos.x + x, centerWorldPos.y + (float)maze.Height / 2);
                var downBorderPosition = new Vector2(centerWorldPos.x + x, centerWorldPos.y - (float)maze.Height / 2 - 1);

                var upBorder = _cellFactory.GetViewBorder();
                var downBorder = _cellFactory.GetViewBorder();

                upBorder.transform.position = upBorderPosition;
                downBorder.transform.position = downBorderPosition;

                upBorder.transform.parent = _viewParent;
                downBorder.transform.parent = _viewParent;

                borders.Add(upBorder);
                borders.Add(downBorder);
            }

            for (float y = -(float)maze.Height / 2; y <= (float)maze.Height / 2; y++)
            {
                var upBorderPosition = new Vector2(centerWorldPos.x + (float)maze.Width / 2, centerWorldPos.y + y);
                var downBorderPosition = new Vector2(centerWorldPos.x - (float)maze.Width / 2 - 1, centerWorldPos.y + y);

                var upBorder = _cellFactory.GetViewBorder();
                var downBorder = _cellFactory.GetViewBorder();

                upBorder.transform.position = upBorderPosition;
                downBorder.transform.position = downBorderPosition;

                upBorder.transform.parent = _viewParent;
                downBorder.transform.parent = _viewParent;

                borders.Add(upBorder);
                borders.Add(downBorder);
            }

            return borders;
        }

        private CellView CreateCellViewAt(float worldX, float worldY, MazeCellState mazeCellState, Transform parent)
        {
            var view = _cellFactory.GetView(mazeCellState);
            view.transform.position = new UnityEngine.Vector3(worldX, worldY, 0);
            view.transform.SetParent(parent);

            return view;
        }
    }
}
