using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    public class MazeView
    {
        public List<CellView> Borders { get; private set; }
        public Dictionary<CellView, Vector2Int> Cells { get; private set; }

        public CellView PlayerBase { get; private set; }

        public MazeView(Dictionary<CellView, Vector2Int> cells, CellView playerBase, List<CellView> borders)
        {
            Cells = cells;
            PlayerBase = playerBase;
            Borders = borders;
        }

        public void Destroy()
        {
            foreach (var cell in Cells)
            {
                Object.Destroy(cell.Key.gameObject);
            }

            foreach (var border in Borders)
            {
                Object.Destroy(border);
            }
        }

    }
}
