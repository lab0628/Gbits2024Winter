using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : EnergiedComponent
{
    /// <summary>
    /// 需要生成的物体预制体
    /// </summary>
    public GameObject spawnPrefab;
    private GameObject insObject;
    private bool lastEnergy = false;
    void Start()
    {
        // 电信号是一直传输的，记录一下不同信号才触发
        onEnergized.AddListener((bool energy)=>{
            if(lastEnergy != energy){
                lastEnergy = energy;
                if(energy){
                    if(insObject != null){
                        Destroy(insObject);
                    }
                    insObject = Spawn();
                }
            }

            
        });
    }



    private GameObject Spawn(){
        GameObject obj = Instantiate(spawnPrefab);
        obj.transform.position = transform.position;
        obj.transform.parent = transform;
        return obj;
    }

    
}
