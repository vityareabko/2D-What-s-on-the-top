using System.Collections;
using GG.Infrastructure.Utils.Swipe;
using UI.GameScreenDefeatView;
using UnityEngine;
using Zenject;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private SwipeListener _swipeListener;
    [SerializeField] private CharacterMover _characterMover;

    private GameScreenDefeatPresenter _defeatPresenter;
    private CharacterData _characterData;
    private Stamina _stamina;
    
    [Inject] private void Construct(CharacterData characterData, Stamina stamina, GameScreenDefeatPresenter DefeatPresenter)
    {
        _characterData = characterData;
        _stamina = stamina;
        _defeatPresenter = DefeatPresenter;
    }
    

    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);
        _characterMover.DrainStaminaRunningWalking += OnDrainStaminaRunningWalking;
    }

    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        _characterMover.DrainStaminaRunningWalking -= OnDrainStaminaRunningWalking;
    }

    private void OnDrainStaminaRunningWalking(float amount)
    {
        if (_stamina.isEnough())
        {
            _stamina.DrainStamina(amount * Time.deltaTime);
        }
        else
        {
            _characterMover.StaminaIsEnough(false);
            StartCoroutine(DefeatCoroutine());
        }
    }

    private void OnSwipeHandler(string swipe)
    {
        if (_stamina.isEnough() == false)
            return;
        

        if (swipe == DirectionId.ID_LEFT)
        {
            _characterMover.Jump(false);
            _stamina.DrainStamina(_characterData.StaminaData.StaminaDrainRateJumping);
        }

        if (swipe == DirectionId.ID_RIGHT)
        {
            _characterMover.Jump(true);
            _stamina.DrainStamina(_characterData.StaminaData.StaminaDrainRateJumping);
        }

        if (swipe == DirectionId.ID_DOWN)
        {
            _characterMover.SlowDownFlag(true);
            _stamina.DrainStamina(_characterData.StaminaData.StaminaDrainRateWalking * Time.deltaTime);
        }

        if (swipe == DirectionId.ID_UP)
        {
            _characterMover.PerformRoll();
            _stamina.DrainStamina(_characterData.StaminaData.StaminaDrainRateRoll);
        }
    }

    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _defeatPresenter.Show();
        
        yield return new WaitForSeconds(2f);
        _characterMover.gameObject.SetActive(false);
    }
}
