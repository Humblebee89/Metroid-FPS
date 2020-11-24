using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerCamera;

    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = new PlayerInput();
    }

    private void Update()
    {

    }
}
