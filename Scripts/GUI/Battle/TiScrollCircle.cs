using UnityEngine;

namespace rocker
{
    public class TiScrollCircle : CooldownScrollCircle
    {
        protected override string ImageName => "TiCDMask";
        private Vector2 _moveRate = Vector2.zero;
        protected override float MRadiusScale => 1f;

        protected override void OnTriggerBackToBegin()
        {
            _moveRate = MoveRate;
        }

        public Vector2 GetSkillVector()
        {
            return _moveRate;
        }

        public void ClearRecord()
        {
            _moveRate = Vector2.zero;
        }
    }
}
