using System;
using System.Collections;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private Monster[] monsters;
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
        // monster = monsters[0];
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

    public void ReceiveMonsterSO(MonsterSO monsterSO, Stage currentStage)
    {
        if (monster != null)
        {
            monster.gameObject.SetActive(false);
        }
        switch (currentStage)
        {
            case Stage.Stage1:
                monster = monsters[0];
                break;
            case Stage.Stage2:
                monster = monsters[1];
                break;
            case Stage.Stage3:
                monster = monsters[2];
                break;
            case Stage.Stage4:
                monster = monsters[3];
                break;
            case Stage.Stage5:
                monster = monsters[4];
                break;

        }
        originMonsterSO = monsterSO;
        if (monsterSO == null)
        {
            Debug.LogError("monsterSO is NULL");
        }
        print(currentStage);
        monster.MonsterSO = ScriptableObject.CreateInstance<MonsterSO>();
        originMonsterSO.Copy(monster.MonsterSO);
        monster.Initialize();
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
        int sprayLevel = cardResultWrapper.UsedCardDic[Shape.Diamond];

        WaitForSeconds waitForSeconds = new(1f);
        yield return null;
        float damage = originDamage;

        print(damage);

        if (isTZFZmultipleCalc)
        {
            damage *= tZFZMultiplierValue;
        }

        damage += elemEffectDic[ElementType.Embers].CardEffectCalc(EffectType.Attack, damage, emberLevel);

        if (player.IsDamageHalf)
        {
            damage /= 2;
            player.IsDamageHalf = false;

        }

        if (player.IsDamageDouble)
        {
            damage *= 2;
            player.IsDamageDouble = false;
            player.SlashPlay();
            yield return new WaitForSeconds(player.GetAnimationClipLength("Slash"));
            yield return new WaitForSeconds(player.GetAnimationClipLength("Dodge"));
        }
        else
        {
            player.Attack();
            yield return new WaitForSeconds(player.GetAnimationClipLength("Attack"));
        }
        print($"적용된 데미지: {damage}");
        yield return waitForSeconds;

        isMonsterDie = monster.TakeDamage(damage);
        if (isMonsterDie)
        {
            ReflectUI();
            yield return new WaitForSeconds(monster.GetAnimationClipLength("Die"));
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
            yield return new WaitForSeconds(monster.GetAnimationClipLength("Attack"));

            isPlayerDie = player.TakeDamage(monster.MonsterSO._damage);
            yield return new WaitForSeconds(player.GetAnimationClipLength("TakeDamage"));
            if (isPlayerDie)
            {
                yield return new WaitForSeconds(player.GetAnimationClipLength("Die"));
                ManagerHandler.Instance.gameManager.GameOver(isPlayerDie);
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

    public void StageEnterEffect(CardHandler.CardResultWrapper cardResultWrapper)
    {
        player.PlayerSO.Barriar = 0;
        ApplyEffectSafe(
            wrapper: cardResultWrapper,
            elementType: ElementType.Nuri,
            effectType: EffectType.ShieldBaseCurrentHP,
            getter: () => player.PlayerSO.CurHP,
            setter: val => player.PlayerSO.Barriar += val
            );

        ApplyEffectSafe(wrapper: cardResultWrapper,
        elementType: ElementType.Nuri,
        effectType: EffectType.ShieldBaseLostHP,
        getter: () => player.PlayerSO.MaxHP - player.PlayerSO.CurHP,
         setter: val => player.PlayerSO.Barriar += val);
        player.ReflectUI();
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
            ElementType.Spray => Shape.Diamond,
            ElementType.Nuri => Shape.Heart,
            ElementType.Fair_Wind => Shape.Club,
            _ => Shape.Spade
        };
    }

    public IEnumerator GetBuffResult(bool isCorrect)
    {
        player.Buff(isCorrect);
        monster.Buff(!isCorrect);
        if (isCorrect)
        {
            player.PowerUp();
            yield return new WaitForSeconds(player.GetAnimationClipLength("PowerUp"));
        }
        else
        {
            player.DamageHalf();
            yield return new WaitForSeconds(player.GetAnimationClipLength("DamageHalf"));
        }
    }
}
