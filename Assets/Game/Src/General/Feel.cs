using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Src.General
{
    public class Feel : MonoBehaviour
    {
        public Vector3 CameraInitialPosition;
        public SpriteRenderer FadeSprite;
        public List<SpriteRenderer> Fades = new List<SpriteRenderer>();
        private void Awake()
        {
            if (G.Feel != null)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                G.Feel = this;
                CameraInitialPosition = Camera.main.transform.position;
            }
        }
        public void PunchScreen(float duration)
        {
            var camera = Camera.main;
            camera.transform.DOKill();
            camera.transform.position = CameraInitialPosition;
            camera.transform.DOPunchPosition(new Vector3(0.1f, 0.1f, 0f), duration);
        }
    }
}