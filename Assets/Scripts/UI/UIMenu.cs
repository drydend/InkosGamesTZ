using UnityEngine;

namespace GameUI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _menu;

        public virtual void Open()
        {
            _menu.SetActive(true);
        }

        public virtual void Close() 
        {
            _menu.SetActive(false);
        }
    }
}
