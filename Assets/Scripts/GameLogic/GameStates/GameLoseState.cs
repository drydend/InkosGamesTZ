using GameUI;
using StateMachines;
using TMPro;
using UnityEngine;

namespace GameLogic.GameStates
{
    public class GameLoseState : BaseState
    {
        private UIMenu _loseScreen;
        private readonly AudioClip _loseSound;
        private readonly AudioSource _audioSource;

        public GameLoseState(UIMenu loseScreen, AudioClip loseSound, AudioSource audioSource)
        {
            _loseScreen = loseScreen;
            _loseSound = loseSound;
            _audioSource = audioSource;
        }

        public override void Enter()
        {
            _loseScreen.Open();
            _audioSource.PlayOneShot(_loseSound);
        }

        public override void Exit()
        {
            _loseScreen.Close();
        }
    }
}
