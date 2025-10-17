using System.Collections;
using System.Collections.Generic;
using Game.Src.General;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    public bool Transitioning = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Transitioning == false)
        {
            Transitioning = true;
            StartCoroutine(TransitionToTheGame());
        }
    }

    public IEnumerator TransitionToTheGame()
    {
        AudioController.Instance.PlaySound2D("Frog");
        G.Feel.PunchScreen(1f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/Main");
    }
}
