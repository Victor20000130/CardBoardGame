using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class GameUIHandler : Handler
{
    public override void Initialize()
    {
        base.Initialize();
        handlerType = HandlerType.GameUIHandler;
    }
}