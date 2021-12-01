using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            StateController.floorNum = 0;
            StateController.health = StateController.baseHealth;
            StateController.maxHealth = StateController.baseHealth;
            StateController.attack = StateController.baseAttack;
            StateController.belly = StateController.maxBelly;
            StateController.statusText = "";
            SceneManager.LoadScene("Dungeon"); 
        }
    }
}
