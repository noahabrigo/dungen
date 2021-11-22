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
    Vector3 pos = new Vector3(0,0,0);

    RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        inventoryBox.SetActive(false);
        rect = cursor.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
            }
            else
            {
                cursorPos++;
            }
        }
        pos = rect.position;
        pos.y++;
    }
}
