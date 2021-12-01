using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    // Update is called once per frame
    public float timer = 2.0f;
    void Update()
    {
        if(StateController.transaction){
            timer -= Time.deltaTime;
            if(timer <= 0){
                StateController.transaction = false;
                StateController.halt = false;
                Destroy(gameObject);
            }
        }
    }
}
