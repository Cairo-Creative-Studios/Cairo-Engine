﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CairoEngine
{
    /// <summary>
    /// The Level Manager is a layer on top of Scene Management that allows you to spawn and manipulate the Scene as a whole.
    /// </summary>
    public class LevelManager
    {

        // Abstract Objects
        /// <summary>
        /// The spawn points that currently exist in the Game.
        /// </summary>
        public static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();


        /// <summary>
        /// The levels that are currently loaded into the game.
        /// </summary>
        private static List<Level> levels = new List<Level>();

        /// <summary>
        /// The levels that can be loaded into the Game
        /// </summary>
        private static List<LevelInfo> levelInfos = new List<LevelInfo>();

        public static void Init()
        {
            levelInfos.AddRange(Resources.LoadAll<LevelInfo>(""));
            CreateLevelForScene(SceneManager.GetActiveScene());
        }

        public static void Update()
        {

        }

        /// <summary>
        /// Creates a Level for the given Scene.
        /// </summary>
        /// <param name="scene">Scene.</param>
        public static Level CreateLevelForScene(Scene scene)
        {
            GameObject levelObject = new GameObject
            {
                name = "Level_" + scene.name
            };
            Level level = levelObject.AddComponent<Level>();
            level.info = GetLevelInfoForScene(scene.name);

            GameObject[] gameObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in gameObjects)
            {
                rootObject.transform.parent = level.transform;
                SpawnPoint rootObjectAsSpawn = rootObject.GetComponent<SpawnPoint>();

                if (rootObjectAsSpawn != null)
                    spawnPoints.Add(rootObjectAsSpawn);
            }
            level.childInstances.AddRange(gameObjects);

            return level;
        }
        /// <summary>
        /// Loads the given level.
        /// </summary>
        /// <param name="ID">
        /// The ID of the level to load.
        /// </param>
        public static void LoadLevel(string ID)
        {
            foreach (LevelInfo levelInfo in levelInfos)
            {
                if (levelInfo.ID == ID)
                {
                    GameObject levelObject = new GameObject
                    {
                        name = "Level_" + levelInfo.ID
                    };
                    Level level = levelObject.AddComponent<Level>();
                    level.info = levelInfo;
                    levels.Add(level);
                    return;
                }
            }
            Debug.LogWarning("Level " + ID + " not found when Load was requested.");
        }

        /// <summary>
        /// Draws the given level.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        public static void DrawLevel(string ID)
        {
            int index = FindLevelIndex(ID);
            if (index != -1)
                levels[index].Draw();


        }

        /// <summary>
        /// Removes the level from the game.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        public static void RemoveLevel(string ID)
        {
            int index = FindLevelIndex(ID);
            if (index != -1)
            {
                levels.RemoveAt(index);
                levels[index].Destroy();
            }
        }

        public static void MoveLevel(string ID, Vector3 moveAmount)
        {
            levels[FindLevelIndex(ID)].transform.position += moveAmount;
        }

        /// <summary>
        /// Finds the index of the level.
        /// </summary>
        /// <returns>The level index.</returns>
        /// <param name="ID">Identifier.</param>
        private static int FindLevelIndex(string ID)
        {
            int index = -1;
            foreach (Level level in levels)
            {
                index++;
                if (level.info.ID == ID)
                {
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// Searches for Level Info for the Given scene, or creates new Level Info.
        /// </summary>
        /// <returns>The level info for scene.</returns>
        /// <param name="sceneName">Scene name.</param>
        private static LevelInfo GetLevelInfoForScene(string sceneName)
        {
            foreach (LevelInfo levelInfo in levelInfos)
            {
                if (levelInfo.sceneName == sceneName)
                {
                    return levelInfo;
                }
            }

            LevelInfo newLevelInfo = ScriptableObject.CreateInstance<LevelInfo>();
            newLevelInfo.sceneName = sceneName;
            newLevelInfo.name = sceneName;

            return newLevelInfo;
        }
    }
}