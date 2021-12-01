using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDraw : MonoBehaviour
{
    struct Node
    {
        public int width;
        public int height;
        public int x;
        public int y;
        public int nearestX;
        public int nearestY;
        public bool touching;
        public bool enemies;
        public bool items;
        public bool shop;
        public bool spawn;
        public bool stairs;
    };

    public Vector2Int size;
    public int minNodes = 4;
    public int maxNodes = 10;
    public int minSize = 5;
    public int maxSize = 15;
    public int padding = 25;
    public Item[] item;
    public Enemy[] enemy;
    public GameObject shop;
    int numCoins = 0;
    int numGreen = 0;
    int numBlue = 0;
    int numRed = 0;
    int numDonut = 0;
    int numBurger = 0;
    int numPizza = 0;
    int numWoodSword = 0;
    int numPhantomSword = 0;
    int numSteelSword = 0;
    int numOberonSword = 0;
    int numGoldKnight = 0;

    public Vector3Int test = new Vector3Int(0,0,0);

    public TileBase[] tile = new TileBase[1];
    Texture2D texture;
    Tilemap tilemap;
    Color[] color = new Color[8] {
        new Color(0,0,0,0),         //  Blackness
        new Color(1,1,1,1),         //  Walkable area
        new Color(0,0,1,1),         //  Player Spawn
        new Color(0,1,0,1),         //  Stairs
        new Color(1,0,0,1),         //  Enemy
        new Color(1,1,0,1),         //  Item
        new Color(1,0,1,1),         //  Wall
        new Color(0,1,1,1)          //  Shop
    };

    // Start is called before the first frame update
    void Start()
    {
        //for(int i = 0; i < 50; i++){
            StateController.floorNum = StateController.floorNum + 1;
            StateController.texture = generateMap(minNodes, maxNodes, minSize, maxSize, padding);
            StateController.tilemap = GetComponent<Tilemap>();
            loadMap(StateController.texture, StateController.tilemap);
            Debug.Log("Coins: " + numCoins + " Green: " + numGreen + " Blue: " + numBlue + " Red: " + numRed + " Donut: " + numDonut + " Burger: " + numBurger + " Pizza: " + numPizza + " Wood Sword: " + numWoodSword + " Phantom Sword: " + numPhantomSword + " Steel Sword: " + numSteelSword + " Oberon Sword: " + numOberonSword + " Gold Knight: " + numGoldKnight);
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    void loadMap(Texture2D texture, Tilemap tilemap)
    {
        Vector3Int[] positions = new Vector3Int[size.x * size.y];
        TileBase[] tileArray = new TileBase[positions.Length];

        Color curPixel;
        int xPos = 0;
        int yPos = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            if (i % size.x == 0)
            {
                xPos = 0;
                yPos++;
            }
            else
            {
                xPos++;
            }

            positions[i] = new Vector3Int(xPos, yPos, 0);
            curPixel = texture.GetPixel(xPos, -yPos);

            if (curPixel == color[1])
            {
                tileArray[i] = tile[2];
                if (diceRoll(3)) { tileArray[i] = tile[3]; }
                if (diceRoll(6)) { tileArray[i] = tile[4]; }
                if (diceRoll(9)) { tileArray[i] = tile[5]; }
            }
            else if (curPixel == color[2])
            {
                tileArray[i] = tile[2];
                StateController.setSpawn(tilemap.GetCellCenterWorld(positions[i]).x, tilemap.GetCellCenterWorld(positions[i]).y);
                StateController.cell = tilemap.WorldToCell(tilemap.GetCellCenterWorld(positions[i]));
                StateController.nextCell = StateController.cell;
            }
            else if (curPixel == color[3])
            {
                tileArray[i] = tile[0];
                StateController.setStairs(tilemap.GetCellCenterWorld(positions[i]).x, tilemap.GetCellCenterWorld(positions[i]).y);
            }
            else if (curPixel == color[4])
            {
                tileArray[i] = tile[2];
                int enemyNum = 0;
                if(StateController.floorNum >= 0 && StateController.floorNum < 10){
                    if(diceRoll(3)){ enemyNum = 1; }
                }
                if(StateController.floorNum >= 10 && StateController.floorNum < 25){
                    if(diceRoll(2)){ enemyNum = 1; }
                }
                if(StateController.floorNum >= 25){
                    enemyNum = 1;
                }
                if(diceRoll(250)){ enemyNum = 2; numGoldKnight++; }
                Instantiate(enemy[enemyNum], tilemap.GetCellCenterWorld(positions[i]), Quaternion.identity);
            }
            else if (curPixel == color[5])
            {
                tileArray[i] = tile[2];
                int id = 0;
                int type = 1;
                if (diceRoll(5)) { type = 2; }
                if (diceRoll(10)) { type = 3; }
                if (type == 1)
                {
                    id = 1; //coin
                    //if (diceRoll(3)) { id = 1; numGreen++; }    //emerald
                    if (diceRoll(5)) { id = 2; numBlue++; }    //sapphire
                    if (diceRoll(10)) { id = 3; numRed++; }     //ruby
                    if (id == 0) { numCoins++; }
                }
                else if (type == 2)
                {
                    id = 4; // Donut
                    if (diceRoll(5)) { id = 5; numBurger++; }   //burger
                    if (diceRoll(15)) { id = 6; numPizza++; }   //pizza
                    if (id == 4) { numDonut++; }
                }
                else if (type == 3)
                {
                    id = 7;                                              //Wooden sword
                    if (diceRoll(10)) { id = 8; numPhantomSword++; }     //Phantom sword
                    if (diceRoll(20)) { id = 9; numSteelSword++; }       //Steel sword
                    if (diceRoll(50)) { id = 10; numOberonSword++; }     //Oberon's sword
                    if (id == 7) { numWoodSword++; }
                }
                Instantiate(item[id], tilemap.GetCellCenterWorld(positions[i]), Quaternion.identity);
            }
            else if (curPixel == color[6])
            {
                tileArray[i] = tile[8];
            }
            else if (curPixel == color[7])
            {
                tileArray[i] = tile[2];
                Instantiate(shop, tilemap.GetCellCenterWorld(positions[i]), Quaternion.identity);
            }
            else
            {
                tileArray[i] = tile[1];
                if (diceRoll(5))  { tileArray[i] = tile[7]; }
                if (diceRoll(10)) { tileArray[i] = tile[6]; }
            }
        }
        tilemap.SetTiles(positions, tileArray);
    }

    Texture2D generateMap(int minNodes, int maxNodes, int minSize, int maxSize, int padding)
    {
        Texture2D texture = new Texture2D(size.x,size.y);
        int numOfNodes = Random.Range(minNodes,maxNodes);
        Node[] node = new Node[numOfNodes];
        int playerSpawn = Random.Range(0, numOfNodes);
        int stairs = Random.Range(0, numOfNodes);
        int shop = Random.Range(0, numOfNodes);
        node[playerSpawn].spawn = true;
        node[stairs].stairs = true;
        if(StateController.diceRoll(5)){node[shop].shop = true;}

        //Node placement
        for(int i = 0; i < numOfNodes; i++)
        {
            node[i].x = Random.Range(padding, size.x-padding);
            node[i].y = Random.Range(padding, size.y-padding);
            node[i].width = Random.Range(minSize, maxSize);
            node[i].height = Random.Range(minSize, maxSize);
            node[i].nearestX = 255;
            node[i].nearestY = 255;
            node[i].touching = true;

            if (diceRoll(2)) { node[i].items = true; }
            if (!node[i].spawn && !node[i].shop && diceRoll(2)) { node[i].enemies = true; }
        }

        for(int i = 0; i < numOfNodes; i++)
        {
            while(node[i].touching == true){
            for(int j = 0; j < numOfNodes; j++){
                int compX = Mathf.Abs(node[i].x - node[j].x);
                int compY = Mathf.Abs(node[i].y - node[j].y);
                if(i != j){
                    if(compX < node[i].nearestX){ 
                            node[i].nearestX = compX;
                    }
                    if(compY < node[i].nearestY){ 
                            node[i].nearestY = compY;
                    }
                }
            }
            if (node[i].nearestX <= node[i].width && node[i].nearestY <= node[i].height){
                node[i].touching = true;
                node[i].x = node[i].x - (node[i].width - node[i].nearestX) - 2;
                node[i].y = node[i].y - (node[i].height - node[i].nearestY) - 2;
                node[i].nearestX = node[i].nearestX + (node[i].width-node[i].nearestX) + 2;
                node[i].nearestY = node[i].nearestY + (node[i].height-node[i].nearestY) + 2;
            }
            else{
                node[i].touching = false;
            }
            }
        }

        //Draw nodes to screen
        for (int i = 0; i < numOfNodes; i++)
        {
            for(int curY = 0; curY < node[i].height; curY++)
            {
                for (int curX = 0; curX < node[i].width; curX++)
                {
                    texture.SetPixel(node[i].x + curX, node[i].y + curY, color[1]);
                }
            }
                
        }

        //Drawing the paths
        for (int j = 0; j < numOfNodes-1; j++)
        {
            int join = j + 1;

            int pathX = 0;
            int pathY = 0;

            int joinX = 0;
            int joinY = 0;

            int pointX = 0;
            int pointY = 0;

            pathX = Random.Range(node[j].x + 1,node[j].x + node[j].width-1);
            pathY = Random.Range(node[j].y + 1, node[j].y + node[j].height-1);

            joinX = Random.Range(node[join].x + 1, node[join].x + node[join].width - 1);
            joinY = Random.Range(node[join].y + 1, node[join].y + node[join].height - 1);

            pointX = Random.Range(pathX,joinX);
            pointY = Random.Range(pathY,joinY);

            drawPath(pointX,pointY,pathX,pathY,texture);
            drawPath(pointX, pointY, joinX, joinY, texture);
        }
        

        for (int i = 0; i < numOfNodes; i++)
        {

            //Drawing items in a single node
            if (node[i].items) //If node.items is true, draw number of items to screen based on rarity.
            {
                int numItems = 1;
                if (diceRoll(2)) { numItems++;
                    if (diceRoll(2)) { numItems++;
                        if (diceRoll(3)) {numItems++;
                            if (diceRoll(5)) {numItems++; 
                            }
                        }
                    }
                }
                for(int j = 0; j < numItems; j++) //Drawing items pixels
                {
                        texture.SetPixel(node[i].x + Random.Range(0, node[i].width), node[i].y + Random.Range(0, node[i].height), color[5]);
                }
            }

            //Drawing enemies in a single node
            if(node[i].enemies) //If node.enemies is true, draw number of enemies to screen based on rarity.
            {
                int numEnemies = 1;
                if (diceRoll(2)) { numEnemies++;
                    if (diceRoll(3)) { numEnemies++;
                        if (diceRoll(4)) { numEnemies++;
                            if (diceRoll(4)) { numEnemies++; 
                            }
                        }
                    }
                }
                for(int j = 0; j < numEnemies; j++) //Drawing enemy pixels
                {
                        texture.SetPixel(node[i].x + Random.Range(0, node[i].width), node[i].y + Random.Range(0, node[i].height), color[4]);
                }
            }

            if (node[i].shop) //Drawing shop pixel
            {
                    texture.SetPixel(node[i].x + Random.Range(2, node[i].width-2), node[i].y + Random.Range(2, node[i].height-2), color[7]);
            }

            if (node[i].spawn) //Drawing spawn pixel
            {
                    texture.SetPixel(node[i].x + Random.Range(0, node[i].width), node[i].y + Random.Range(0, node[i].height), color[2]);
            }
            if (node[i].stairs) //Drawing stairs pixel
            {
                    texture.SetPixel(node[i].x + Random.Range(0, node[i].width), node[i].y + Random.Range(0, node[i].height), color[3]);
            }
        }
            return texture;
        }

    void drawPath(int fromX, int fromY, int toX, int toY, Texture2D texture)
    {
        int distX = toX - fromX;
        int distY = toY - fromY;
        int dirX = 0;
        int dirY = 0;
        if (distX < 0) { dirX = -1; }
        else { dirX = 1; }
        if (distY < 0) { dirY = -1; }
        else { dirY = 1; }
        distX = Mathf.Abs(distX);
        distY = Mathf.Abs(distY);

        for (int k = 0; k < distX; k++)
        {
            texture.SetPixel(fromX + dirX * k, fromY, color[1]);
        }
        for (int k = 0; k < distY; k++)
        {
            texture.SetPixel(fromX + dirX * distX, fromY + dirY * k, color[1]);
        }
    }

    bool diceRoll (int odds)
    {
        int num = Random.Range(0,odds);
        if (num == 0) { return true; }
        else { return false; }
    }
}