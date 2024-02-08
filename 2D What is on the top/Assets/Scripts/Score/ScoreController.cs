using UnityEngine;

namespace Score
{
    public class ScoreController : MonoBehaviour
    {
        // [SerializeField] private HeightTrackerView _heightTrackerView;
        // [SerializeField] private Transform _character;
        //
        // [SerializeField] private CharacterController _characterController;
        //
        // private HeightTracker _heightTracker;
        //
        // private void Awake()
        // {
        //     _heightTracker = new HeightTracker(_character.position.y);
        // }
        //
        // private void Update()
        // {
        //     _heightTracker.CalculateHeight(_character.position.y);
        // }
        //
        // private void OnEnable()
        // {
        //     _heightTracker.HeightTrackerChange += OnHeightTrackerUpdate;
        // }
        //
        // private void OnDisable()
        // {
        //     _heightTracker.HeightTrackerChange -= OnHeightTrackerUpdate;
        // }
        //
        // private void OnHeightTrackerUpdate(int heightAmount)
        // {
        //     _heightTrackerView.HeightScoreUpdate(heightAmount);
        // }
    }
}