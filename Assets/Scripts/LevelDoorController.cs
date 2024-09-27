using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelDoorDirection
{
    North,
    East,
    South,
    West
}

public static class LevelDoorDirectionMethods
{
    public static LevelDoorDirection Opposite(this LevelDoorDirection direction)
    {
        switch (direction)
        {
            case LevelDoorDirection.North:
                return LevelDoorDirection.South;
            case LevelDoorDirection.East:
                return LevelDoorDirection.West;
            case LevelDoorDirection.South:
                return LevelDoorDirection.North;
            default:
                return LevelDoorDirection.East;
        }
    }
}

public class LevelDoorController : MonoBehaviour
{
    public LevelDoorDirection direction;
    public Transform spawnPoint;
    public GameObject door;

    public int nextScene;

    private PlayerManager _pm;

    private void Start()
    {
        _pm = PlayerManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _pm.hasUsedDoor = true;
            _pm.lastEnteredDoorDir = direction;
            SceneManager.LoadScene(nextScene);
        }
    }

    public void Open()
    {
        door.SetActive(false);
    }

    public void Close()
    {
        door.SetActive(true);
    }
}
