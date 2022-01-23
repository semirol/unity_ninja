using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.FixedArithmetic;
using Game;
using UnityEngine;
using Utils;

namespace Managers
{
    public class BattleManager : UnitySingleton<BattleManager>
    {
        private volatile NinjaFrameMessage _frameMessage;
        private volatile bool _ifReceiveFrameMessage;
        
        private int _playerId;
        private int _frameNo;
        private volatile bool _battleEnd;
        private int _losePlayerId;
        private bool _ifSurrenderRequestSend;
        
        private CancellationTokenSource _tokenSource;
        
        /**
         * 每次调用后相当于重置整个BattleManager状态，重新开始一场新的游戏。由GameApp调用
         */
        public void StartBattle()
        {
            EnsureCanStartBattle();
            // 初始化战斗数据、帧序号、战斗是否结束的变量等
            _frameMessage = null;
            _ifReceiveFrameMessage = false;
            _frameNo = 0;
            _battleEnd = false;
            _losePlayerId = -1;
            _ifSurrenderRequestSend = false;
            _playerId = LocalDataManager.Instance.Get<int>("playerId");
            Task.Run(ReceiveMessage, _tokenSource.Token);
        }
        
        public void Update()
        {
            //首先判断是否有新的帧同步消息
            if (_ifReceiveFrameMessage && !_battleEnd)
            {
                HandleMessage();
                BattleOperation operation = GenerateThisFrameOperation();
                NetWorkManager.Instance.Send(new NinjaOperationMessage(operation));
                //通知InputManager更新输入状态
                InputManager.Instance.ClearRecord();
            }
        }

        /**
         * 清除上场战斗中可能残留的逻辑
         */
        private void EnsureCanStartBattle()
        {
            //如果上次战斗没结束的话会在这里强制结束，一般不会出现这种情况
            try
            {
                _tokenSource.Cancel();
            }
            catch
            {
                //ignore
            }
            _tokenSource = new CancellationTokenSource();
        }

        private void ReceiveMessage()
        {
            while (!_battleEnd)
            {
                // 阻塞接收服务器的帧消息，并进行处理
                NinjaFrameMessage message = NetWorkManager.Instance.Receive<NinjaFrameMessage>();
                _frameMessage = message;
                _ifReceiveFrameMessage = true;
            }
        }
        
        private void HandleMessage()
        {
            lock (_frameMessage)
            {
                List<Frame> frameList = _frameMessage.FrameList;
                foreach (var frame in frameList)
                {
                    if (frame.FrameNo == _frameNo) // 小于的帧已经处理完，所以直接丢弃掉
                    {
                        List<BattleOperation> operationList = frame.OperationList;
                        foreach (var operation in operationList)
                        {
                            //检查游戏结束
                            if (operation.OperationType == OperationTypeEnum.LOSE)
                            {
                                HandleBattleEnd(operation.PlayerId);
                            }
                            
                            if (operation.PlayerId == _playerId)
                            {
                                // 更新玩家（自己）的游戏逻辑信息
                                PlayerBehaviour.Instance.HandlePlayerOperation(operation);
                            }
                            else
                            {
                                // 更新对手的游戏逻辑信息
                                EnemyBehaviour.Instance.HandleEnemyOperation(operation);
                            }
                        }
                        _frameNo = frame.FrameNo + 1; 
                    }
                
                }
                // 修改该变量表示消息处理完毕
                _ifReceiveFrameMessage = false;
            }
        }

        private BattleOperation GenerateThisFrameOperation()
        {
            // 获取操作信息，包含操作类型和方向
            int direction;
            int operationType = GetOperation(out direction);
            // 移动信息直接同步，防止积累过大浮点误差（为了避免写复杂的定点运算库。。）
            if (operationType == OperationTypeEnum.MOVE)
            {
                Vector3 offset = Quaternion.Euler(new Vector3(0, direction, 0)) * Vector3.forward 
                    * Constants.PLAYER_MOVE_SPEED * Constants.FRAME_INTERVAL / 1000;
                FVector3 fOffset = new FVector3(offset);
                FVector3 nextPosition = PlayerBehaviour.Instance.GetLogicPosition() + fOffset;
                return new BattleOperation(_frameNo, _playerId, operationType, nextPosition);
            }
            return new BattleOperation(_frameNo, _playerId, operationType, direction);
        }

        private int GetOperation(out int direction)
        {
            if (_ifSurrenderRequestSend)
            {
                direction = 0;
                return OperationTypeEnum.LOSE;
            }
            else if (InputManager.Instance.GetEnergyWave())
            {
                direction = ToAngelDirection(InputManager.Instance.GetEnergyWaveVector());
                return OperationTypeEnum.WAVE;
            }
            else if (InputManager.Instance.GetDart())
            {
                direction = ToAngelDirection(InputManager.Instance.GetDartVector());
                return OperationTypeEnum.DART;
            }
            else if (InputManager.Instance.GetShield())
            {
                direction = 0;
                return OperationTypeEnum.SHIELD;
            }
            else if (InputManager.Instance.GetBlink())
            {
                direction = ToAngelDirection(InputManager.Instance.GetBlinkVector());
                return OperationTypeEnum.BLINK;
            }
            else if (InputManager.Instance.GetMoveDirection() != Vector3.zero)
            {
                direction = ToAngelDirection(InputManager.Instance.GetMoveDirection());
                return OperationTypeEnum.MOVE;
            }

            direction = 0;
            return OperationTypeEnum.STAY;
        }

        private int ToAngelDirection(Vector3 vector3)
        {
            return (int) Vector3.SignedAngle(Vector3.forward, vector3, Vector3.up);
        }

        /**
         * 被自己和其他逻辑调用：playerBehaviour中判断碰撞
         */
        private void HandleBattleEnd(int playerId)
        {
            _losePlayerId = playerId;
            _battleEnd = true;
            if (_losePlayerId == _playerId)
            {
                LogUtils.Log("you lose");
            }
            else
            {
                LogUtils.Log("you win");
            }
            Invoke(nameof(InvokeLoadMainScene), 2f);
        }

        public void HandleBattleEndLose()
        {
            HandleBattleEnd(_playerId);
        }

        private void InvokeLoadMainScene()
        {
            GameApp.Instance.LoadMainScene();
        }

        /**
         * 被battleGUI的投降按钮的click事件调用
         */
        public void HandleSurrenderRequestSend()
        {
            _ifSurrenderRequestSend = true;
        }
    }
}