using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 movement;
    private Vector3 velocity;

    [Range(0.01f, 10f)]
    [SerializeField] private float damping;

    public float Speed => velocity.magnitude;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        movement.x = input.x;
        movement.z = input.y;
    }

    private void Update()
    {
        Physics.SyncTransforms(); // This is necessary in case the player's transform has been changed by something other than player input (for instance, changing the map size). See: https://issuetracker.unity3d.com/issues/charactercontroller-overrides-objects-position-when-teleporting-with-transform-dot-position
        movement.y = characterController.isGrounded ? 0.0f : -9.8f;
        velocity = Vector3.Lerp(velocity, movement, damping * Time.deltaTime);
        velocity = velocity.magnitude < 0.001f ? Vector3.zero : velocity;   // To prevent infinitely small lerping.
        characterController.Move(velocity * Time.deltaTime);
    }
}
