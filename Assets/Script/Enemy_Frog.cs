using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{

    //private Rigidbody2D rb;
    //private Animator Anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftpoint, rightpoint;

    private float leftx, rightx;
    public float speed, jumpforce;

    private bool Faceleft = true;

    // Start is called before the first frame update
    protected override void Start()//继承Enemy父类，Start重写
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        //Anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();//断绝子关系  
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        SwitchAnim();
    }

    void Movement()
    {

        if (Faceleft)//面向左侧
        {
            if (coll.IsTouchingLayers(ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpforce); //-speed向左
            }
                               
            //超过左边，掉头
            if (transform.position.x < leftx)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);//转向
                Faceleft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpforce);
            }

            //超过右边，掉头
            if (transform.position.x > rightx)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);//转向
                Faceleft = true;
            }
        }
        
        
    }


    void SwitchAnim()
    {
        if (Anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0.1)
            {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true); 
            }
        }
        if (coll.IsTouchingLayers(ground) && Anim.GetBool("falling"))
        {
            Anim.SetBool("falling", false);
        }
    }

}
