using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public int GameFieldWidth { get; private set; }
        [field: SerializeField] public int GameFieldHeight { get; private set; }
         
        [field: SerializeField] public int PlayerHeals { get; private set; }
        [field: SerializeField] public float PlayerRespawnTime { get; private set; }

        [field: SerializeField] public int AllEnemiesNumber { get; private set; }
        [field: SerializeField] public int MaxEnemiesInGame { get; private set; }
        [field: SerializeField] public float EnemySpawnTime { get; private set; }

        [field: SerializeField] public AudioClip WinSound { get; private set; }
        [field: SerializeField] public AudioClip LoseSound { get; private set; }

    }
}