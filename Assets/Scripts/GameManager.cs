using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int levelCount;
    private Queue<int> sceneQueue;
    private int _nextScene;
    private bool _started = false;
    private bool _toMainMenu = false;

    private PlayerManager _pm;
    private UIManager _ui;
    private InputManager _im;
    private PersistanceManager _persist;

    private HashSet<EnemyController> _enemies = new HashSet<EnemyController>();

    private HashSet<LevelDoorController> _doors = new HashSet<LevelDoorController>();

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Create scene queue
        List<int> sceneList = new List<int>();
        for (int i = 2; i <= levelCount + 1; i++)
            sceneList.Add(i);
        ShuffleList(sceneList);
        sceneQueue = new Queue<int>();
        foreach (int i in sceneList)
            sceneQueue.Enqueue(i);
        _nextScene = -1;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleDoors();
        AddSceneEnemies();
        if (_ui != null)
            _ui.SetDoorsOpenNotificationActive(false);
    }

    private void Start()
    {
        _pm = PlayerManager.instance;
        _ui = UIManager.instance;
        _im = InputManager.instance;
        _persist = PersistanceManager.instance;

        _started = true;
        _im.Allow = true;
        _ui.SetDoorsOpenNotificationActive(false);
    }

    private void Update()
    {
        UpdateLevelClear();
    }

    private void HandleDoors()
    {
        _doors = new HashSet<LevelDoorController>();
        LevelDoorController[] doors = FindObjectsOfType<LevelDoorController>();
        List<LevelDoorController> spawnDoorCandidates = new List<LevelDoorController>();
        int nextScene = sceneQueue.Count == 0 ? -1 : sceneQueue.Dequeue();
        _nextScene = nextScene;
        foreach (LevelDoorController door in doors)
        {
            door.nextScene = nextScene;
            door.Close();
            _doors.Add(door);
            if (_pm != null && _pm.hasUsedDoor && door.direction == _pm.lastEnteredDoorDir.Opposite())
                spawnDoorCandidates.Add(door);
        }
        if (spawnDoorCandidates.Count > 0)
        {
            LevelDoorController spawnDoor = spawnDoorCandidates[Random.Range(0, spawnDoorCandidates.Count)];
            _pm.transform.position = spawnDoor.spawnPoint.position;
        }
    }

    private void AddSceneEnemies()
    {
        _enemies = new HashSet<EnemyController>();
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
            if (enemy.gameObject.activeInHierarchy)
                AddEnemy(enemy);
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (!enemy.respawn) _enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
    }

    private void UpdateLevelClear()
    {
        if (_enemies.Count == 0)
        {
            if (_nextScene == -1)
            {
                _im.Allow = false;
                _ui.ShowYouWinUI(true);
            }
            else
                foreach (LevelDoorController door in _doors)
                    if (!_pm.hasUsedDoor || door.direction != _pm.lastEnteredDoorDir.Opposite())
                    {
                        door.Open();
                        _ui.SetDoorsOpenNotificationActive(true);
                    }
        }
    }

    public void GameOver()
    {
        _im.Allow = false;
        _ui.ShowGameOverUI(true);
    }

    public void RestartGame()
    {
        _toMainMenu = false;
        _persist.DestroyPersistantObjects();
    }

    public void GoToMainMenu()
    {
        _toMainMenu = true;
        _persist.DestroyPersistantObjects();
    }

    private void OnDestroy()
    {
        if (_started)
            SceneManager.LoadScene(_toMainMenu ? 0 : 1);
    }

    private void ShuffleList(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
