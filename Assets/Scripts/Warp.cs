using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int sceneIndex = 0;
    public int warpNumber = 0;
    public bool dungeon;

    void Start()
    {
        StateController.stairsInit = true;
    }

    void Update()
    {
        if (StateController.stairsInit)
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.position = StateController.getStairs();
            StateController.stairsInit = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) { SceneManager.LoadScene(sceneIndex); }
    }
}
