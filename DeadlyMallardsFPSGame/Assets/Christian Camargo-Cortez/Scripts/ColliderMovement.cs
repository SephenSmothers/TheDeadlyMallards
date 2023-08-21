using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform where;
    [SerializeField] GameObject colliderObj;
    void Awake()
    {
        colliderObj.transform.position = where.position;
    }

    // Update is called once per frame
    void Update()
    {
        colliderObj.transform.position = where.position;
        colliderObj.transform.rotation= where.rotation;
    }
}
