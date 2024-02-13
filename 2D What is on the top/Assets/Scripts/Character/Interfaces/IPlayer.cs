using System;

public interface IPlayer
{
    public event Action LevelDefeat; 
    public event Action LevelWin;
    public void BlockSwipe(bool isBlock);
}