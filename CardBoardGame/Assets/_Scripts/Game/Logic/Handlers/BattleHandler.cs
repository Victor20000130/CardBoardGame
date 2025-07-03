using System;
using System.Collections;
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
        ElementEffectSO.Initialize();
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

    public void RecieveDamageValue(float originDamage, ElementType elementType, int elementLevel)
    {
        float damage = originDamage;
        // TODO 연산식 서순에 따라 데미지 다르게
        ElementEffectSO.GetEffectValue(elementType, damage, elementLevel);

        if (player.IsDamageDouble)
        {
            damage *= 2;
            player.IsDamageDouble = false;
        }
    }

}
