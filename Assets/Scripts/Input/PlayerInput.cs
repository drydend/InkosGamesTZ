using System;
using UnityEngine;

namespace GameInput
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private KeyCode _shootButton;
        private bool _isLastInputHorizontal;


        public bool IsShootButtonHolded { get; private set; }

        public Vector2 MovementDirection { get; private set; }

        private void Update()
        {
            if (Input.GetKeyDown(_shootButton))
            {
                IsShootButtonHolded = true;
            }
            if(Input.GetKeyUp(_shootButton)) 
            {
                IsShootButtonHolded = false;
            }

            MovementDirection = Vector2.zero;
            if (Input.GetAxis("Horizontal") != 0)
            {
                MovementDirection = new Vector2(Input.GetAxisRaw("Horizontal"),0);
            }

            if(Input.GetAxis("Vertical") != 0)
            {
                MovementDirection = new Vector2(0, Input.GetAxis("Vertical"));
            }
        }

    }
}
