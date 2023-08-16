using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamiteControl : MonoBehaviour
{
    public int damage;
    [SerializeField] List<GameObject> damageables;
    public GameObject leBoom;
    void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnDestroy()
    {
        Instantiate(leBoom,transform.position,Quaternion.identity);
        for (int i = 0; i < damageables.Count; i++)
        {
            damageables[i].GetComponent<TakeDamage>().CanTakeDamage(damage);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent("TakeDamage"))
        {
            if (!checkIfInList(other.gameObject))
            {
              damageables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent("TakeDamage"))
        {
            damageables.Remove(other.gameObject);
        }
    }

    bool checkIfInList(GameObject obj)
    {
        for(int i = 0;i < damageables.Count;i++)
        {
            if (damageables[i] == obj)
            {
                return true;
            }
        }
        return false;
    }
}
