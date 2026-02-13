using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private static readonly int Movement = Animator.StringToHash("movement");
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;
    public float fixedRotation = 0;
    
    [FormerlySerializedAs("_animator")] public Animator animator;
    private Rigidbody2D _rb;
    private bool _enSuelo = false;
    private bool atacando = false;
    private Coroutine attackCoroutine;

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
        animator.SetFloat("VelocidadY", _rb.linearVelocity.y);
        animator.SetBool("enSuelo", _enSuelo);
        animator.SetBool("atacando",atacando);
        
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
                animator.SetTrigger("Salto");
                // Aplicamos la fuerza de salto
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, fuerzaSalto);
                _enSuelo = false;
            }
        }

        if (Input.GetKey(KeyCode.E) && !atacando)
        {
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            
            attackCoroutine = StartCoroutine(PerformAttack());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject g = collision.gameObject;
        if (g.CompareTag("Ground") && 
            g.transform.position.y<= transform.position.y)
        {
            foreach (ContactPoint2D punto in collision.contacts)
            {
                // La "Normal" es la dirección hacia donde apunta la cara del objeto chocado.
                // Si la normal apunta hacia arriba (y > 0.5), es que estamos pisando una superficie plana.
                if (!(punto.normal.y > 0.5f)) continue;
                _enSuelo = true;
                break;
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D punto in collision.contacts)
            {
                // Si detectamos un punto de apoyo válido, estamos en el suelo
                if (punto.normal.y > 0.5f)
                {
                    _enSuelo = true;
                    break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject g = collision.gameObject;
        if (g.CompareTag("Ground"))
        {
            _enSuelo = false;
        }
    }
    
    IEnumerator PerformAttack()
    {
        atacando = true;
        animator.SetBool("atacando", true); // Asegúrate de tener este parámetro en tu Animator

        // Esperar exactamente 1 segundo
        yield return new WaitForSeconds(0.30f);

        // Volver al estado normal después del segundo
        FinishAttack();
    }
    public void InterruptAttack()
    {
        if (atacando)
        {
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            FinishAttack();
            Debug.Log("Ataque interrumpido");
        }
    }

    private void FinishAttack()
    {
        atacando = false;
        animator.SetBool("atacando", false);
        attackCoroutine = null;
    }
}