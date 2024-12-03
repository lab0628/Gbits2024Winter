using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 世界管理器 用于生成物体
/// </summary>
public class WorldMgr : SingletonAutoMono<WorldMgr>
{
    void Awake(){
        // 手动移除不销毁
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="porpType">道具类型</param>
    /// <param name="worldPos">世界坐标</param>
    public void GnerateProp(PorpType porpType, Vector3 worldPos){
        switch (porpType)
        {
            case PorpType.SleepClock:
                GameObject obj = Instantiate(ResMgr.Ins.Load<GameObject>("Prefabs/Props/SleepClock"));
                obj.transform.parent = transform;
                obj.transform.position = worldPos;
                break;
            default:break;
        }
    }
}
