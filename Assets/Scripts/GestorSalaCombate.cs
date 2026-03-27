using System.Collections.Generic;
using UnityEngine;

public class GestorSalaCombate : MonoBehaviour
{
    [Header("Prefabs de enemigos")]
    public GameObject prefabSlime;
    public GameObject prefabMurcielago;
    public GameObject prefabTirador;
    public GameObject prefabGolem;
    public GameObject prefabSkull;

    [Header("Estado")]
    public bool combateIniciado = false;
    private readonly List<VidaEnemigo> enemigosVivos = new();
    
    [Header("Spawns")]
    public int minimoEnemigos = 2;
    public int maximoEnemigos = 5;
    
    [Header("FX de aparición")]
    public GameObject prefabPortalSpawn;
    public float tiempoAntesDeAparecer = 0.5f;
    public float tiempoVidaPortal = 0.8f;

    private Room salaBase;

    private void Awake()
    {
        salaBase = GetComponentInParent<Room>();
    }

    public void ActivarCombate()
    {
        if (combateIniciado) return;

        combateIniciado = true;
        CerrarPuertas();
        GenerarEnemigos();
    }

    private void GenerarEnemigos()
    {
        PuntoAparicionEnemigo[] puntos = salaBase.GetComponentsInChildren<PuntoAparicionEnemigo>();

        if (puntos.Length == 0)
        {
            AbrirPuertas();
            return;
        }
        
        int cantidad = Random.Range(minimoEnemigos, maximoEnemigos + 1);
        cantidad = Mathf.Min(cantidad, puntos.Length);

        List<PuntoAparicionEnemigo> lista = new List<PuntoAparicionEnemigo>(puntos);
        for (int i = 0; i < lista.Count; i++)
        {
            int j = Random.Range(i, lista.Count);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }

        for (int i = 0; i < cantidad; i++)
        {
            PuntoAparicionEnemigo punto = lista[i];

            GameObject prefab = ObtenerPrefab(punto.tipoEnemigo);
            if (prefab == null) continue;

            GameObject enemigo = Instantiate(prefab, punto.transform.position, Quaternion.identity, transform);

            VidaEnemigo vida = enemigo.GetComponent<VidaEnemigo>();
            if (vida != null) enemigosVivos.Add(vida);
        }

        if (enemigosVivos.Count == 0)
            AbrirPuertas();
    }

    private GameObject ObtenerPrefab(TipoEnemigo tipo)
    {
        switch (tipo)
        {
            case TipoEnemigo.Slime: return prefabSlime;
            case TipoEnemigo.Murcielago: return prefabMurcielago;
            case TipoEnemigo.Tirador: return prefabTirador;
            case TipoEnemigo.Golem: return prefabGolem;
            case TipoEnemigo.Skull: return prefabSkull;
            default: return null;
        }
    }

    public void NotificarMuerteEnemigo(VidaEnemigo enemigo)
    {
        enemigosVivos.Remove(enemigo);

        if (enemigosVivos.Count == 0)
            AbrirPuertas();
    }

    private void CerrarPuertas()
    {
        CambiarEstadoPuertas(EstadoPuerta.Cerrada);
    }

    private void AbrirPuertas()
    {
        CambiarEstadoPuertas(EstadoPuerta.Abierta);
    }

    private void CambiarEstadoPuertas(EstadoPuerta estado)
    {
        if (salaBase == null) return;

        Vector2Int[] direcciones =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int dir in direcciones)
        {
            Door puerta = salaBase.getDoor(dir);
            if (puerta != null && puerta.estado != EstadoPuerta.Pared)
            {
                puerta.estado = estado;
            }
        }
    }
}