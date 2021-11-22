using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int type; // (1) money, (2) food, (3) weapon
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) {
            if (StateController.invIndex < StateController.inventory.Length)
            {
                StateController.inventory[StateController.invIndex].type = type;
                StateController.inventory[StateController.invIndex].name = name;
                StateController.invIndex++;
                Destroy(gameObject);
            }
        }
    }
}
