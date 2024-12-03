using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class SpringState : CanSleepObject
{
    public float springForce = 300f;

    private SkeletonAnimation skeleton;
    // Start is called before the first frame update
    private float skeletonOriginScale;
    void Start()
    {
        skeleton = transform.Find("@Spine").GetComponent<SkeletonAnimation>();
        skeletonOriginScale = skeleton.transform.localScale.x;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        
        Rigidbody2D otherRb = collider.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb != null)
        {
            // otherRb.AddForce(new Vector2(0, springForce));
            if (isSleeping) return;
            otherRb.velocity = new Vector2(0, springForce);

            PlayJumpAnimation();
        }
    }
    
    private void PlayJumpAnimation()
    {
        // 播放弹簧动画
        var te = skeleton.state.SetAnimation(0, "jump", false);
        te.TimeScale = 2.5f;
        te.Complete += entry =>
        {
            skeleton.state.SetAnimation(0, "idle", true);
            te.TimeScale = 1.0f;
        };
    }

    public override void PlayScaleAnimation(){
        base.PlayScaleAnimation();
        skeleton.transform.DOScale(skeletonOriginScale + 0.2f, 0.2f);
    }

    public override void ExitScaleAnimation(){
        skeleton.transform.DOScale(skeletonOriginScale, 0.2f);
    }
}
