using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Text[] itemLabel;
    public GameObject inventoryBox;
    public GameObject cursor;
    public bool toggle = false;
    int cursorPos = 0;
    Rigidbody2D pos;
    Vector2 temp;

    // Start is called before the first frame update
    void Start()
    {
        inventoryBox.SetActive(false);
        pos = cursor.GetComponent<Rigidbody2D>();
        temp.x = cursor.transform.position.x;
        temp.y = cursor.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!toggle)
            {
                toggle = true;
                StateController.halt = true;
                inventoryBox.SetActive(true);
                for (int i = 0; i < itemLabel.Length; i++)
                {
                    itemLabel[i].text = StateController.inventory[i].name;
                }
            }
            else
            {
                toggle = false;
                StateController.halt = false;
                inventoryBox.SetActive(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.S) && toggle)
        {
            if(cursorPos == itemLabel.Length - 1)
            {
                cursorPos = 0;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
            else
            {
                cursorPos++;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) && toggle)
        {
            if (cursorPos == 0)
            {
                cursorPos = itemLabel.Length - 1;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
            else
            {
                cursorPos--;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && toggle)
        {
            if (StateController.inventory[cursorPos].set)
            {
                if (StateController.invIndex > 0) { StateController.invIndex--;  }
                Debug.Log(StateController.invIndex);
                for (int i = cursorPos; i < StateController.inventory.Length-1; i++)
                {
                    int next = i + 1;
                    StateController.inventory[i].name = StateController.inventory[next].name;
                    StateController.inventory[i].type = StateController.inventory[next].type;
                    StateController.inventory[i].set = StateController.inventory[next].set;
                }
                for (int i = 0; i < itemLabel.Length; i++)
                {
                    itemLabel[i].text = StateController.inventory[i].name;
                }
            }
        }
    }
}
