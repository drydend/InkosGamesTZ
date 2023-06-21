using GameUI;
using StateMachines;
using UnityEngine;

namespace GameLogic.GameStates
{
    public class GameWinState : BaseState
    {
        private UIMenu _winScreen;
        private readonly AudioClip _winSound;
        private AudioSource _audioSource;

        public GameWinState(UIMenu winScreen, AudioClip winSound,AudioSource audioSource)
        {
            _winScreen = winScreen;
            _winSound = winSound;
            _audioSource = audioSource;
        }

        public override void Enter()
        {
            _winScreen.Open();
            _audioSource.PlayOneShot(_winSound);
        }

        public override void Exit()
        {
            _winScreen.Close();
        }
    }
}
