using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Game.Skills;
using UnityEngine;
using Utils;

public class EnergyWaveBehaviour : SkillBehaviour
{
    public override void Init(BattleOperation battleOperation)
    {
        base.Init(battleOperation);
        Destroy (gameObject, Constants.ENERGY_WAVE_DURING_TIME);
    }
    
    private void FixedUpdate()
    {
        Vector3 offset = Quaternion.Euler(0, BattleOperation.Direction, 0)
                         * Vector3.forward * Constants.ENERGY_WAVE_SPEED * Time.fixedDeltaTime;
        Rigidbody.MovePosition(transform.position + offset);
    }
}
