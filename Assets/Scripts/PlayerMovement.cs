using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public float moveSpeed;
    public Vector2 PlayerInput;
    public bool isMoving;

    void Start() {
        moveSpeed = 7f;
    }
    void Update()
    {
        
        if(!isMoving) {
            PlayerInput.x = Input.GetAxisRaw("Horizontal");
            PlayerInput.y = Input.GetAxisRaw("Vertical");
            if(PlayerInput != Vector2.zero) 
            {
                var targetPos = transform.position;
                targetPos.x += PlayerInput.x;
                targetPos.y += PlayerInput.y;

                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos) 
    {
        isMoving = true;
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) 
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

}
