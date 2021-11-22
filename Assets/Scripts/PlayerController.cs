using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float speed = 5.0f;
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(0, -1);
    Vector2Int curCel = new Vector2Int(0,0);
    int move = 1;

    Animator animator;

    void Start()
    {
        StateController.playerInit = true;
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StateController.playerInit)
        {
            rigidbody2d.position = StateController.getSpawn();
            StateController.playerInit = false;
        }
        

        if (Input.GetKey(KeyCode.W))
        {
            horizontal = 0.0f;
            vertical = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            horizontal = 0.0f;
            vertical = -1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1.0f;
            vertical = 0.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1.0f;
            vertical = 0.0f;
        }
        else
        {
            vertical = 0.0f;
            horizontal = 0.0f;
        }

        Vector2 moveVector = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(moveVector.x, 0.0f) || !Mathf.Approximately(moveVector.y, 0.0f))
        {
            lookDirection.Set(moveVector.x, moveVector.y);
            lookDirection.Normalize();
        }

        if (move == 1)
        {
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", moveVector.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (StateController.halt)
        {
            move = 0;
            vertical = 0.0f;
            horizontal = 0.0f;
        }
        else
        {
            move = 1;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime * move;
        position.y = position.y + speed * vertical * Time.deltaTime * move;
        rigidbody2d.MovePosition(position);

        StateController.setPlayer(rigidbody2d.position.x, rigidbody2d.position.y);
    }
}
