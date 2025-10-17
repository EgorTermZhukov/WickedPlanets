using System;
using Game.Source;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Src.General
{
    public class Main : MonoBehaviour
    {
        public Interactor Interactor;
        private void Awake()
        {
            Interactor = new Interactor();
            Interactor.Init();
        }
        private void Start()
        {
            CMS.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Scenes/Main");
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                SceneManager.LoadScene("Scenes/Intro");
            }
        }
    }
}