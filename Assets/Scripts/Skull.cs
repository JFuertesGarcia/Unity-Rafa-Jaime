using UnityEngine;

public class Calavera : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo; // jugador
    public Rigidbody2D rb;

    [Header("Movimiento")]
    public float velocidad = 3.5f;
    public float rangoDeteccion = 6f;
    public float distanciaFrenado = 1.0f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Auto-buscar jugador por nombre/tag
        if (objetivo == null)
        {
            GameObject jugadorObj = GameObject.Find("JugadorObj");
            if (jugadorObj != null) objetivo = jugadorObj.transform;
        }
    }

    private void FixedUpdate()
    {
        if (objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (distancia > rangoDeteccion)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (distancia < distanciaFrenado)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direccion = (objetivo.position - transform.position).normalized;
        rb.linearVelocity = direccion * velocidad;

        // Voltear sprite
        if (direccion.x > 0.05f) transform.localScale = new Vector3(1, 1, 1);
        else if (direccion.x < -0.05f) transform.localScale = new Vector3(-1, 1, 1);
    }
}