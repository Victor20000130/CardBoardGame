using DG.Tweening;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    private PieceHandler pieceHandler;
    private Animator _anim;
    [SerializeField]
    private AnimationClip turnClip;

    public float TurnClipLength => turnClip.length;

    public bool LootMotion
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
        // _anim.applyRootMotion = true;
        _anim.SetTrigger("Turn");
        _anim.SetBool("IsRun", false);
    }
    public void Run()
    {
        _anim.applyRootMotion = false;
        _anim.SetBool("IsRun", true);
    }
    public void RunStop()
    {
        _anim.SetBool("IsRun", false);

    }

}

