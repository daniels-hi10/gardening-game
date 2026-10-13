using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
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
    public Beans bean; // reference to beans script
    public Rigidbody RB;


    // character stats
    public float Speed = 3;
    public float JumpPower = 4;

    public List<GameObject> Floors; // list of solid objects im touching
    public Tools SelectedTool = Tools.Hands; // current tool equipped
    public GameObject beans; // beans prefab

    // score + time tracking 
    public int Score;
    public float GameTimer;
    public TextMeshPro ScoreText;
    public TextMeshPro ToolText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false; // turns cursor invisible
        Cursor.lockState = CursorLockMode.Locked; // locks mouse to center of screen
        spawner = FindFirstObjectByType<Spawner>();
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MouseControl(); KeyboardControl();
        ToolSelection();
        ScoreText.text = "Score: " + Score;
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
            // hands equipped
            case Tools.Hands:
                // ToolText.text = "Currently Equipped: " + SelectedTool;
                /// switch tool
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Seed;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Can;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hoe;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }

                // remove beans
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (Physics.Raycast(Eyes.transform.position, Eyes.transform.forward, out RaycastHit hit, 10f))
                    {
                        Beans grown = hit.collider.GetComponent<Beans>();

                        if (grown != null && grown.CurrentStage == Beans.Stages.Mature) // if beans are grown
                        {
                            Vector3 g = grown.transform.position;

                            if (spawner != null)
                            {
                                spawner.beans.Remove(grown); // remove the beans from the soil
                                spawner.occupiedPos.Remove(g); // remove the empty soil from list of occupiedPos
                                spawner.vacantPos.Add(g); // readd the empty soil to vacantPos
                            }

                            Score += 3;
                            Destroy(grown.gameObject); // remove beans
                            Debug.Log("flower harvested, Score: " + Score);
                        }
                    }
                }
                break;


            // seed equipped
            case Tools.Seed:
                // switch weapon
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Can;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hoe;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hands;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }


                // grow seed
                if (Mouse.current.leftButton.wasPressedThisFrame) // if i click
                {
                    if (Physics.Raycast(Eyes.transform.position, Eyes.transform.forward, out RaycastHit hit, 10))
                    {
                        if (spawner != null && spawner.vacantPos.Count > 0) // if there are still empty spots available
                        {
                            Vector3 view = Eyes.transform.position + (Eyes.transform.forward * 1f); // find whichever spot is closest within 1 unit ahead of player's pov


                            Vector3 ClosestDirt = spawner.vacantPos[0];
                            float closest = Vector3.Distance(view, ClosestDirt);

                            foreach (Vector3 dirt in spawner.vacantPos) // looks through the list of each empty patch
                            {
                                float distance = Vector3.Distance(view, dirt); 

                                if (distance < closest) // if a specific patch of dirt is closer to the player
                                {
                                    closest = distance;
                                    ClosestDirt = dirt; // update to closer dirt patch
                                }
                            }

                            if (closest <= 1f)
                            {
                                Instantiate(beans, ClosestDirt, Quaternion.identity);
                                spawner.vacantPos.Remove(ClosestDirt);
                                spawner.occupiedPos.Add(ClosestDirt);
                                Debug.Log("beans planted");
                            }
                        }
                    }
                }
                break;


            // hoe equipped
            case Tools.Hoe:
                // switch weapons
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Seed;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Can;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hands;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }

                // if i click
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (Physics.Raycast(Eyes.transform.position, Eyes.transform.forward, out RaycastHit hit, 10f))
                    {
                        Weed weed = hit.collider.GetComponent<Weed>(); // if im aiming at weeds
                        Beans wilt = hit.collider.GetComponent<Beans>(); // if im aiming at beans

                        if (weed != null) // if there is a weed
                        {
                            Vector3 w1 = weed.transform.position;
                            Score++;
                            Destroy(weed.gameObject); // i remove the weed

                            if (spawner != null)
                            {
                                spawner.weedPos.Remove(w1);
                                spawner.vacantPos.Add(w1);
                            }
                            Debug.Log("weed gone, Score: " + Score);
                        }

                        else if (wilt != null) // if i see dead beans
                        {
                            if (wilt.CurrentStage == Beans.Stages.Wilted)
                            {
                                Vector3 w2 = wilt.transform.position; // remove beans

                                if (spawner != null)
                                {
                                    spawner.beans.Remove(wilt);
                                    spawner.occupiedPos.Remove(w2);
                                    spawner.vacantPos.Add(w2);
                                }

                                Score -= 2;
                                Destroy(wilt.gameObject);
                                Debug.Log("beans gone, Score: " + Score);
                            }
                        }
                    }
                }
                break;


            // can equipped
            case Tools.Can:

                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Seed;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hoe;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    SelectedTool = Tools.Hands;
                    Debug.Log(SelectedTool);
                    ToolText.text = "Currently Equipped: " + SelectedTool;
                }

                // water seed
                if (Mouse.current.leftButton.wasPressedThisFrame) // if i click
                {
                    if (Physics.Raycast(Eyes.transform.position, Eyes.transform.forward, out RaycastHit hit, 30f))
                    {
                        Beans seed = hit.collider.GetComponent<Beans>(); // aiming at seed

                        if (seed != null) // if seed is there
                        {
                            seed.Grow(); // have the beans grow and change color
                        }
                    } 
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