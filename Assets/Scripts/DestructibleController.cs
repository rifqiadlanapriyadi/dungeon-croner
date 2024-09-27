using Pathfinding;
using UnityEngine;

public class DestructibleController : MonoBehaviour
{
    private Bounds _collBounds;

    private void Start()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float boundsXSize = bounds.size.x;
        float boundsYSize = bounds.size.y;
        _collBounds = new Bounds(bounds.center, new Vector2(Mathf.Max(boundsXSize, 1), Mathf.Max(boundsYSize, 1)));
    }

    private void OnDestroy()
    {
        GraphUpdateObject guo = new GraphUpdateObject(_collBounds);
        guo.modifyWalkability = true;
        guo.setWalkability = true;
        if (AstarPath.active != null)
            AstarPath.active.UpdateGraphs(guo);
    }
}
