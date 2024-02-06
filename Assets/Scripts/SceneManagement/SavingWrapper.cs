using System;
using System.Collections;
using System.Collections.Generic;

using RPG.Saving;

using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Save()
        {
            _savingSystem.Save(defaultSaveFile);
        }

        private void Load()
        {
            // call to saving system load
            _savingSystem.Load(defaultSaveFile);
        }
    }
}
