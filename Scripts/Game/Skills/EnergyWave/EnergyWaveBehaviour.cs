using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Utils;

public class EnergyWaveBehaviour : MonoBehaviour
{
    private BattleOperation _battleOperation;

    private Rigidbody _rigidbody;

    public void Init(BattleOperation battleOperation)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _battleOperation = battleOperation;
        Destroy (gameObject, Constants.ENERGY_WAVE_DURING_TIME);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector3 offset = Quaternion.Euler(0, _battleOperation.Direction, 0)
                         * Vector3.forward * Constants.ENERGY_WAVE_SPEED * Time.fixedDeltaTime;
        _rigidbody.MovePosition(transform.position + offset);
    }
}
