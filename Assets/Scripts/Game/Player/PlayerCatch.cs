using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public partial class Player : MonoBehaviour
{
    private bool _movingBox = false;    // 是否正在控制箱子
    private Collider2D _targetBox = null;   // 可控或正控的箱子
    private bool _catchingBox = false;  // 防连点
    private GameObject pullHand; // 拉箱子用
    private GameObject keyHint; // 推拉箱子提示
    //public GameObject pullHandPrefab;   // 拉箱子用

    //public bool _movingBox = false;    // 是否正在控制箱子
    //public Collider2D _targetBox = null;   // 可控或正控的箱子
    //public bool _catchingBox = false;  // 防连点
    private void DrawCatchGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(transform.localScale.x + 0.5f, transform.localScale.y - 0.1f));
    }
    private void CheckBox()
    {
        Collider2D[] boxes = Physics2D.OverlapBoxAll(transform.position, new Vector2(transform.localScale.x + 0.5f, transform.localScale.y - 0.1f),
            0, 1 << LayerMask.NameToLayer("Interact"));
        if (_movingBox)
        {
            // 箱子是否仍在范围
            if (_targetBox & _targetBox.isActiveAndEnabled & boxes.Contains(_targetBox))
            {
                BoxState bs = _targetBox?.GetComponent<BoxState>();
                if (bs & !bs.isSleeping)  // 需要是清醒的Box
                    return;
            }
            // 箱子离开玩家时
            FreeBox();
            return;
        }
        // 没在控制箱子时
        if (!_onGround)
        {
            NotReadyToCatchBox();
            return;  // 仅在地面时可控制箱子
        } 
        // 选取最近的Box作为可推拉目标
        float nearestDis = 512f;
        Collider2D nearestBox = null;
        foreach (Collider2D box in boxes)
        {
            BoxState bs = box?.GetComponent<BoxState>();
            if (!bs || bs.isSleeping)  // 需要是清醒的Box
                continue;
            float currentDis = Vector2.Distance(transform.position, box.transform.position);
            if (currentDis >= nearestDis)
                continue;
            nearestDis = currentDis;
            nearestBox = box;
        }
        _targetBox = nearestBox;
        if (nearestBox)
            ReadyToCatchBox();
        else
            NotReadyToCatchBox();
    }

    /// <summary>
    /// 解绑箱子，取消推拉状态
    /// </summary>
    public void FreeBox()
    {
        //if (!_targetBox) return;    // 无目标
        if (!_movingBox) // 没在推拉
        {
            return;
        }
        //Debug.Log("Freeing Box, " + ((int)_targetBox.forceReceiveLayers).ToString());
        // Todo
        if(_targetBox & _targetBox.isActiveAndEnabled)
        {
            _targetBox.forceReceiveLayers &= ~(1 << LayerMask.NameToLayer("Player"));
            Rigidbody2D rb = _targetBox.GetComponent<Rigidbody2D>();
            rb.freezeRotation = false;
            rb.drag = 1;
        }
        pullHand.SetActive(false);
        //Debug.Log(((int)_targetBox.forceReceiveLayers).ToString());

        _movingBox = false;
        _targetBox = null;
        return;
    }
    private void ReadyToCatchBox()
    {
        if (_catchingBox) return;
        // 视觉效果，示意可以推拉箱子
        // 提示位置
        //Vector3 pullBias = new Vector3(0.2f + _targetBox.transform.lossyScale.x + transform.localScale.x / 2, 0, 0);
        //float keyBiasX = _targetBox.transform.lossyScale.x / transform.localScale.x / 2f + 0.5f;
        //Vector3 keyBias = new Vector3(transform.position.x > _targetBox.transform.position.x ? -keyBiasX : keyBiasX, 
        //    _targetBox.transform.lossyScale.y / transform.localScale.y, 0);
        Vector3 keyPos = new Vector3(_targetBox.transform.position.x,_targetBox.transform.position.y + _targetBox.transform.lossyScale.y, 0);
        keyHint.SetActive(true);
        //keyHint.transform.localPosition = keyBias;
        keyHint.transform.SetPositionAndRotation(keyPos,Quaternion.identity);
    }
    private void NotReadyToCatchBox()
    {
        keyHint.SetActive(false);
    }

    private void CatchBox()
    {
        if (_movingBox) // 若正在推拉则释放
        {
            FreeBox();
            return;
        }
        if (!_targetBox) return;    // 无目标
        NotReadyToCatchBox();
        // 抓住箱子
        Debug.Log("Catching Box, "+((int)_targetBox.forceReceiveLayers).ToString());
        _targetBox.forceReceiveLayers |= 1 << LayerMask.NameToLayer("Player");
        Rigidbody2D rb = _targetBox.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.drag = 0;
        // 抓手位置
        //Vector3 pullBias = new Vector3(0.2f + _targetBox.transform.lossyScale.x + transform.localScale.x / 2, 0, 0);
        Vector3 pullBias = new Vector3(0.1f + _targetBox.transform.lossyScale.x / transform.localScale.x + 0.5f, 0, 0);
        pullHand.SetActive(true);
        pullHand.transform.localPosition = transform.position.x > _targetBox.transform.position.x ? 
            - pullBias : pullBias;
        Debug.Log(((int)_targetBox.forceReceiveLayers).ToString());
        _movingBox = true;
    }
    public void ForceBox(InputAction.CallbackContext context)   //移动
    {
        if (_catchingBox) return;
        _catchingBox = true;
        TimersManager.SetTimer(this, 0.02f, AllowCatch);   //0.02秒后允许再点

        float _inputKey = context.ReadValue<float>(); //获取输入值
        Debug.Log("Input");
        if (_inputKey == 1f)
        {
            Debug.Log("Valid Input");
            CatchBox();
        }
    }

    /// <summary>
    /// 判断箱子是否在玩家右边
    /// 注意如果没有抓箱子也会返回true
    /// </summary>
    /// <returns></returns>
    private bool BoxIsRight(){
        if(_targetBox != null){
            return transform.position.x < _targetBox.transform.position.x;
        }
        return true;
    }
    private void AllowCatch()
    {
        _catchingBox = false;
    }
}
