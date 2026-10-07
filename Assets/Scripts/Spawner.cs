using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public List<GameObject> Plants = new List<GameObject>(); // list of plants: weeds, flowers, whatever whimsical shit

    public float Timer; // timer before next spawn
    public float SpawnRate; // interval between plants to spawn
    public bool IsPlanted; // checks if a plant is already planted

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Timer = 0f;
        SpawnRate = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            // if (!IsPlanted) // only executes if a weed is not planted
            // {
                SpawnPlant(); // spawn plant
                Timer = SpawnRate; // reset timer IF the weed isn't alr planted
            // }
        }
    }


    void SpawnPlant()
    {
        float yPos = 0.13f;
        float zTop = 16.5f;
        float zMid = 12.85f;
        float zLow = 9.3f;
        for (int i = 0; i < 1; i++)
        {
            List<Vector3> plantPos = new List<Vector3>(); // list of dirt patches
            
            // first row (ok thinking about it i could've maybe created a list of just the zcoords)
            plantPos.Add(new Vector3(-8f, yPos, zTop));
            plantPos.Add(new Vector3(-4.75f, yPos, zTop));
            plantPos.Add(new Vector3(-1.55f, yPos, zTop));

            // second row
            plantPos.Add(new Vector3(-8f, yPos, zMid));
            plantPos.Add(new Vector3(-4.75f, yPos, zMid));
            plantPos.Add(new Vector3(-1.55f, yPos, zMid));

            // third row
            plantPos.Add(new Vector3(-8f, yPos, zLow));
            plantPos.Add(new Vector3(-4.75f, yPos, zLow));
            plantPos.Add(new Vector3(-1.55f, yPos, zLow));

            int p = Random.Range(0, Plants.Count); // pick an random location from the stored place (list)
            GameObject PlantType = Plants[p];

            int randomPos = Random.Range(0, plantPos.Count); // pick random number from 1st to 9th position

            Instantiate(Plants[p], plantPos[randomPos], Quaternion.identity); // instantiate needs the game object, vector3 location, rotation, spawn at that location (location as vector3)
        }
    }
}
