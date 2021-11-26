using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Text status;
    bool scroll = false;
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
}
