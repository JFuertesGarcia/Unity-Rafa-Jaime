using UnityEngine;
using System.Collections;

public class Golem : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo;
    public Transform puntoDisparo;
    public GameObject prefabProyectil;

    [Header("Ataque")]
    public float rangoDisparo = 8f;
    public float cooldownDisparo = 1.5f;
    public float tiempoTelegrapho = 0.25f;

    float siguienteDisparo = 0f;
    Animator animador;

    void Awake()
    {
        animador = GetComponent<Animator>();

        if (objetivo == null)
        {
            GameObject jugadorObj = GameObject.Find("JugadorObj");
            if (jugadorObj != null) objetivo = jugadorObj.transform;
        }

        if (puntoDisparo == null)
        {
            GameObject pd = new GameObject("PuntoDisparo");
            pd.transform.SetParent(transform);
            pd.transform.localPosition = new Vector3(0.6f, 0.2f, 0);
            puntoDisparo = pd.transform;
        }
    }

    void Update()
    {
        if (objetivo == null || prefabProyectil == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        // mirar al jugador
        float dirX = objetivo.position.x - transform.position.x;
        if (dirX > 0.05f) transform.localScale = new Vector3(1, 1, 1);
        else if (dirX < -0.05f) transform.localScale = new Vector3(-1, 1, 1);

        if (distancia > rangoDisparo) return;

        if (Time.time >= siguienteDisparo)
        {
            siguienteDisparo = Time.time + cooldownDisparo;
            StartCoroutine(DispararRutina());
        }
    }

    IEnumerator DispararRutina()
    {
        if (animador != null) animador.SetTrigger("disparar");
        yield return new WaitForSeconds(tiempoTelegrapho);

        if (objetivo == null) yield break;

        Vector2 direccion = (objetivo.position - puntoDisparo.position).normalized;

        GameObject instancia = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);
        ProyectilGolem proyectil = instancia.GetComponent<ProyectilGolem>();
        if (proyectil != null) proyectil.Disparar(direccion);
    }
}