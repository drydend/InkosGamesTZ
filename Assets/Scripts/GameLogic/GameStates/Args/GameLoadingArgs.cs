namespace GameLogic.GameStates.Args
{
    public class GameLoadingArgs
    {
        public int FieldWidth { get; private set; }
        public int FieldHeight { get; private set;}

        public GameLoadingArgs(int fieldWidth, int fieldHeight)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
        }
    }
}