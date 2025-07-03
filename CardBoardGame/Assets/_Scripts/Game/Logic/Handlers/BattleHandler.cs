using System;
using System.Collections;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private PlayerSO originPlayerSO;
    [SerializeField] private readonly MonsterSO originMonsterSO;
    [SerializeField] private ElementEffectSO ElementEffectSO;
    private PlayerSO PlayerSO;
    private MonsterSO curMonsterSO;
    private Dictionary<ElementType, ElementEffect> effectDic = new Dictionary<ElementType, ElementEffect>();

    public MonsterSO CurMonsterSO
    {
        get => curMonsterSO;
        set => curMonsterSO = value;
    }
    public int CanThrowCount => player.PlayerSO.CanThrowCount;

    private void Start()
    {
        Debug.Log(PlayerSO);
        player.PlayerSO = PlayerSO;
        player.Initialize();
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        monster = FindAnyObjectByType<Monster>();
        originPlayerSO = Resources.Load<PlayerSO>("Data/PlayerData/PlayerSO");
        PlayerSO = ScriptableObject.CreateInstance<PlayerSO>();
        originPlayerSO.Copy(PlayerSO);
        if (ElementEffectSO == null)
        {
            ElementEffectSO = Resources.Load<ElementEffectSO>("Data/UtilityData/ElementEffectSO");
        }
        ElementEffectSO.Initialize(effectDic);
    }

    public void ReceiveMonsterSO(MonsterSO monsterSO)
    {
        curMonsterSO = monsterSO;
        monster.MonsterSO = curMonsterSO;
        print(curMonsterSO);
        monster.Initialize();
    }

    public void SODataLoad()
    {

    }

    public void ReceiveGridType(GridType gridType)
    {
        StartCoroutine(ApplyEffect(gridType));
    }

    private IEnumerator ApplyEffect(GridType gridType)
    {
        print($"적용되는 효과 : {gridType}");
        player.applyEffectAct?.Invoke(gridType);
        monster.applyEffectAct?.Invoke(gridType);
        yield return new WaitForSeconds(3f);
    }
    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.BattleHandler;
    }

    public void RecieveDamageValue(float originDamage, ElementType elemType, int elemLevel)
    {
        float damage = originDamage;
        // TODO 연산식 서순에 따라 데미지 다르게

        //TODO 이펙트 기획 변경완료 후 작업
        // ApplyEffect(effectDic[elemType], damage, elemLevel);

        if (player.IsDamageDouble)
        {
            damage *= 2;
            player.IsDamageDouble = false;
        }

    }

    private void ApplyEffect(ElementEffect curEffect, float damage, int elemLevel)
    {

        switch (curEffect.EffectType)
        {
            case EffectType.None:
                break;
            case EffectType.Attack:
                damage += curEffect.EffectCalc(damage, elemLevel);
                break;
            case EffectType.Heal:
                PlayerSO.CurHP += curEffect.EffectCalc(PlayerSO.MaxHP, elemLevel);
                break;
            case EffectType.Shield:
                PlayerSO.Barriar += curEffect.EffectCalc(PlayerSO.CurHP, elemLevel);
                break;
            case EffectType.AdditionalCard:
                int temp = 0;
                temp += curEffect.EffectCalc(temp, elemLevel);
                break;
        }
    }

}
