using MazeSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    [CreateAssetMenu(menuName = "Cell Factory")]
    public class CellFactory : ScriptableObject
    {
        [SerializeField]
        private CellView _borderPrefab;
        [SerializeField]
        private List<CellTypeCellViewPair> _views;

        public CellView GetView(MazeCellState cellState)
        {
            var viewPrefab = _views.Find(pair => pair.Type == cellState).View;

            if(viewPrefab == null)
            {
                throw new Exception($"CellFacroty does not support cell type: {cellState}");
            }

            var viewInstance = Instantiate(viewPrefab);

            return viewInstance;
        }

        public CellView GetViewBorder()
        {
            var viewInstance = Instantiate(_borderPrefab);

            return viewInstance;
        }
    }
}
