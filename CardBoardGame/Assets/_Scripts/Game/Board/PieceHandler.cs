using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;

public class PieceHandler : MonoBehaviour
{
    [SerializeField]
    private Transform[] piecePositions;
    [SerializeField] int[] cornerIdx;
    private Dictionary<int, bool> cornerDic;
    private int curIdx = 0;
    private bool isMoveDone = false;
    public PlayerPiece playerPiece;

    private void Awake()
    {
        cornerDic = new Dictionary<int, bool>();
        foreach (int corner in cornerIdx)
        {
            cornerDic.Add(corner, true);
        }
    }
    public IEnumerator MoveCorou(int diceValue, Action<int> onPieceMove)
    {
        print($"말 움직임 시작/밸류{diceValue}");
        while (diceValue != 0)
        {
            curIdx++;
            if (curIdx % piecePositions.Length == 0)
            {
                print("마지막 그리드 도착");
                curIdx %= piecePositions.Length;
            }

            // TODO : 다음 포지션 계산하는 로직
            if (cornerDic.TryGetValue(curIdx, out bool value))
            {
                MoveNextandTurn();
            }
            else
            {
                MoveNext();
            }
            onPieceMove?.Invoke(curIdx);
            diceValue--;

            yield return new WaitUntil(() => isMoveDone);
            isMoveDone = false;
        }
    }

    private void MoveNext()
    {
        playerPiece.gameObject.transform.DOMove(piecePositions[curIdx].position, 1f).onComplete += () => Move();
    }
    private void MoveNextandTurn()
    {
        playerPiece.gameObject.transform.DOMove(piecePositions[curIdx].position, 1f).onComplete += () => Turn();
    }

    private void Turn()
    {
        //TODO : 애니메이션으로 턴 후 이동
        isMoveDone = true;
        print("Trun");
    }
    private void Move()
    {
        isMoveDone = true;
    }
}
