using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [Header("주사위 설정")]
    public float jumpForce = 1.5f;
    public float rollForce = 10f;
    public float rollTourque = 10f;
    public float minForce = -5f;
    public float maxForce = 5f;
    public float minTorque = -5f;
    public float maxTorque = 5f;
    // 각 면의 이름(또는 번호)을 담을 배열
    private int[] faceNames = new int[6];
    [Header("주사위 눈금 설정")]
    public int up;
    public int down;
    public int left;
    public int right;
    public int front;
    public int back;

    private bool isRolling = false;

    // 주사위의 각 면이 로컬에서 바라보는 방향(노멀)
    private Vector3[] localNormals = new Vector3[]
    {
        Vector3.up,    // 1번 면 (예시)
        Vector3.down,  // 2번 면
        Vector3.left,  // 3번 면
        Vector3.right, // 4번 면
        Vector3.forward, // 5번 면
        Vector3.back    // 6번 면
    };
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // 주사위의 각 면의 이름(또는 번호)을 설정합니다.
        faceNames = new int[] { up, down, left, right, front, back };
    }
    // GameUIHandler의 버튼 통해 주사위를 굴리는 버튼 클릭 이벤트를 처리합니다.
    public void OnButtonClick()
    {
        isRolling = true;
    }

    /// <summary>
    /// 주사위의 위쪽 면을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public int GetUpFace()
    {
        float maxDot = float.NegativeInfinity;
        int upFaceIndex = -1;

        for (int i = 0; i < localNormals.Length; i++)
        {
            // 현재 면의 노멀을 월드 좌표계로 변환
            Vector3 worldNormal = transform.TransformDirection(localNormals[i]);
            float dot = Vector3.Dot(worldNormal, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                upFaceIndex = i;
            }
        }

        return faceNames[upFaceIndex];
    }

    private void Update()
    {
        if (_rigidbody.linearVelocity.magnitude <= 0.1f)
        {
            Debug.Log("위쪽 면: " + GetUpFace().ToString());
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _rigidbody.linearVelocity + (Time.fixedDeltaTime * Physics.gravity);
        transform.position += Time.fixedDeltaTime * _rigidbody.linearVelocity;
        if (isRolling)
        {
            RollDice();
            isRolling = false; // 주사위가 굴러간 후에는 다시 굴리지 않도록 설정
        }
    }

    private void RollDice()
    {
        // AddForce를 사용하여 주사위 방향 설정
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(minForce, maxForce), 0, UnityEngine.Random.Range(minForce, maxForce)).normalized;
        randomDirection.y = jumpForce;
        _rigidbody.AddForce(randomDirection * rollForce, ForceMode.Impulse);

        // AddTorque를 사용하여 주사위 회전
        Vector3 randomTorque = new Vector3(UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque));
        _rigidbody.AddTorque(randomTorque * rollTourque, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 충돌 지점 및 법선 벡터
            ContactPoint contact = collision.contacts[0];
            Vector3 contactPoint = contact.point;
            Vector3 normal = collision.contacts[0].normal;

            // 입사 속도 벡터
            Vector3 income = _rigidbody.linearVelocity;

            // 반사 방향 계산
            Vector3 pushDirection = Vector3.Reflect(income, normal).normalized;

            // 레버 암 기반 토크 계산
            Vector3 leverArm = contactPoint - _rigidbody.worldCenterOfMass;
            Vector3 pushTorque = Vector3.Cross(leverArm, normal).normalized;

            float force = collision.impulse.magnitude;

            // 힘과 토크 적용
            _rigidbody.AddForce(pushDirection * force * Time.fixedDeltaTime, ForceMode.Impulse);
            _rigidbody.AddTorque(pushTorque * force, ForceMode.Impulse);
        }
    }
}
