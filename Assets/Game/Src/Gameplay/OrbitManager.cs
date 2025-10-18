using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class OrbitManager : MonoBehaviour
    {
        public List<Orbit> Orbits;
        public float OrbitOffset = 2f;
        public Orbit OrbitPfb;

        public IEnumerator AddOrbit()
        {
            var orbit = Instantiate(OrbitPfb, Vector3.zero, Quaternion.identity);
            Orbits.Add(orbit);
            
            float radius = Orbits.Count * OrbitOffset;
            Debug.Log(radius);
            
            orbit.SetOrbit(radius);
            orbit.transform.parent = transform;
            
            // if too big to see, scale map
            var camera = Camera.main;
            float currentOrbitRadius = (Orbits.Count + 1) * OrbitOffset;

            // Calculate maximum allowed radius before exceeding screen borders
            float maxAllowedRadius = CalculateMaxAllowedRadius(camera);

            // Check if we're approaching the limit (with 1 unit buffer)
            if (currentOrbitRadius >= maxAllowedRadius - 1f)
            {
                // Calculate required camera size to fit the orbit
                float requiredSize = CalculateRequiredCameraSize(camera, currentOrbitRadius);
    
                // Scale camera using DOTween
                camera.DOOrthoSize(requiredSize, 0.5f) // 0.5 second animation
                    .SetEase(Ease.OutCubic); // Smooth easing
            }

            yield break;

        }
        public IEnumerator AddPlanetToEmptyOrbitOrCreateIt(PlanetView planet)
        {
            var orbit = Orbits.Find(x=> x.Planet == null);
            if(orbit == null)
            {
                yield return AddOrbit();
                orbit = Orbits.Last();
            }

            orbit.ClaimPlanet(planet);
        }
        public IEnumerator AdvanceTurn()
        {
            foreach (var orbit in Orbits)
            {
                yield return orbit.TryAdvance();
            }
            yield break;
        }
        private float CalculateMaxAllowedRadius(Camera cam)
        {
            // Get camera's viewport size in world units
            float cameraHeight = 2f * cam.orthographicSize;
            float cameraWidth = cameraHeight * cam.aspect;
    
            // The maximum radius is half the smallest dimension (to keep orbit within view)
            return Mathf.Min(cameraWidth, cameraHeight) * 0.5f;
        }
        private float CalculateRequiredCameraSize(Camera cam, float orbitRadius)
        {
            // Calculate required orthographic size to fit the orbit with some padding
            float requiredSize;
    
            if (cam.aspect > 1f)
            {
                // Wider than tall - height is limiting factor
                requiredSize = orbitRadius + 1f; // +1 for the buffer
            }
            else
            {
                // Taller than wide - width is limiting factor
                requiredSize = (orbitRadius + 1f) / cam.aspect;
            }
    
            return requiredSize;
        }
    }
}