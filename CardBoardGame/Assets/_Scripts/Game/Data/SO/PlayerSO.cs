using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Scriptable Objects/PlayerScriptableObject")]
public class PlayerSO : ScriptableObject
{
    public string Name;
    public int MaxHP;
    public int CurHP;
    public int ChangeCardCount;
    public bool IsNight;
    public bool IsHeal;
    public bool IsBuff;
    public bool IsMiniGame;

    public void Initialize(PlayerSO copyTarget)
    {
        copyTarget.Name = Name;
        copyTarget.MaxHP = MaxHP;
        copyTarget.CurHP = CurHP;
        copyTarget.ChangeCardCount = ChangeCardCount;
        copyTarget.IsNight = IsNight;
    }
}
