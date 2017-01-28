using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameTick : MonoBehaviour {

    private int createdOnCurrentLevel = 0;
    private int toSpawn = 10;
    private float minimumSpace = 3;
    private float currentLevel = 0;
    private float height;
    private System.Random random = new System.Random();
    private bool trigger;
    private List<GameObject> asteroidsThisLevel;

    public enum GameMode { Endless, Stages, Attack }
    public enum Difficulty { Beginner, Easy, Normal, Hard, Extreme }


    public bool procedural = true;
    public Transform stars;
    public GameMode mode = GameMode.Endless;
    public Difficulty difficulty;
    public GameObject TemplateAsteroid;

    // Use this for initialization
    void Start() {
        difficulty = Difficulty.Beginner;

        InitDiff();

        //Asteroid's own width
        minimumSpace += 1;

        asteroidsThisLevel = new List<GameObject>();

        Generate();
    }


    void Generate() {
        Loom.Initialize();

        new Thread(() => {
            while(true) {

                Loom.QueueOnMainThread(() => {
                    #region Asteroid Procedural Generation
                    if(procedural && createdOnCurrentLevel < toSpawn) {
                        GameObject asteroid = TemplateAsteroid;

                        try {
                            Vector3 pos = new Vector3(random.Next(-6, 6),
                                random.Next((int)height + 7, (int)height + 20), 1);

                            bool rethrow = false;
                            float minDistance = 10;

                            foreach(GameObject go in asteroidsThisLevel) {
                                Vector3 vec3 = go.transform.position;
                                float distance = Vector3.Distance(vec3, pos);

                                if(distance < minDistance) {
                                    minDistance = distance;
                                }
                            }


                            if(minDistance < minimumSpace) {
                                rethrow = true;
                                Debug.Log("Rethrowinng! (Distance: " + minDistance + ")");
                            }

                            if(!rethrow) {
                                asteroid.transform.position = pos;

                                asteroid = Instantiate(asteroid);

                                currentLevel = height;

                                createdOnCurrentLevel++;
                                asteroidsThisLevel.Add(asteroid);
                            }
                        } catch(Exception) { }
                    }
                    #endregion

                    if(currentLevel + 5 < height) {
                        createdOnCurrentLevel = 0;

                        //Do some cleaning
                        foreach(GameObject go in asteroidsThisLevel) {
                            Destroy(go, 10);
                        }

                        asteroidsThisLevel.Clear();
                    }
                });

                Thread.Sleep(10);

            }
        }).Start();
    }

    // Update is called once per frame
    void Update() {
        InitDiff();

        height = this.transform.position.y;

        if(mode == GameMode.Endless) {
            stars.position = new Vector3(transform.position.x, transform.position.y, 5);
        } else if(mode == GameMode.Stages) {
            //Change Background depending on Height
        } else {
            //Do not Change Stars position
        }
    }

    void InitDiff() {
        //TODO: Show Message when difficulty changes
        if(height > 50) {
            difficulty = Difficulty.Easy;

            if(height > 500) {
                difficulty = Difficulty.Normal;

                if(height > 1500) {
                    difficulty = Difficulty.Hard;

                    if(height > 3000) {
                        difficulty = Difficulty.Extreme;
                    }
                }
            }
        }

        switch(difficulty) {
            case Difficulty.Beginner:
                toSpawn = 5;
                minimumSpace = 5;
                break;
            case Difficulty.Easy:
                toSpawn = 8;
                minimumSpace = 3.3f;
                break;
            case Difficulty.Normal:
                toSpawn = 10;
                minimumSpace = 2.3f;
                break;
            case Difficulty.Hard:
                toSpawn = 15;
                minimumSpace = 1;
                break;
            case Difficulty.Extreme:
                toSpawn = 25;
                minimumSpace = 0.7f;
                break;
        }
    }
}
