using System;
using Core;
using Game;
using Managers;
using UnityEngine;
using Utils;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour Player;

    public static PlayerBehaviour Enemy;
    
    private Animator _animator;

    private int _state = PlayerStateEnum.INVALID;

    private Rigidbody _rigidbody;

    private BattleOperation _battleOperation;
    
    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _animator.avatar = ResourceManager.Instance.GetAssetFromShortPath<Avatar>("Characters/Player/player.fbx[playerAvatar]");
        _animator.runtimeAnimatorController = ResourceManager.Instance.GetAssetFromShortPath<RuntimeAnimatorController>("Characters/Player/PlayerAnimatorController.controller");
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
    
    public void HandlePlayerOperation(BattleOperation operation)
    {
        if (operation.OperationType == OperationTypeEnum.STAY)
        {
            SetState(PlayerStateEnum.IDLE);
            DoStay();
        }
        else if (operation.OperationType == OperationTypeEnum.MOVE)
        {
            SetState(PlayerStateEnum.MOVE);
            UpdateLogicTransform(operation);
        }
        else if (operation.OperationType == OperationTypeEnum.WAVE)
        {
            DoEnergyWave(operation);
        }
        else if (operation.OperationType == OperationTypeEnum.DART)
        {
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

    private void DoStay()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void UpdateLogicTransform(BattleOperation operation)
    {
        transform.position = operation.Position.ToVector3();
        transform.rotation = Quaternion.Euler(0, operation.Direction, 0);
        _rigidbody.velocity = transform.forward * Constants.PLAYER_MOVE_SPEED;
        // Vector3 direction = InputManager.Instance.GetMoveDirection();
        // transform.forward = direction;
        // _rigidbody.velocity = transform.forward * Constants.PLAYER_MOVE_SPEED;
    }

    // private void FixedUpdate()
    // {
    //     Vector3 direction = InputManager.Instance.GetMoveDirection();
    //     transform.forward = direction;
    //     // _rigidbody.velocity = transform.forward * Constants.PLAYER_MOVE_SPEED;
    //     transform.position += transform.forward * Constants.PLAYER_MOVE_SPEED * Time.fixedDeltaTime;
    // }

    private void DoEnergyWave(BattleOperation operation)
    {
        GameObject energyWave = InstantiateByCondition(operation, "Effects/Skills/EnergyWave/EnergyWave.prefab",
            gameObject.name + "EnergyWave", Constants.SKILL_OFFSET);
        energyWave.AddComponent<EnergyWaveBehaviour>().Init(operation);
    }

    private void DoDart(BattleOperation operation)
    {
        GameObject dart = InstantiateByCondition(operation, "Effects/Skills/Dart/Dart.prefab",
            gameObject.name + "Dart", Constants.SKILL_OFFSET);
        dart.AddComponent<DartBehaviour>().Init(operation);
    }

    private void DoShield(BattleOperation operation)
    {
        GameObject shield = InstantiateByCondition(operation, "Effects/Skills/Shield/Shield.prefab",
            gameObject.name + "Shield", 0);
        shield.AddComponent<ShieldBehaviour>().Init(operation);
    }

    private void DoBlink(BattleOperation operation)
    {
        GameObject blink = InstantiateByCondition(operation, "Effects/Skills/Blink/Blink.prefab",
            gameObject.name + "Blink", 0);
        blink.AddComponent<BlinkBehaviour>().Init(operation);
        transform.rotation = Quaternion.Euler(0, operation.Direction, 0);
        _rigidbody.MovePosition(transform.position + transform.forward * Constants.BLINK_DISTANCE);
        _battleOperation = operation;
        
        // todo 闪退，，
        // Invoke(nameof(InvokeAfterBlink), 0.0f);
        
    }

    private void InvokeAfterBlink()
    {
        GameObject afterBlink = InstantiateByCondition(_battleOperation, "Effects/Skills/Blink/AfterBlink.prefab",
            gameObject.name + "AfterBlink", 0);
        afterBlink.AddComponent<AfterBlinkBehaviour>().Init(_battleOperation);
    }

    private GameObject InstantiateByCondition(BattleOperation operation, string path, string objectName, float skillOffset)
    {
        GameObject skillPrefab = ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>(path);
        Quaternion skillRotation = Quaternion.Euler(0, operation.Direction, 0);
        Vector3 offset = Quaternion.Euler(0, operation.Direction, 0) * Vector3.forward * skillOffset + new Vector3(0,1,0);
        GameObject skill = Instantiate(skillPrefab, transform.position + offset, skillRotation);
        skill.name = objectName;
        return skill;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.name == GetEnemyName() + "EnergyWave")
        {
            BattleManager.Instance.HandleBattleEnd(gameObject.name);
        }
        else if (other.name == GetEnemyName() + "Dart")
        {
            BattleManager.Instance.HandleBattleEnd(gameObject.name);
        }
    }

    private string GetEnemyName()
    {
        if (gameObject.name == "Player")
        {
            return "Enemy";
        }
        return "Player";
    }
}
