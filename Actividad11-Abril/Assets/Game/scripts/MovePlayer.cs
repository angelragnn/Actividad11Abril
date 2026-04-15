using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        float velocidadX = rb.linearVelocity.x;

        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (animator != null)
        {
            animator.SetFloat("movement", Mathf.Abs(velocidadX));
        }
    }
}