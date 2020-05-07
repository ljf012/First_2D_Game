using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator Anim;

    private float timeBtwAttack;//攻击间隔，随着时间减小，减到0就可以再攻击
    public float startTimeBtwAttack;//常量，用来重置攻击间隔时间

    public Transform attackPos;
    public LayerMask isEnemies;
    public float attackRange;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeBtwAttack <= 0)
        {
            //
            if (Input.GetKey(KeyCode.X))
            {
                Anim.SetBool("attacking", true);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++){
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                } 
            }
            else
            {
                Anim.SetBool("attacking", false);
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
