using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
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
            if(StateController.items[id].type == 1)
            {
                StateController.money += StateController.items[id].value;
                Destroy(gameObject);
            }
            else
            {
                if (StateController.invIndex < StateController.inventory.Length)
                {
                    StateController.inventory[StateController.invIndex].id = id;
                    StateController.inventory[StateController.invIndex].attack = StateController.items[id].attack;
                    StateController.inventory[StateController.invIndex].uses = StateController.items[id].uses;
                    StateController.invIndex++;
                    Destroy(gameObject);
                }
            }
        }
    }
}
