using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Text money;
    public Text health;
    public Text belly;
    public Text level;

    // Start is called before the first frame update
    void Start()
    {
        money.text = ""+StateController.money;
        health.text = ""+StateController.health;
        belly.text = ""+StateController.belly;
        level.text = ""+StateController.floorNum;

    }

    // Update is called once per frame
    void Update()
    {
        money.text = "" + StateController.money;
        health.text = "" + StateController.health;
        belly.text = "" + StateController.belly;
        level.text = "" + StateController.floorNum;
    }
}
