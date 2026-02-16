using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private static readonly int Movement = Animator.StringToHash("movement");
    public float fuerzaSalto = 10f;

    [FormerlySerializedAs("_animator")] public Animator animator;
    private Rigidbody2D _rb;
    private bool _enSuelo = false;
    private bool atacando = false;
    private Coroutine attackCoroutine;

    public float velocidad = 5f;
    public int vidaMax;
    public int vidaActual;
    public float ataque;
    public float velocidadAtaque = 2f; // Tiempo que dura el ataque
    public float cooldownAtaque = 2f; // Tiempo extra de espera entre ataques
    private float tiempoSiguienteAtaque = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Falta Rigidbody2D");
    }

    void Update()
    {
        animator.SetFloat("VelocidadY", _rb.linearVelocity.y);
        animator.SetBool("enSuelo", _enSuelo);
        animator.SetBool("atacando", atacando);

        // Si quieres que se pueda mover mientras ataca, NO bloquees velocidadX aquí.
        float velocidadX = Input.GetAxis("Horizontal");

        if (animator != null)
        {
            animator.SetFloat(Movement, Mathf.Abs(velocidadX * velocidad));
        }

        // Movimiento siempre (si lo quieres así)
        if (velocidadX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (velocidadX < 0) transform.localScale = new Vector3(-1, 1, 1);

        transform.position += new Vector3(velocidadX * velocidad * Time.deltaTime, 0, 0);

        if (Input.GetButtonDown("Jump") && _enSuelo && !atacando)
        {
            animator.SetTrigger("Salto");
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, fuerzaSalto);
            _enSuelo = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && !atacando && Time.time >= tiempoSiguienteAtaque)
        {
            attackCoroutine = StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        atacando = true;
        animator.SetBool("atacando", true);

        Debug.Log($"[Attack] START t={Time.time:F3} velocidadAtaque={velocidadAtaque}");

        yield return new WaitForSeconds(velocidadAtaque);

        FinishAttack();
    }

    public void InterruptAttack()
    {
        if (atacando)
        {
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            FinishAttack();
        }
    }

    private void FinishAttack()
    {
        if (!atacando) return;

        atacando = false;
        animator.SetBool("atacando", false);
        attackCoroutine = null;

        tiempoSiguienteAtaque = Time.time + cooldownAtaque;

        Debug.Log($"[Attack] END   t={Time.time:F3} next={tiempoSiguienteAtaque:F3}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D punto in collision.contacts)
            {
                if (punto.normal.y > 0.5f) { _enSuelo = true; break; }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D punto in collision.contacts)
            {
                if (punto.normal.y > 0.5f) { _enSuelo = true; break; }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) _enSuelo = false;
    }
}