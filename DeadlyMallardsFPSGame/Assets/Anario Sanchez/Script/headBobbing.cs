using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(objectPosition))]
public class headBobbing : MonoBehaviour
{
    [Header("-----Head Bobbing Settings-----")]
    public float speed;
    public float intensity;
    public float intensityx;


    private objectPosition objectPosition;
    private Vector3 origOffset;
    float time;
    private void Start()
    {
        objectPosition = GetComponent<objectPosition>();
        origOffset = objectPosition.offset;
    }
    void Update()
    {
        Vector3 vector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        if (vector.magnitude > 0f)
        {
            time += Time.deltaTime * speed;
        }
        else
        {
            time = 0f;
        }

        float amountY = -Mathf.Abs(intensity * Mathf.Sin(time));
        Vector3 amountX = objectPosition.transform.right * intensity * Mathf.Cos(time) * intensityx;

        objectPosition.offset = new Vector3(origOffset.x, origOffset.y + amountY, origOffset.z);

        objectPosition.offset += amountX;
    }
}
