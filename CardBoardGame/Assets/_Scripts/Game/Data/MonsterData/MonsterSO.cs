using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterScriptableObject", menuName = "Scriptable Objects/MonsterScriptableObject")]
public class MonsterSO : ScriptableObject
{
    public string _name;
    public float _maxHP;
    public float _damage;
    public int _turn;

}
