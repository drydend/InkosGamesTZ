using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Utils
{
    public class Path
    {
        public List<Vector2> _points = new List<Vector2>();

        private int _index = 0;

        public Vector2 DesiredPoint
        {
            get
            {   
                return _points[_index];
            }
        }

        public bool IsAtEnd => _index >= _points.Count;

        public Path(List<Vector2> points)
        {
            _points = points;
        }

        public void MoveNext()
        {   
            _index++;
        }
    }
}