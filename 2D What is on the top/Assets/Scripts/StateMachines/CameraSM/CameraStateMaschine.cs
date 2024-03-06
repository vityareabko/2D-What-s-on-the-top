
using System;

public enum CameraState
{
    PlayerOnMainMenuPlatform,
    PlayerOnPlatformLeft,
    PlayerOnPlatformRight,
    PlayerIsNotOnThePlatform,
    PlayerLoseIsNotOnPlatform,
    ShopSkinsMenu,
}

public interface ICameraStateMachine 
{
    public CameraState CurrentCameraState { get; }
}

public class CameraStateMaschine : ICameraStateMachine, IDisposable
{
    public CameraState CurrentCameraState { get; private set; }

    public CameraStateMaschine(CameraState currentCameraState)
    {
        CurrentCameraState = currentCameraState;
        
        EventAggregator.Subscribe<SwitchCameraStateOnMainMenuPlatform>(OnSwitchToMainMenuPlatfromState);
        EventAggregator.Subscribe<SwitchCameraStateOnPlayerLeftPlatform>(OnSwitchToPlayerOnLeftPlatformState);
        EventAggregator.Subscribe<SwitchCameraStateOnPlayerRightPlatform>(OnSwitchToPlayerOnRightPlatformState);
        EventAggregator.Subscribe<SwitchCameraStateOnPlayerIsNotOnThePlatform>(OnSwitchToPlayerIsNotOnPlatformState);
        EventAggregator.Subscribe<SwitchCameraStateOnPlayerLoseIsNotOnPlatform>(OnSwitchToPlayerLoseIsNotOnPlatformState);
        EventAggregator.Subscribe<SwitchCameraStateOnMainMenuShopSkins>(OnSwitchCameraStateOnMainMenuShopSkins);
    }


    public void Dispose()
    {
        EventAggregator.Unsubscribe<SwitchCameraStateOnMainMenuPlatform>(OnSwitchToMainMenuPlatfromState);
        EventAggregator.Unsubscribe<SwitchCameraStateOnPlayerLeftPlatform>(OnSwitchToPlayerOnLeftPlatformState);
        EventAggregator.Unsubscribe<SwitchCameraStateOnPlayerRightPlatform>(OnSwitchToPlayerOnRightPlatformState);
        EventAggregator.Unsubscribe<SwitchCameraStateOnPlayerIsNotOnThePlatform>(OnSwitchToPlayerIsNotOnPlatformState);
        EventAggregator.Unsubscribe<SwitchCameraStateOnPlayerLoseIsNotOnPlatform>(OnSwitchToPlayerLoseIsNotOnPlatformState);
        EventAggregator.Unsubscribe<SwitchCameraStateOnMainMenuShopSkins>(OnSwitchCameraStateOnMainMenuShopSkins);
    }

    private void SwitchCameraState(CameraState state)
    {
        switch (state)
        {
            case CameraState.PlayerOnMainMenuPlatform:
                CurrentCameraState = CameraState.PlayerOnMainMenuPlatform;
                break;
            case CameraState.PlayerOnPlatformLeft:
                CurrentCameraState = CameraState.PlayerOnPlatformLeft;
                break;
            case CameraState.PlayerOnPlatformRight:
                CurrentCameraState = CameraState.PlayerOnPlatformRight;
                break;
            case CameraState.PlayerIsNotOnThePlatform:
                CurrentCameraState = CameraState.PlayerIsNotOnThePlatform;
                break;
            case CameraState.PlayerLoseIsNotOnPlatform:
                CurrentCameraState = CameraState.PlayerLoseIsNotOnPlatform;
                break;
            case CameraState.ShopSkinsMenu:
                CurrentCameraState = CameraState.ShopSkinsMenu;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void
        OnSwitchCameraStateOnMainMenuShopSkins(object sender, SwitchCameraStateOnMainMenuShopSkins eventData) =>
        SwitchCameraState(CameraState.ShopSkinsMenu);
    
    private void OnSwitchToPlayerLoseIsNotOnPlatformState(object sender, SwitchCameraStateOnPlayerLoseIsNotOnPlatform eventData) =>
        SwitchCameraState(CameraState.PlayerLoseIsNotOnPlatform);
    
    private void OnSwitchToPlayerIsNotOnPlatformState(object sender, SwitchCameraStateOnPlayerIsNotOnThePlatform eventData) =>
        SwitchCameraState(CameraState.PlayerIsNotOnThePlatform);
    
    private void OnSwitchToPlayerOnRightPlatformState(object sender, SwitchCameraStateOnPlayerRightPlatform eventData) =>
        SwitchCameraState(CurrentCameraState = CameraState.PlayerOnPlatformRight);
    
    private void OnSwitchToPlayerOnLeftPlatformState(object sender, SwitchCameraStateOnPlayerLeftPlatform eventData) =>
        SwitchCameraState(CurrentCameraState = CameraState.PlayerOnPlatformLeft);
    
    private void OnSwitchToMainMenuPlatfromState(object sender, SwitchCameraStateOnMainMenuPlatform eventData) =>
        SwitchCameraState(CurrentCameraState = CameraState.PlayerOnMainMenuPlatform);

}
