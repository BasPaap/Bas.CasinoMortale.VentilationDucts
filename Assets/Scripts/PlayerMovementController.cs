using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 movement = new Vector3();

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        //characterController.attachedRigidbody.useGravity = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        movement.x = input.x;
        movement.z = input.y;
    }

    private void Update()
    {        
        characterController.Move(movement * Time.deltaTime);
    }
}
