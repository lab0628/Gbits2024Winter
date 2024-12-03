using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSAD : MonoBehaviour
{
    private Animator wAnimator;
    private Animator sAnimator;
    private Animator aAnimator;
    private Animator dAnimator;
    // Start is called before the first frame update
    void Start()
    {
        wAnimator = transform.Find("WKey").GetComponent<Animator>();
        sAnimator = transform.Find("SKey").GetComponent<Animator>();
        aAnimator = transform.Find("AKey").GetComponent<Animator>();
        dAnimator = transform.Find("DKey").GetComponent<Animator>();
        
        wAnimator.speed = 0.0f;
        sAnimator.speed = 0.0f;
        aAnimator.speed = 0.0f;
        dAnimator.speed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
