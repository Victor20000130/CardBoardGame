using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using CardBoardGame.Assets._Scripts.Utility;

public class DiceHandler : Handler
{

    private Rigidbody _rigidbody;
    private Renderer _renderer;
    [SerializeField] private Transform startPosLeft;
    [SerializeField] private Transform startPosRight;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;

    private enum StartPos
    {
        Left,
        Right,
        None
    }
    StartPos startPos;

    [Header("주사위 설정")]
    public float rollForce = 10f;
    public float rollTourque = 10f;
    public float minTorque = -5f;
    public float maxTorque = 5f;
    public float wallForce = 2;
    public int bounceCount = 5;
    private int defaultBounceCount;
    private float defaultWallForce;
    // 각 면의 이름(또는 번호)을 담을 배열
    private int[] faceNames = new int[6];
    [Header("주사위 눈금 설정")]
    public int up;
    public int down;
    public int left;
    public int right;
    public int front;
    public int back;

    private bool isGrounded = true;
    private bool isSended = true;
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
    protected override void OnInitialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        // 주사위의 각 면의 이름(또는 번호)을 설정합니다.
        faceNames = new int[] { up, down, left, right, front, back };
        defaultWallForce = wallForce;
        defaultBounceCount = bounceCount;
        ResetPosition();
    }
    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.DiceHandler;
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
    private IEnumerator FadeCorou()
    {
        yield return new WaitForSeconds(2f);
        Material mat = _renderer.materials[0];
        mat.DOFloat(1, "_Dissolve", 1.5f).onComplete += ResetPosition;
        yield return null;
    }
    private void ResetPosition()
    {
        _rigidbody.useGravity = false;
        _rigidbody.linearVelocity = Vector3.zero;
        // transform.eulerAngles = Vector3.zero;
        startPos = (StartPos)UnityEngine.Random.Range(0, 2);
        switch (startPos)
        {
            case StartPos.Left:
                transform.position = startPosLeft.transform.position;
                leftWall.gameObject.SetActive(false);
                rightWall.gameObject.SetActive(true);
                break;
            case StartPos.Right:
                transform.position = startPosRight.transform.position;
                rightWall.gameObject.SetActive(false);
                leftWall.gameObject.SetActive(true);
                break;
            default:
                print($"확인되지 않은 랜덤 밸류 {startPos}");
                break;
        }
        Material mat = _renderer.materials[0];
        mat.SetFloat("_Dissolve", 0f);
        wallForce = defaultWallForce;
        bounceCount = defaultBounceCount;
    }

    /// <summary>
    /// FixedUpdate에서 호출해야 정상 작동
    /// </summary>
    public void RollDice()
    {
        // AddForce를 사용하여 주사위 방향 설정
        _rigidbody.useGravity = true;
        Vector3 randomDirection = new();
        switch (startPos)
        {
            case StartPos.Left:
                randomDirection = rightWall.position - transform.position;
                break;
            case StartPos.Right:
                randomDirection = leftWall.position - transform.position;
                break;
            default:
                print($"확인되지 않은 밸류 {startPos}");
                break;
        }
        randomDirection = GetRandomDirectionAround(randomDirection);
        // randomDirection = randomDirection.normalized;
        _rigidbody.AddForce(randomDirection * rollForce, ForceMode.Impulse);

        // AddTorque를 사용하여 주사위 회전
        Vector3 randomTorque = new Vector3(UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque));
        _rigidbody.AddTorque(randomTorque * rollTourque, ForceMode.Impulse);
        isSended = false;
    }

    public Vector3 GetRandomDirectionAround(Vector3 baseDir, float angleRange = 100f)
    {
        if (baseDir == Vector3.zero)
        {
            return Vector3.forward;
        }
        baseDir.y = 0;
        baseDir.Normalize();

        float halfRange = angleRange * 0.5f;
        float randomAngle = UnityEngine.Random.Range(-halfRange, halfRange);

        // Y축 기준 회전
        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);

        return rotation * baseDir;
    }

    private void Update()
    {
        if (_rigidbody.linearVelocity.magnitude < 0.1f &&
            _rigidbody.angularVelocity.magnitude < 0.1F &&
            isSended == false)
        {
            ManagerHandler.Instance.gameManager.ReceiveDiceValue(GetUpFace());
            isSended = true;
            StartCoroutine(FadeCorou());
        }
    }

    private void FixedUpdate()
    {
        // if (isGrounded == false && _rigidbody.useGravity == true)
        // {
        //     _rigidbody.linearVelocity = _rigidbody.linearVelocity + (Time.fixedDeltaTime * Physics.gravity);
        //     transform.position += Time.fixedDeltaTime * _rigidbody.linearVelocity;
        // }
        if (isGrounded == false && _rigidbody.useGravity)
        {
            // 중력 직접 적용
            _rigidbody.linearVelocity += Physics.gravity * Time.fixedDeltaTime;
            transform.position += _rigidbody.linearVelocity * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            HandleWallCollision(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private void HandleWallCollision(Collision collision)
    {
        // 벽 상태 전환
        switch (startPos)
        {
            case StartPos.Left:
                leftWall.gameObject.SetActive(true);
                break;
            case StartPos.Right:
                rightWall.gameObject.SetActive(true);
                break;
        }
        startPos = StartPos.None;

        // 충돌 정보
        ContactPoint contact = collision.contacts[0];
        Vector3 contactPoint = contact.point;
        Vector3 normal = contact.normal.normalized;
        Vector3 velocity = _rigidbody.linearVelocity;

        // 반사 방향 계산 (속도 보존)
        Vector3 reflectDir = Vector3.Reflect(velocity.normalized, normal).normalized;
        float speed = velocity.magnitude;

        // 벽 관통 방지: 반사 방향으로 속도 교체
        _rigidbody.linearVelocity = reflectDir * speed;

        // 적절한 힘 적용
        float force = Mathf.Clamp(collision.impulse.magnitude, 5f, 25f);
        _rigidbody.AddForce(reflectDir * wallForce, ForceMode.Impulse);

        // 회전과 무관한 튕김: 충돌 위치에서 강제 토크
        Vector3 leverArm = contactPoint - _rigidbody.worldCenterOfMass;
        Vector3 torque = Vector3.Cross(leverArm, normal).normalized;
        _rigidbody.AddTorque(torque * force * 0.5f, ForceMode.Impulse);

        // 디버그 시각화
        Debug.DrawRay(contactPoint, normal * 2f, Color.red, 1f);
        Debug.DrawRay(contactPoint, reflectDir * 2f, Color.cyan, 1f);

        // 반사 횟수 제한
        bounceCount--;
        if (bounceCount <= 0)
        {
            wallForce = 0;
        }
    }

    // private void HandleWallCollision(Collision collision)
    // {
    //     // 벽 상태 전환
    //     switch (startPos)
    //     {
    //         case StartPos.Left:
    //             leftWall.gameObject.SetActive(true);
    //             break;
    //         case StartPos.Right:
    //             rightWall.gameObject.SetActive(true);
    //             break;
    //         default:
    //             break;
    //     }
    //     startPos = StartPos.None;

    //     ContactPoint contact = collision.contacts[0];
    //     Vector3 contactPoint = contact.point;
    //     Vector3 normal = contact.normal.normalized;
    //     Vector3 velocity = _rigidbody.linearVelocity;

    //     // 반사 방향 계산
    //     Vector3 reflectDir = Vector3.Reflect(velocity, normal).normalized;
    //     _rigidbody.AddForce(contactPoint.normalized * wallForce, ForceMode.Impulse);

    //     // 디버깅용 시각화
    //     Debug.DrawRay(contactPoint, normal, Color.red, 0f);
    //     Debug.DrawRay(contactPoint, velocity, Color.green, 0f);
    //     Debug.DrawRay(contactPoint, reflectDir, Color.cyan, 3f);

    //     // 반사력 클램핑
    //     float force = Mathf.Min(collision.impulse.magnitude, 25f);
    //     float torqueStrength = force * 0.5f;

    //     // 벽 관통 방지를 위해 속도 보정
    //     _rigidbody.linearVelocity = reflectDir * velocity.magnitude;
    //     _rigidbody.AddForce(contactPoint.normalized * (wallForce / 2), ForceMode.Impulse);

    //     // 토크 계산
    //     Vector3 leverArm = contactPoint - _rigidbody.worldCenterOfMass;
    //     Vector3 torque = Vector3.Cross(leverArm, collision.relativeVelocity.normalized);

    //     _rigidbody.AddTorque(torque.normalized * torqueStrength, ForceMode.Impulse);
    //     if (bounceCount == 0)
    //     {
    //         wallForce = 0;
    //     }
    //     bounceCount--;
    // }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Wall"))
    //     {
    //         switch (startPos)
    //         {
    //             case StartPos.Left:
    //                 leftWall.gameObject.SetActive(true);
    //                 startPos = StartPos.None;
    //                 break;
    //             case StartPos.Right:
    //                 rightWall.gameObject.SetActive(true);
    //                 startPos = StartPos.None;
    //                 break;
    //             case StartPos.None:
    //                 break;
    //         }
    //         // 충돌 지점 및 법선 벡터
    //         ContactPoint contact = collision.contacts[0];
    //         Vector3 contactPoint = contact.point;
    //         Vector3 normal = collision.contacts[0].normal;

    //         // 입사 속도 벡터
    //         Vector3 income = _rigidbody.linearVelocity;

    //         // 반사 방향 계산
    //         Vector3 pushDirection = Vector3.Reflect(income, normal).normalized;

    //         // 레버 암 기반 토크 계산
    //         Vector3 leverArm = contactPoint - _rigidbody.worldCenterOfMass;
    //         Vector3 pushTorque = Vector3.Cross(leverArm, normal).normalized;

    //         float force = collision.impulse.magnitude;
    //         // 힘과 토크 적용
    //         _rigidbody.AddForce(pushDirection * force, ForceMode.Impulse);
    //         _rigidbody.AddTorque(pushTorque * force, ForceMode.Impulse);
    //         if (wallForce >= 1)
    //         {
    //             wallForce--;
    //         }
    //     }
    //     if (collision.gameObject.CompareTag("Floor"))
    //     {
    //         isGrounded = true;
    //     }
    // }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Floor"))
    //     {
    //         isGrounded = false;
    //     }
    // }

}
