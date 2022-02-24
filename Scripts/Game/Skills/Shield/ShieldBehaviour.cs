using System;
using Core;
using Game.Skills;
using UnityEngine;
using Utils;

public class ShieldBehaviour : SkillBehaviour
{
    private Transform _playerTransform;
    public override void Init(BattleOperation battleOperation)
    {
        base.Init(battleOperation);
        _playerTransform = GameObject.Find(GetPlayerName()).transform;
        Destroy (gameObject, Constants.SHIELD_DURING_TIME);
    }

    public void Update()
    {
        transform.position = _playerTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == GetEnemyName() + "EnergyWave")
        {
            Destroy(other.gameObject);
        }
        else if (other.name == GetEnemyName() + "Dart")
        {
            Destroy(other.gameObject);
        }
    }
}
