using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player2Controller : MonoBehaviour
{
    /*[SerializeField]*/
    private Rigidbody2D Rb;
    private Animator Anim;

    public Collider2D coll; //碰撞体
    public Collider2D Discoll;

    public Transform cellingCheck;//人物头部
    public Transform groundCheck;//人物脚部

    public LayerMask ground;//地面
                            //public AudioSource jumpAudio, hurtAudio, cherryAudio;

    // 速度
    public float speed;
    public float jumpforce;

    //收集物
    public int Cherry;


    private bool isHurt;//受伤，默认false
    private bool isGround;//是否在地面
    private bool isJump;

    private int jumpCount;//二段跳
    bool jumpPress;

    //UI
    public Text CherryNum;


    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);

        if (!isHurt)
        {
            Movement();
        }
        newJump();
        SwitchAnim();

    }

    private void Update()
    {
        //Jump();
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPress = true;
        }

        //isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);
        //newJump();
        Crouch();
        CherryNum.text = Cherry.ToString();
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float face_dircetion = Input.GetAxisRaw("Horizontal");

        // 角色移动
        if (horizontalMove != 0)
        {
            Rb.velocity = new Vector2(horizontalMove * speed, Rb.velocity.y);
            Anim.SetFloat("running", Mathf.Abs(face_dircetion));
        }

        if (face_dircetion != 0)
        {
            transform.localScale = new Vector3(face_dircetion, 1, 1);
        }

    }


    // 角色跳跃
    void Jump()
    {
        if (Input.GetButton("Jump") && Anim.GetBool("idleing") && coll.IsTouchingLayers(ground))
        {
            //jumpAudio.Play();
            SoundManager.instance.JumpAudio();
            Rb.velocity = new Vector2(Rb.velocity.x, jumpforce * Time.fixedDeltaTime);
            Anim.SetBool("jumping", true);
            //Anim.SetBool("idleing", false);
        }
    }

    void newJump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPress && isGround)
        {
            isJump = true;
            SoundManager.instance.JumpAudio();
            Rb.velocity = Vector2.up * jumpforce;// = new Vector2(0,1)
            Anim.SetBool("jumping", true);
            jumpCount--;
            jumpPress = false;
        }
        else if (jumpPress && jumpCount > 0 && !isGround)
        {
            SoundManager.instance.JumpAudio();
            Rb.velocity = Vector2.up * jumpforce;
            Anim.SetBool("jumping", true);
            jumpCount--;
            jumpPress = false;
        }
    }

    //蹲下
    void Crouch()
    {

        if (Input.GetButton("Crouch"))
        {
            Anim.SetBool("crouching", true);
            Discoll.enabled = false;
        }
        else
        {
            if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
            {
                Anim.SetBool("crouching", false);
                Discoll.enabled = true;
            }
        }
    }

    // 切换动画
    void SwitchAnim()
    {
        Anim.SetBool("idleing", false);

        //普通落下
        if (Rb.velocity.y < -0.5f && !coll.IsTouchingLayers(ground))
        {
            Anim.SetBool("falling", true);
        }
        //跳跃落下
        if (Anim.GetBool("jumping"))
        {
            if (Rb.velocity.y < 0)
            {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            Anim.SetBool("falling", false);
            Anim.SetBool("idleing", true);
        }
        // 受伤
        if (isHurt)
        {
            Anim.SetBool("hurt", true);
            Anim.SetFloat("running", 0);
            if (Mathf.Abs(Rb.velocity.x) < 1f)
            {
                Anim.SetBool("hurt", false);
                Anim.SetBool("idleing", true);
                isHurt = false;
            }
        }
    }

    // 触发器
    private void OnTriggerEnter2D(Collider2D collision) //collision为被player撞到的物体
    {
        // 收集物品
        if (collision.tag == "Collection")
        {
            //cherryAudio.Play();
            SoundManager.instance.CherryAudio();
            //Destroy(collision.gameObject);
            //Cherry += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //CherryNum.text = Cherry.ToString();
        }

        //碰到死线
        if (collision.tag == "Deadline")
        {
            //GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    //消灭敌人&受伤
    private void OnCollisionEnter2D(Collision2D collision) //collision为被player撞到的物体
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (Anim.GetBool("falling") && transform.position.y > (collision.gameObject.transform.position.y + 1))
            {
                //Destroy(collision.gameObject);
                enemy.JumpOn();

                // 反弹一点
                Rb.velocity = new Vector2(Rb.velocity.x, jumpforce);
                Anim.SetBool("jumping", true);
            }
            // 在左边
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                Rb.velocity = new Vector2(-8, Rb.velocity.y);
                isHurt = true;
            }
            // 在右边
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                Rb.velocity = new Vector2(8, Rb.velocity.y);
                isHurt = true;
            }

        }
    }


    void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        Cherry += 1;
        //CherryNum.text = Cherry.ToString();
    }

}
