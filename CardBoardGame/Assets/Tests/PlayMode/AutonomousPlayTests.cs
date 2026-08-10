using System;
using System.Collections;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public sealed class AutonomousPlayTests
{
    private const float TimeoutSeconds = 20f;
    private string outputDirectory;
    private string currentTest;
    private string currentPersona;
    private int currentSeed;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        Time.timeScale = 1f;
        outputDirectory = Environment.GetEnvironmentVariable("UNITY_PLAYTEST_OUTPUT_DIR");
        if (string.IsNullOrWhiteSpace(outputDirectory))
            outputDirectory = Path.Combine(Application.dataPath, "../Temp/AutonomousPlaytests");
        Directory.CreateDirectory(outputDirectory);
        Application.logMessageReceived += RecordLog;
        yield return LoadSceneAndWait(0);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Record("finish", "scenario teardown");
        Application.logMessageReceived -= RecordLog;
        Time.timeScale = 1f;
        yield return null;
    }

    [UnityTest] public IEnumerator Novice_CanEnterEasyFirstStage() => PlayCriticalPath("novice", 1101, "easyButton", 1, false);
    [UnityTest] public IEnumerator Explorer_CanReplayEasyFirstStage() => PlayCriticalPath("explorer", 2202, "easyButton", 1, false);
    [UnityTest] public IEnumerator Optimizer_CanReplayEasyFirstStage() => PlayCriticalPath("optimizer", 3303, "easyButton", 1, false);
    [UnityTest] public IEnumerator Chaos_RepeatedLobbyActionDoesNotBlockProgress() => PlayCriticalPath("chaos", 4404, "easyButton", 1, true);

    private IEnumerator PlayCriticalPath(string persona, int seed, string difficultyField, int expectedScene, bool repeatStart)
    {
        currentTest = TestContext.CurrentContext.Test.Name;
        currentPersona = persona;
        currentSeed = seed;
        UnityEngine.Random.InitState(seed);
        Record("start", $"difficultyField={difficultyField}");

        MonoBehaviour mainPanel = FindBehaviour("MainPanel");
        Assert.That(mainPanel, Is.Not.Null, "MainPanel was not found in the lobby scene.");
        InvokeButtonField(mainPanel, "gameStartButton");
        if (repeatStart) InvokeButtonField(mainPanel, "gameStartButton");
        Record("action", "opened difficulty panel");
        yield return null;

        MonoBehaviour difficultyPanel = FindBehaviour("DifficultyPanel");
        Assert.That(difficultyPanel, Is.Not.Null, "DifficultyPanel was not found.");
        InvokeButtonField(difficultyPanel, difficultyField);
        Record("action", $"clicked {difficultyField}");
        yield return WaitUntil(() => SceneManager.GetActiveScene().buildIndex == expectedScene,
            $"scene {expectedScene} did not activate");

        Record("state", "game scene activated");
        Assert.That(Time.timeScale, Is.EqualTo(1f),
            "Gameplay scene activated with paused time; animations, physics, and scaled waits cannot progress.");

        MonoBehaviour stageHandler = FindBehaviour("StageHandler");
        Assert.That(stageHandler, Is.Not.Null, "StageHandler was not found in the gameplay scene.");
        object[] stageButtons = GetArrayField(stageHandler, "stageButtons");
        Assert.That(stageButtons, Is.Not.Empty, "No stage buttons were configured.");
        InvokeButton(stageButtons[0]);
        Record("action", "selected first stage");
        for (int frame = 0; frame < 10; frame++) yield return null;
        Record("state", "first stage remained responsive for 10 frames");
    }

    private static IEnumerator LoadSceneAndWait(int buildIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);
        while (!operation.isDone) yield return null;
        yield return null;
    }

    private static IEnumerator WaitUntil(Func<bool> predicate, string failure)
    {
        float deadline = Time.realtimeSinceStartup + TimeoutSeconds;
        while (!predicate() && Time.realtimeSinceStartup < deadline) yield return null;
        Assert.That(predicate(), Is.True, failure);
    }

    private static MonoBehaviour FindBehaviour(string typeName)
    {
        foreach (MonoBehaviour behaviour in UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            if (behaviour != null && behaviour.GetType().Name == typeName) return behaviour;
        return null;
    }

    private static void InvokeButtonField(object owner, string fieldName)
    {
        object button = GetField(owner, fieldName);
        Assert.That(button, Is.Not.Null, $"Button field {fieldName} is null.");
        InvokeButton(button);
    }

    private static void InvokeButton(object button)
    {
        PropertyInfo onClickProperty = button.GetType().GetProperty("onClick");
        Assert.That(onClickProperty, Is.Not.Null, "Button does not expose onClick.");
        object onClick = onClickProperty.GetValue(button);
        MethodInfo invoke = onClick.GetType().GetMethod("Invoke", Type.EmptyTypes);
        Assert.That(invoke, Is.Not.Null, "Button event does not expose Invoke().");
        invoke.Invoke(onClick, null);
    }

    private static object[] GetArrayField(object owner, string fieldName)
    {
        Array array = GetField(owner, fieldName) as Array;
        Assert.That(array, Is.Not.Null, $"Array field {fieldName} was not found.");
        object[] values = new object[array.Length];
        for (int index = 0; index < array.Length; index++) values[index] = array.GetValue(index);
        return values;
    }

    private static object GetField(object owner, string fieldName)
    {
        for (Type type = owner.GetType(); type != null; type = type.BaseType)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null) return field.GetValue(owner);
        }
        Assert.Fail($"Field {fieldName} was not found on {owner.GetType().FullName}.");
        return null;
    }

    private void RecordLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
            Record("error", $"{type}: {condition}\n{stackTrace}");
        else if (type == LogType.Warning) Record("warning", condition);
    }

    private void Record(string eventName, string detail)
    {
        string json = JsonUtility.ToJson(new PlaytestEvent
        {
            timestamp = DateTime.UtcNow.ToString("O"), test = currentTest ?? TestContext.CurrentContext.Test.Name,
            persona = currentPersona ?? "setup", seed = currentSeed, eventName = eventName,
            scene = SceneManager.GetActiveScene().name, frame = Time.frameCount,
            timeScale = Time.timeScale, detail = detail
        });
        File.AppendAllText(Path.Combine(outputDirectory, "events.jsonl"),
            json.Replace("\"eventName\":", "\"event\":") + Environment.NewLine);
    }

    [Serializable]
    private sealed class PlaytestEvent
    {
        public string timestamp, test, persona, eventName, scene, detail;
        public int seed, frame;
        public float timeScale;
    }
}
