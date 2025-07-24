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
    [SerializeField] private ElementEffectSO[] elemEffectsSO;

    private readonly Dictionary<ElementType, ElementEffectSO> elemEffectDic = new();
    private PlayerSO PlayerSO;
    private MonsterSO originMonsterSO;
    public MonsterSO OriginMonsterSO
    {
        get => originMonsterSO;
    }
    public int CanThrowCount => player.PlayerSO.CanThrowCount;

    private bool isMonsterDie = false;
    private bool isPlayerDie = false;

    private float tZFZMultiplierValue;

    public float DamageMultiplierValue
    {
        get => tZFZMultiplierValue;
        set
        {
            isTZFZmultipleCalc = true;
            print("DamageMultiplierValue Propertie");
            tZFZMultiplierValue = value;
        }
    }

    private bool isTZFZmultipleCalc = false;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        monster = FindAnyObjectByType<Monster>();
        originPlayerSO = Resources.Load<PlayerSO>("Data/PlayerData/PlayerSO");
        PlayerSO = ScriptableObject.CreateInstance<PlayerSO>();
        originPlayerSO.Copy(PlayerSO);

        player.PlayerSO = PlayerSO;
        player.Initialize();

        elemEffectsSO = Resources.LoadAll<ElementEffectSO>("Data/UtilityData/EffectSO/");

        foreach (ElementEffectSO elem in elemEffectsSO)
        {
            elemEffectDic.Add(elem.ElementType, elem);
        }
    }

    public void ReceiveMonsterSO(MonsterSO monsterSO)
    {
        originMonsterSO = monsterSO;
        monster.MonsterSO = ScriptableObject.CreateInstance<MonsterSO>();
        originMonsterSO.Copy(monster.MonsterSO);
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

    public IEnumerator RecieveDamageValue(float originDamage, CardHandler.CardResultWrapper cardResultWrapper)
    {
        int emberLevel = cardResultWrapper.UsedCardDic[Shape.Spade];
        int sprayLevel = cardResultWrapper.UsedCardDic[Shape.Club];

        WaitForSeconds waitForSeconds = new(1f);
        yield return null;
        float damage = originDamage;

        print(damage);

        if (isTZFZmultipleCalc)
        {
            damage *= tZFZMultiplierValue;
        }

        damage += elemEffectDic[ElementType.Embers].CardEffectCalc(EffectType.Attack, damage, emberLevel);

        print($"{damage}");

        if (player.IsDamageDouble)
        {
            damage *= 2;
            player.IsDamageDouble = false;
            // player.SlashPlay();
        }
        else
        {
            // player.Attack();
        }

        yield return waitForSeconds;

        isMonsterDie = monster.TakeDamage(damage);
        if (isMonsterDie)
        {
            ReflectUI();
            yield return waitForSeconds;
            ManagerHandler.Instance.gameManager.NextStage(isMonsterDie);
            yield break;
        }

        monster.MonsterSO._turn--;

        if (monster.MonsterSO._turn == 0)
        {

            isTZFZmultipleCalc = false;
            tZFZMultiplierValue = 1;

            monster.ReflectUI();
            yield return waitForSeconds;

            monster.Attack();

            yield return waitForSeconds;

            isPlayerDie = player.TakeDamage(monster.MonsterSO._damage);

            if (isPlayerDie)
            {
                yield return waitForSeconds;
                ManagerHandler.Instance.gameManager.GameOver(isPlayerDie);
                print("Player Die");
                yield break;
            }

            yield return waitForSeconds;

            if (player.PlayerSO.CurHP < player.PlayerSO.MaxHP)
            {
                player.PlayerSO.CurHP = elemEffectDic[ElementType.Spray].
                                        CardEffectCalc(EffectType.Heal, player.PlayerSO.CurHP, sprayLevel);

                if (player.PlayerSO.CurHP > player.PlayerSO.MaxHP)
                {
                    player.PlayerSO.CurHP = player.PlayerSO.MaxHP;
                }
            }

            monster.MonsterSO._turn = originMonsterSO._turn;
            ReflectUI();
            ManagerHandler.Instance.gameManager.DiceRollCoroutine();
            yield break;
        }
        ReflectUI();
        EveryTurnEffect(cardResultWrapper);
        ManagerHandler.Instance.gameManager.AfterBattleRoutine();
    }

    public void StageEnterEffect()
    {

    }

    public void EveryTurnEffect(CardHandler.CardResultWrapper cardResultWrapper)
    {
        ApplyEffectSafe(
            wrapper: cardResultWrapper,
            elementType: ElementType.Fair_Wind,
            effectType: EffectType.ThrowCount,
            getter: () => cardResultWrapper.CanThrowCount,
            setter: val => cardResultWrapper.CanThrowCount = (int)val
        );

        ApplyEffectSafe(
            wrapper: cardResultWrapper,
            elementType: ElementType.Fair_Wind,
            effectType: EffectType.AdditionalCard,
            getter: () => cardResultWrapper.AdditionalCardCount,
            setter: val => cardResultWrapper.AdditionalCardCount = (int)val
        );

    }

    private void ReflectUI()
    {
        player.ReflectUI();
        monster.ReflectUI();
    }

    private void ApplyEffectSafe(
        CardHandler.CardResultWrapper wrapper,
        ElementType elementType,
        EffectType effectType,
        Func<float> getter,
        Action<float> setter
        )
    {
        if (!elemEffectDic.TryGetValue(elementType, out var effectSO))
        {
            Debug.LogWarning($"[효과 누락] ElementType {elementType}에 대한 데이터 없음");
            return;
        }

        if (!effectSO.ElementEffects.Exists(e => e.EffectType == effectType))
        {
            Debug.LogWarning($"[효과 누락] {elementType}에 {effectType} 효과 없음");
            return;
        }

        int level =
        wrapper.UsedCardDic.TryGetValue(
            ElementTypeToShape(elementType), out var usedCount) ? usedCount : 0;

        float baseValue = getter();
        float result = effectSO.CardEffectCalc(effectType, baseValue, level);

        Debug.Log($"[효과 적용] {elementType}.{effectType}: {baseValue} → {result} (레벨: {level})");

        setter(result);
    }

    private Shape ElementTypeToShape(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Embers => Shape.Spade,
            ElementType.Spray => Shape.Club,
            ElementType.Nuri => Shape.Diamond,
            ElementType.Fair_Wind => Shape.Heart,
            _ => Shape.Spade
        };
    }
}
