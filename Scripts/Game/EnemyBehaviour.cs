using Core;
using Game;
using Managers;
using UnityEngine;
using Utils;

public class EnemyBehaviour : MonoBehaviour
{
    private Animator _animator;
    
    private int _state = PlayerStateEnum.INVALID;
    
    private Rigidbody _rigidbody;
    
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
    }
    
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
        
    }
    
    private GameObject InstantiateByCondition(BattleOperation operation, string path, string objectName, float skillOffset)
    {
        GameObject skillPrefab = ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>(path);
        Quaternion skillRotation = Quaternion.Euler(0, operation.Direction, 0);
        Vector3 offset = Quaternion.Euler(0, operation.Direction, 0) * Vector3.forward * skillOffset;
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
