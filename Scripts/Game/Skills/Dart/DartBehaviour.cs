using Core;
using Game.Skills;
using UnityEngine;
using Utils;

public class DartBehaviour : SkillBehaviour
{
    private bool _ifAutoTrack = false;
    
    private Transform _enemyTransform;

    public override void Init(BattleOperation battleOperation)
    {
        base.Init(battleOperation);
        
        _enemyTransform = GameObject.Find(GetEnemyName()).transform;
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
        Rigidbody.MovePosition(transform.position + offset);
    }
}