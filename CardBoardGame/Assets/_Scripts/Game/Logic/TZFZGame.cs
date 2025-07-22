using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEditor.Rendering;
public class TZFZGame : MonoBehaviour
{
    private MiniGameHandler miniGameHandler;
    [SerializeField]
    private InputActionAsset tZFZActionAsset;
    private InputActionMap tZFZActionMap;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;
    public TZFZGrid[] gridArr;
    private TZFZGrid[][] tZFZGrids = new TZFZGrid[4][];

    private bool isInitialized = false;

    private bool hasInput = false;

    private Direction direction = Direction.None;

    private Dictionary<int, Color> tileColors = new Dictionary<int, Color>()
    {
         // 빈칸
        { 0, new Color32(205, 193, 180, 255) },
        { 2, new Color32(238, 228, 218, 255) },
        { 4, new Color32(237, 224, 200, 255) },
        { 8, new Color32(242, 177, 121, 255) },
        { 16, new Color32(245, 149, 99, 255) },
        { 32, new Color32(246, 124, 95, 255) },
        { 64, new Color32(246, 94, 59, 255) },
        { 128, new Color32(237, 207, 114, 255) },
        { 256, new Color32(237, 204, 97, 255) },
        { 512, new Color32(237, 200, 80, 255) },
        { 1024, new Color32(237, 197, 63, 255) },
        { 2048, new Color32(237, 194, 46, 255) },
        // 그 이상은 계속 추가 가능
    };
    [SerializeField]
    private TextMeshProUGUI moveCountTmp;
    [field: SerializeField]
    private int moveCount = 500;

    private int tempMoveCount;

    [SerializeField]
    private Button endButton;
    [SerializeField]
    private Button[] moveButtons;

    private void OnEnable()
    {
        Initialize();
    }
    private void OnDisable()
    {
        EndTZFZGame();
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
        direction = Direction.Up;
        GetDir(direction);
    }
    private void OnMoveDown(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        direction = Direction.Down;
        GetDir(direction);
    }
    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        direction = Direction.Left;
        GetDir(direction);
    }
    private void OnMoveRight(InputAction.CallbackContext context)
    {
        if (hasInput) return;
        hasInput = true;
        direction = Direction.Right;

        GetDir(direction);
    }

    private void GetDir(Direction dir)
    {
        if (moveCount <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            moveCount--;
            moveCountTmp.text = moveCount.ToString();
        }

        switch (dir)
        {
            case Direction.None:
                Debug.LogWarning("Direction None");
                break;
            case Direction.Up:
                TZFZMove(-1, 0);
                break;
            case Direction.Down:
                TZFZMove(1, 0);
                break;
            case Direction.Left:
                TZFZMove(0, -1);
                break;
            case Direction.Right:
                TZFZMove(0, 1);
                break;
            default:
                Debug.LogError("TZFZ_Direction Error");
                break;
        }
        SpawnRandomTile();
    }

    private void SpawnRandomTile()
    {
        List<(int, int)> emptyCells = new List<(int, int)>();

        // 1. 빈 칸 수집
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (string.IsNullOrEmpty(tZFZGrids[i][j].Tmp.text))
                {
                    emptyCells.Add((i, j));
                }
            }
        }

        // 2. 빈 칸이 없으면 리턴
        if (emptyCells.Count == 0)
            return;

        // 3. 랜덤 위치 선택
        var randomIndex = Random.Range(0, emptyCells.Count);
        var (x, y) = emptyCells[randomIndex];

        // 4. 2 또는 4 생성 (2가 나올 확률 80%, 4는 20%)
        int newValue = Random.value < 0.8f ? 2 : 4;

        // 5. 숫자 삽입
        tZFZGrids[x][y].Tmp.text = newValue.ToString();

        tZFZGrids[x][y].Color = tileColors.ContainsKey(newValue) ? tileColors[newValue] : Color.black;
    }

    private void TZFZMove(int dr, int dc)
    {
        for (int line = 0; line < 4; line++)
        {
            List<int> values = new List<int>();

            // 1. 한 줄 추출
            for (int offset = 0; offset < 4; offset++)
            {
                int i = dr == 0 ? line : (dr > 0 ? 3 - offset : offset);
                int j = dc == 0 ? line : (dc > 0 ? 3 - offset : offset);

                string text = tZFZGrids[i][j].Tmp.text;
                if (!string.IsNullOrEmpty(text))
                {
                    values.Add(int.Parse(text));
                }

                // 초기화
                tZFZGrids[i][j].Tmp.text = "";
                tZFZGrids[i][j].Color = Color.white;
            }

            // 숫자 병합    
            List<int> merged = new List<int>();

            for (int i = 0; i < values.Count; i++)
            {
                if (i < values.Count - 1 && values[i] == values[i + 1])
                {
                    merged.Add(values[i] * 2);
                    i++; // 다음 숫자는 이미 병합했으니 건너뜀
                }
                else
                {
                    merged.Add(values[i]);
                }
            }

            // 3. 결과 재삽입
            for (int offset = 0; offset < merged.Count; offset++)
            {
                int i = dr == 0 ? line : (dr > 0 ? 3 - offset : offset);
                int j = dc == 0 ? line : (dc > 0 ? 3 - offset : offset);

                tZFZGrids[i][j].Tmp.text = merged[offset].ToString();
                UpdateTileColor(i, j);
            }
        }
    }

    public void Initialize()
    {
        if (isInitialized == false)
        {
            miniGameHandler = GetComponentInParent<MiniGameHandler>();
            ActionMapInit();
            ButtonInit();
            int j = 0;

            for (int i = 0; i < 4; i++)
            {

                tZFZGrids[i] = new TZFZGrid[4];
                for (int k = 0; k < 4; k++)
                {
                    tZFZGrids[i][k] = gridArr[j];
                    gridArr[j].Init();
                    gridArr[j].Tmp.text = "";

                    j++;
                }
            }

            tempMoveCount = moveCount;

            endButton.onClick.AddListener(() => gameObject.SetActive(false));

            isInitialized = true;
        }
        moveCount = tempMoveCount;
        moveCountTmp.text = moveCount.ToString();
        SpawnInitialTiles();
    }

    private void ButtonInit()
    {
        moveButtons[0].onClick.AddListener(() => GetDir(Direction.Up));
        moveButtons[1].onClick.AddListener(() => GetDir(Direction.Down));
        moveButtons[2].onClick.AddListener(() => GetDir(Direction.Left));
        moveButtons[3].onClick.AddListener(() => GetDir(Direction.Right));
    }

    private void SpawnInitialTiles()
    {
        HashSet<int> result = new HashSet<int>();
        while (result.Count < 2)
        {
            result.Add(Random.Range(0, 16));
        }

        int[] startIdxs = new int[2];
        result.CopyTo(startIdxs);

        float rand = Random.value;

        int val1, val2;

        if (rand < 0.6f)
        {
            val1 = 2;
            val2 = 2;
        }
        else if (rand < 0.9f)
        {
            val1 = 2;
            val2 = 4;
        }
        else
        {
            val1 = 4;
            val2 = 4;
        }

        gridArr[startIdxs[0]].Tmp.text = val1.ToString();
        gridArr[startIdxs[1]].Tmp.text = val2.ToString();
        gridArr[startIdxs[0]].Color = tileColors.ContainsKey(val1) ? tileColors[val1] : Color.black;
        gridArr[startIdxs[1]].Color = tileColors.ContainsKey(val2) ? tileColors[val2] : Color.black;

    }

    private void UpdateTileColor(int i, int j)
    {
        var grid = tZFZGrids[i][j];
        int value = 0;

        if (!string.IsNullOrEmpty(grid.Tmp.text))
        {
            value = int.Parse(grid.Tmp.text);
        }

        Color bgColor = tileColors.ContainsKey(value) ? tileColors[value] : Color.black;

        grid.Color = bgColor;
    }

    private void EndTZFZGame()
    {
        int value = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                if (!string.IsNullOrEmpty(tZFZGrids[i][k].Tmp.text))
                {
                    if (value < int.Parse(tZFZGrids[i][k].Tmp.text))
                    {
                        value = int.Parse(tZFZGrids[i][k].Tmp.text);
                    }
                }
                tZFZGrids[i][k].Tmp.text = "";
            }
        }
        miniGameHandler.GetTZFZGameResult(value);
    }
}

