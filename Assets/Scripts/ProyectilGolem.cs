using UnityEngine;

public class ProyectilGolem : MonoBehaviour
{
    public float velocidad = 8f;
    public float tiempoVida = 3f;
    public int danio = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Disparar(Vector2 direccion)
    {
        if (rb == null) return;
        rb.linearVelocity = direccion.normalized * velocidad;
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            PlayerController jugador = otro.GetComponent<PlayerController>();
            if (jugador != null) jugador.RecibirDaño();
            Destroy(gameObject);
            return;
        }

        if (otro.CompareTag("Ground") || otro.CompareTag("Pared"))
        {
            Destroy(gameObject);
        }
    }
}