using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "ZaoMengSo", menuName = "MapCfg")]
public class MapCfg : ScriptableObject
{
    [Header("地图名称")] public string MapName;

    [Header("地图预制体")] [OnValueChanged("GetPrefabPath")]
    public GameObject MapPrefab;

    [Header("角色出生点")]
    public Vector3 JueSeChuShengDian;

    [Header("地图路径")] [ReadOnly] public string MapPrefabPath;

    [Header("刷怪点")] public Vector2[] ShuaGuaDian;

    [Header("相机位移阈值")] public Vector2[] XiangJiWeiZhiYuZhi;

#if UNITY_EDITOR
    public void GetPrefabPath()
    {
        MapPrefabPath = AssetDatabase.GetAssetPath(MapPrefab);
    }
#endif
}