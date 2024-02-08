using System;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

namespace Controllers
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private SwipeListener _swipeListener;
        [SerializeField] private CharacterMover _characterMover;

        [SerializeField] private StaminaView _staminaView;
        
        [SerializeField] private CharacterData _characterData;
        
        private Stamina _stamina;

        private void Awake()
        {
            _stamina = new Stamina(_characterData._staminaData);
            _staminaView.Initialize(_characterData._staminaData.MinStamina, _characterData._staminaData.MaxStamina);
            _characterMover.Initialize(_characterData, _stamina);
        }
        
        private void OnEnable()
        {
            _swipeListener.OnSwipe.AddListener(OnJumpedSwiped);
            _stamina.StaminaChange += OnStaminaChange;
        }

        private void OnDisable()
        {
            _swipeListener.OnSwipe.RemoveListener(OnJumpedSwiped);
        }

        private void OnStaminaChange(float staminaValue)
        { 
            _staminaView.SetStaminaValue(staminaValue);
        }

        private void OnJumpedSwiped(string swipe)
        {
            if (_stamina.isEnough() == false)
                return;
            
            if (swipe == DirectionId.ID_LEFT)
            {
                _characterMover.Jump(false);
            }

            if (swipe == DirectionId.ID_RIGHT)
            {
                _characterMover.Jump(true);
            }

            if (swipe == DirectionId.ID_DOWN)
            {
                _characterMover.SlowDownFlag(true);
            }

            if (swipe == DirectionId.ID_UP)
            {
                _characterMover.PerformRoll();
            }
        }
    }
}