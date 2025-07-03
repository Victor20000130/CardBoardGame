using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Scriptable Objects/PlayerScriptableObject")]
public class PlayerSO : ScriptableObject
{
    public string Name;
    public int MaxHP;
    public float CurHP;
    public float Barriar;
    public int CanThrowCount;
    public bool IsStart;
    public bool IsHeal;
    public bool IsBuff;

    public void Copy(PlayerSO copyTarget)
    {
        copyTarget.Name = Name;
        copyTarget.MaxHP = MaxHP;
        copyTarget.CurHP = CurHP;
        copyTarget.Barriar = Barriar;
        copyTarget.CanThrowCount = CanThrowCount;
        copyTarget.IsStart = IsStart;
        copyTarget.IsHeal = IsHeal;
        copyTarget.IsBuff = IsBuff;
    }
}
