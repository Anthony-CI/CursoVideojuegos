using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = 0;
        //condicional para iniciar movimineto
        if (Input.GetKey(KeyCode.RightArrow)) //movimeinto ala derecha
        {
            rb.linearVelocityX = 5; //velocidad del movimiento
        }
        if (Input.GetKey(KeyCode.LeftArrow)) //movimeinto ala izquierda
        {
            rb.linearVelocityX = -5; //velocidad del movimiento
        }

    }
}
