using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private CharacterMovement movement;

    private void Update()
    {
        Vector3 input = new Vector3(joystick.Horizontal,0f,joystick.Vertical);
        movement.SetMoveInput(input);
    }
}