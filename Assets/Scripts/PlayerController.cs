using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;

    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float velocidadX = Input.GetAxis("Horizontal");
        
        animator.SetFloat("movement", velocidadX*velocidad);
        
        if (velocidadX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (velocidadX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        Vector3 posicion = transform.position;
        transform.position = new Vector3(posicion.x + velocidadX * velocidad * Time.deltaTime, posicion.y, posicion.z);
    }
}
