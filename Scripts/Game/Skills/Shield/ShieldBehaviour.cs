using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Utils;

public class ShieldBehaviour : MonoBehaviour
{
    private BattleOperation _battleOperation;

    private Rigidbody _rigidbody;
    public void Init(BattleOperation battleOperation)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _battleOperation = battleOperation;
        Destroy (gameObject, Constants.SHIELD_DURING_TIME);
    }
}
