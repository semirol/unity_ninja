using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.FixedArithmetic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class BattleManager : UnitySingleton<BattleManager>
    {
        private bool _battleEnd;
        private int _playerId;
        private int _losePlayerId;

        private volatile NinjaOperationMessage _operationMessage;
        private volatile bool _ifReceiveOperationMessage;
        
        private CancellationTokenSource _tokenSource;

        public void StartBattle()
        {
            EnsureCanStartBattle();
            _operationMessage = null;
            _ifReceiveOperationMessage = false;
            _battleEnd = false;
            _playerId = LocalDataManager.Instance.Get<int>("playerId");
            Task.Run(ReceiveMessage, _tokenSource.Token);
        }

        public void Update()
        {
            
        }

        public void FixedUpdate()
        {
            if (!_battleEnd)
            {
                //首先判断是否有新的同步消息
                if (_ifReceiveOperationMessage)
                {
                    if (_operationMessage.Operation.OperationType == OperationTypeEnum.LOSE)
                    {
                        HandleBattleEnd("Enemy");
                    }
                    else
                    {
                        PlayerBehaviour.Enemy.HandlePlayerOperation(_operationMessage.Operation);
                    }
                    _ifReceiveOperationMessage = false;
                }
            }
            BattleOperation operation = GenerateThisFrameOperation();
            NetWorkManager.Instance.Send(new NinjaOperationMessage(operation));
            //通知InputManager更新输入状态
            InputManager.Instance.ClearRecord();
            PlayerBehaviour.Player.HandlePlayerOperation(operation);
        }

        private void ReceiveMessage()
        {
            while (!_battleEnd)
            {
                // 阻塞接收服务器的帧消息，并进行处理
                NinjaOperationMessage message = NetWorkManager.Instance.Receive<NinjaOperationMessage>();
                _operationMessage = message;
                _ifReceiveOperationMessage = true;
            }
        }
        
        public void HandleBattleEnd(string playerName)
        {
            _battleEnd = true;
            if (playerName == "Player")
            {
                LogUtils.Log("you lose");
                NetWorkManager.Instance.Send(new NinjaOperationMessage(
                    new BattleOperation(0, _playerId, OperationTypeEnum.LOSE, 0)));
            }
            else
            {
                LogUtils.Log("you win");
            }
            Invoke(nameof(InvokeLoadMainScene), 2f);
        }

        private void InvokeLoadMainScene()
        {
            GameApp.Instance.LoadMainScene();
        }

        private BattleOperation GenerateThisFrameOperation()
        {
            // 获取操作信息，包含操作类型和方向
            int direction;
            int operationType = GetOperation(out direction);
            // 移动信息直接同步，防止积累过大浮点误差
            if (operationType == OperationTypeEnum.MOVE)
            {
                return new BattleOperation(0, _playerId, operationType, direction, new FVector3(PlayerBehaviour.Player.transform.position));
            }
            return new BattleOperation(0, _playerId, operationType, direction);
        }

        private int GetOperation(out int direction)
        {
            if (InputManager.Instance.GetEnergyWave())
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
    }
}