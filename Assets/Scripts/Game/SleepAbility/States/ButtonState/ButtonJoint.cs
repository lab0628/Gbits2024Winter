using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonJoint : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    public float tolerance = 0.18f;
    public bool isPressed = false;
    public Vector2 minAndMax;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.localPosition.y > minAndMax.y)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = -3f;
        }
        float y = Math.Clamp(transform.localPosition.y, minAndMax.x, minAndMax.y);
        transform.localPosition = new Vector3(0, y, 0);


        isPressed = transform.localPosition.y <= tolerance;
    }
    
    


}
