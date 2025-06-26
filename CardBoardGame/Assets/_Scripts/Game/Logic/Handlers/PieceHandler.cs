using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using CardBoardGame.Assets._Scripts.Utility;

public class PieceHandler : Handler
{
    [SerializeField]
    private Transform[] piecePositions;
    [SerializeField] int[] cornerIdx;
    private Dictionary<int, bool> cornerDic;
    private int nextMoveIdx = 0;
    private bool isMoveDone = false;
    public PlayerPiece playerPiece;

    public IEnumerator MoveCorou(int diceValue, Action<int> onPieceMove)
    {
        while (diceValue != 0)
        {
            nextMoveIdx++;

            if (nextMoveIdx % piecePositions.Length == 0)
            {
                nextMoveIdx %= piecePositions.Length;
            }
            // TODO : 다음 포지션 계산하는 로직
            MoveNext();
            yield return new WaitUntil(() => isMoveDone);
            if (cornerDic.TryGetValue(nextMoveIdx, out bool value))
            {
                yield return StartCoroutine(Turn());
            }
            diceValue--;

            isMoveDone = false;
        }
        int arriveGridIdx = nextMoveIdx;
        onPieceMove?.Invoke(arriveGridIdx);
        playerPiece.RunStop();

    }

    private void MoveNext()
    {
        playerPiece.Run();
        playerPiece.gameObject.transform.DOMove(piecePositions[nextMoveIdx].position, 1f).SetEase(Ease.Linear).onComplete += () => Move();
    }

    private IEnumerator Turn()
    {
        playerPiece.Turn();
        yield return new WaitForSeconds(playerPiece.TurnClipLength);
    }
    private void Move()
    {
        isMoveDone = true;
    }
    protected override void OnInitialize()
    {
        cornerDic = new Dictionary<int, bool>();
        foreach (int corner in cornerIdx)
        {
            cornerDic.Add(corner, true);
        }
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.PieceHandler;
    }
}
