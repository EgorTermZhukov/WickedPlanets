using System.Collections;
using DG.Tweening;
using Game.Src.General;
using Game.Src.Tags;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class Orbit : MonoBehaviour
    {
        public OrbitRenderer OrbitRenderer;
        
        public PlanetView Planet;
        public GameObject FullTurnTarget;
        
        public float Radius;

        public int StartingStep;
        public int MoveSteps;
        public int StepsProgress;

        public void SetOrbit(float radius, int moveSteps = 4, int steps = 30)
        {
            Radius = radius;
            MoveSteps = moveSteps;
            OrbitRenderer.DrawCircle(steps, radius);
            OrbitRenderer.DrawNotches(MoveSteps, radius, 0.2f);
        }
        public void ClaimPlanet(int circumference, PlanetView planet)
        {
            StartingStep = circumference;
            Vector3 planetPosition = CalculatePlanetPosition(circumference);
            FullTurnTarget.SetActive(true);
            FullTurnTarget.transform.position = planetPosition;
            
            OrbitRenderer.Notches[circumference].startColor = Color.yellow;
            
            planet.transform.position = planetPosition;
            Planet = planet;
        }
        // really long method, probably should split it later on, though i dont really see points of division there oof
        public IEnumerator TryAdvance()
        {
            if (Planet == null)
                yield break;

            G.Main.Orbits.MovePointerTo(Planet.Point.transform.position);
            Planet.transform.DOShakePosition(0.3f, new Vector3(0.1f, 0.1f, 0.1f));
            
            yield return new WaitUntil(G.Ticker.CreatePr(0.3f));

            StepsProgress++;
            
            int actualStep = (StartingStep + StepsProgress) % MoveSteps;
            
            Vector3 currentPosition = CalculatePlanetPosition(actualStep);
            
            G.Main.Orbits.MovePointerTo(Planet.Point.transform.position);
            
            Planet.transform.position = currentPosition;
            Planet.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
            AudioController.Instance.PlaySound2D("Marble", 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Medium));
            
            yield return OnMoved(Planet.PlanetState);

            if (StepsProgress >= MoveSteps)
            {
                StepsProgress = 0;
                yield return G.Main.DealDamageToSun(currentPosition, Planet.PlanetState.Damage, Planet.PlanetState.Get<TagColorDefinition>().Color.Color);
                yield return OnAttack(Planet.PlanetState);
            }
        }
        public IEnumerator OnMoved(PlanetState planet)
        {
            var moveInteractions = G.Main.Interactor.FindAll<IOnMoved>();
            foreach (var move in moveInteractions)
            {
                yield return move.OnMoved(planet, this);
            }
        }
        public IEnumerator OnAttack(PlanetState planet)
        {
            var reachInteractions = G.Main.Interactor.FindAll<IOnAttack>();
            foreach (var reached in reachInteractions)
            {
                yield return reached.OnAttack(planet, this);
            }
        }
        public Vector3 CalculatePlanetPosition(int step)
        {
            float circumferenceProgress = (float)step / MoveSteps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * Radius;
            float y = yScaled * Radius;
            return new Vector3(x, y, 0f);
        }

        public int CalculatePlanetCircFromMouseDir(Vector3 mouseDirection)
        {
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x);
    
            // Convert angle from [-π, π] to [0, 2π]
            if (angle < 0)
                angle += 2 * Mathf.PI;
            // Convert radians to circumference progress (0 to 4 range)
            int progressInt = Mathf.RoundToInt((angle / (2 * Mathf.PI) * 4));
            
            if (progressInt == 4) 
                progressInt = 0;
            
            return progressInt;
        }
    }
}