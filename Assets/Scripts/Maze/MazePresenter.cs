using GameView;
using Unity.VisualScripting;

namespace MazeSystem
{
    public class MazePresenter
    {
        public Maze Maze { get; private set; }
        public MazeView MazeView { get; private set; }

        public MazePresenter(Maze maze, MazeView mazeView)
        {
            Maze = maze;
            MazeView = mazeView;
        }

        public void Initialize()
        {
            foreach (var cell in MazeView.Cells)
            {
                cell.Key.OnDestroyed += OnCellDestroyed;
            }
        }

        public void Destroy()
        {
            foreach (var cell in MazeView.Cells)
            {
                cell.Key.OnDestroyed -= OnCellDestroyed;
            }

            MazeView.Destroy();
        }

        private void OnCellDestroyed(CellView cell)
        {   
            var position = MazeView.Cells[cell];

            Maze[position.x, position.y] = MazeCellState.Empty;

            MazeView.Cells.Remove(cell);
            cell.OnDestroyed -= OnCellDestroyed;
        }
    }
}
