using System.Collections;
using TMPro;
using UnityEngine;

public class Notificacion : MonoBehaviour
{
    [Header("Referencia al TMP dentro de Notif > Canvas > Text (TMP)")]
    [SerializeField] private TMP_Text textoNotificacion;

    [Header("Animación")]
    [SerializeField] private float duracionRotacion = 0.2f;

    GameObject camara;
    
    private Coroutine rotacionCoroutine;

    private void Start()
    {
        camara = GameObject.Find("Main Camera");
        textoNotificacion.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
    
    private void OnUpdate()
    {
        textoNotificacion.transform.position = camara.transform.position+new Vector3(0,15,10);
    }

    public void Notificar(string mensaje)
    {
        textoNotificacion.text = mensaje;
        rotacionCoroutine = StartCoroutine(Mostrar(textoNotificacion.rectTransform, duracionRotacion));
    }

    private static IEnumerator Mostrar(RectTransform rt, float duracionCadaTramo)
    {
        Quaternion rot90 = Quaternion.Euler(90f, 0f, 0f);
        Quaternion rot0 = Quaternion.Euler(0f, 0f, 0f);

        // Empezamos "como antes"
        rt.localRotation = rot90;

        // 90 -> 0
        yield return RotarEntre(rt, rot90, rot0, duracionCadaTramo);

        // espera
        
        yield return new WaitForSeconds(2f);
        
        // 0 -> 90 (vuelta)
        yield return RotarEntre(rt, rot0, rot90, duracionCadaTramo);

        // Asegura que termina exactamente "como antes"
        rt.localRotation = rot90;
    }

    private static IEnumerator RotarEntre(RectTransform rt, Quaternion inicio, Quaternion fin, float duracion)
    {
        float t = 0f;
        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Clamp01(t / duracion);

            // Suavizado (ease in-out suave)
            float suave = a * a * (3f - 2f * a);

            rt.localRotation = Quaternion.SlerpUnclamped(inicio, fin, suave);
            yield return null;
        }

        rt.localRotation = fin;
    }
}