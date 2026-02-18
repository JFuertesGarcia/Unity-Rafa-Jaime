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
    
    Notificacion notificacion;

    public float velocidad = 5f;
    public int vidaMax = 6;
    public int vidaActual;
    public float ataque = 5f;
    public float velocidadAtaque = 2f;
    public float cooldownAtaque = 2f;
    private float tiempoSiguienteAtaque = 0;
    public float tiempoInmunidad = 1.5f;
    public float tiempoStun = 0.5f;
    private float timerInmunidad;
    private float timerStun;
    private bool esInmune = false;

    void Start()
    {
        vidaActual = vidaMax;
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Falta Rigidbody2D");
        notificacion = GameObject.Find("Notif").GetComponent<Notificacion>();
    }
    
    void Update()
    {
        if (timerInmunidad > 0) timerInmunidad -= Time.deltaTime;
        else esInmune = false;

        if (timerStun > 0) timerStun -= Time.deltaTime;
        
        animator.SetFloat("VelocidadY", _rb.linearVelocity.y);
        animator.SetBool("enSuelo", _enSuelo);
        animator.SetBool("atacando", atacando);

        // Si quieres que se pueda mover mientras ataca, NO bloquees velocidadX aquí.
        float velocidadX = Input.GetAxis("Horizontal");

        if (animator != null)
        {
            animator.SetFloat(Movement, Mathf.Abs(velocidadX * velocidad));
        }
        
        if (timerStun <= 0){
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

        if (collision.gameObject.CompareTag("Enemigo"))
        {
            RecibirDaño();
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

    public void RecibirDaño()
    {
        // Si somos inmunes, salimos de la función sin hacer nada
        if (esInmune) return;

        vidaActual--;
        Debug.Log("Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Debug.Log("Jugador Muerto");
            // Aquí llamarías a tu lógica de muerte/game over
            return;
        }

        // Activamos Inmunidad
        esInmune = true;
        timerInmunidad = tiempoInmunidad;

        // Activamos Stun (Bloqueo de movimiento)
        timerStun = tiempoStun;
        
        // Lanzamos el trigger para la animación de daño
        animator.SetTrigger("Daño"); 

        // Si estaba atacando, interrumpimos el ataque
        InterruptAttack();
    }

    public void recibirMejora(Mejoras mejora)
    {
        switch (mejora)
        {
            case Mejoras.Velocidad: velocidad += 2; break;
            case Mejoras.VelocidadAtaque: velocidadAtaque += 5; break;
            case Mejoras.Fuerza: ataque += 1; break;
            case Mejoras.Vida: vidaMax += 2; vidaActual += 2; break;
        }
        notificacion.Notificar($"{mejora.ToString().ToUpper()} +");
    }
}