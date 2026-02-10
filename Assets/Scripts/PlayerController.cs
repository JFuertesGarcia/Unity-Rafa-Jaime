using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int Movement = Animator.StringToHash("movement");
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;
    public float fixedRotation = 0;
    
    public Animator animator;
    private Rigidbody2D _rb;
    private bool _enSuelo = true;

    void Start()
    {
        // Buscamos el Rigidbody2D
        _rb = GetComponent<Rigidbody2D>();

        // Verificación de seguridad
        if (_rb == null) {
            Debug.LogError("¡Oye! Falta el componente Rigidbody2D en este objeto.");
        }
        if (animator == null) {
            Debug.LogWarning("No has asignado el Animator en el Inspector.");
        }
    }

    void Update()
    {
        
        // Bloqueo rotation pa q no se gire
        
        float velocidadX = Input.GetAxis("Horizontal");
        
        // Usamos Mathf.Abs para que la animación detecte movimiento tanto a derecha como izquierda
        if(animator != null) {
            animator.SetFloat(Movement, Mathf.Abs(velocidadX * velocidad));
        }
        
        // Giro del personaje
        if (velocidadX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (velocidadX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        // Movimiento manual (Transform)
        transform.position += new Vector3(velocidadX * velocidad * Time.deltaTime, 0, 0);
        
        // --- LÓGICA DE SALTO ---
        if (Input.GetButtonDown("Jump") && _enSuelo)
        {
            if (_rb != null) {
                // Aplicamos la fuerza de salto
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, fuerzaSalto);
                _enSuelo = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject g = collision.gameObject;
        if (g.CompareTag("Ground") && 
            g.transform.position.y +g.transform.localScale.y<= transform.position.y)
        {
            _enSuelo = true;
        }
    }
}