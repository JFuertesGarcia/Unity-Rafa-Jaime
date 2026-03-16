using UnityEngine;

public class SlimeSaltador : MonoBehaviour
{
    [Header("Referencias")]
    public Rigidbody2D rb;
    public Animator animador;
    public Transform jugador;
    public Transform comprobadorSuelo;
    public LayerMask capaSuelo;

    [Header("Salto")]
    public float fuerzaSalto = 6f;
    public float fuerzaHorizontal = 2f;
    public float tiempoEntreSaltos = 2f;
    public float radioComprobacionSuelo = 0.15f;
    public float rangoActivacion = 6f;

    private float temporizadorSalto;
    private bool estaEnSuelo;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animador == null) animador = GetComponent<Animator>();
    }

    private void Start()
    {
        if (jugador == null)
        {
            GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
            if (objJugador != null)
                jugador = objJugador.transform;
        }

        temporizadorSalto = tiempoEntreSaltos;
    }

    private void Update()
    {
        ComprobarSuelo();

        if (animador != null)
            animador.SetBool("isJumping", !estaEnSuelo);

        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia > rangoActivacion) return;

        temporizadorSalto -= Time.deltaTime;

        if (temporizadorSalto <= 0f && estaEnSuelo)
        {
            Saltar();
            temporizadorSalto = tiempoEntreSaltos;
        }
    }

    private void ComprobarSuelo()
    {
        if (comprobadorSuelo == null) return;

        estaEnSuelo = Physics2D.OverlapCircle(
            comprobadorSuelo.position,
            radioComprobacionSuelo,
            capaSuelo
        );
    }

    private void Saltar()
    {
        if (jugador == null) return;

        float direccionX = Mathf.Sign(jugador.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(direccionX * fuerzaHorizontal, fuerzaSalto);

        if (animador != null)
            animador.SetTrigger("Ability");
    }

    private void OnDrawGizmosSelected()
    {
        if (comprobadorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(comprobadorSuelo.position, radioComprobacionSuelo);
        }
    }
}