using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StateController : MonoBehaviour
{
    public static Texture2D texture;
    public static Tilemap tilemap;



    public struct InventoryItem {
        public int id;
        public int attack;
        public int uses;
    }

    public struct Items
    {
        public int id;
        public int type;
        public int value;
        public int attack;
        public int uses;
        public int health;
        public int belly;
        public string name;
        public string desc;
    }

    public struct Enemy
    {
        public int id;
        public string name;
        public int maxHealth;
        public int minHealth;
        public string attack;
        public int maxAttack;
        public int minAttack;
        public string superAttack;
        public int maxSuper;
        public int minSuper;
    }

    public static InventoryItem[] inventory = new InventoryItem[10] { 
        new InventoryItem() { id = 8, attack = 20, uses = 10},
        new InventoryItem() { id = 9, attack = 100, uses = 8},
        new InventoryItem() { id = 5, attack = 0, uses = 1},
        new InventoryItem() { id = 6, attack = 0, uses = 1},
        new InventoryItem() { id = 7, attack = 0, uses = 1},
        new InventoryItem() { id = 11, attack = 300, uses = 40},
        new InventoryItem() { id = 0, attack = 0, uses = 0},
        new InventoryItem() { id = 0, attack = 0, uses = 0},
        new InventoryItem() { id = 0, attack = 0, uses = 0},
        new InventoryItem() { id = 0, attack = 0, uses = 0}
    };
    public static int invIndex = 6;

    public static Items[] items = new Items[12] {
        new Items() { id = 0, type = 0, value = 0, attack = 0, uses = 0, health = 0, belly = 0, name = "Empty", desc = "Nothing to see here."},
        new Items() { id = 1, type = 1, value = 1, attack = 0, uses = 0, health = 0, belly = 0, name = "Gold Coin", desc = ""},
        new Items() { id = 2, type = 1, value = 5, attack = 0, uses = 0, health = 0, belly = 0, name = "Emerald", desc = ""},
        new Items() { id = 3, type = 1, value = 10, attack = 0, uses = 0, health = 0, belly = 0, name = "Sapphire", desc = ""},
        new Items() { id = 4, type = 1, value = 20, attack = 0, uses = 0, health = 0, belly = 0, name = "Ruby", desc = ""},
        new Items() { id = 5, type = 2, value = 5, attack = 0, uses = 1, health = 0, belly = 10, name = "Odin's Donut", desc = "It is said that this is the daily breakfast of Odin. +10 belly"},
        new Items() { id = 6, type = 2, value = 10, attack = 0, uses = 1, health = 0, belly = 25, name = "Magic Burger", desc = "Nobody knows the origins of this magical sandwich of german origin. +25 belly"},
        new Items() { id = 7, type = 2, value = 20, attack = 0, uses = 1, health = 0, belly = 50, name = "Nero's Pizza", desc = "The last remnants of pizza from Nero Caesar's reign. +FULL belly"},
        new Items() { id = 8, type = 3, value = 10, attack = 10, uses = 20, health = 0, belly = 0, name = "Wooden Sword", desc = "This sword was made from the pegleg of dead pirates."},
        new Items() { id = 9, type = 3, value = 25, attack = 10, uses = 15, health = 0, belly = 0, name = "Phantom Sword", desc = "Magical sword that sends out beams of pure energy."},
        new Items() { id = 10, type = 3, value = 50, attack = 15, uses = 30, health = 0, belly = 0, name = "Steel Sword", desc = "Sword made from the strongest of steel."},
        new Items() { id = 11, type = 3, value = 125, attack = 25, uses = 50, health = 0, belly = 0, name = "Oberon's Sword", desc = "One of the most rarest and highest caliber swords."}
    };

    public static Enemy[] enemies = new Enemy[2] {
        new Enemy() {id = 0, name = "", maxHealth = 0, minHealth = 0, attack = "", maxAttack = 0, minAttack = 0, superAttack = "", maxSuper = 0, minSuper = 0},
        new Enemy() {id = 1, name = "Lil' Knight", maxHealth = 16, minHealth = 9, attack = "Butterknife", maxAttack = 10, minAttack = 5, superAttack = "Serrated Blade", maxSuper = 20, minSuper = 10}
    };

    public static int money = 0;
    public static int floorNum = 0;
    public static int equipped = -1;
    public static int health = 25;
    public static int maxHealth = 50;
    public static int belly = 10;
    public static int maxBelly = 50;
    public static int attack = 15;
    public static bool halt = false;
    public static bool dead = false;
    public static string statusText;

    public static float lvlHealthMult = 0.11f;
    public static float lvlAttackMult = 0.08f;

    public static Vector3 spawn = new Vector3(0, 0, 0);
    public static Vector3 stairs = new Vector3(0, 0, 0);
    public static Vector3Int cell = new Vector3Int(0, 0, 0);
    public static Vector3Int nextCell = new Vector3Int(0, 0, 0);
    public static Vector3 player = new Vector3(0, 0, 0);
    public static bool playerInit = true;
    public static bool stairsInit = true;

    public static void setSpawn(float x, float y)
    {
        spawn.x = x;
        spawn.y = y;
    }
    public static Vector3 getSpawn()
    {
        return spawn;
    }

    public static void setStairs(float x, float y)
    {
        stairs.x = x;
        stairs.y = y;
    }
    public static Vector3 getStairs()
    {
        return stairs;
    }

    public static void setCell(int x, int y)
    {
        cell.x = x;
        cell.y = y;
    }
    public static Vector3Int getCell()
    {
        return cell;
    }

    public static void setPlayer(float x, float y)
    {
        player.x = x;
        player.y = y;
    }
    public static Vector3 getPlayer()
    {
        return player;
    }
    public static Vector3Int getPlayerInt()
    {
        Vector3Int temp = new Vector3Int((int)player.x,(int)player.y,0);
        return temp;
    }

    public static void takeDamage(int amount){
        if(amount > health){
            health = 0;
            dead = true;
            Debug.Log("YOU LOSE.");
        }else{
            health -= amount;
        }
    }

    public static void addHealth(int amount){
        if(amount + health >= maxHealth){
            health = maxHealth;
        }else{
            health += amount;
        }
    }
    public static void addBelly(int amount){
        if(amount + belly >= maxBelly){
            health = maxBelly;
        }else{
            belly += amount;
        }
    }

    public static void takeBelly(int amount){
        if(amount > belly){
            belly = 0;
        }else{
            belly -= amount;
        }
    }

    public static bool diceRoll (int odds)
    {
        int num = Random.Range(0,odds);
        if (num == 0) { return true; }
        else { return false; }
    }

    public static void deleteInventory(int index)
    {
        if (invIndex > 0) { 
            invIndex--;
            if (index == equipped) { equipped = -1; }
            if (index < equipped) { equipped--;  }
        }

        for (int i = index; i < inventory.Length - 1; i++)
        {
            int next = i + 1;
            inventory[i].id = inventory[next].id;
            inventory[i].attack = inventory[next].attack;
            inventory[i].uses = inventory[next].uses;
        }
        inventory[inventory.Length - 1].id = 0;
        inventory[inventory.Length - 1].attack = 0;
        inventory[inventory.Length - 1].uses = 0;
    }

}
