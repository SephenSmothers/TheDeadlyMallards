using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionScript : MonoBehaviour

{
    [SerializeField] int explostiondmg;
    [SerializeField] GameObject Boom;
    KamikazeZombieAI zombo;
    Collider player;
    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(Boom,gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
            other.GetComponent<playerControl>().CanTakeDamage(explostiondmg); 
        }
    }

    
    
    
}
