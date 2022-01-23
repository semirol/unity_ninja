using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace rocker
{
    public class CooldownScrollCircle: ScrollCircle
    {
        protected virtual string ImageName=> "";
        private Image _coolingImage;
        private float _CD = 5f;
        private float _currentCD = 0f;
        
        protected override void Start()
        {
            base.Start();
            _coolingImage = GameObject.Find(ImageName).GetComponent<Image>();
        }
        
        public override void BackToBegin()
        {
            if (_currentCD < float.Epsilon)
            {
                OnTriggerBackToBegin();
                _currentCD = _CD;
            }
            base.BackToBegin();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (_currentCD > float.Epsilon)
            {
                _currentCD -= Time.deltaTime;
                //按时间比例计算出Fill Amount值
                _coolingImage.fillAmount = _currentCD / _CD;
            }
        }

        
        
        protected virtual void OnTriggerBackToBegin()
        {
            
        }
        
    }
}