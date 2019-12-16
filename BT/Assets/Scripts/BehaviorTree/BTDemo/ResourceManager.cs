using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MBSingleton<ResourceManager>
{
    [SerializeField] GameObject resourcePrefab;
    List<GameObject> resources = new List<GameObject>();

    public void GenerateResource(int cant, int radio){
        for (int i = 0; i < cant; i++){

            Vector3 randomPos = new Vector3(Random.Range(-radio, radio),0.0f,Random.Range(-radio, radio));
            resources.Add( Instantiate(resourcePrefab, randomPos, Quaternion.identity));
        }
    }

    public void DestroyResource(GameObject re){
        resources.Remove(re);
        Destroy(re);
    }

    public Vector3 GetResourcePosition(){
        return resources.ToArray()[0].transform.position;
    }

    public Resource GetResource(){
        return resources.ToArray()[0].GetComponent<Resource>();
    }

    public bool HaveResource(){
        return (resources.Count > 0);
    }

}
