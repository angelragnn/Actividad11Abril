using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sprintMultiplier = 2f; 
    [SerializeField] private float jumpForce = 6f;

    [Header("Configuración de Suelo")]
    [SerializeField] private float longitudRaycast = 0.5f;
    [SerializeField] private LayerMask capaSuelo;

    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool enSuelo;
    private bool estaCorriendo; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    
    public void OnSprint(InputAction.CallbackContext context)
    {
        
        if (context.performed) estaCorriendo = true;
        if (context.canceled) estaCorriendo = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

        
        float velocidadActual = estaCorriendo ? speed * sprintMultiplier : speed;
        rb.linearVelocity = new Vector2(moveInput.x * velocidadActual, rb.linearVelocity.y);

        float velocidadX = rb.linearVelocity.x;

        
        if (velocidadX < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (velocidadX > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

       
        if (animator != null)
        {
            
            animator.SetFloat("movement", Mathf.Abs(velocidadX));
            animator.SetBool("ensuelo", enSuelo);
            animator.SetBool("estaCorriendo", estaCorriendo);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}