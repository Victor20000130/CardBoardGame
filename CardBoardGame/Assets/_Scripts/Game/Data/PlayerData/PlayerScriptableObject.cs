using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Scriptable Objects/PlayerScriptableObject")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField] public string _name;
    [SerializeField] public int _health;
    [SerializeField] public int _changeCardCount;
}
