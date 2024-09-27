using System.Collections.Generic;
using UnityEngine;

public class PersistanceManager : MonoBehaviour
{
    public static PersistanceManager instance;

    public List<GameObject> persistentObjects;

    private void Awake()
    {
        if (instance != null)
        {
            List<GameObject> addPersists = new List<GameObject>();
            List<GameObject> toDestroy = new List<GameObject>();
            foreach (GameObject obj in persistentObjects)
            {
                bool foundSameName = false;
                foreach (GameObject exist in instance.persistentObjects)
                    if (obj.name == exist.name)
                    {
                        foundSameName = true;
                        break;
                    }
                if (foundSameName) toDestroy.Add(obj);
                else addPersists.Add(obj);
            }

            foreach (GameObject persist in addPersists)
            {
                DontDestroyOnLoad(persist);
                instance.persistentObjects.Add(persist);
            }

            foreach (GameObject dest in toDestroy)
                Destroy(dest);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (GameObject persist in persistentObjects)
                DontDestroyOnLoad(persist);
        }
    }

    public void DestroyPersistantObjects()
    {
        foreach (GameObject persist in persistentObjects)
            if (persist.gameObject != gameObject)
                Destroy(persist.gameObject);
        Destroy(gameObject);
    }
}
