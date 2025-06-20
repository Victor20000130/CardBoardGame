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
    private int nextMoveIdx = 0;
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
            nextMoveIdx++;

            if (nextMoveIdx % piecePositions.Length == 0)
            {
                nextMoveIdx %= piecePositions.Length;
            }
            print("1");
            // TODO : 다음 포지션 계산하는 로직
            MoveNext();
            yield return new WaitUntil(() => isMoveDone);
            if (cornerDic.TryGetValue(nextMoveIdx, out bool value))
            {
                print(nextMoveIdx);
                yield return StartCoroutine(Turn());
            }
            diceValue--;

            isMoveDone = false;
        }
        onPieceMove?.Invoke(nextMoveIdx);
        playerPiece.RunStop();

    }

    private void MoveNext()
    {
        playerPiece.Run();
        playerPiece.gameObject.transform.DOMove(piecePositions[nextMoveIdx].position, 1f).onComplete += () => Move();
    }

    private IEnumerator Turn()
    {
        playerPiece.Turn();
        yield return new WaitForSeconds(playerPiece.TurnClipLength);
        playerPiece.transform.eulerAngles += new Vector3(90, 0, 0);
    }
    private void Move()
    {
        isMoveDone = true;
    }
}
