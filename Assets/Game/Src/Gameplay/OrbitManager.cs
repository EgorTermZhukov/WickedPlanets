using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class OrbitManager : MonoBehaviour
    {
        public GameObject Pointer;
        
        public List<Orbit> Orbits;
        public float OrbitOffset = 2f;
        
        public Orbit OrbitPfb;

        private void Start()
        {
        }

        public IEnumerator RemoveAll()
        {
            yield break;
        }
        public Orbit AddOrbit()
        {
            var orbit = Instantiate(OrbitPfb, Vector3.zero, Quaternion.identity);
            Orbits.Add(orbit);
            orbit.transform.parent = transform;
            
            float radius = Orbits.Count * OrbitOffset;
            
            orbit.SetOrbit(radius);
            
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

            return orbit;
        }

        public Orbit GetUnoccupiedOrbit => Orbits.Find(x => x.Planet == null);

        public IEnumerator AddPlanetToOrbit(PlanetView planet, int circ, Orbit orbit)
        {
            orbit.ClaimPlanet(circ, planet);
            yield break;
        }
        // public IEnumerator AddPlanetToEmptyOrbitOrCreateIt(PlanetView planet)
        // {
        //     var orbit = GetUnoccupiedOrbit;
        //     if(orbit == null)
        //     {
        //         yield return AddOrbit();
        //         orbit = Orbits.Last();
        //     }
        //
        //     orbit.ClaimPlanet(planet);
        // }
        public IEnumerator AdvanceTurn()
        {
            Pointer.SetActive(true);
            foreach (var orbit in Orbits)
            {
                yield return orbit.TryAdvance();
            }
            Pointer.SetActive(false);
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
        public void MovePointerTo(Vector3 position)
        {
            Pointer.transform.position = position;
        }
    }
}