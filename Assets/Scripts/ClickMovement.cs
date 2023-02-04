using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class ClickMovement : MonoBehaviour
{
    private GameInputs gameInputs;
    private InputAction clickMoveAction;

    private Camera mainCamera;  
    private CharacterController characterController;
    private int groundLayerMask;

    private Coroutine moveCoroutine;
    private Coroutine speedCoroutine;

    [Header("Movement")]
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField] private float accelerationTime = 0.7f;

    private float moveSpeed;

    //Using actions is prefereable as you can have many objects listening for maybe things such as adding sounds, updating ui and much more,
    //and it gets messy calling each functions directly 
    public static event Action OnSpeedIncreased; 
    public static event Action OnSpeedDecreased;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        mainCamera = Camera.main;

        gameInputs = new GameInputs();
        clickMoveAction = gameInputs.Player.Click;

        characterController = GetComponent<CharacterController>();
        groundLayerMask = LayerMask.GetMask("Ground"); //only allow raycast on ground

        //sets our speed variable to the default value
        moveSpeed = defaultSpeed;

    }

    private void OnEnable()
    {
        //activated the input system
        gameInputs.Enable();

        
        clickMoveAction.performed += MoveToPointer;
        clickMoveAction.Enable();
    }

    private void OnDisable()
    {
        gameInputs.Disable();

        clickMoveAction.performed -= MoveToPointer;
        clickMoveAction.Enable();
    }

    private void MoveToPointer(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIObject())
            return;
        
        Ray ray;

        //takes position from touch if platform set to android or ios
#if UNITY_ANDROID || UNITY_IOS
        ray = mainCamera.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());

        //takes mouse position if pc
#elif UNITY_STANDALONE
        ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
#endif
        //Checks if touch raycast is on ground and gets the point where it hit
        if (Physics.Raycast(ray : ray, hitInfo: out RaycastHit hit,maxDistance: Mathf.Infinity ,layerMask: groundLayerMask) && hit.collider)
        {
            //Stops the previous coroutine if already moving
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            
            moveCoroutine = StartCoroutine(MoveTowards(hit.point));
        }
    }

    private IEnumerator MoveTowards(Vector3 position)
    {
        //prevents character from going inside ground
        float distanceFromFloor = transform.position.y - position.y;
        position.y = distanceFromFloor;

        while(Vector3.Distance(transform.position, position) > moveThreshold) //move if it passes some threshold value, 0.1f is default
        {

            Vector3 dir = position - transform.position;
            Vector3 target = dir.normalized * moveSpeed * Time.deltaTime;
            characterController.Move(target);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }

    #region SpeedChange
    public void IncreaseSpeed()
    {

        if (moveSpeed >= maxSpeed)
            return;

        float target = moveSpeed + 3f; //Speed Increased
        target = Mathf.Clamp(target, 1f, maxSpeed);
        
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(SmoothSpeedChange(target, accelerationTime));

        if (OnSpeedIncreased != null)
            OnSpeedIncreased();
    }

    public void DecreaseSpeed()
    {
        if (moveSpeed <= 1f)
            return;

        float target = moveSpeed - 3f; //Speed Decreased
        target = Mathf.Clamp(target,1f,maxSpeed);
        
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(SmoothSpeedChange(target, accelerationTime));

        if (OnSpeedDecreased != null)
            OnSpeedDecreased();

    }

    IEnumerator SmoothSpeedChange(float end, float duration)
    {
        float t = 0;
        float start = moveSpeed;
        while(t < duration)
        {
            moveSpeed = Mathf.Lerp(start, end, t/duration);
            t += Time.deltaTime;
            yield return null;
        }

        moveSpeed = end;
        yield return null;
    }

    #endregion // Containts all speed related functions

    //function to detect if touch is over ui
    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if UNITY_ANDROID || UNITY_IOS
        eventDataCurrentPosition.position = new Vector2(Touchscreen.current.primaryTouch.position.ReadValue().x, Touchscreen.current.primaryTouch.position.ReadValue().y);
#elif UNITY_STANDALONE
        eventDataCurrentPosition.position = new Vector2Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
#endif
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
