using MazeSystem;

namespace GameLogic.GameStates.Args
{
    public class GameRuningStateArgs
    {
        public MazePresenter MazePresenter { get; private set; }

        public GameRuningStateArgs(MazePresenter mazePresenter)
        {
            MazePresenter = mazePresenter;
        }
    }
}