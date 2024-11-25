using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitBySphere : MonoBehaviour
{
    [SerializeField] MonsterProperties monsterProps;
    private void OnCollisionEnter2D (Collision2D other)
    {
        if(monsterProps.canCapture && other.gameObject.tag == "Sphere")
        {
            Debug.Log("Hit by sphere");
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Sphere")
        {
            Debug.Log("Hit by sphere");
            other.gameObject.GetComponent<SphereProps>().hitMonster = true;
        }

        //Unity event call to indicate monster was hit without direct reference
        GameEvents.current.MonsterHit();
    }


 
}
