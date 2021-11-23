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
        public int numEnemies;
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

    public Vector3Int test = new Vector3Int(0,0,0);

    public TileBase[] tile = new TileBase[1];
    Texture2D texture;
    Tilemap tilemap;
    Color[] color = new Color[7] {
        new Color(0,0,0,0),         //  Blackness
        new Color(1,1,1,1),         //  Walkable area
        new Color(0,0,1,1),         //  Player Spawn
        new Color(0,1,0,1),         //  Stairs
        new Color(1,0,0,1),         //  Enemy
        new Color(1,1,0,1),         //  Item
        new Color(1,0,1,1)          //  Wall
    };

    // Start is called before the first frame update
    void Start()
    {
        StateController.floorNum = StateController.floorNum + 1;
        texture = generateMap(minNodes, maxNodes, minSize, maxSize, padding);
        tilemap = GetComponent<Tilemap>();
        loadMap(texture, tilemap);
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
            }
            else if (curPixel == color[3])
            {
                tileArray[i] = tile[0];
                StateController.setStairs(tilemap.GetCellCenterWorld(positions[i]).x, tilemap.GetCellCenterWorld(positions[i]).y);
            }
            else if (curPixel == color[5])
            {
                tileArray[i] = tile[2];
                Instantiate(item[Random.Range(0,11)], tilemap.GetCellCenterWorld(positions[i]),Quaternion.identity);
            }
            else if(curPixel == color[6])
            {
                tileArray[i] = tile[8];
            }
            else
            {
                tileArray[i] = tile[1];
                if (diceRoll(5)) { tileArray[i] = tile[7]; }
                if (diceRoll(10)) { tileArray[i] = tile[6]; }
            }
        }
        tilemap.SetTiles(positions, tileArray);
        //init = true;
    }

    Texture2D generateMap(int minNodes, int maxNodes, int minSize, int maxSize, int padding)
    {
        Texture2D texture = new Texture2D(size.x,size.y);
        int numOfNodes = Random.Range(minNodes,maxNodes);
        Node[] node = new Node[numOfNodes];
        int playerSpawn = Random.Range(0, numOfNodes);
        int stairs = Random.Range(0, numOfNodes);
        node[playerSpawn].spawn = true;
        node[stairs].stairs = true;

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

            if (diceRoll(4)) {node[i].items = true;}
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
            if(node[i].items)
            {
                int numItems = 1;
                if (diceRoll(3)) { 
                    numItems++;
                    if (diceRoll(4)) { 
                        numItems++;
                        if (diceRoll(4)) {
                            numItems++;
                            if (diceRoll(5)) {
                                numItems++; 
                            }
                        }
                    }
                }
                for(int j = 0; j < numItems; j++)
                {
                    texture.SetPixel(node[i].x + Random.Range(0, node[i].width), node[i].y + Random.Range(0, node[i].height), color[5]);
                }
            }
            if (node[i].spawn)
            {
                    texture.SetPixel(node[i].x + Random.Range(0, node[i].width), node[i].y + Random.Range(0, node[i].height), color[2]);
            }
            if (node[i].stairs)
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
