using UnityEngine;

public interface IPlayer
{
    public Transform Transform { get; }
    public bool IsOnPlatform { get; }
}