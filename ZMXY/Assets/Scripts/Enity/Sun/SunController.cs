using UnityEngine;

public enum SunWuKongState
{
    Idle,
    Walk,
    Run,
    Jump,
    DoubleJump,
    Fall,
    Attack
}
public partial class SunController : Enity
{
    public PhysicsMaterial2D MoveMaterial;
    
    public PhysicsMaterial2D IdleMaterial;
    
    protected SunWuKongState state;

    private int attackCount = 0;
    
    #region 键盘输入

    private int mInputX = 0;

    private bool isOpenInputCheck = true;

    public void SetInputX(int input)
    {
        mInputX = input;
    }

    public int GetInputX()
    {
        return mInputX;
    }

    #endregion

    #region 各个状态的数据
    //移动速度
    public float moveSpeed;
    
    public float runSpeed;

    #endregion

    #region 空中属性

    public float airSpeed = 4;
    
    private bool doubleJump = false;
    
    #endregion

    #region 地面检测

    [Header("射线检测设置")]
    public float raycastDistance = 1f;    // 射线检测距离
    public LayerMask groundLayer;         // 地面层级
    public Vector2 raycastOffset = Vector2.zero; // 射线起始点偏移

    private bool isGrounded;
    void CheckGround()
    {
        // 计算射线起始位置（考虑偏移）
       // Vector2 rayOrigin = (Vector2)transform.position + raycastOffset ;
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(raycastOffset.x* GetMianChaoXiang(), raycastOffset.y);
        
        // 根据角色朝向决定射线方向
        float facingDirection = GetMianChaoXiang() > 0 ? 1f : -1f;
        Vector2 rayDirection = Vector2.right * facingDirection;
        
        // 向右发射射线
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection , raycastDistance , groundLayer);
        
        // 可视化射线（在Scene视图中显示）
        Debug.DrawRay(rayOrigin, rayDirection * raycastDistance , hit.collider != null ? Color.green : Color.red);
        
        // 检测是否碰到地面
        if (hit.collider != null)
        {
            isGrounded = true;
            Debug.Log($"碰到地面: {hit.collider.name}");
        }
        else
        {
            isGrounded = false;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    #endregion
    
    public override void Start()
    {
        base.Start();
        state =  SunWuKongState.Idle;
    }
    
    private void Update()
    {
        //是否接触到地面
        CheckGround();

        //是否进入奔跑状态
        DetectDoubleClick();

        #region 移动输入

        if (isOpenInputCheck && Input.GetKey(KeyCode.D))
        {
            ChangeMianChaoXiang(1);
            SetInputX(1);
        }

        if (isOpenInputCheck && Input.GetKey(KeyCode.A))
        {
            ChangeMianChaoXiang(-1);
            SetInputX(-1);
        }
        

        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D))
        {
            SetInputX(0);
        }

        if (state == SunWuKongState.Idle)
        {
            rigidbody2D.sharedMaterial =  IdleMaterial;
        }
        else
        {
            rigidbody2D.sharedMaterial = MoveMaterial;
        }

        #endregion

        #region 攻击

        if (isGrounded && Input.GetKeyDown(KeyCode.J))
        {
            NewAttckSkill(1000+attackCount);
            state = SunWuKongState.Attack;
        }

        #endregion


        switch (state)
        {
            case SunWuKongState.Idle:
                IdleState();
                break;
            case SunWuKongState.Walk:
                WalkState();
                break;
            case SunWuKongState.Run:
                SunRun();
                break;
            case SunWuKongState.Jump:
                JumpState();
                break;
            case SunWuKongState.Fall:
                FallState();
                break;
            case SunWuKongState.DoubleJump:
                DoubleJump();
                break;
            case SunWuKongState.Attack:
                SunAttackState();
                break;
        }

        #region 地面跳跃

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                resetJumpValues = false;
                state = SunWuKongState.Jump;
            }
        }

        #endregion

        #region 空中跳跃

        if (!doubleJump && !IsGrounded() && Input.GetKeyDown(KeyCode.K))
        {
            doubleJump = true;
            resetDoubleJumpValue = false;
            state = SunWuKongState.DoubleJump;
        }

        #endregion
        
    }

    // public override void Idel()
    // {
    //     base.Idel();
    //     state = SunWuKongState.Idle;
    // }

    /// <summary>
    /// 更改面朝向
    /// </summary>
    /// <param name="mianChaoXiang"></param>
    public void ChangeMianChaoXiang(int mianChaoXiang)
    {
        mMianChaoXiang = mianChaoXiang;

        if (mianChaoXiang ==-1 && transform.rotation.y!=0)
        {
            transform.eulerAngles = new Vector3(0,0,0);
        }
        else if(mianChaoXiang == 1 && !Mathf.Approximately(transform.rotation.y, 180))
        {
            transform.eulerAngles = new Vector3(0,180,0);
        }
    }

    /// <summary>
    /// 得到面朝向
    /// </summary>
    /// <returns></returns>
    public override int GetMianChaoXiang()
    {
        base.GetMianChaoXiang();
        return mMianChaoXiang;
    }
    
    
     #region 双击检测

    [Header("双击检测设置")]
    
    public bool isRunning = false;
    
    public float doubleClickATime = 0.5f; // 双击时间间隔
    public KeyCode AtKey = KeyCode.A; // 目标键位
    
    protected int clickACount = 0;
    protected float lastClickATime = 0f;
    
    public float doubleClickBTime = 0.5f; // 双击时间间隔
    public KeyCode BtKey = KeyCode.D; // 目标键位
    
    protected int clickBCount = 0;
    private float lastClickBTime = 0f;

    void DetectDoubleClick()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(AtKey))
            {
                clickACount++;
            
                if (clickACount == 1)
                {
                    lastClickATime = Time.time;
                }
            
                if (clickACount > 1 && Time.time - lastClickATime < doubleClickATime)
                {
                    //双击成功
                    isRunning = true;
                    clickACount = 0;
                }
                else if (Time.time - lastClickATime >= doubleClickATime)
                {
                    // 超时，重置计数
                    isRunning = false;
                    clickACount = 1;
                    lastClickATime = Time.time;
                }
            }

            if (Input.GetKeyDown(BtKey))
            {
                clickBCount++;

                if (clickBCount == 1)
                {
                    lastClickBTime = Time.time;
                }

                if (clickBCount>1&&Time.time - lastClickBTime < doubleClickBTime)
                {
                    isRunning = true;
                    clickBCount = 0;
                }
                else if (Time.time - lastClickBTime >= doubleClickBTime)
                {
                    isRunning = false;
                    clickBCount = 1;
                    lastClickBTime = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                //stateMachine.ChangeState<SunJumpState>();
            }
        }
        
    }

    #endregion
}