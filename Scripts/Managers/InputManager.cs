using rocker;
using UnityEngine;

namespace Managers
{
    public class InputManager : UnitySingleton<InputManager>
    {

        private RockerScrollCircle _joystick;
        private BoScrollCircle _boScrollCircle;
        private TiScrollCircle _tiScrollCircle;
        private DuScrollCircle _duScrollCircle;
        private BiScrollCircle _biScrollCircle;
        private float _joyStickThreshold; // 摇杆触发阈值

        private Vector2 preMoveDirection;
        // Start is called before the first frame update
        public void Init()
        {
            _joyStickThreshold = 0.3f;
            var canvas = GameObject.Find("BattleGUI");
            _joystick = canvas.GetComponentInChildren<RockerScrollCircle>();
            _boScrollCircle = canvas.GetComponentInChildren<BoScrollCircle>();
            _tiScrollCircle = canvas.GetComponentInChildren<TiScrollCircle>();
            _duScrollCircle = canvas.GetComponentInChildren<DuScrollCircle>();
            _biScrollCircle = canvas.GetComponentInChildren<BiScrollCircle>();
        }

        /**
         * 清空记录的输入触发状态，一般被外部游戏逻辑迭代组件所调用
         * 例如在帧同步中，在每一次逻辑帧更新时被BattleManager调用
         */
        public void ClearRecord()
        {
            _boScrollCircle.ClearRecord();
            _tiScrollCircle.ClearRecord();
            _duScrollCircle.ClearRecord();
            _biScrollCircle.ClearRecord();
        }

        public Vector3 GetMoveDirection()
        {

            var h = _joystick.GetAxis("Horizontal");
            var v = _joystick.GetAxis("Vertical");

            if (h == 0f)
            {
                h = Input.GetAxis("Horizontal");
            }

            if (v == 0f)
            {
                v = Input.GetAxis("Vertical");
            }

            if (Mathf.Abs(h) > _joyStickThreshold || Mathf.Abs(v) > _joyStickThreshold)
            {
                return new Vector3(h, 0, v).normalized;  
            }
            return Vector3.zero;
        }

        public bool GetEnergyWave()
        {
            return _boScrollCircle.GetSkillVector() != Vector2.zero;
        }

        public Vector3 GetEnergyWaveVector()
        {
            return ToVector3(_boScrollCircle.GetSkillVector());
        }
        
        public bool GetBlink()
        {
            return _tiScrollCircle.GetSkillVector() != Vector2.zero;
        }

        public Vector3 GetBlinkVector()
        {
            return ToVector3(_tiScrollCircle.GetSkillVector());
        }

        public bool GetShield()
        {
            return _duScrollCircle.GetSkillVector() != Vector2.zero;
        }

        public Vector3 GetShieldVector()
        {
            return ToVector3(_duScrollCircle.GetSkillVector());
        }

        public bool GetDart()
        {
            return _biScrollCircle.GetSkillVector() != Vector2.zero;
        }

        public Vector3 GetDartVector()
        {
            return ToVector3(_biScrollCircle.GetSkillVector());
        }

        //(x,y) -> (x, 0, y)
        private Vector3 ToVector3(Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

    }
}