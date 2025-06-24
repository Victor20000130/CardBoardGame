using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator monsterAnim;

    public override void Initialize()
    {
        base.Initialize();
        handlerType = HandlerType.BattleHandler;
    }
}
