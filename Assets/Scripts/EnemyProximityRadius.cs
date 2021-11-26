using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityRadius : MonoBehaviour
{
    public bool follow = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            follow = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            StateController.statusText = "";
            follow = false;
        }
    }
}
