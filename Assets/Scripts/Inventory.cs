using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Text[] itemLabel;
    public Text statType;
    public Text statUses;
    public Text statAttack;
    public Text statDesc;
    public GameObject inventoryBox;
    public GameObject cursor;
    public GameObject cursorHelper;
    bool toggle = false;
    int cursorPos = 0;
    Rigidbody2D pos;
    Vector2 temp;

    // Start is called before the first frame update
    void Start()
    {
        inventoryBox.SetActive(false);
        pos = cursor.GetComponent<Rigidbody2D>();
        temp.x = cursorHelper.transform.position.x;
        temp.y = cursor.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !StateController.inShop)
        {
            if (!toggle)
            {
                toggle = true;
                StateController.halt = true;
                inventoryBox.SetActive(true);
                UpdateInventory();
            }
            else
            {
                toggle = false;
                StateController.halt = false;
                inventoryBox.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) && toggle)
        {
            if (cursorPos == itemLabel.Length - 1)
            {
                cursorPos = 0;
                temp.x = cursorHelper.transform.position.x;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
            else
            {
                cursorPos++;
                temp.x = cursorHelper.transform.position.x;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
            UpdateInventory();
        }
        else if (Input.GetKeyDown(KeyCode.W) && toggle)
        {
            if (cursorPos == 0)
            {
                cursorPos = itemLabel.Length - 1;
                temp.x = cursorHelper.transform.position.x;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
            else
            {
                cursorPos--;
                temp.x = cursorHelper.transform.position.x;
                temp.y = itemLabel[cursorPos].transform.position.y;
                pos.position = temp;
            }
            UpdateInventory();
        }
        else if (Input.GetKeyDown(KeyCode.A) && toggle)
        {
            int itemType = StateController.items[StateController.inventory[cursorPos].id].type;
            int itemUses = StateController.inventory[cursorPos].uses;
            int itemBelly = StateController.items[StateController.inventory[cursorPos].id].belly;
            int itemHealth = StateController.items[StateController.inventory[cursorPos].id].health;
            if (itemType == 2)
            {
                if (StateController.belly + itemBelly > StateController.maxBelly) { StateController.belly = StateController.maxBelly;}
                else if (itemBelly == -1) { StateController.belly = StateController.maxBelly;}
                else { StateController.belly += itemBelly;}

                if(itemHealth == -1){
                    StateController.addHealth(StateController.maxHealth);
                }else{
                    StateController.addHealth(itemHealth);
                }
                
                itemUses--;
                if (itemUses <= 0)
                {
                    deleteInventory(cursorPos);
                    UpdateInventory();
                }
            }
            else if (itemType == 3)
            {
                StateController.equipped = cursorPos;
                UpdateInventory();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && toggle)
        {
            if (StateController.items[StateController.inventory[cursorPos].id].type > 0)
            {
                deleteInventory(cursorPos);
                UpdateInventory();
            }
        }
    }

    void UpdateInventory()
    {
        for (int i = 0; i < itemLabel.Length; i++)
        {
            string itemName = StateController.items[StateController.inventory[i].id].name;
            itemLabel[i].text = itemName;

            Color equipped = new Color(1, 1, 0, 1);
            Color unequipped = new Color(1, 1, 1, 1);
            if (i == StateController.equipped){itemLabel[i].color = equipped;}
            else{ itemLabel[i].color = unequipped; }
        }
        int inttype = StateController.items[StateController.inventory[cursorPos].id].type;
        int intuses = StateController.inventory[cursorPos].uses;
        int intattack = StateController.inventory[cursorPos].attack;
        string txttype;
        string txtuses;
        string txtattack;
        if (inttype == 2){txttype = "TYPE: FOOD";}
        else if(inttype == 3){txttype = "TYPE: WEAPON";}
        else{txttype = "TYPE: NONE";}

        if(intuses > 0) { txtuses = "USES: " + intuses; }
        else { txtuses = "USES: NONE"; }
        
        if(intattack > 0) { txtattack = "ATK: " + intattack; }
        else { txtattack = "ATK: NONE"; }
        
        string txtdesc = StateController.items[StateController.inventory[cursorPos].id].desc;
        statType.text = txttype;
        statUses.text = txtuses;
        statAttack.text = txtattack;
        statDesc.text = txtdesc;
    }

    public void deleteInventory(int index)
    {
        if (StateController.invIndex > 0) { 
            StateController.invIndex--;
            if (index == StateController.equipped) { StateController.equipped = -1; }
            if (index < StateController.equipped) { StateController.equipped--;  }
        }

        for (int i = index; i < StateController.inventory.Length - 1; i++)
        {
            int next = i + 1;
            StateController.inventory[i].id = StateController.inventory[next].id;
            StateController.inventory[i].attack = StateController.inventory[next].attack;
            StateController.inventory[i].uses = StateController.inventory[next].uses;
        }
        StateController.inventory[StateController.inventory.Length - 1].id = 0;
        StateController.inventory[StateController.inventory.Length - 1].attack = 0;
        StateController.inventory[StateController.inventory.Length - 1].uses = 0;
    }
}
