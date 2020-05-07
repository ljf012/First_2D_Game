using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator Anim;
    protected Rigidbody2D rb;
    protected AudioSource deathAudio;

    public int health;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        deathAudio = GetComponent<AudioSource>();
    }


    public void Death()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        deathAudio.Play();
        Anim.SetTrigger("death");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("damage Taken!");
        if (health <= 0)
        {
            deathAudio.Play();
            Anim.SetTrigger("death");
            //Death();
        }
    }
}
