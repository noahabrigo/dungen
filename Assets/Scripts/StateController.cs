using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StateController : MonoBehaviour
{
    public struct Item {
        public int type;
        public string name;
    }

    public static Item[] inventory = new Item[10] { 
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" },
        new Item() { type = 0, name = "-----------------" }
    };
    public static int invIndex = 0;

    public static int money = 0;
    public static int belly = 0;
    public static int lvl = 1;
    public static int exp = 0;
    public static int floorNum = 0;
    public static int health = 10;

    public static bool halt = false;

    static Vector3 spawn = new Vector3(0,0);
    static Vector3 stairs = new Vector3(0, 0);
    static Vector3 cel = new Vector3(0, 0);
    static Vector3 player = new Vector3(0, 0, 0);
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

    public static void setCel(float x, float y)
    {
        cel.x = x;
        cel.y = y;
    }
    public static Vector3 getCel()
    {
        return cel;
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

}
