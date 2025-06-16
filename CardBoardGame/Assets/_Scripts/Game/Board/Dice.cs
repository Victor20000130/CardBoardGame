using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float upDirection = 1f;
    public float rollForce = 10f;
    public float rollTourque = 10f;
    public float minForce = -5f;
    public float maxForce = 5f;
    public float minTorque = -5f;
    public float maxTorque = 5f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void OnButtonClick()
    {
        RollDice();
    }

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

    // 각 면의 이름(또는 번호)
    private int[] faceNames = new int[]
    {
        4, 3, 6, 1, 5, 2
    };

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
        // Debug.Log(_rigidbody.linearVelocity);
        if (_rigidbody.linearVelocity.magnitude <= 0.1f)
        {

            Debug.Log("위쪽 면: " + GetUpFace().ToString());
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _rigidbody.linearVelocity + (Time.fixedDeltaTime * Physics.gravity);
        transform.position += Time.fixedDeltaTime * _rigidbody.linearVelocity;
    }

    private void RollDice()
    {
        // Apply a random force to the dice to simulate rolling
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(minForce, maxForce), upDirection, UnityEngine.Random.Range(minForce, maxForce)).normalized;
        _rigidbody.AddForce(randomDirection * rollForce, ForceMode.Impulse);

        // Optionally, you can add a torque to make it spin
        Vector3 randomTorque = new Vector3(UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque), UnityEngine.Random.Range(minTorque, maxTorque));
        _rigidbody.AddTorque(randomTorque * rollTourque, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 income = collision.transform.position - transform.position;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 pushDirection = Vector3.Reflect(income, normal).normalized;
            Vector3 currTorque = _rigidbody.angularVelocity;
            _rigidbody.AddForce(pushDirection * rollForce, ForceMode.Impulse);
            _rigidbody.AddTorque(new Vector3(currTorque.x, currTorque.y, currTorque.z) * rollTourque, ForceMode.Impulse);
        }
    }
}
