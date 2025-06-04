using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public static SwipeManager Instance { get; private set; }
    public event EventHandler<OnSwipeEventArgs> OnSwipe;
    public class OnSwipeEventArgs : EventArgs {
        public Vector2 Direction { get; set; }
    }

    void Awake()
    {
        Instance = this;
    }

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    [SerializeField] private float swipeThreshold = 50f;

    private void Update()
    {
        if (!GameManagerScript.Instance.CanSwipe()) return;
        #if UNITY_EDITOR
        HandleMouseInput();
        HandleKeyboardInput();
        #else
        HandleMouseInput();
        #endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            startTouchPosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            OnSwipe?.Invoke(this, new OnSwipeEventArgs { Direction = Vector2.left });

        if (Input.GetKeyDown(KeyCode.RightArrow))
            OnSwipe?.Invoke(this, new OnSwipeEventArgs { Direction = Vector2.right });
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
            OnSwipe?.Invoke(this, new OnSwipeEventArgs { Direction = Vector2.up });

        if (Input.GetKeyDown(KeyCode.DownArrow))
            OnSwipe?.Invoke(this, new OnSwipeEventArgs { Direction = Vector2.down });
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouchPosition = touch.position;

            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        Vector2 delta = endTouchPosition - startTouchPosition;

        if (delta.magnitude < swipeThreshold)
            return;

        Vector2 direction;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            direction = delta.x > 0 ? Vector2.right : Vector2.left;
        else
            direction = delta.y > 0 ? Vector2.up : Vector2.down;

        OnSwipe?.Invoke(this, new OnSwipeEventArgs { Direction = direction });
    }
}
