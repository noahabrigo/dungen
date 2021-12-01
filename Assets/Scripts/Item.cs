using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) {
            if(StateController.items[id].type == 1)
            {
                StateController.money += StateController.items[id].value;
                StateController.statusText = "You picked up " + StateController.items[id].name;
                StateController.eraseText = true;
                Destroy(gameObject);
            }
            else
            {
                if (StateController.invIndex < StateController.inventory.Length)
                {
                    float attackMult = StateController.items[id].attack * (1 + (StateController.lvlItemMult * StateController.floorNum));
                    StateController.inventory[StateController.invIndex].id = id;
                    StateController.inventory[StateController.invIndex].attack =  (int)attackMult;
                    StateController.inventory[StateController.invIndex].uses = StateController.items[id].uses;
                    StateController.invIndex++;
                    StateController.statusText = "You picked up " + StateController.items[id].name;
                    StateController.eraseText = true;
                    Destroy(gameObject);
                }else{
                    StateController.statusText = "Inventory is full";
                    StateController.eraseText = true;
                }
            }
        }
    }
}
