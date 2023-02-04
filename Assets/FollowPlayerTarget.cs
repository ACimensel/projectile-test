using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerTarget : MonoBehaviour
{
    private ProjectileLaunch pl;

    void Start(){
        pl = gameObject.GetComponentInParent<ProjectileLaunch>();
    }

    void Update(){
        transform.position = new Vector3(pl.targetPos.x, 0, pl.targetPos.z);
    }
}
