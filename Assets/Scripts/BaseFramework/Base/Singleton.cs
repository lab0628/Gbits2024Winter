using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现单例 类 继承此类即可
/// </summary>
/// <typeparam name="T">需可实例化的对象</typeparam>
public abstract class Singleton<T> where T:new()
{
    private static T _instance;

    public static T Ins
    {
        get{if (_instance == null)
                _instance = new T();
            return _instance;}
    }
}

