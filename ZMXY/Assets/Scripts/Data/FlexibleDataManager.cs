using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine;

public class FlexibleDataManager : MonoBehaviour
{
    private static FlexibleDataManager _instance;
    public static FlexibleDataManager Instance => _instance;

    private string _savePath;
    private JObject _currentData;
    private DataMigrationManager _migrationManager;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void Initialize()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "SaveData", "game_save.json");
        _migrationManager = new DataMigrationManager();
        Directory.CreateDirectory(Path.GetDirectoryName(_savePath));
    }

    /// <summary>
    /// 设置数据值
    /// </summary>
    public void Set<T>(string key, T value)
    {
        if (_currentData == null)
            LoadOrCreate();

        _currentData[key] = JToken.FromObject(value);
    }

    /// <summary>
    /// 获取数据值，如果不存在返回默认值
    /// </summary>
    public T Get<T>(string key, T defaultValue = default(T))
    {
        if (_currentData == null)
            LoadOrCreate();

        if (_currentData.TryGetValue(key, out JToken token))
        {
            try
            {
                return token.ToObject<T>();
            }
            catch
            {
                return defaultValue;
            }
        }
        
        Set(key, defaultValue);
        return defaultValue;
    }

    /// <summary>
    /// 检查键是否存在
    /// </summary>
    public bool HasKey(string key)
    {
        if (_currentData == null)
            LoadOrCreate();
        return _currentData.ContainsKey(key);
    }

    /// <summary>
    /// 删除指定键
    /// </summary>
    public void DeleteKey(string key)
    {
        if (_currentData == null)
            LoadOrCreate();

        _currentData.Remove(key);
    }

    /// <summary>
    /// 加载或创建数据
    /// </summary>
    public void LoadOrCreate()
    {
        if (File.Exists(_savePath))
        {
            try
            {
                string json = File.ReadAllText(_savePath);
                
                // 使用迁移管理器处理数据
                _currentData = _migrationManager.MigrateData(json);
                
                Debug.Log("存档数据加载并迁移成功");
            }
            catch (Exception e)
            {
                Debug.LogError($"加载存档失败，创建新数据: {e.Message}");
                CreateNewData();
            }
        }
        else
        {
            CreateNewData();
        }
    }

    private void CreateNewData()
    {
        var baseData = new SerializableGameData();
        _currentData = JObject.FromObject(baseData);
        Debug.Log("创建新的存档数据");
    }

    /// <summary>
    /// 保存数据到文件
    /// </summary>
    public void Save()
    {
        if (_currentData == null)
        {
            Debug.LogWarning("没有数据可保存");
            return;
        }

        try
        {
            _currentData["lastSaved"] = DateTime.Now;
            _currentData["version"] = _migrationManager.GetCurrentVersion();
            
            string json = _currentData.ToString(Formatting.Indented);
            File.WriteAllText(_savePath, json);
            
            Debug.Log($"数据保存成功: {_savePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存数据失败: {e.Message}");
        }
    }

    /// <summary>
    /// 导出所有数据（调试用）
    /// </summary>
    public string ExportAllData()
    {
        return _currentData?.ToString(Formatting.Indented) ?? "No data";
    }

    /// <summary>
    /// 获取所有键（调试用）
    /// </summary>
    public System.Collections.Generic.List<string> GetAllKeys()
    {
        if (_currentData == null)
            LoadOrCreate();

        var keys = new System.Collections.Generic.List<string>();
        foreach (var property in _currentData.Properties())
        {
            keys.Add(property.Name);
        }
        return keys;
    }
}