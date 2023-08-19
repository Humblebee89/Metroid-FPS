using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpread : MonoBehaviour
{
    [SerializeField] private float maxSpread = 8;
    [SerializeField] private GameObject subprojectile;
    [SerializeField] private int numberOfSubprojectiles = 10;

    private void OnEnable()
    {
        for (int i = 0; i < numberOfSubprojectiles; i++)
        {
            Vector3 spreadVector = transform.localEulerAngles + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
            Quaternion spreadDirection = Quaternion.Euler(spreadVector);
            Instantiate(subprojectile, transform.position, spreadDirection);
        }

        Destroy(gameObject);
    }
}
