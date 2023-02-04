using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    public Transform projectile; // TODO instantiate
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    [Range(1f, 8f)] public float minDist = 5f;
    
    [HideInInspector] public Vector3 targetPos;
    private int speed = 10;
    private float distToThrow;
    private int throwIncreaseSpeed = 10;
    private Coroutine coroutine;
    
    void Start(){          
        distToThrow = minDist;
    }

    void HandleMouseButton(){
        if(coroutine == null){
            // if (Input.GetMouseButtonDown(0)){
            //     Debug.Log("Pressed primary button.");
            // }
            if (Input.GetMouseButton(0)){
                Debug.Log("Holding primary button.");
                distToThrow += Time.deltaTime * throwIncreaseSpeed;
            }
            else if (Input.GetMouseButtonUp(0)){
                Debug.Log("Primary button UP.");
                coroutine = StartCoroutine(Simulateprojectile(transform.position, targetPos));
                distToThrow = minDist;
            }
        }
    }

    void Update(){
        // Debug.Log(coroutine);

        Vector3 v = transform.position + transform.forward * distToThrow;
        targetPos = new Vector3(v.x, projectile.localScale.y / 2, v.z);

        HandleMouseButton();
    }
 
    IEnumerator Simulateprojectile(Vector3 playerPos, Vector3 targPos){
        // Move projectile to the position of throwing object + add some offset if needed
        projectile.speed = Vector3.zero;
        projectile.position = playerPos;
       
        // Calculate distance to target
        float targetDist = Vector3.Distance(projectile.position, targPos);
 
        // Calculate the velocity needed to throw the object to the target at specified angle
        float projectileVel = targetDist / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectileVel) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVel) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time
        float flightDuration = targetDist / Vx;
   
        // Rotate projectile to face the target
        projectile.rotation = Quaternion.LookRotation(targPos - projectile.position);
       
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
