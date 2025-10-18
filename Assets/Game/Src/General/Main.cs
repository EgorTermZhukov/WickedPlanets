using System;
using System.Collections;
using System.Collections.Generic;
using Game.Source;
using Game.Src.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Src.General
{
    public class Main : MonoBehaviour
    {
        
        public Interactor Interactor;
        public PlanetView PlanetPfb;
        public OrbitManager Orbits;
        public Hand Hand;

        public TMP_Text SunText;

        public int SunAmount = 0;
        public bool Advancing = false;
        private void Awake()
        {
            G.Main = this;
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
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Orbits.AddOrbit());
            }
            else if (Input.GetMouseButtonDown(0))
            {
                var planet = GetRandomPlanet();
                AudioController.Instance.PlaySound2D("Ahh", 0.3f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Large));
                StartCoroutine(Orbits.AddPlanetToEmptyOrbitOrCreateIt(planet));
            }
            else if (Input.GetMouseButtonDown(1) && !Advancing)
            {
                StartCoroutine(AdvanceAll());
            }
        }

        public IEnumerator AdvanceAll()
        {
            Advancing = true;
            yield return Orbits.AdvanceTurn();
            Advancing = false;
        }
        public PlanetView GetRandomPlanet()
        {
            var planet = Instantiate(PlanetPfb);
            
            var planetFaceModel = CMS.Get<CMSEntity>("CMS/Models/Planets/SadFace");
            
            Debug.Log(planetFaceModel.id);

            var planetState = new PlanetState();
            planetState.View = planet;
            
            planetState.CopyFromEntity(planetFaceModel);
            
            planet.SetData(planetState);
            return planet;
        }

        public void GainSun(Vector3 sourcePosition)
        {
            StartCoroutine(GainSunSequence(sourcePosition));
        }

        public IEnumerator GainSunSequence(Vector3 sourcePosition)
        {
            var textPos = SunText.transform.position;
            G.ParticleController.SpawnAndMoveToWithDuration(0.2f, sourcePosition, Vector3.zero);
            yield return new WaitUntil(G.Ticker.CreatePr(0.2f));
            SunAmount++;
            SunText.text = SunAmount.ToString();
            AudioController.Instance.PlaySound2D("Sigh", 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        }
    }
}