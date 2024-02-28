using UnityEngine;

public interface IPlayer
{
    public Transform Transform { get; }
    public Transform TransformPlatformDetection { get; }
    // public bool IsOnPlatform { get; }
}