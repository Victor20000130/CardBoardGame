using DG.Tweening;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public Vector3 rotateDir;
    public float rotateSpeed;

    private void Update()
    {
        transform.Rotate(rotateDir, rotateSpeed);
    }

}
