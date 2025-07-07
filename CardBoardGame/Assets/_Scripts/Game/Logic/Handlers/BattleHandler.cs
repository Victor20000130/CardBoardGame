using System;
using System.Collections;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    private const int StackPerLevel = 10;
    private const int PercentBase = 100;
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private PlayerSO originPlayerSO;
    [SerializeField] private ElementEffectSO ElementEffectSO;
    private PlayerSO PlayerSO;
    private MonsterSO originMonsterSO;
    public MonsterSO OriginMonsterSO
    {
        get => originMonsterSO;
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

    public IEnumerator RecieveDamageValue(float originDamage, Dictionary<Shape, int> spEffectDic)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        yield return null;
        float damage = originDamage;

        //TODO 연산식: (카드 + 특수효과 적용) + 미니게임효과 + 마블효과(보드판)

        ApplySPEffects(spEffectDic, ref damage, ref player.PlayerSO.CurHP);

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

        monster.TakeDamage(damage);

        monster.MonsterSO._turn--;

        if (monster.MonsterSO._turn == 0)
        {
            monster.ReflectUI();
            yield return waitForSeconds;

            monster.Attack();

            yield return waitForSeconds;

            player.TakeDamage(monster.MonsterSO._damage);

            monster.MonsterSO._turn = originMonsterSO._turn;
        }

        player.ReflectUI();
        monster.ReflectUI();

    }
    private void ApplySPEffects(Dictionary<Shape, int> spEffectDic, ref float damage, ref float hp)
    {
        int emberLevel = spEffectDic[Shape.Spade] / StackPerLevel;
        print(emberLevel);
        print(damage);
        if (emberLevel > 5)
        {
            emberLevel = 5;
        }
        switch (emberLevel)
        {
            case 0:
                print("ember 적용안함");
                break;
            case 1:
                print("ember 1");
                damage += damage * (10 / PercentBase);
                print(damage * (10 / PercentBase));
                print(damage);
                break;
            case 2:
                print("ember 2");
                damage += damage * (15 / PercentBase);
                break;
            case 3:
                print("ember 3");
                damage += damage * (20 / PercentBase);
                break;
            case 4:
                print("ember 4");
                damage += damage * (25 / PercentBase);
                break;
            case 5:
                print("ember 5");
                damage += damage * (30 / PercentBase);
                break;
            default:
                break;
        }
    }

}
