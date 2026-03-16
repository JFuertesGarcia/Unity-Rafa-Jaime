using System.Collections.Generic;
using UnityEngine;

public class GestorSalaCombate : MonoBehaviour
{
    [Header("Prefabs de enemigos")]
    public GameObject prefabSlime;
    public GameObject prefabMurcielago;
    public GameObject prefabTirador;

    [Header("Estado")]
    public bool combateIniciado = false;
    private readonly List<VidaEnemigo> enemigosVivos = new();

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
        PuntoAparicionEnemigo[] puntos = GetComponentsInChildren<PuntoAparicionEnemigo>();

        foreach (PuntoAparicionEnemigo punto in puntos)
        {
            GameObject prefab = ObtenerPrefab(punto.tipoEnemigo);
            if (prefab == null) continue;

            GameObject enemigo = Instantiate(prefab, punto.transform.position, Quaternion.identity, transform);
            VidaEnemigo vida = enemigo.GetComponent<VidaEnemigo>();

            if (vida != null)
                enemigosVivos.Add(vida);
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