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
    public float minForce = -5f;
    public float maxForce = 5f;
    public float minTorque = -5f;
    public float maxTorque = 5f;
    public float wallForce = 2;
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
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        // 주사위의 각 면의 이름(또는 번호)을 설정합니다.
        faceNames = new int[] { up, down, left, right, front, back };
        defaultWallForce = wallForce;
        ResetPosition();
    }

    protected override void OnInitialize()
    {
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
    }

    /// <summary>
    /// FixedUpdate에서 호출해야 정상 작동
    /// </summary>
    public void RollDice()
    {
        // AddForce를 사용하여 주사위 방향 설정
        _rigidbody.useGravity = true;
        Vector3 randomDirection = new Vector3();
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
        randomDirection = randomDirection.normalized;
        _rigidbody.AddForce(randomDirection * rollForce, ForceMode.Impulse);

        // AddTorque를 사용하여 주사위 회전
        Vector3 randomTorque = new Vector3(UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque));
        _rigidbody.AddTorque(randomTorque * rollTourque, ForceMode.Impulse);
        isSended = false;
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
        if (isGrounded == false && _rigidbody.useGravity == true)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity + (Time.fixedDeltaTime * Physics.gravity);
            transform.position += Time.fixedDeltaTime * _rigidbody.linearVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            switch (startPos)
            {
                case StartPos.Left:
                    leftWall.gameObject.SetActive(true);
                    startPos = StartPos.None;
                    break;
                case StartPos.Right:
                    rightWall.gameObject.SetActive(true);
                    startPos = StartPos.None;
                    break;
                case StartPos.None:
                    break;
            }
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
            _rigidbody.AddForce(pushDirection * force * wallForce * Time.fixedDeltaTime, ForceMode.Impulse);
            _rigidbody.AddTorque(pushTorque * force, ForceMode.Impulse);
            if (wallForce > 1)
            {
                wallForce--;
            }
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

}
