using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("따라갈 타겟 오브젝트")]
    public GameObject target;

    [Header("축별 따라가기")]
    public bool followX = true;
    public bool followY = false;
    public bool followZ = true;

    [Header("축별 거리 유지")]
    public bool keepDistanceX = true;
    public bool keepDistanceY = false;
    public bool keepDistanceZ = true;

    private Vector3 initialOffset;

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("Target is not set for TargetFollower.", this);
            enabled = false;
            return;
        }

        // 초기 오프셋 계산
        initialOffset = transform.position - target.transform.position;
        if (!keepDistanceX) initialOffset.x = 0f;
        if (!keepDistanceY) initialOffset.y = 0f;
        if (!keepDistanceZ) initialOffset.z = 0f;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.transform.position;
        Vector3 newPos = transform.position;

        // 축별 따라가기 및 오프셋 적용
        newPos.x = followX ? targetPos.x + (keepDistanceX ? initialOffset.x : 0f) : transform.position.x;
        newPos.y = followY ? targetPos.y + (keepDistanceY ? initialOffset.y : 0f) : transform.position.y;
        newPos.z = followZ ? targetPos.z + (keepDistanceZ ? initialOffset.z : 0f) : transform.position.z;

        transform.position = newPos;
    }

}
