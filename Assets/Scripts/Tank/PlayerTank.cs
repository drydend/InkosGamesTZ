using GameInput;
using System;
using UnityEngine;

namespace TanksSystem
{
    public class PlayerTank : Tank
    {
        private PlayerInput _input;

        public void SetInput(PlayerInput input)
        {
            _input = input;
        }


        private void LateUpdate()
        {
            if (_isPaused)
            {
                return;
            }

            if (_reloadTime >= 0)
            {
                _reloadTime -= Time.deltaTime;
            }

            Move(_input.MovementDirection);

            if (_input.IsShootButtonHolded)
            {
                TryShoot();
            }
        }
    }
}