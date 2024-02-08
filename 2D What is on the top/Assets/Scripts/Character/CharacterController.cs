using System;
using System.Collections;
using Character.Deafet;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;


public class CharacterController : MonoBehaviour
{
    public event Action ChareacterLose; // Это для бутстрапа где будет камера отвязыватся от игрока 
    
    [SerializeField] private CharacterData _characterData;
    
    [SerializeField] private SwipeListener _swipeListener;
    
    [SerializeField] private CharacterMover _characterMover;
    
    [SerializeField] private StaminaView _staminaView;
    
    private Stamina _stamina;
    private CharacterDeafet _characterDeafet;

    public bool aaaaSuka { get; }

    private void Awake()
    {
        _characterDeafet = new CharacterDeafet();
        _stamina = new Stamina(_characterData._staminaData);
        _staminaView.Initialize(_characterData._staminaData.MinStamina, _characterData._staminaData.MaxStamina);
        _characterMover.Initialize(_characterData, _stamina);
    }

    private void Update()
    {
        if (_stamina.isEnough() == false)
        {
            _characterDeafet.CharacterDefeat();
            return;
        }
    }

    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnJumpedSwiped);
        _stamina.StaminaChange += OnStaminaChange;
        _characterDeafet.CharacterDead += OnCharacterDefeat;
    }

    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnJumpedSwiped);
        _stamina.StaminaChange -= OnStaminaChange;
        _characterDeafet.CharacterDead -= OnCharacterDefeat;
    }
    
    private void OnCharacterDefeat()
    {

        StartCoroutine(DefeatCoroutine());
        
        // показывать popup поражения
        
        // нужно сделать боотстрап уровня чтобы взять к примеру Score и иницализировать попуп 
        
        Debug.Log("игрок проиграл");
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

    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(5f);
        _characterMover.gameObject.SetActive(false);
        ChareacterLose?.Invoke();

    }
}
