using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterScriptableObject", menuName = "Scriptable Objects/MonsterScriptableObject")]
public class MonsterSO : ScriptableObject
{
    [SerializeField] public string _name;
    [SerializeField] public int _health;
    [SerializeField] public int _damage;
    [SerializeField] public int _turn;

}
