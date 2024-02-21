using UnityEngine;

namespace Obstacles
{
    public class FallObstacle : FallObjectBase
    {
        private int _staminaDrainRateForColision;

        public void Initialize(float speed, int staminaDrainRate)
        {
            base.Initialize(speed);
            _staminaDrainRateForColision = staminaDrainRate;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(ConstTags.Player))
            {
                EventAggregator.Post(this, new PopupTextDrainStaminEvent()
                {
                    DrainAmount = _staminaDrainRateForColision
                });
            }
        }
    }
}