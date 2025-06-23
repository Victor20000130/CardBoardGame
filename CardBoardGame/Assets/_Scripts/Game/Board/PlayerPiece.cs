using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    private PieceHandler pieceHandler;
    private Animator _anim;
    [SerializeField]
    private AnimationClip turnClip;

    public float TurnClipLength => turnClip.length;

    public bool RootMotion
    {
        get { return _anim.applyRootMotion; }
        set { _anim.applyRootMotion = value; }
    }
    private void Awake()
    {
        pieceHandler = GetComponentInParent<PieceHandler>();
        _anim = GetComponent<Animator>();
        if (pieceHandler == null)
        {
            Debug.LogError("PieceHandler can't found");
            return;
        }
        pieceHandler.playerPiece = this;
    }

    public void Turn()
    {
        _anim.SetTrigger("Turn");
        StartCoroutine(TurnCoroutine());
        _anim.SetBool("IsRun", false);
    }
    public void Run()
    {
        _anim.SetBool("IsRun", true);
    }
    public void RunStop()
    {
        _anim.SetBool("IsRun", false);

    }

    private IEnumerator TurnCoroutine()
    {
        // 현재 바라보고 있는 방향의 오른쪽 벡터
        Vector3 rightDir = transform.right;
        // 오른쪽을 바라보는 회전값 생성 (up은 현재 up 유지)
        Quaternion targetRot = Quaternion.LookRotation(rightDir, transform.up);
        float duration = TurnClipLength;

        // DOTween으로 부드럽게 회전
        yield return transform.DORotateQuaternion(targetRot, duration).SetEase(Ease.Linear).WaitForCompletion();
    }
}

