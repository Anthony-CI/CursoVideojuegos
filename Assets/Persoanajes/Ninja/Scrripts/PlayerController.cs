using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Desplazamiento();
        Saltar(); 

        

    }

    void Desplazamiento()
    {
        rb.linearVelocityX = 0;
        animator.SetInteger("EstadoAni", 0);
        //condicional para iniciar movimineto
        if (Input.GetKey(KeyCode.RightArrow)) //movimeinto ala derecha
        {
            rb.linearVelocityX = 5; //velocidad del movimiento
            sr.flipX = false;
            animator.SetInteger("EstadoAni", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) //movimeinto ala izquierda
        {
            rb.linearVelocityX = -5; //velocidad del movimiento
            sr.flipX = true;
            animator.SetInteger("EstadoAni", 1);
        }
    }

    void Saltar()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocityY = 10;
        }
    }

}
