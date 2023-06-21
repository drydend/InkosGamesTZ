using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace TanksSystem
{
    [CreateAssetMenu(menuName ="Enemy Factory")]
    public class EnemyTankFactory : ScriptableObject
    {
        [SerializeField]
        private List<EnemyTankConfig> _enemyTanks;

        public EnemyTank CreateRandomTank()
        {
            var config = _enemyTanks.GetRandom();
            var instance = Instantiate(config.TankPrefab);

            var weapon = new Weapon(config.BulletPrefab);
            instance.Initialize(config, weapon);

            return instance;
        }
    }
}
