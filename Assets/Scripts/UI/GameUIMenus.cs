using UnityEngine;

namespace GameUI
{
    public class GameUIMenus : MonoBehaviour
    {
        [field: SerializeField] public UIMenu LoadingScreen { get; private set; }
        [field: SerializeField] public UIMenu RuningGameUI { get; private set; }
        [field: SerializeField] public UIMenu WinScreen { get; private set; }
        [field: SerializeField] public UIMenu LoseScreen { get; private set; }
    }
}
