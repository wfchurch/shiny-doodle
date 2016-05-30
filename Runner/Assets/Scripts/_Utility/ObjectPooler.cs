using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler current; // more than one pooler cannot use the same name current, change this.
    public GameObject pooledObject;
    public int pooledAmount = 0;  // set amount to be pooled, best if this is as large as we need it.
    public bool willGrow = true;  // our pool of objects is allowed to grow.

    List<GameObject> pooledObjects;

    void Awake()
    {
        current = this;
    }

	// Use this for initialization
	void Start () {
        pooledObjects = new List<GameObject>();
        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }   
	}

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if(willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

    // to get..
    /* GameObject obj = ObjectPooler.current.GetPooledObject();
    if( obj == null ) return; // couldn't get an object
    // do obj setup here
    obj.SetActive(true);*/

    // to deactivate..
    /*gameObject.SetActive(false);*/


}
