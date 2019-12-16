using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MBSingleton<GameManager>
{
    [SerializeField] private GameObject DepositPosition;

    [SerializeField]private int actualBagLevel = 0;
    [SerializeField]private int maxBagLevel = 20;

    private float coolDown = 2.0f;
    private float actualCoolDown = 0.0f;

    private void Start() {
        ResourceManager.Instance.GenerateResource(5, 10);
        actualCoolDown = 0.0f;
    }

    public Vector3 GetDepositPosition(){
        return DepositPosition.transform.position;
    }

    public bool IsBagFull(){
        return (actualBagLevel >= maxBagLevel);
    }

    public void SaveResource(){
        actualBagLevel++;
    }

    public bool EmptyBag(){
        if(actualCoolDown >= coolDown){
            actualBagLevel = 0;
            actualCoolDown = 0.0f;
            return true;
        }else{
            actualCoolDown += Time.deltaTime;
            return false;
        }
    }
}
