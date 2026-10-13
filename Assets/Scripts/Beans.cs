using System.Collections.Generic;
using UnityEngine;

public class Beans : MonoBehaviour
{
    public enum Stages
    {
        Seed,
        Clover,
        Mature,
        Wilted
    }


    public Player player;
    public Spawner spawner;
    public Stages CurrentStage = Stages.Seed;

    private Vector3 pos;
    public float WiltTimer;
    public float WaterTimer;

    private Renderer Bean;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Bean = GetComponent<Renderer>();
        spawner = FindAnyObjectByType<Spawner>();
        pos = transform.position;

        if (spawner != null)
        {
            spawner.beans.Add(this);
        }

        WiltTimer = 15f;
        WaterTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (WaterTimer > 0f)
        {
            WaterTimer -= Time.deltaTime;
        }

        if (CurrentStage == Stages.Mature)
        {
            WiltTimer -= Time.deltaTime;
            if (WiltTimer <= 0f)
            {
                CurrentStage = Stages.Wilted;
                Bean.material.color = Color.red;
                Debug.Log("your plant died");
            }
        }
    }

    public void Grow()
    {
        if (WaterTimer <= 0f)
        {
            switch (CurrentStage)
            {
                case Stages.Seed:
                    {
                        CurrentStage = Stages.Clover;
                        Player.Score++;
                        Debug.Log("seed is now clover, Score: " + Player.Score);
                        WaterTimer = 5f;
                        Bean.material.color = Color.yellow;
                        break;
                    }

                case Stages.Clover:
                    {
                        CurrentStage = Stages.Mature;
                        Debug.Log("clover is now a flower, ready to harvest");
                        Bean.material.color = Color.green;
                        break;
                    }

                case Stages.Mature:
                    {
                        break;
                    }

                case Stages.Wilted:
                    {
                        break;
                    }
            }
        }
    }
}
