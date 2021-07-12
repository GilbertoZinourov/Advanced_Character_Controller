using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private PlayerMovement _movement;
    private Vector2 _inputWASD;
    private Vector2 _inputMouse;
    public bool _isRunningInputPressed;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MovePlayer();
        LookAround();
    }

    private void MovePlayer()
    {
        GetWASDInput();
        _movement.Move(_inputWASD.normalized, _isRunningInputPressed);
    }

    private void GetWASDInput()
    {
        _inputWASD = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRunningInputPressed = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRunningInputPressed = false;
        }
    }

    private void LookAround()
    {
        GetMouseInput();
        _movement.LookAround(_inputMouse);
    }

    private void GetMouseInput()
    {
        _inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
