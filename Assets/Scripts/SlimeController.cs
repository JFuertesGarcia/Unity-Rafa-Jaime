using UnityEngine;

public class SlimeAnimatorDriver : MonoBehaviour
{
    [Header("Refs")]
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Ground check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;

    // Animator parameter names
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Ability = Animator.StringToHash("Ability");

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool onGround = false;

        if (groundCheck != null)
            onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // isJumping = NOT on ground (simple)
        if (animator) animator.SetBool(IsJumping, !onGround);

        // Test rápido con teclas (para probar anims sin IA)
        if (Input.GetKeyDown(KeyCode.H) && animator) animator.SetTrigger(Hit);
        if (Input.GetKeyDown(KeyCode.J) && animator) animator.SetTrigger(Ability);
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
