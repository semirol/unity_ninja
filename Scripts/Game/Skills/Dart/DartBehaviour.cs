using System;
using Core;
using UnityEngine;
using Utils;

public class DartBehaviour : MonoBehaviour
{
    private BattleOperation _battleOperation;

    private Rigidbody _rigidbody;
    
    private bool _ifAutoTrack = false;
    
    private Transform _enemyTransform;

    public void Init(BattleOperation battleOperation)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _battleOperation = battleOperation;
        
        _enemyTransform = GameObject.Find("Enemy").transform;
        if (IfFindEnemy())
        {
            Destroy(gameObject, Constants.DART_DURING_TIME);
            _ifAutoTrack = true;
        }
        else
        {
            // 直接销毁
            Destroy(gameObject, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (_ifAutoTrack)
        {
            AutoTrack();
        }
    }

    private bool IfFindEnemy()
    {
        float distance = (transform.position - _enemyTransform.position).magnitude;
        return distance < Constants.FIND_ENEMY_DISTANCE;
    }
        
    private void AutoTrack()
    {
        Vector3 direction = (_enemyTransform.position - transform.position).normalized;
        Vector3 offset = direction * Constants.DART_SPEED * Time.fixedDeltaTime;
        _rigidbody.MovePosition(transform.position + offset);
    }
}