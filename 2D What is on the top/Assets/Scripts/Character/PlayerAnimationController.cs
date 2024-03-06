
using UnityEngine;

public class PlayerAnimationController
{

    private Animator _playerAnimator;

    public PlayerAnimationController(Animator playerAnimator) =>
        _playerAnimator = playerAnimator;
    
        public void JumpAnimationTrigger() => _playerAnimator.SetTrigger("Jump"); // # Todo : - вынести названия анимации в static class
    
    public void RollUpwardAnimationTrigger() => _playerAnimator.SetTrigger("RollUpward");

    public void RunAnimation(float speed) => _playerAnimator.SetFloat("Speed", speed);
    
    public void WalkAnimation(float speed) => _playerAnimator.SetFloat("Speed", speed);
    
    public void IdleAnimation() => _playerAnimator.SetFloat("Speed", 0);

    public void IsPlatform(bool isPlatform) => _playerAnimator.SetBool("IsPlatform", isPlatform); // это для выхода из анимации крутящегошися щита; 
    
    public void LoseBounceAnimationTrigger() => _playerAnimator.SetTrigger("StartLoseBounce");
    
    public void LoseBouncePrepareToLand(bool isPrepareToFall) => _playerAnimator.SetBool("LoseBouncePrepareToLand", isPrepareToFall);

    public void LoseBounceLanding(bool isMainPlatform)
    { 
        _playerAnimator.SetBool("LoseBouncePrepareToLand", false);
        _playerAnimator.SetBool("IsMainPlatform", isMainPlatform);
    }
}
