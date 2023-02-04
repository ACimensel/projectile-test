using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    private Transform player;
    public Transform projectile;
    public Transform target;

    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    
    private Vector3 yTargetOffset;

    Coroutine cr;

    void Awake(){
        player = transform;      
    }
    
    void Start(){          
        cr = StartCoroutine(Simulateprojectile());
        yTargetOffset = target.position + new Vector3(0, 0.25f, 0);
    }

    void Update(){
        Debug.Log(cr);
    }
 
    IEnumerator Simulateprojectile(){
        // Short delay added before projectile is thrown
        yield return new WaitForSeconds(1.5f);
       
        // Move projectile to the position of throwing object + add some offset if needed.
        projectile.position = player.position + new Vector3(0, 0.0f, 0);
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(projectile.position, yTargetOffset);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target.
        projectile.rotation = Quaternion.LookRotation(yTargetOffset - projectile.position);
       
        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
            
            yield return null;
            cr = null;
        }
    }  
}