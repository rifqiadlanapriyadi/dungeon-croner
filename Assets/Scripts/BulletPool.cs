using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    public GameObject bulletPrefab;
    public int initialCount = 10;

    private Queue<GameObject> _queue;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        _queue = new Queue<GameObject>();
        for (int i = 0; i < initialCount; i++)
            InstantiateBullet();
    }

    private void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.parent = transform;
        bullet.SetActive(false);
        _queue.Enqueue(bullet);
    }

    public GameObject DequeueFromPool(Vector3 position, Quaternion rotation, float maxDistance)
    {
        if (_queue.Count == 0)
            InstantiateBullet();
        GameObject bullet = _queue.Dequeue();
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);
        bulletBehavior.SetColliderBodyActive(true);

        bulletBehavior.SetMaxDistance(maxDistance > 0, maxDistance);

        return bullet;
    }

    public void EnqueueToPool(GameObject bulletObject)
    {
        bulletObject.SetActive(false);
        _queue.Enqueue(bulletObject);
    }
}
