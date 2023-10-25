using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBeamIpactController : MonoBehaviour
{
    [SerializeField] private GameObject[] IceCrystalVisual;

    private void Awake()
    {
        int randomInt = Random.Range(0, IceCrystalVisual.Length);

        IceCrystalVisual[randomInt].SetActive(true);
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
