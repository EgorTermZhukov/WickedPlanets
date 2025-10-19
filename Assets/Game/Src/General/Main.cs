using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Source;
using Game.Src.Gameplay;
using Game.Src.Tags;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Src.General
{
    public class Main : MonoBehaviour
    {
        public Interactor Interactor;
        public PlanetView PlanetPfb;
        public PlanetHandView PlanetHandPfb;
        public OrbitManager OrbPfb;
        public OrbitManager Orbits;
        public Hand Hand;
        public DrawpileManager Drawpile;
        public GameObject CombatUI;
        

        // use this to add planets
        public PlanetBag PlanetBag;
        public EncounterManager Encounter;
        private void Awake()
        {
            G.Main = this;
            Interactor = new Interactor();
            Interactor.Init();
        }
        private void Start()
        {
            PlanetBag = new PlanetBag();
            CMS.Init();
            AudioController.Instance.SetLoopAndPlay("SpaceAmbient");
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
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                var planet = GetRandomPlanet();
                Hand.Add(planet);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                var combatState = CreateCombatState();
                StartCoroutine(Encounter.Setup(combatState, OrbPfb));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(AdvanceAll());
            }
        }

        public void ShowCombatUI()
        {
            CombatUI.SetActive(true);
        }

        public EncounterState CreateCombatState()
        {
            var encounterState = new EncounterState();
            var enemyModel = CMS.Get<CMSEntity>("Enemy_1");
            var enemyState = new EnemyState();
            enemyState.SetState(enemyModel);
            encounterState.EnemyState = enemyState;
            
            var playerState = new PlayerState()
            {
                AvaliableTurns = 8,
                PlayerHp = 15
            };
            encounterState.PlayerState = playerState;
            return encounterState;
        }
        public void HideCombatUI()
        {
            CombatUI.SetActive(false);
        }
        public void TryChoosePlanetPosition(PlanetHandView planet)
        {
            StartCoroutine(ChoosePlanetPosition(planet));
        }
        public IEnumerator ChoosePlanetPosition(PlanetHandView handViewPlanet)
        {
            var orbit = Orbits.GetUnoccupiedOrbit;
            
            if (orbit == null)
                orbit = Orbits.AddOrbit();
            var ghostPlanet = CreatePlanetFromState(handViewPlanet.PlanetState);
            ghostPlanet.Ghost();
            
            while (true)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(ghostPlanet.gameObject);
                    Hand.CancelPlay(handViewPlanet);
                    yield break;
                }
                
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y);
                var mouseDirection = mouseWorldPosition.normalized;

                int circ = orbit.CalculatePlanetCircFromMouseDir(mouseDirection);
                var position = orbit.CalculatePlanetPosition(circ);
                Debug.Log(position);

                ghostPlanet.transform.position = position;
                
                if (Input.GetMouseButtonDown(0))
                {
                    ghostPlanet.UnGhost();
                    StartCoroutine(PlayPlanet(handViewPlanet, ghostPlanet, circ, position, orbit));
                    yield break;
                }
                yield return null;
            }
            yield break;
        }
        private IEnumerator PlayPlanet(PlanetHandView handViewPlanet, PlanetView planetView, int circ, Vector3 position, Orbit orbit)
        {
            var state = handViewPlanet.PlanetState;
            
            Hand.Remove(handViewPlanet);
            Destroy(handViewPlanet.gameObject);
            
            planetView.SetData(state);
            state.View = planetView;
            
            G.Feel.PunchScreen(0.2f);
            AudioController.Instance.PlaySound2D("Ahh", 0.3f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Large));

            yield return Orbits.AddPlanetToOrbit(planetView, circ, orbit);
        }
        public PlanetView CreatePlanetFromState(PlanetState planetState)
        {
            var planet = Instantiate(PlanetPfb);
            
            planet.SetData(planetState);
            return planet;
        }
        public IEnumerator AdvanceAll()
        {
            int CurrentSteps = 8;
            while (CurrentSteps >= 0)
            {
                Debug.Log("Step: " + CurrentSteps);
                CurrentSteps -= 1;
                yield return Orbits.AdvanceTurn();
                yield return new WaitUntil(G.Ticker.CreatePr(0.2f));
            }
        }
        public PlanetHandView GetRandomPlanet()
        {
            var planet = Instantiate(PlanetHandPfb);
            
            var planetFaceModels = CMS.GetAll<CMSEntity>().Where(x=> x.Is<TagFaceDefinition>()).ToList();
            var planetShapeModels = CMS.GetAll<CMSEntity>().Where(x => x.Is<TagShapeDefinition>()).ToList();
            var planetColorModels = CMS.GetAll<CMSEntity>().Where(x => x.Is<TagColorDefinition>()).ToList();

            var faceModel = planetFaceModels[UnityEngine.Random.Range(0, planetFaceModels.Count)];
            var shapeModel = planetShapeModels[UnityEngine.Random.Range(0, planetShapeModels.Count)];
            var colorModel = planetColorModels[UnityEngine.Random.Range(0, planetColorModels.Count)];

            var models = new List<CMSEntity>()
            {
                faceModel,
                shapeModel,
                colorModel
            };
            
            var planetState = new PlanetState();
            
            planetState.CopyFromEntityList(models);
            
            planet.SetData(planetState);
            return planet;
        }

        public void StartTurn()
        {
            
        }
        public IEnumerator DealDamageToSun(Vector3 sourcePosition, int damage, Color color)
        {
            Encounter.State.EnemyState.EnemyHp -= damage;
            Encounter.UpdateEverything();
            
            G.ParticleController.SpawnAndMoveToWithDuration(0.5f, sourcePosition, Vector3.zero, color);
            yield return new WaitUntil(G.Ticker.CreatePr(0.5f));
            AudioController.Instance.PlaySound2D("Sigh", 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        }
    }
}