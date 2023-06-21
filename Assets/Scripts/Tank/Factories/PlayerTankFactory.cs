using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace TanksSystem
{
    [CreateAssetMenu(menuName ="Player Tank Factory")]
    public class PlayerTankFactory : ScriptableObject
    {
        [SerializeField]
        private TankConfig _config;
        [SerializeField]
        private PlayerTank _prefab;

        public PlayerTank GetPlayerTank()
        {
            var instance = Instantiate(_prefab);

            var weapon = new Weapon(_config.BulletPrefab);
            instance.Initialize(_config, weapon);
            
            return instance;
        }
    }
}
