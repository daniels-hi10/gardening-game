using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // camera movement
    public Camera Eyes; // camera variable
    public float MouseSens = 1; // mouse sensitivity
    public Rigidbody RB;

    // character stats
    public float Speed = 3;
    public float JumpPower = 4;

    public List<GameObject> Floors; // list of solid objects im touching


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

        /* if (Mouse.current.leftButton.wasPressedThisFrame) // if i click
        {
        // basically perform an action
            // if i have a flowering pot
                //accelerate growing speed
            // if i have a hoe
                // cut weed
            // if i have a shovel
                // remove flowers
        } */
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

        // space - if i jump and grounded, jump
        if (JumpPower > 0 && Keyboard.current.spaceKey.wasPressedThisFrame && OnGround())
            vel.y = JumpPower;
        else // otherwise, y velocity is whatever it was last frame
            vel.y = RB.linearVelocity.y;
        
        RB.linearVelocity = vel;
    }

    public bool OnGround()
    {
        return Floors.Count > 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        // if i touch something that's not in the list of things im touching
        if (!Floors.Contains(other.gameObject))
        {
            Floors.Add(other.gameObject); // add the object to the list
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Floors.Remove(other.gameObject);
    }
}
