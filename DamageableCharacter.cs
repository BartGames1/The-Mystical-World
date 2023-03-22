using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour, IDamageable
{   
    public GameObject healthText;
    public bool disableSimulation = false;

    public bool isInvincibillityEnabled = false;
    public float invincibillityTime = 0.25f;

    Animator animator;
    Rigidbody2D rb;
    Collider2D physicsCollider;

    bool isAlive = true;
    private float invincibillityTimeElapsed = 0f;

    public float Health{
        set {
            if(value < _health){
                animator.SetTrigger("hit");
                RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                textTransform.SetParent(canvas.transform);
            }

            _health = value;
            
            if(_health <= 0) {
                animator.SetBool("isAlive", false);
                Targetable = false;
            }
        }
        get {
            return _health;
        }
    }

    public bool Targetable { 
        get {
            return _targetable;
        } set {
            _targetable = value;

            if(disableSimulation) {
                rb.simulated = false;
            }

            physicsCollider.enabled = value;
        }
    }

    public bool Invincible { 
        get {
            return _invincible;
        } set {
            _invincible = value;

            if(_invincible == true) {
                invincibillityTimeElapsed = 0f;
            }
        }
    }


    public float _health = 5f;
    bool _targetable = true;
    bool _invincible = false;
    public void Start(){
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
    }


    public void OnHit(float damage) {
        if(!Invincible) {
            Health -= damage;

            if(isInvincibillityEnabled){
                Invincible = true;
            }

        }

        
    }

    public void OnHit(float damage, Vector2 knockback){

        if(!Invincible) {
            Health -= damage;
            rb.AddForce(knockback, ForceMode2D.Impulse);

            if(isInvincibillityEnabled){
                Invincible = true;
            }
        }

        
    }

    public void OnObjectDestroyed() {
        
        Destroy(gameObject);
    }

    public void FixedUpdate() {
        if(Invincible) {
            invincibillityTimeElapsed += Time.deltaTime;

            if(invincibillityTimeElapsed > invincibillityTime) {
                Invincible = false;
            }
        }
    }
}