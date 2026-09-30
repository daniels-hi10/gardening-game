using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Camera Eyes; // camera variable
    public float MouseSens = 1; // mouse sensitivity
    public Rigidbody RB;

    public float Speed = 3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false; // turns cursor invisible
        Cursor.lockState = CursorLockMode.Locked; // locks mouse to center of screen
    }

    // Update is called once per frame
    void Update()
    {
        MouseControl(); KeyboardControl();
    }

    public void MouseControl() // function for mouse controls
    {
        Vector2 mouse = Mouse.current.delta.ReadValue(); // gets the x and y values for mouse movement

        transform.Rotate(0, mouse.x * MouseSens, 0); // left and right
        Eyes.transform.Rotate(mouse.y * MouseSens * -1, 0, 0); // rotates mouse up and down, negative for inverted mouse reasons

    }

    public void KeyboardControl() // function for keyboard controls
    {
        Vector3 vel = new Vector3(0, RB.linearVelocity.y, 0); 

        // wasd - only transform.right and transform.forward exists
        if (Keyboard.current.wKey.isPressed)
            vel += transform.forward * Speed;
        if (Keyboard.current.aKey.isPressed)
            vel += transform.right * -Speed;
        if (Keyboard.current.sKey.isPressed)
            vel += transform.forward * -Speed;
        if (Keyboard.current.dKey.isPressed)
            vel += transform.right * Speed;

        if (Keyboard.current.leftShiftKey.isPressed) // shift to run
            Speed = 6;
        else
            Speed = 3;

        RB.linearVelocity = vel;
    }
}
