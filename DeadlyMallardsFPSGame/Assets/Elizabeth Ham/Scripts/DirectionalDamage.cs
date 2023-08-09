using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DirectionalDamage : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public GameObject damageIndicator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && enemy != null)
        {
            Vector3 directional = enemy.position - player.position;
            directional.y = 0;

            Quaternion rotation = Quaternion.LookRotation(directional);

            damageIndicator.transform.rotation = rotation;  
        }
    }

    public IEnumerator DamageDirection()
    {
        damageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        damageIndicator.SetActive(false);
    }
}
