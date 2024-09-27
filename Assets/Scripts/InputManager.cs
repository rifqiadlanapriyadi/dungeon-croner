using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private bool _allow;
    public bool Allow
    {
        get { return _allow; }
        set { _allow = value; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        Allow = false;
    }

    public bool GetKeyDown(KeyCode keyCode)
    {
        return Allow && Input.GetKeyDown(keyCode);
    }

    public float GetAxisRaw(string axisName)
    {
        return !Allow ? 0 : Input.GetAxisRaw(axisName);
    }

    public Vector3 MousePosition()
    {
        return !Allow ? Vector3.zero : Input.mousePosition;
    }

    public bool GetMouseButtonDown(int button)
    {
        return Allow && Input.GetMouseButtonDown(button);
    }
}
