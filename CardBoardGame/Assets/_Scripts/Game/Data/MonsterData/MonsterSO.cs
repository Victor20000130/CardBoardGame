using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterScriptableObject", menuName = "Scriptable Objects/MonsterScriptableObject")]
public class MonsterSO : ScriptableObject
{
    public string _name;
    public float _maxHP;
    public float _curHP;
    public float _damage;
    public int _turn;
    public bool IsHeal;

    public void Copy(MonsterSO copyTarget)
    {
        copyTarget._name = _name;
        copyTarget._maxHP = _curHP;
        copyTarget._curHP = _curHP;
        copyTarget._damage = _damage;
        copyTarget._turn = _turn;
    }
}
