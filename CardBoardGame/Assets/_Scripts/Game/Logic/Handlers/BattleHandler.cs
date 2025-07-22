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

    private float damageMultiplierValue;

    public float DamageMultiplierValue
    {
        get => damageMultiplierValue;
        set
        {
            isTZFZmultipleCalc = true;
            print("DamageMultiplierValue Propertie");
            damageMultiplierValue = value;
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

    public IEnumerator RecieveDamageValue(float originDamage, Dictionary<Shape, int> usedCardDic)
    {
        WaitForSeconds waitForSeconds = new(1f);
        yield return null;
        float damage = originDamage;

        print(damage);

        if (isTZFZmultipleCalc)
        {
            damage *= damageMultiplierValue;
        }

        damage += elemEffectDic[ElementType.Embers].CardEffectCalc(damage, usedCardDic[Shape.Spade]);

        print($"{damage}");

        if (player.IsDamageDouble)
        {
            damage *= 2;
            player.IsDamageDouble = false;
            player.SlashPlay();
        }
        else
        {
            player.Attack();
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
            damageMultiplierValue = 1;

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
                player.PlayerSO.CurHP = elemEffectDic[ElementType.Spray].CardEffectCalc(player.PlayerSO.CurHP, usedCardDic[Shape.Club]);

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
        ManagerHandler.Instance.gameManager.AfterBattleRoutine();
    }

    public void StageEnterEffect()
    {

    }

    private void ReflectUI()
    {
        player.ReflectUI();
        monster.ReflectUI();
    }
}
