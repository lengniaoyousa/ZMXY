using UnityEngine;

public class SunController : Enity
{
    [HideInInspector] public StateMachine<State> stateMachine;  // 状态机实例
    [HideInInspector] public Animator animator; // 动画控制器
    [HideInInspector] public Rigidbody2D rigidbody2D;

    private int mMianChaoXiang = -1;
    


    #region 键盘输入

    private int mInputX = 0;

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
        Vector2 rayOrigin = (Vector2)transform.position + raycastOffset;
        
        // 向下发射射线
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);
        
        // 可视化射线（在Scene视图中显示）
        Debug.DrawRay(rayOrigin, Vector2.down * raycastDistance, hit.collider != null ? Color.green : Color.red);
        
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



    
    private void Start()
    {
        //获取组件引用
        animator = GetComponentInChildren<Animator>();
        
        //刚体
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        //创建状态机实例
        stateMachine = new StateMachine<State>();
        
        // 注册所有状态
        stateMachine.AddState(new SunState(this));
        stateMachine.AddState(new SunGroundState(this));
        stateMachine.AddState(new SunIdleState(this));
        stateMachine.AddState(new SunWalkState(this));
        stateMachine.AddState(new SunRunState(this));
        stateMachine.AddState(new SunJumpState(this));
        stateMachine.AddState(new SunFallState(this));
        stateMachine.AddState(new SunDoubleJump(this));
        stateMachine.AddState(new SunAttackState(this));
        
        //设置初始状态
        stateMachine.ChangeState<SunIdleState>();
    }
    
    private void Update()
    {
        // 5. 每帧更新状态机
        stateMachine.Update();

        CheckGround();
    }

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
    public int GetMianChaoXiang()
    {
        return mMianChaoXiang;
    }
}