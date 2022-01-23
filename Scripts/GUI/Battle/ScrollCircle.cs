using UnityEngine;
using UnityEngine.UI;

namespace rocker
{
    public abstract class ScrollCircle : ScrollRect
    {
        protected virtual float MRadiusScale => 1f; // 摇杆块移动半径缩放比例
        private float _mRadius = 0f; // 摇杆块移动半径
        protected Vector2 MoveRate = Vector2.zero; // 摇杆块移动比例（移动到最大是单位向量）
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            //计算摇杆块的半径
            _mRadius = ((RectTransform) transform).sizeDelta.x * MRadiusScale;
        }
        
        public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData)
        {
            base.OnDrag (eventData);
            var contentPosition = content.GetComponent<RectTransform>().anchoredPosition;
            if (contentPosition.magnitude > _mRadius){
                contentPosition = contentPosition.normalized * _mRadius ;
                SetContentAnchoredPosition(contentPosition);
            }

            MoveRate = contentPosition / _mRadius; // 计算摇杆摇动比例
        }

        public virtual void BackToBegin()
        {
            content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            MoveRate = Vector2.zero;
        }
    }
}
