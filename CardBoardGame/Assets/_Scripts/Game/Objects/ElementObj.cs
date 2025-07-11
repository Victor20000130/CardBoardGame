using UnityEngine;

public class ElementObj : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        if (meshRenderers.Length < 4)
        {
            Debug.LogError($"{gameObject.name}: Level이 할당되지 않음.");
            Debug.LogError($"할당된 레벨갯수 {meshRenderers.Length}");
        }
    }

    public void SetMaterial(int level, Material mat)
    {
        if (level <= 0)
        {
            return;
        }
        meshRenderers[level - 1].material = mat;
    }
}
