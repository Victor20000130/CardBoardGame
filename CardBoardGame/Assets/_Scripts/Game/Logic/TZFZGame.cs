using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.InputSystem;
public class TZFZGame : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset tZFZActionAsset;
    private InputActionMap tZFZActionMap;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;
    [SerializeField]
    private TextMeshProUGUI[] tmps;
    private TextMeshProUGUI[][] gridTexts = new TextMeshProUGUI[4][];
    private bool isInitialized = false;

    private bool hasInput = false;

    private void OnEnable()
    {
        Initialize();
    }
    private void OnDisable()
    {
        GridReset();
        tZFZActionMap.Disable();
    }
    private void ActionMapInit()
    {
        tZFZActionMap = tZFZActionAsset.FindActionMap("TZFZ");
        if (isInitialized == false)
        {
            upAction = tZFZActionMap.FindAction("MoveUp");
            downAction = tZFZActionMap.FindAction("MoveDown");
            leftAction = tZFZActionMap.FindAction("MoveLeft");
            rightAction = tZFZActionMap.FindAction("MoveRight");

            upAction.performed += OnMoveUp;
            downAction.performed += OnMoveDown;
            leftAction.performed += OnMoveLeft;
            rightAction.performed += OnMoveRight;

            upAction.canceled += ctx => hasInput = false;
            downAction.canceled += ctx => hasInput = false;
            leftAction.canceled += ctx => hasInput = false;
            rightAction.canceled += ctx => hasInput = false;

        }
        tZFZActionMap.Enable();
    }

    private void OnMoveUp(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        Debug.Log("↑ Move Up");
    }
    private void OnMoveDown(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        Debug.Log("↑ Move Down");
    }
    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        Debug.Log("↑ Move Left");
    }
    private void OnMoveRight(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        Debug.Log("↑ Move Right");
    }

    // private void GetDir()
    // {
    //     switch()
    // }

    public void Initialize()
    {
        if (isInitialized == false)
        {
            ActionMapInit();
            int j = 0;
            for (int i = 0; i < 4; i++)
            {
                gridTexts[i] = new TextMeshProUGUI[4];
                for (int k = 0; k < 4; k++)
                {
                    gridTexts[i][k] = tmps[j];
                    tmps[j].text = "";
                    j++;
                }
            }
            isInitialized = true;
        }
        GenerateTwoNum();
    }
    private void GridReset()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                gridTexts[i][k].text = "";
            }
        }

    }

    private void GenerateTwoNum()
    {
        HashSet<int> result = new HashSet<int>();
        while (result.Count < 2)
        {
            result.Add(Random.Range(0, 16));
        }

        int[] startIdxs = new int[2];
        result.CopyTo(startIdxs);

        tmps[startIdxs[0]].text = "2";
        tmps[startIdxs[1]].text = "2";
    }

    public IEnumerator TZFZCorou()
    {
        while (true)
        {
        }
    }

    private void AddNumbers()
    {

    }
}
