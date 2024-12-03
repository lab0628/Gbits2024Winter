using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public abstract class CanInteractObject : MonoBehaviour
{
    protected Collider2D coll;
    private bool inObject = false;
    private bool lastInObject = false;
    protected void Awake()
    {
        coll = GetComponent<Collider2D>();
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }
    
    protected void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,100.0f,
            1 << LayerMask.NameToLayer("Interact"));
        
        inObject = hit.collider == coll;
        if (lastInObject != inObject)
        {
            if (inObject)
            {
                // EventSystem.Ins.Emit(new PlayerCursorEnterInteractEvent(this));
            }
            else
            {
                // EventSystem.Ins.Emit(new PlayerCursorExitInteractEvent(this));
            }
        }
        lastInObject = inObject;
        // if(hit.collider!=null)Debug.Log(hit.collider.gameObject.name);
        if (Input.GetMouseButtonDown(0) && inObject)
        {
            OnInteract();
        }
    }

    public abstract void OnInteract();
    public virtual void PlayScaleAnimation(){

    }

    public virtual void ExitScaleAnimation(){

    }
}
