using MazeSystem;
using System;
using UnityEngine;

namespace GameView
{
    [Serializable]
    public class CellTypeCellViewPair
    {
        [field: SerializeField] public MazeCellState Type { get; private set; }
        [field: SerializeField] public CellView View { get; private set; }

    }
}
