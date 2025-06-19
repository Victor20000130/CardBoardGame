using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    Button button;
    private void Awake()
    {
        Debug.LogWarning("테스트용 스크립트 동작중입니다.");
        button = GetComponent<Button>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(ManagerHandler.Instance.gameManager.StartGame);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
