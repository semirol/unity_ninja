using System;
using Core;
using Core.FixedArithmetic;
using Game;
using Managers;
using UnityEngine;
using Utils;

public class PlayerBehaviour : UnitySingleton<PlayerBehaviour>
{
    private Transform _mainCameraTransform;
    private Vector3 _offset;

    private Animator _animator;

    private FVector3 _logicPosition;
    private Vector3 _virtualPosition; // 从logicPosition开始每一帧的运动叠加到该位置，并将实际位置向该位置插值

    private int _state = PlayerStateEnum.INVALID;
    
    public override void Awake()
    {
        base.Awake();
        _animator = gameObject.GetComponent<Animator>();
        _animator.avatar = ResourceManager.Instance.GetAssetFromShortPath<Avatar>("Characters/Player/player.fbx[playerAvatar]");
        _animator.runtimeAnimatorController = ResourceManager.Instance.GetAssetFromShortPath<RuntimeAnimatorController>("Characters/Player/PlayerAnimatorController.controller");
        
        _mainCameraTransform = GameObject.Find("MainCamera").transform;
        _offset = new Vector3(0, 100, -40);
        
        _logicPosition = new FVector3(transform.position);
        _virtualPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.GetMoveDirection() != Vector3.zero)
        {
            //切换动画
            SetState(PlayerStateEnum.MOVE);
            // 调整方向
            transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, 
                InputManager.Instance.GetMoveDirection(), Vector3.up), 0);
            // 计算偏移量
            Vector3 offset = transform.forward * Constants.PLAYER_MOVE_SPEED * Time.deltaTime;
            // 更新虚拟位置
            _virtualPosition += offset;
            // 本应到达的目的位置
            Vector3 targetPosition = transform.position + offset;
            // 向虚拟位置插值
            Vector3 lerpPosition = Vector3.Lerp(targetPosition, _virtualPosition, Constants.FRAME_FREQ * Time.deltaTime);
            transform.position = lerpPosition;
        }
        else
        {
            //切换动画
            SetState(PlayerStateEnum.IDLE);
        }
    }

    private void SetState(int state)
    {
        if (state == _state)
        {
            return;
        }
        else if (state == PlayerStateEnum.IDLE)
        {
            _animator.SetBool("ifWalk", false);
        }
        else if (state == PlayerStateEnum.MOVE)
        {
            _animator.SetBool("ifWalk", true);
        }
    }
    private void LateUpdate()
    {
        // _mainCameraTransform.position = transform.position + _offset;
        _mainCameraTransform.position = Vector3.Lerp (_mainCameraTransform.position, transform.position + _offset, Time.deltaTime * 5f);
    }
    
    
    //被BattleManager每次收到FrameMessage时调用
    public void HandlePlayerOperation(BattleOperation operation)
    {
        if (operation.OperationType == OperationTypeEnum.STAY) return;
        else if (operation.OperationType == OperationTypeEnum.MOVE)
        {
            UpdateLogicTransform(operation);
        }
        else if (operation.OperationType == OperationTypeEnum.WAVE)
        {
            DoEnergyWave(operation);
        }
        else if (operation.OperationType == OperationTypeEnum.DART)
        {
            Debug.Log("dart");
            DoDart(operation);
        }
        else if (operation.OperationType == OperationTypeEnum.SHIELD)
        {
            DoShield(operation);
        }
        else if (operation.OperationType == OperationTypeEnum.BLINK)
        {
            DoBlink(operation);
        }
    }

    private void UpdateLogicTransform(BattleOperation operation)
    {
        _logicPosition.X = operation.Position.X;
        _logicPosition.Z = operation.Position.Z;
        _virtualPosition = _logicPosition.ToVector3();
    }

    private void DoEnergyWave(BattleOperation operation)
    {
        GameObject energyWave = InstantiateByCondition(operation, "Effects/Skills/EnergyWave/EnergyWave.prefab",
            "PlayerEnergyWave", Constants.SKILL_OFFSET);
        energyWave.AddComponent<EnergyWaveBehaviour>().Init(operation);
    }

    private void DoDart(BattleOperation operation)
    {
        Debug.Log("dodart");
        GameObject dart = InstantiateByCondition(operation, "Effects/Skills/Dart/Dart.prefab",
            "PlayerDart", Constants.SKILL_OFFSET);
        dart.AddComponent<DartBehaviour>().Init(operation);
    }

    private void DoShield(BattleOperation operation)
    {
        GameObject shield = InstantiateByCondition(operation, "Effects/Skills/Shield/Shield.prefab",
            "PlayerShield", 0);
        shield.AddComponent<ShieldBehaviour>().Init(operation);
    }

    private void DoBlink(BattleOperation operation)
    {
        
    }

    private GameObject InstantiateByCondition(BattleOperation operation, string path, string objectName, float skillOffset)
    {
        GameObject skillPrefab = ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>(path);
        Quaternion skillRotation = Quaternion.Euler(0, operation.Direction, 0);
        Vector3 offset = Quaternion.Euler(0, operation.Direction, 0) * Vector3.forward * skillOffset;
        GameObject skill = Instantiate(skillPrefab, _virtualPosition + offset, skillRotation);
        skill.name = objectName;
        return skill;
    }

    public FVector3 GetLogicPosition()
    {
        return _logicPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "EnemyEnergyWave")
        {
            BattleManager.Instance.HandleBattleEndLose();
        }
        else if (other.name == "EnemyDart")
        {
            BattleManager.Instance.HandleBattleEndLose();
        }
    }
}
