using System;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string id;
    public string name;
    public int count;
    public int maxStack = 99;
}

[System.Serializable]
public class GameSettings
{
    public float musicVolume = 0.8f;
    public float soundVolume = 0.7f;
    public string language = "Chinese";
    public int qualityLevel = 2;
}

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3() { }
    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public static implicit operator SerializableVector3(Vector3 vector)
    {
        return new SerializableVector3(vector.x, vector.y, vector.z);
    }
    
    public static implicit operator Vector3(SerializableVector3 serializable)
    {
        return new Vector3(serializable.x, serializable.y, serializable.z);
    }
}