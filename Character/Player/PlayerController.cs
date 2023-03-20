using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    bool IsMoving {
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }

    public float moveSpeed = 1000f;
    public float maxSpeed = 8f;
    public float idleFriction = 0.9f;
    
    
    public GameObject swordHitbox; 

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Collider2D swordCollider;
    Vector2 moveInput = Vector2.zero;

    bool isMoving = false;
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitbox.GetComponent<Collider2D>();
    }

    private void FixedUpdate(){
        if(canMove == true && moveInput != Vector2.zero) {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * moveSpeed *Time.deltaTime), maxSpeed);
            if(moveInput.x > 0) {
                spriteRenderer.flipX = false;
            } else if (moveInput.x < 0) {
                spriteRenderer.flipX = true;
            }

            IsMoving = true;

        } else {
            //rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

            IsMoving = false;
        }



    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }


    void OnFire() {
        animator.SetTrigger("swordAttack");
    }


    void LockMovement() {
        canMove = false;
    }
    void UnlockMovement(){
        canMove = true;
    }


}
