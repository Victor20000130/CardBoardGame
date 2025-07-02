using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Scriptable Objects/PlayerScriptableObject")]
public class PlayerSO : ScriptableObject
{
    public string Name;
    public int MaxHP;
    public int CurHP;
    public int CanThrowCount;
    public bool IsStart;
    public bool IsHeal;
    public bool IsBuff;

    public void Copy(PlayerSO copyTarget)
    {
        copyTarget.Name = Name;
        copyTarget.MaxHP = MaxHP;
        copyTarget.CurHP = CurHP;
        copyTarget.CanThrowCount = CanThrowCount;
        copyTarget.IsStart = IsStart;
        copyTarget.IsHeal = IsHeal;
        copyTarget.IsBuff = IsBuff;
    }
}
