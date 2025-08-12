using TMPro;
using UnityEngine;

public class CombiElemInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _damage;
    [SerializeField]
    private TextMeshProUGUI _info;

    public void SetInfos(string _name, string _damage, string _info)
    {
        this._name.text = _name;
        this._damage.text = _damage;
        this._info.text = _info;
    }
}
