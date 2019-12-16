using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int quantity;
    private float coolDown = 1.0f;
    private float actualCoolDown;

    private void Start() {
        quantity = (int)Random.Range(1,20);
        actualCoolDown = 0.0f;
    }

    public bool getResource(){
        if(actualCoolDown >= coolDown){
            actualCoolDown = 0.0f;
            if(quantity > 0){
                --quantity;
                return true;
            }else{
                ResourceManager.Instance.DestroyResource(gameObject);
                return false;
            }
        }else{
            actualCoolDown += Time.deltaTime;
            return false;
        }
    }
}
