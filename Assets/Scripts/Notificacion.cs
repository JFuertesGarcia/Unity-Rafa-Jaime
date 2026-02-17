using System.Timers;
using UnityEngine;

public class Notificacion : MonoBehaviour
{   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {;
        this.gameObject.SetActive(true);
        GameObject[] a = GameObject.FindGameObjectsWithTag("InterfazNoti");
        foreach (GameObject b in a)
        {
            b.transform.eulerAngles = Vector3.left * 90;
        }
    }
    
    public void Notificar(string mensaje)
    {
        
    }
}
