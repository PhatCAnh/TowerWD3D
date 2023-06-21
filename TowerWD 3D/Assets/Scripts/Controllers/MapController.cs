using CanasSource;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private void Awake()
    {
        Singleton<MapController>.Instance = this;
    }
}
