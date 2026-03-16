using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [Header("Vida")]
    public float vidaMaxima = 10f;
    private float vidaActual;

    [Header("Referencias")]
    public Animator animador;
    private bool muerto = false;

    private void Awake()
    {
        vidaActual = vidaMaxima;

        if (animador == null)
            animador = GetComponent<Animator>();
    }

    public void RecibirDanio(float cantidad)
    {
        if (muerto) return;

        vidaActual -= cantidad;

        if (animador != null)
            animador.SetTrigger("Hit");

        if (vidaActual <= 0)
            Morir();
    }

    private void Morir()
    {
        if (muerto) return;
        muerto = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (animador != null)
            animador.SetTrigger("Die");

        GestorSalaCombate sala = GetComponentInParent<GestorSalaCombate>();
        if (sala != null)
            sala.NotificarMuerteEnemigo(this);

        Destroy(gameObject, 0.4f);
    }
}