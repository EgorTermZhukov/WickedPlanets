using System.Collections;
using DG.Tweening;
using Game.Src.General;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class Orbit : MonoBehaviour
    {
        public OrbitRenderer OrbitRenderer;
        public PlanetView Planet;
        public float Radius;

        public int Steps;
        public int StepsProgress;

        public void SetOrbit(float radius, int steps = 30)
        {
            Radius = radius;
            Steps = (int)radius * 2;
            OrbitRenderer.DrawCircle(steps, radius);
            OrbitRenderer.DrawNotches(radius, 0.2f);
        }

        public void ClaimPlanet(PlanetView planet)
        {
            float circumferenceProgress = (float)StepsProgress / Steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * Radius;
            float y = yScaled * Radius;

            Vector3 currentPosition = new Vector3(x, y, 0f);
            
            var planetPosition = currentPosition;
            planet.transform.position = planetPosition;
            
            Planet = planet;
        }

        public IEnumerator TryAdvance()
        {
            if (Planet == null)
                yield break;
            StepsProgress++;
            
            float circumferenceProgress = (float)StepsProgress / Steps;

            Vector3 currentPosition = CalculatePlanetPosition(circumferenceProgress);
            Planet.transform.DOShakePosition(0.1f, new Vector3(0.1f, 0.1f, 0.1f));
            
            yield return new WaitUntil(G.Ticker.CreatePr(0.1f));
            
            Planet.transform.position = currentPosition;
            Planet.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
            AudioController.Instance.PlaySound2D("Marble", 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Medium));

            if (circumferenceProgress >= 1f)
            {
                StepsProgress = 0;
                G.Main.GainSun(Planet.transform.position);
            }
        }
        public Vector3 CalculatePlanetPosition(float circumferenceProgress)
        {
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * Radius;
            float y = yScaled * Radius;
            return new Vector3(x, y, 0f);
        }
    }
}