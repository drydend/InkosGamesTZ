using UnityEngine;

namespace TanksSystem
{
    [CreateAssetMenu(menuName ="Enemy Tank Config", fileName = "Enemy Tank Config")]
    public class EnemyTankConfig : TankConfig
    {
        [field: SerializeField] public EnemyTank TankPrefab { get; private set; }
        [field: SerializeField] public int ScoreForKill { get; private set; }
    }
}
