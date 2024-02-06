using System;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

namespace Controllers
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterMover _characterMover;
        [SerializeField] private SwipeListener _swipeListener;

        private void OnEnable()
        {
            _swipeListener.OnSwipe.AddListener(OnJumpedSwiped);
        }
        
        private void OnDisable()
        {
            _swipeListener.OnSwipe.RemoveListener(OnJumpedSwiped);
        }

        private void OnJumpedSwiped(string swipe)
        {
            if (swipe == DirectionId.ID_LEFT)
            {
                _characterMover.Jump(false);
                _characterMover.SetToDefaultSpeed(false);
            }

            if (swipe == DirectionId.ID_RIGHT)
            {
                _characterMover.Jump(true);
                _characterMover.SetToDefaultSpeed(false);
            }

            if (swipe == DirectionId.ID_DOWN)
            {
                _characterMover.SetToDefaultSpeed(true);
            }

            if (swipe == DirectionId.ID_UP)
            {
                _characterMover.PerformRoll();
                _characterMover.SetToDefaultSpeed(false);
            }

        }
    }
}