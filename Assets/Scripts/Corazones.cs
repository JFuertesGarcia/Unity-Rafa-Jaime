using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Corazones : MonoBehaviour
{
    private List<GameObject> corazones =  new List<GameObject>();
    private GameObject _jugador;
    private PlayerController _jugadorScript;
    private GameObject camara;
    void Start()
    {
        camara = GameObject.Find("Main Camera");
        _jugador = GameObject.FindGameObjectWithTag("Player");
        _jugadorScript = _jugador.GetComponent<PlayerController>();
        // Aquí ya puedes usar _jugadorScript.vidaActual, vidaMax, etc.
        _jugadorScript.vidaMax = 6;
    }

    void Update()
    {
        Actualizar(_jugadorScript.vidaActual,_jugadorScript.vidaMax);
        
        
    }

    void Actualizar(int vida, int vidaMax) 
{
    // Limpieza habitual
    for (int corazon = 0; corazon < corazones.Count; corazon++)
    {
        Destroy(corazones[corazon]);
    }
    corazones.Clear();

    int cuentavida = vida;
    int cuentavidamax = vidaMax;
    float cuentapos = 0;

    // Calculamos cuántos OBJETOS corazón necesitamos (redondeando hacia arriba)
    // Ejemplo: si vidaMax es 5, necesitamos 3 objetos (2 llenos, 1 medio)
    int cantidadDeObjetos = Mathf.CeilToInt(vidaMax / 2f);

    for (int i = 0; i < cantidadDeObjetos; i++)
    {
        GameObject nuevoCorazon = Instantiate(Resources.Load<GameObject>("Prefabs/Corazon"));
        nuevoCorazon.transform.SetParent(transform);
        corazones.Add(nuevoCorazon);

        Corazon corazonScript = nuevoCorazon.GetComponent<Corazon>(); 
        SpriteRenderer spriteRenderer = nuevoCorazon.GetComponent<SpriteRenderer>(); 
        
        float camx = camara.transform.position.x;
        float camy = camara.transform.position.y;
        nuevoCorazon.transform.position = new Vector3(cuentapos + camx - 11.5f, 6 + camy, -15);
        cuentapos += 1; 

        // LÓGICA DE SPRITES
        if (cuentavidamax >= 2) // ¿Es un contenedor completo?
        {
            if (cuentavida >= 2) {
                spriteRenderer.sprite = corazonScript.agarrarSprite(0); // Lleno
                cuentavida -= 2;
            } else if (cuentavida == 1) {
                spriteRenderer.sprite = corazonScript.agarrarSprite(1); // Medio lleno
                cuentavida = 0;
            } else {
                spriteRenderer.sprite = corazonScript.agarrarSprite(2); // Vacío
            }
            cuentavidamax -= 2;
        }
        else if (cuentavidamax == 1) // ¿Es un medio contenedor (capacidad impar)?
        {
            if (cuentavida >= 1) {
                spriteRenderer.sprite = corazonScript.agarrarSprite(3); // Medio contenedor lleno
                cuentavida -= 1;
            } else {
                spriteRenderer.sprite = corazonScript.agarrarSprite(4); // Medio contenedor vacío
            }
            cuentavidamax -= 1;
        }
    }
}
}