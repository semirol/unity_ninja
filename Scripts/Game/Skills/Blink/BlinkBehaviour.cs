using System;
using Core;
using Game.Skills;
using UnityEngine;
using UnityEngine.VFX;
using Utils;

public class BlinkBehaviour : SkillBehaviour
{
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private VisualEffect _visualEffect;

    private void OnEnable()
    {
        _skinnedMeshRenderer = GameObject.Find(GetPlayerName()).transform.Find("Alpha_Surface").GetComponent<SkinnedMeshRenderer>();
        Mesh m = new Mesh();
        _skinnedMeshRenderer.BakeMesh(m);
        _visualEffect = GetComponent<VisualEffect>();
        _visualEffect.SetMesh("Mesh", m);
    }

    public override void Init(BattleOperation battleOperation)
    {
        base.Init(battleOperation);

        // 直接销毁
        Destroy(gameObject, 5f);
    }
}