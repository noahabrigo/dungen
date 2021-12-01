using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRadius : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            StateController.statusText = "Buy somethin' will ya!";
            StateController.eraseText = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            StateController.statusText = "bye bye!";
            StateController.eraseText = true;
        }
    }
}
