using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class BoxState : CanSleepObject
{
    private Rigidbody2D rb;
    private SkeletonAnimation skeleton;
    private float skeletonOriginScale;

    protected new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        skeleton = GetComponentInChildren<SkeletonAnimation>();
        if(skeleton != null)
            skeletonOriginScale = skeleton.transform.localScale.x;
        
        onSleep.AddListener(OnSleep);
        onCancelSleep.AddListener(OnCancelSleep);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }
    

    public void OnSleep()
    {
        if(skeleton!=null)
            skeleton.state.SetAnimation(0,"sleep",true);
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void OnCancelSleep()
    {
        if(skeleton!=null)
            skeleton.state.SetAnimation(0,"idle",true);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void PlayScaleAnimation(){
        base.PlayScaleAnimation();
        if(skeleton !=null)
            skeleton.transform.DOScale(skeletonOriginScale + 0.2f, 0.2f);
    }

    public override void ExitScaleAnimation(){
        if(skeleton !=null)
            skeleton.transform.DOScale(skeletonOriginScale, 0.2f);
    }

}
