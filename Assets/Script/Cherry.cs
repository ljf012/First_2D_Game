using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void Death()
    {
        //FindObjectOfType<PlayerController>().CherryCount();
        FindObjectOfType<Player2Controller>().CherryCount();
        Destroy(gameObject);
    }
}
