using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text[] itemLabel; //The text showing the item name in the list
    public GameObject shopBox; // The entire list containing the shop
    bool toggle = false;
    int cursorPos = 0;
    Rigidbody2D pos;
    Vector2 temp;
    public Text statType;
    public Text statUses;
    public Text statAttack;
    public Text statDesc;
    public Text statPrice;
    public GameObject cursor;
    public GameObject cursorHelper;
    // Start is called before the first frame update
    void Start()
    {
        shopBox.SetActive(false);
        resetShop();
        updateShop();
        pos = cursor.GetComponent<Rigidbody2D>();
        temp.x = cursorHelper.transform.position.x;
        temp.y = cursor.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && StateController.inShop == true){
            if(!toggle){
                toggle = true;
                shopBox.SetActive(true);
                StateController.halt = true;
            }else{
                toggle = false;
                shopBox.SetActive(false);
                StateController.halt = false;
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
            updateShop();
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
            updateShop();
        }
        else if (Input.GetKeyDown(KeyCode.A) && toggle)
        {
            if (StateController.invIndex < StateController.inventory.Length)
            {
                int itemMoney = StateController.items[StateController.shop[cursorPos].id].value;
                if(StateController.money >= itemMoney){
                    StateController.inventory[StateController.invIndex] = StateController.shop[cursorPos];
                    StateController.invIndex++;
                    StateController.money -= itemMoney;
                    StateController.statusText = "Thank you! Cya later!";
                    StateController.eraseText = true;
                    toggle = false;
                    StateController.transaction = true;
                    shopBox.SetActive(false);
                }else{
                    StateController.statusText = "You don't have enough gemstones!";
                    StateController.eraseText = true;
                }
            }else{
                StateController.statusText = "Your inventory is full!";
                StateController.eraseText = true;
            }
        }
    }

    void updateShop(){
        for(int i = 0; i < StateController.shop.Length; i++){
            itemLabel[i].text = StateController.items[StateController.shop[i].id].name;
        }
        int inttype = StateController.items[StateController.shop[cursorPos].id].type;
        int intuses = StateController.shop[cursorPos].uses;
        int intattack = StateController.shop[cursorPos].attack;
        int intprice = StateController.items[StateController.shop[cursorPos].id].value;
        string txttype;
        string txtuses;
        string txtattack;
        string txtprice = "Price: $" + intprice;
        if (inttype == 2){txttype = "TYPE: FOOD";}
        else if(inttype == 3){txttype = "TYPE: WEAPON";}
        else{txttype = "TYPE: NONE";}

        if(intuses > 0) { txtuses = "USES: " + intuses; }
        else { txtuses = "USES: NONE"; }
        
        if(intattack > 0) { txtattack = "ATK: " + intattack; }
        else { txtattack = "ATK: NONE"; }
        
        string txtdesc = StateController.items[StateController.shop[cursorPos].id].desc;
        statType.text = txttype;
        statUses.text = txtuses;
        statAttack.text = txtattack;
        statDesc.text = txtdesc;
        statPrice.text = txtprice;
    }

    void resetShop(){
        StateController.shop[0].id = 13;
        StateController.shop[1].id = 5;
        if(StateController.diceRoll(2)){ StateController.shop[1].id = 6; }
        if(StateController.diceRoll(3)){ StateController.shop[1].id = 7; }
        StateController.shop[2].id = 8;
        if(StateController.diceRoll(3)){ StateController.shop[2].id = 9; }
        if(StateController.diceRoll(5)){ StateController.shop[2].id = 10; }

        for(int i = 0; i < StateController.shop.Length; i++){
            float attack = StateController.items[StateController.shop[i].id].attack * (1 + ((StateController.floorNum + 5) * StateController.lvlItemMult));
            StateController.shop[i].uses = StateController.items[StateController.shop[i].id].uses;
            StateController.shop[i].attack = (int)attack;
        }
    }
}
