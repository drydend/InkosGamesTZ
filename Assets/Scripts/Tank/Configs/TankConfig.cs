
using UnityEngine;
namespace TanksSystem
{
    [CreateAssetMenu(menuName = "Tank config", fileName = "Tank Config")]
    public class TankConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxHeals { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float TankSize { get; private set; }
        [field: SerializeField] public float ReloadingTime { get; private set; }
        [field: SerializeField] public Bullet BulletPrefab { get; private set; }
    }
}