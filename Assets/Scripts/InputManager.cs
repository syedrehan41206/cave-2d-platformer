using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public PlayerInput PlayerInputs;
    public bool JumpInput;
    public Vector2 MoveInput;
    public bool dashInput;
    private void Awake()
    {
        PlayerInputs = new PlayerInput();

    }
    public void OnEnable()
    {
        PlayerInputs.Enable();
    }
    public void OnDisable()
    {
        PlayerInputs.Disable();
    }
    void Update()
    {
        PlayerInputs.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        PlayerInputs.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
        PlayerInputs.Player.Jump.performed += ctx => JumpInput = true;
        PlayerInputs.Player.Jump.canceled += ctx => JumpInput = false;
        PlayerInputs.Player.Interact.performed += ctx => dashInput = true;
        PlayerInputs.Player.Interact.canceled += ctx => dashInput = false;
    }
}