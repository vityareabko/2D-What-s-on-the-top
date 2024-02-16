using System;

public interface IPlayer
{
    public event Action<bool> PlayerIsOnRightWall;
    public event Action LevelDefeat; 
    public event Action LevelWin;
    public void BlockSwipe(bool isBlock);

    public void PlayerLose();
}