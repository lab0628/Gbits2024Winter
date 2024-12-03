
using UnityEngine;
using UnityEngine.InputSystem;
public enum GroundType
{
    Ground,
    Cloud,
    Null
}
public partial class Player : MonoBehaviour //玩家移动
{
    
    //[SerializeField] private Rigidbody2D rb ;   //刚体
    //[SerializeField] private float speed ;  //速度
    public float moveSpeed = 3f;  //速度
    [Tooltip("推拉箱子时的速度")]
    public float pushSpeed = 1.5f;
    public float jumpSpeed = 10f;    // 起跳速度
    //public float jumpForce = 100f;    // 弹跳力
    private Rigidbody2D rb;   //刚体
    private float _inputDirection;    //输入行走方向
    private float _inputJump;   //输入跳跃
    public bool _onGround = false; // 是否在地面上

    public Vector2 chechGorundOffset;
    public Vector2 chechGorundSize;
    //private PhysicsMaterial2D materialNB;   // no bounce
    //private PhysicsMaterial2D materialNFNB;   // no friction no bounce

    public void Move(InputAction.CallbackContext context)   //移动
    {
        _inputDirection = context.ReadValue<float>(); //获取输入值
        //Debug.Log(_inputDirection);
    }

    public void Jump(InputAction.CallbackContext context)   //跳跃
    {
        if (_onGround && context.ReadValue<float>() == 1f)
            _inputJump = 1f;
        //_inputJump = context.ReadValue<float>(); //获取输入值
        //Debug.Log(_inputJump);
    }

    private void FixedUpdate()      //固定更新（一直执行）
    {
        CheckGround();
        // Debug.Log(rb.totalForce);
        // Debug.Log(rb.velocity.y);

        float velY = rb.velocity.y;
        if (_onGround)
        {
            rb.sharedMaterial = ResMgr.Ins.Load<PhysicsMaterial2D>("Materials/NoBounce");
            if (ReadyToJump())
            {
                GetComponent<Player>().FreeBox();
                if (velY < jumpSpeed)
                    velY = jumpSpeed;
                //rb.AddForce(new Vector2(0f, jumpForce));
                //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            _inputJump = 0;
        }
        else
        {
            rb.sharedMaterial = ResMgr.Ins.Load<PhysicsMaterial2D>("Materials/NoFrictionNoBounce");
        }

        rb.velocity = new Vector2((_movingBox ? pushSpeed : moveSpeed) * _inputDirection, velY);

        //var position = (Vector2)transform.position; //获取位置
        //var targetPosition = position + new Vector2(_inputDirection, 0);    //目标位置
        //if (position == targetPosition) return;  //如果位置等于目标位置，返回
        //rb.DOMove(targetPosition, speed).SetSpeedBased();   //移动DOMove方案
    }

    private bool ReadyToJump()
    {
        return _onGround && (_inputJump == 1f);
    }

    private void CheckGround()
    {
        Vector3 offset = chechGorundOffset;
        Collider2D[] grounds = Physics2D.OverlapBoxAll(transform.position + offset, chechGorundSize,
            0, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Interact")
                                                    | 1 << LayerMask.NameToLayer("Cloud"));
        if (grounds.Length > 0)
        {
            _onGround = true;
            //rb.sharedMaterial = materialNB;
        }
        else
        {
            _onGround = false;
            //rb.sharedMaterial = materialNFNB;
        }
        // Debug.Log(grounds.Length);
    }



    private void DrawMovementGizmos()
    {
        Vector3 offset = chechGorundOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, chechGorundSize);

    }

    public GroundType GetPlayerGroundType()
    {
        Vector3 offset = chechGorundOffset;
        Collider2D[] grounds = Physics2D.OverlapBoxAll(transform.position + offset, chechGorundSize,
            0, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Cloud"));
        if (grounds.Length > 0)
        {
            foreach (var otherColl in grounds)
            {
                if (otherColl.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    return GroundType.Ground;
                }
                if (otherColl.gameObject.layer == LayerMask.NameToLayer("Cloud"))
                {
                    return GroundType.Cloud;
                }
            }

            
        }
        return GroundType.Null;
        
        
    }

    //private void Update()
    //{
    //    //transform.Translate(new Vector2(_inputDirection, 0)*Time.fixedDeltaTime*speed);
    //    if (_onGround && _inputJump == 1f)
    //    {
    //        rb.AddForce(Vector2.up * jumpForce);  // AddForce方案
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Ground")
    //    {
    //        _onGround = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        _onGround = false;
    //    }
    //}
}
