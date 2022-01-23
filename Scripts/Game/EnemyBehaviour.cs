using System;
using Core;
using Core.FixedArithmetic;
using Managers;
using UnityEngine;
using Utils;

// todo 两个behaviour重构公共父类
public class EnemyBehaviour : UnitySingleton<EnemyBehaviour>
{

    private FVector3 _logicPosition;
    private Vector3 _virtualPosition;
    
    private Animator _animator;

    public readonly int Speed = 20;
    
    public override void Awake()
    {
        base.Awake();
        _animator = gameObject.GetComponent<Animator>();
        _animator.avatar = ResourceManager.Instance.GetAssetFromShortPath<Avatar>("Characters/Player/player.fbx[playerAvatar]");
        _animator.runtimeAnimatorController = ResourceManager.Instance.GetAssetFromShortPath<RuntimeAnimatorController>("Characters/Player/PlayerAnimatorController.controller");
        _logicPosition = new FVector3(transform.position);
        _virtualPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //切换动画
        _animator.SetBool("ifWalk", true);
        // 调整方向
        if ((_virtualPosition - transform.position).magnitude > Single.Epsilon)
        {
            // todo 有可能set 0向量，目前没有逻辑问题
            transform.forward = Vector3.Lerp(transform.forward, _virtualPosition - transform.position, 2 * Constants.FRAME_FREQ * Time.deltaTime);
        }
        // 向虚拟位置匀速运动
        transform.position = Vector3.Lerp(transform.position, _virtualPosition, Constants.FRAME_FREQ * Time.deltaTime);
    }

    private void LateUpdate()
    {
    }
    
    
    //被BattleManager每次收到FrameMessage时调用
    public void HandleEnemyOperation(BattleOperation operation)
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
    }

    private void UpdateLogicTransform(BattleOperation operation)
    {
        _logicPosition.X = operation.Position.X;
        _logicPosition.Z = operation.Position.Z;
        _virtualPosition = _logicPosition.ToVector3();
    }
    
    private void DoEnergyWave(BattleOperation operation)
    {
        GameObject energyWavePrefab = ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>("Effects/Skills/EnergyWave/EnergyWave.prefab");
        Quaternion skillRotation = Quaternion.Euler(0, operation.Direction, 0);
        Vector3 offset = Quaternion.Euler(0, operation.Direction, 0) * Vector3.forward * 5;
        GameObject energyWave = Instantiate(energyWavePrefab, _virtualPosition + offset, skillRotation);
        energyWave.name = "EnemyEnergyWave";
        energyWave.AddComponent<EnergyWaveBehaviour>().Init(operation);
    }
}