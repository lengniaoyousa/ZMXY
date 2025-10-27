using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏数据静态访问类 - 游戏中所有数据访问都通过这个类
/// </summary>
public static class GameData
{
    #region 玩家基础信息
    /// <summary>
    /// 玩家名称
    /// </summary>
    public static string PlayerName
    {
        get => FlexibleDataManager.Instance.Get("playerName", "孙悟空");
        set => FlexibleDataManager.Instance.Set("playerName", value);
    }

    /// <summary>
    /// 等级
    /// </summary>
    public static int Level
    {
        get => FlexibleDataManager.Instance.Get("level", 1);
        set => FlexibleDataManager.Instance.Set("level", value);
    }

    /// <summary>
    /// 经验
    /// </summary>
    public static int Experience
    {
        get => FlexibleDataManager.Instance.Get("experience", 0);
        set => FlexibleDataManager.Instance.Set("experience", value);
    }
    #endregion

    #region 角色属性
    /// <summary>
    /// 生命
    /// </summary>
    public static int Health
    {
        get => FlexibleDataManager.Instance.Get("health", 100);
        set => FlexibleDataManager.Instance.Set("health", value);
    }

    /// <summary>
    /// 最大生命值
    /// </summary>
    public static int MaxHealth
    {
        get => FlexibleDataManager.Instance.Get("maxHealth", 100);
        set => FlexibleDataManager.Instance.Set("maxHealth", value);
    }

    // 新增字段示例 - 旧存档会自动获得默认值50
    /// <summary>
    /// 魔法
    /// </summary>
    public static int Mana
    {
        get => FlexibleDataManager.Instance.Get("mana", 50);
        set => FlexibleDataManager.Instance.Set("mana", value);
    }

    /// <summary>
    /// 魔法最大值
    /// </summary>
    public static float MaxMana
    {
        get => FlexibleDataManager.Instance.Get("maxMana", 50f);
        set => FlexibleDataManager.Instance.Set("maxMana", value);
    }
    #endregion

    #region 游戏系统
    /// <summary>
    /// 背包
    /// </summary>
    public static List<InventoryItem> Inventory
    {
        get => FlexibleDataManager.Instance.Get("inventory", new List<InventoryItem>
        {
            new InventoryItem { id = "health_potion", name = "生命药水", count = 3 },
            new InventoryItem { id = "bread", name = "面包", count = 5 }
        });
        set => FlexibleDataManager.Instance.Set("inventory", value);
    }
    
    /// <summary>
    /// 技能
    /// </summary>
    public static Dictionary<string, int> Skills
    {
        get => FlexibleDataManager.Instance.Get("skills", new Dictionary<string, int>
        {
            {"strength", 10},
            {"agility", 10},
            {"intelligence", 10}
        });
        set => FlexibleDataManager.Instance.Set("skills", value);
    }

    /// <summary>
    /// 游戏设置
    /// </summary>
    public static GameSettings Settings
    {
        get => FlexibleDataManager.Instance.Get("settings", new GameSettings());
        set => FlexibleDataManager.Instance.Set("settings", value);
    }
    #endregion

    #region 工具方法
    /// <summary>
    /// 添加装备
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="itemName"></param>
    /// <param name="count"></param>
    public static void AddItem(string itemId, string itemName, int count = 1)
    {
        var inventory = Inventory;
        var existingItem = inventory.Find(item => item.id == itemId);
        
        if (existingItem != null)
        {
            existingItem.count += count;
        }
        else
        {
            inventory.Add(new InventoryItem { id = itemId, name = itemName, count = count });
        }
        
        Inventory = inventory;
    }

    /// <summary>
    /// 添加经验
    /// </summary>
    /// <param name="exp"></param>
    public static void AddExperience(int exp)
    {
        Experience += exp;
        int expRequired = Level * 100;
        if (Experience >= expRequired)
        {
            Level++;
            Experience -= expRequired;
            MaxHealth += 10;
            Health = MaxHealth;
            
            Debug.Log($"升级了！当前等级: {Level}");
        }
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    public static void SaveGame()
    {
        FlexibleDataManager.Instance.Save();
        Debug.Log("游戏已保存");
    }
    
    /// <summary>
    /// 加载游戏
    /// </summary>
    public static void LoadGame()
    {
        FlexibleDataManager.Instance.LoadOrCreate();
        Debug.Log("游戏已加载");
    }

    /// <summary>
    /// 是否有存档
    /// </summary>
    /// <returns></returns>
    public static bool HasSaveData(string key)
    {
        return FlexibleDataManager.Instance.HasKey(key);
    }
    #endregion

    #region 初始化数据

    public static void InitSunWuKongData()
    {
        PlayerName = "孙悟空";
        Level = 1;
        Experience = 0;
        Health = 300;
        MaxHealth = 300;
        Mana = 100;
        MaxMana = 100;
    }

    #endregion
}