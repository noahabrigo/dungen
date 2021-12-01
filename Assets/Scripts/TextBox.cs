using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Text status;
    bool scroll = false;
    float timer = 3.0f;
    float maxTimer = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        status.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(!scroll){
            status.text = StateController.statusText;
        }
    }

    void FixedUpdate(){
        if(StateController.eraseText){
            timer -= Time.deltaTime;
            if(timer <= 0.0f){
                StateController.statusText = "";
                StateController.eraseText = false;
                timer = maxTimer;
            }
        }
    }
}
