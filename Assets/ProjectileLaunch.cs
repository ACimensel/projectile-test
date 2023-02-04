using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    public Transform projectile;
    public Transform target;
    public Camera playerCamera;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    [Range(1f, 8f)] public float minDist = 4f;
    
    [HideInInspector] public Vector3 targetPos;
    private Vector3 linePos;
    private int speed = 10;
    private Coroutine coroutine;
    
    void Start(){          
        coroutine = StartCoroutine(Simulateprojectile());
    }

    void Move(){ 
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += movement * speed * Time.deltaTime;
    }

    void RotatePlayer(){ 
        Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane( Vector3.up, transform.position );
        if( p.Raycast( mouseRay, out float hitDist) ){
            Vector3 hitPoint = mouseRay.GetPoint( hitDist );
            transform.LookAt( hitPoint );
        }
    }
    
    void Update(){
        // Debug.Log(cr);
        linePos = transform.position + transform.forward * minDist;
        targetPos = new Vector3(linePos.x, projectile.localScale.y / 2,linePos.z);

        Move();
        RotatePlayer();
    }

    void OnDrawGizmosSelected(){
        if (target != null){
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, linePos);
        }
    }
 
    IEnumerator Simulateprojectile(){
        // Short delay added before projectile is thrown
        yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed
        projectile.position = transform.position + new Vector3(0, 0.0f, 0);
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(projectile.position, targetPos);
 
        // Calculate the velocity needed to throw the object to the target at specified angle
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target
        projectile.rotation = Quaternion.LookRotation(targetPos - projectile.position);
       
        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
            
            yield return null;
            coroutine = null;
        }
    }  
}
