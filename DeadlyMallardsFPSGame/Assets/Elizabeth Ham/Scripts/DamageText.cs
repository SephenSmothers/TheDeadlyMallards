using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float destroyTimer = 1f;
    public TextMeshPro damageText;
    // Start is called before the first frame update
    void Awake()
    {
        damageText = GetComponentInChildren<TextMeshPro>();
    }

   public void DisplayDamage(int amount)
   {
       
        damageText.text = amount.ToString();
        StartCoroutine(MoveAndDestroy());

   }

    public IEnumerator MoveAndDestroy()
    {
        Vector3 playerDir = Camera.main.transform.position - transform.position;
        Quaternion rotationToCamera = Quaternion.LookRotation(playerDir);
     
        float elapseTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * 2;


        while (elapseTime < destroyTimer) 
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapseTime / destroyTimer);
            transform.rotation = rotationToCamera;
            elapseTime += Time.deltaTime;
            yield return null; 
        }
        damageText.enabled = false;
        Destroy(gameObject);
    }
}
