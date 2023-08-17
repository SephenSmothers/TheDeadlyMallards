using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthCrate : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(floatingObjects());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.GetMaxHealth();
            Destroy(gameObject);
        }
    }
    private IEnumerator floatingObjects()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
            yield return null;
        }
    }
}
