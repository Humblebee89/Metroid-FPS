using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject[] objectToInstantiate;
    private void OnDestroy()
    {
        foreach (GameObject i in objectToInstantiate)
        {
            Instantiate(i, transform.position, transform.rotation);
        }
    }
}
