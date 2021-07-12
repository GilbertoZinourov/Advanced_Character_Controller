using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Enumerators

    [Flags]
    public enum Mechanics
    {
        Nothing = 0,
        Base8DirMovement = 1 << 0,
        Gravity = 1 << 1,
        Jump = 1 << 2,
    }
    
    public enum FirstOrThird
    {
        First,
        Third
    }

    public enum PlayerStates
    {
        Idle,
        Walking,
        Running,
        Falling,
    }

    #endregion

    #region Perspective Variables

    public FirstOrThird desiredPerspective;
    private Transform _camTransform;
    
    // First person
    public Transform firstPersonCameraPosition;
    
    // Third person
    public Transform centerOfRotation;
    public float radiusOfRotation;
    public Vector3 cameraOffset;

    #endregion

    #region Mechanics Init Variables

    private Dictionary<string, bool> _mechanicsDictionary = new Dictionary<string, bool>();
    public Mechanics desiredMechanics;
    public PlayerStates currentState;

    #endregion
    
    #region 8 Directional Movement

    private CharacterController _controller;
    public float walkSpeed;
    public float runSpeed;
    public bool invertedX, invertedY;
    [Range(50, 350)] public float mouseSensitivity;
    public float minVerticalLookClampValue = -90;
    public float maxVerticalLookClampValue = 90;
    public LayerMask groundMask;

    private float _currentMovementSpeed = 0;
    private float _xRotation = 0f;
    private Vector3 _finalDir;

    #endregion

    #region Gravity Variables

    [Range(4, 16)] public int numberOfGroundedCheckPoints = 4;
    public float radiusOfGroundedCheckPoints;
    public float groundCheckCastDistance = .5f;
    public float gravity = 9.8f;
    public float fallMultiplier = 1.5f;

    private List<Vector3> _gravityCheckersList;
    private float _fallSpeed = 0;

    #endregion

    private void Awake()
    {
        PlaceCamera();
        ControllerInit();
        MechanicsInit();
        CreateGravityCheckers();
    }

    private void Update()
    {
        CheckGrounded();
    }

    private void MechanicsInit()
    {
        foreach(Mechanics flag in Enum.GetValues(typeof(Mechanics)))
        {
            if (flag <= 0 || _mechanicsDictionary.ContainsKey(flag.ToString())) continue;
            _mechanicsDictionary.Add(flag.ToString(), (desiredMechanics & flag) > 0);
            Debug.Log(flag + " was added to the dictionary with value: " + _mechanicsDictionary[flag.ToString()]);
        }
    }

    private void PlaceCamera()
    {
        _camTransform = Camera.main.transform;
        if (!_camTransform)
        {
            Debug.LogError("MainCamera was ton found!");
            return;
        }
        switch(desiredPerspective)
        {
            case FirstOrThird.First:
                _camTransform.SetParent(firstPersonCameraPosition);
                _camTransform.localPosition = Vector3.zero;
                _camTransform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case FirstOrThird.Third:
                _camTransform.SetParent(centerOfRotation);
                _camTransform.rotation = Quaternion.Euler(Vector3.zero);
                _camTransform.position = centerOfRotation.position - centerOfRotation.forward * radiusOfRotation +
                                         cameraOffset;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    #region 8 Directional Movement
    
    private void ControllerInit()
    {
        _controller = GetComponent<CharacterController>();
        if (!_controller)
        {
            _controller = gameObject.AddComponent<CharacterController>();
        }
    }

    public void Move(Vector2 input, bool isRunning)
    {
        if (!_mechanicsDictionary[Mechanics.Base8DirMovement.ToString()]) return;
        
        float targetSpeed;
        if (input.magnitude <= .01f)
        {
            targetSpeed = 0;
            currentState = PlayerStates.Idle;
        }
        else
        {
            if (isRunning)
            {
                targetSpeed = runSpeed;
                currentState = PlayerStates.Running;
            }
            else
            {
                targetSpeed = input.magnitude * walkSpeed;
                currentState = PlayerStates.Walking;
            }
        }

        _currentMovementSpeed = targetSpeed;

        Vector3 movementDirection = Vector3.Normalize(new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z)) * input.y + 
                                    _camTransform.right * input.x;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundMask))
        {
            Vector3 right = new Vector3(movementDirection.z, 0, -movementDirection.x);
            _finalDir = right;
            movementDirection = Vector3.Cross(right , hit.normal).normalized;
        }

        //_finalDir = movementDirection;
        movementDirection *= _currentMovementSpeed;
        _controller.Move(movementDirection * Time.deltaTime);
        //Debug.Log("Moving with speed: " + _currentMovementSpeed);
    }

    public void LookAround(Vector2 input)
    {
        if (!_mechanicsDictionary[Mechanics.Base8DirMovement.ToString()]) return;
        
        if (invertedX)
        {
            input.x *= -1;
        }

        if (invertedY)
        {
            input.y *= -1;
        }

        input *= (mouseSensitivity * Time.deltaTime);
        _xRotation -= input.y;
        _xRotation = Mathf.Clamp(_xRotation, minVerticalLookClampValue, maxVerticalLookClampValue);

        switch (desiredPerspective)
        {
            case FirstOrThird.First:
                transform.Rotate(Vector3.up * input.x);
                firstPersonCameraPosition.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                break;
            case FirstOrThird.Third:
                transform.Rotate(Vector3.up * input.x);
                centerOfRotation.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    #endregion
    
    #region Gravity

    private void CreateGravityCheckers()
    {
        if (!_mechanicsDictionary[Mechanics.Gravity.ToString()]) return;

        _gravityCheckersList = new List<Vector3>();
        
        float gradPerSegment = (float) 360 / numberOfGroundedCheckPoints;
        float currentAngle = 0f;
        for (int i = 0; i < numberOfGroundedCheckPoints; i++)
        {
            float currentAngleInRad = currentAngle * Mathf.Deg2Rad;
            Vector3 pointPosition = new Vector3(Mathf.Cos(currentAngleInRad), 0, Mathf.Sin(currentAngleInRad)) * radiusOfGroundedCheckPoints;

            _gravityCheckersList.Add(pointPosition);
            
            currentAngle += gradPerSegment;
        }
    }

    private void CheckGrounded()
    {
        if (!_mechanicsDictionary[Mechanics.Gravity.ToString()]) return;

        foreach (var point in _gravityCheckersList)
        {
            Vector3 pointRelativeToPlayer = transform.position + point;
            if (Physics.Raycast(pointRelativeToPlayer, Vector3.down, groundCheckCastDistance, groundMask))
            {
                if (_fallSpeed != 0)
                {
                    _fallSpeed = 0;
                }
                Debug.Log("Grounded");
                return;
            }
        }
        _fallSpeed -= gravity * Time.deltaTime;
        _controller.Move(Vector3.up * (_fallSpeed * fallMultiplier * Time.deltaTime));
        Debug.Log("InAir");
    }
    
    #endregion

    #region PlayerStates

    private void ChangePlayerState(PlayerStates state)
    {
        if (currentState == state) return;
        currentState = state;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 5);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _finalDir);

        DrawGravityCheckers(Color.red);
        Gizmos.color = defaultColor;
    }

    private void DrawGravityCheckers(Color color)
    {
        Gizmos.color = color;

        float gradPerSegment = (float) 360 / numberOfGroundedCheckPoints;
        float currentAngle = 0f;
        for (int i = 0; i < numberOfGroundedCheckPoints; i++)
        {
            float currentAngleInRad = currentAngle * Mathf.Deg2Rad;
            Vector3 pointPosition =
                transform.position + (new Vector3(Mathf.Cos(currentAngleInRad), 0, Mathf.Sin(currentAngleInRad)) * radiusOfGroundedCheckPoints);
            Gizmos.DrawSphere(pointPosition, .1f);
            Gizmos.DrawLine(pointPosition, pointPosition + Vector3.down * groundCheckCastDistance);
            currentAngle += gradPerSegment;
        }
    }

    #endregion
}
