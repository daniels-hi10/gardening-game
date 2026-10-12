using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum Tools
{
    Hands,
    Seed,

    Can,
    Hoe
}

public class Player : MonoBehaviour
{
    // camera movement
    public Camera Eyes; // camera variable
    public float MouseSens = 1; // mouse sensitivity
    public Spawner spawner; // reference to spawner
    public Rigidbody RB;


    // character stats
    public float Speed = 3;
    public float JumpPower = 4;

    public List<GameObject> Floors; // list of solid objects im touching
    public Tools SelectedTool = Tools.Hands;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false; // turns cursor invisible
        Cursor.lockState = CursorLockMode.Locked; // locks mouse to center of screen
        spawner = FindFirstObjectByType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseControl(); KeyboardControl();
        ToolSelection();
    }

    public void MouseControl() // function for mouse controls
    {
        Vector2 mouse = Mouse.current.delta.ReadValue(); // gets the x and y values for mouse movement

        transform.Rotate(0, mouse.x * MouseSens, 0); // left and right
        Eyes.transform.Rotate(mouse.y * MouseSens * -1, 0, 0); // rotates mouse up and down, negative for inverted mouse reasons

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(Eyes.transform.position, Eyes.transform.forward, out RaycastHit hit, 10))
            {
                Debug.Log(hit.collider.gameObject.name);

                // Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            }
        }
        /* if i click
        {
        // basically perform an action
            // if i have a flowering pot
                //accelerate growing speed
            // if i have a hoe
                // cut weed
            // if i don't have anything
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

    public void ToolSelection()
    {
        switch (SelectedTool)
        {
            case Tools.Hands: // if i am only using my hands
                /// switch tool
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Seed;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Can;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hoe;
                    Debug.Log(SelectedTool);
                }

                // plant a seed

                break;

            case Tools.Seed:
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Can;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hoe;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hands;
                    Debug.Log(SelectedTool);
                }
                break;

            case Tools.Hoe:
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Seed;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Can;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hands;
                    Debug.Log(SelectedTool);
                }
                break;

            case Tools.Can:
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Seed;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hoe;
                    Debug.Log(SelectedTool);
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hands;
                    Debug.Log(SelectedTool);
                }
                break;
        }
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
