using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class SerializableGameData
{
    // 基础元数据
    [JsonProperty("version")]
    public string Version { get; set; } = "1.0.0";
    
    [JsonProperty("playerId")]
    public string PlayerId { get; set; } = Guid.NewGuid().ToString();
    
    [JsonProperty("lastSaved")]
    public DateTime LastSaved { get; set; } = DateTime.Now;

    // 扩展数据存储 - 关键：所有未映射的字段都会自动存储在这里
    [JsonExtensionData]
    public IDictionary<string, JToken> ExtraData { get; set; } = new Dictionary<string, JToken>();

    // 核心数据字段示例（可选）
    [JsonProperty("playerName")]
    public string PlayerName { get; set; } = "冒险者";
    
    [JsonProperty("level")]
    public int Level { get; set; } = 1;
}