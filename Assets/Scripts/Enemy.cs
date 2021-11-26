using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int id;
    string name;
    int health;
    string attackName;
    int attack;
    string superName;
    int superAttack;
    bool dead;
    bool turn = false;
    float timer = 0.0f;
    float maxTimer = 3.0f;

    public EnemyAttackRadius attackRadius;
    public EnemyProximityRadius proximityRadius;
    Vector3 player = new Vector3(0, 0, 0);
    Vector3 enemy = new Vector3(0, 0, 0);
    Rigidbody2D rigidbody2d;

    void Start()
    {
        name = StateController.enemies[id].name;
        health = Random.Range(StateController.enemies[id].minHealth, StateController.enemies[id].maxHealth);
        attackName = StateController.enemies[id].attack;
        attack = Random.Range(StateController.enemies[id].minAttack, StateController.enemies[id].maxAttack);
        superName = StateController.enemies[id].attack;
        superAttack = Random.Range(StateController.enemies[id].minSuper, StateController.enemies[id].maxSuper);

        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (proximityRadius.follow && !attackRadius.attack && !StateController.halt)
        {
            player = StateController.getPlayerInt();
            enemy = rigidbody2d.position;
            rigidbody2d.MovePosition(Vector3.MoveTowards(enemy, player, 0.05f));
            StateController.statusText = "You encountered " + name + ".";
        }
        else if (attackRadius.attack)
        {
            StateController.halt = true;

            if(timer <= 0.0f){
            if(dead){ 
                Destroy(gameObject);
                StateController.halt = false;
                StateController.statusText = "";
            }else{
                int attackChoiceDmg = attack;
                string attackChoiceName = attackName;
                if(StateController.diceRoll(25)){
                    attackChoiceDmg = superAttack;
                    attackChoiceName = superName;
                }
                if(turn){
                    StateController.takeDamage(attack);
                    StateController.statusText = name + " used " + attackChoiceName + ". You took -" + attackChoiceDmg + " DMG";
                    turn = false;
                }
                else{
                    if(StateController.equipped == -1){
                        takeDamage(StateController.attack);
                        StateController.statusText =  "You used bare claws. " + name + " took " + StateController.attack + " damage.";
                    }else{
                        int atk = StateController.inventory[StateController.equipped].attack;
                        takeDamage(atk);
                        StateController.inventory[StateController.equipped].uses--;
                        if(StateController.inventory[StateController.equipped].uses == 0){
                            StateController.statusText = name + " took " + atk + " damage. " + StateController.items[StateController.inventory[StateController.equipped].id].name + " broke.";
                            StateController.deleteInventory(StateController.equipped);
                        }else{
                            StateController.statusText = "You used " + StateController.items[StateController.inventory[StateController.equipped].id].name + ". " + name + " took " + atk + " damage.";
                        }
                        
                    }
                    turn = true;
                }
            }
            }
        }

        if(timer <= 0.0f){
            timer = maxTimer;
        }
    }

    void takeDamage(int amount){
        if(amount > health){
            health = 0;
            dead = true;
        }else{
            health -= amount;
        }
    }
}
