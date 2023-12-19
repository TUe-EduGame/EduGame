using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{   
    public float moveSpeed;
    public Vector2 PlayerInput;
    public bool isMoving;
    public LayerMask solidObjectsLayer;
    private Vector3 targetPos = new Vector3(0.5f, 0.5f, 0);

    public Tilemap obstacles;

    void Start() {
        moveSpeed = 7f;
    }
    void Update()
    {

        if(!isMoving) {
            PlayerInput.x = Input.GetAxisRaw("Horizontal");
            PlayerInput.y = Input.GetAxisRaw("Vertical");

            if(PlayerInput.x != 0) PlayerInput.y = 0;

            if(PlayerInput != Vector2.zero) 
            {
                targetPos = transform.position + new Vector3(PlayerInput.x, PlayerInput.y, 0);
		
		Vector3Int obstacleMap = obstacles.WorldToCell(targetPos);
		if(obstacles.GetTile(obstacleMap) == null) {		
                StartCoroutine(Move(targetPos));
		}
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

    void OnDrawGizmos() {
	Gizmos.color = Color.red;
	Gizmos.DrawSphere(targetPos, 0.2f);
    }
}
