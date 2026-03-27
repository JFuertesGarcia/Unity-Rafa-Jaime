using UnityEngine;

public class Skull : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo;
    public Rigidbody2D rb;

    [Header("Movimiento")]
    public float velocidad = 3.5f;
    public float rangoDeteccion = 6f;
    public float distanciaFrenado = 1.0f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

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

        if (distancia > rangoDeteccion || distancia < distanciaFrenado)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direccion = (objetivo.position - transform.position).normalized;
        rb.linearVelocity = direccion * velocidad;

        if (direccion.x > 0.05f) transform.localScale = new Vector3(1, 1, 1);
        else if (direccion.x < -0.05f) transform.localScale = new Vector3(-1, 1, 1);
    }
}