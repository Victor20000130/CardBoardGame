using UnityEngine;

public class CombiInfos : MonoBehaviour
{
    [TextArea(2, 5)]
    [SerializeField]
    private string[] _names;
    [TextArea(2, 5)]
    [SerializeField]
    private string[] _damages;
    [TextArea(2, 5)]
    [SerializeField]
    private string[] _infos;
    [SerializeField]
    private CombiElemInfo[] combiElemInfos;

    private void Awake()
    {
        for (int i = 0; i < combiElemInfos.Length; i++)
        {
            combiElemInfos[i].SetInfos(_names[i], _damages[i], _infos[i]);
        }
    }
}
