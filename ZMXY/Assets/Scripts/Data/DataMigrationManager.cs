using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataMigrationManager
{
    private Dictionary<string, Action<JObject>> _migrations;
    private string _currentVersion = "1.2.0";

    public DataMigrationManager()
    {
        InitializeMigrations();
    }

    private void InitializeMigrations()
    {
        _migrations = new Dictionary<string, Action<JObject>>
        {
            { "1.1.0", MigrateToV1_1_0 },
            { "1.2.0", MigrateToV1_2_0 }
        };
    }

    /// <summary>
    /// 迁移数据到最新版本
    /// </summary>
    public JObject MigrateData(string json)
    {
        try
        {
            JObject jsonData = JObject.Parse(json);
            string currentVersion = jsonData["version"]?.Value<string>() ?? "1.0.0";

            // 如果版本相同，直接返回
            if (currentVersion == _currentVersion)
            {
                return jsonData;
            }

            Debug.Log($"开始数据迁移: {currentVersion} -> {_currentVersion}");

            // 执行所有需要的迁移
            foreach (var migration in _migrations)
            {
                string migrationVersion = migration.Key;
                if (CompareVersions(currentVersion, migrationVersion) < 0)
                {
                    migration.Value(jsonData);
                    jsonData["version"] = migrationVersion;
                    Debug.Log($"执行迁移到版本: {migrationVersion}");
                }
            }

            // 更新到最新版本
            jsonData["version"] = _currentVersion;
            
            Debug.Log("数据迁移完成");
            return jsonData;
        }
        catch (Exception e)
        {
            Debug.LogError($"数据迁移失败: {e.Message}");
            // 创建新的默认数据
            return JObject.FromObject(new SerializableGameData());
        }
    }

    // 迁移到版本 1.1.0 - 添加魔法值系统
    private void MigrateToV1_1_0(JObject jsonData)
    {
        if (!jsonData.ContainsKey("mana"))
        {
            int level = jsonData["level"]?.Value<int>() ?? 1;
            float baseMana = 50f;
            float mana = baseMana + (level - 1) * 10f;
            
            jsonData["mana"] = mana;
            jsonData["maxMana"] = mana;
        }

        if (!jsonData.ContainsKey("inventory"))
        {
            jsonData["inventory"] = JToken.FromObject(new List<InventoryItem>());
        }
    }

    // 迁移到版本 1.2.0 - 添加技能系统
    private void MigrateToV1_2_0(JObject jsonData)
    {
        if (!jsonData.ContainsKey("skills"))
        {
            var skills = new Dictionary<string, int>
            {
                {"strength", 10},
                {"agility", 10},
                {"intelligence", 10}
            };
            jsonData["skills"] = JToken.FromObject(skills);
        }
    }

    private int CompareVersions(string version1, string version2)
    {
        try
        {
            Version v1 = new Version(version1);
            Version v2 = new Version(version2);
            return v1.CompareTo(v2);
        }
        catch
        {
            return -1;
        }
    }

    public string GetCurrentVersion()
    {
        return _currentVersion;
    }
}