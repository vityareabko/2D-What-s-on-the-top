using System;
using TMPro;
using UnityEngine;

namespace Score
{
    public class HeightTrackerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _heightScoreText;
        [SerializeField] private Transform _characterTransform;

        private HeightTracker _heightTracker;

        private void Awake()
        {
            _heightTracker = new HeightTracker(_characterTransform, 0f);
            var height =_heightTracker.CalculateHeight();
            _heightScoreText.text = height.ToString();
        }

        private void Update()
        {
            if (_heightTracker.CalculateHeight() > 0)
                _heightScoreText.text = _heightTracker.CalculateHeight().ToString();
            else
                _heightScoreText.text = "0";
            
        }
    }
}