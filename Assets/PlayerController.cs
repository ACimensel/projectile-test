using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    private int speed = 10;
    private Vector3 linePos;

    void Update(){
        Move();
        RotatePlayer();

        linePos = transform.position + transform.forward * 4f;
    }

    void Move(){ 
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += movement * speed * Time.deltaTime;
    }

    void RotatePlayer(){ 
        Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane( Vector3.up, transform.position);
        if( p.Raycast( mouseRay, out float hitDist) ){
            Vector3 hitPoint = mouseRay.GetPoint( hitDist );
            transform.LookAt( hitPoint );
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, linePos);
    }
}
