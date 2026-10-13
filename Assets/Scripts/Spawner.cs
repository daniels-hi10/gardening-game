using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public List<GameObject> Plants = new List<GameObject>(); // list of plants: weeds, flowers, whatever whimsical shit
    public List<Vector3> vacantPos = new List<Vector3>(); // list of empty patches
    public List<Vector3> occupiedPos = new List<Vector3>(); // list of full patches
    public List<Vector3> weedPos = new List<Vector3>(); // list with weeds
    public List<Beans> beans = new List<Beans>();

    public float Timer; // timer before next spawn
    public float SpawnRate; // interval between plants to spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlantPos();
        Timer = 0f;
        SpawnRate = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            SpawnWeed(); // spawn plant
            Timer = SpawnRate; // reset timer IF the weed isn't alr planted
        }
    }

    void PlantPos()
    {
        float yPos = 0.13f;
        float zTop = 16.5f;
        float zMid = 12.85f;
        float zLow = 9.3f;

        // first row
        vacantPos.Add(new Vector3(-8f, yPos, zTop));
        vacantPos.Add(new Vector3(-4.75f, yPos, zTop));
        vacantPos.Add(new Vector3(-1.55f, yPos, zTop));

        // second row
        vacantPos.Add(new Vector3(-8f, yPos, zMid));
        vacantPos.Add(new Vector3(-4.75f, yPos, zMid));
        vacantPos.Add(new Vector3(-1.55f, yPos, zMid));

        // third row
        vacantPos.Add(new Vector3(-8f, yPos, zLow));
        vacantPos.Add(new Vector3(-4.75f, yPos, zLow));
        vacantPos.Add(new Vector3(-1.55f, yPos, zLow));
    }

    void SpawnWeed()
    {
        if (vacantPos.Count == 0) // if there are no vacant spots left, stop growing weeds
        {
            return;
        }

        int i = Random.Range(0, vacantPos.Count);
        Vector3 targetPos = vacantPos[i];

        int p = Random.Range(0, Plants.Count); // pick an random location from the stored place (list)
        GameObject PlantType = Plants[p];

        Instantiate(PlantType, targetPos, Quaternion.identity); // instantiate needs the game object, vector3 location, rotation, spawn at that location (location as vector3)
        vacantPos.Remove(targetPos); // removes the recently spawned weed from overlapping with plants
        weedPos.Add(targetPos); // adds the recently spawned to occupied so it can't overlap with plants
    }

    public void FreeUpSpot(Vector3 empty)
    {
        if (occupiedPos.Contains(empty)) // if the patch is empty
        {
            occupiedPos.Remove(empty); // remove empty dirt from occupied so that it can spawn again
            vacantPos.Add(empty); // adds the empty dity back to the list of vacant spots
        }

        if (weedPos.Contains(empty))
        {
            weedPos.Remove(empty);
            vacantPos.Add(empty);
        }
    }
}
