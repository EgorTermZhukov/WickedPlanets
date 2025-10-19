using System;
using System.Collections.Generic;
using Game.Src.General;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class Hand : MonoBehaviour
    {
        public List<PlanetHandView> Planets = new List<PlanetHandView>();
        public Transform Anchor;
        public float Offset = 1f;

        public void Add(PlanetHandView planet)
        {
            Planets.Add(planet);
            planet.transform.SetParent(Anchor, false);
            
            //planet.OnEnter += ShowPlacement;
            //planet.OnExit += HidePlacement;
            planet.OnClicked += SendToMain;
            
            RepositionPlanets();
        }
        private void RepositionPlanets()
        {
            if (Planets.Count == 0) return;
    
            float totalWidth = (Planets.Count - 1) * Offset;
            float startX = -totalWidth / 2f;
    
            for (int i = 0; i < Planets.Count; i++)
            {
                Planets[i].transform.localPosition = Vector3.right * (startX + i * Offset);
            }
        }
        private void RepositionPlanetsCircular()
        {
            float angleStep = 360f / Planets.Count;
            float radius = Offset;
    
            for (int i = 0; i < Planets.Count; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Planets[i].transform.localPosition = position;
            }
        }

        public PlanetHandView Remove(PlanetHandView planet)
        {
            
            Planets.Remove(planet);
            planet.OnClicked -= SendToMain;
            
            RepositionPlanets();
            return planet;
        }
        public void SendToMain(PlanetHandView planet)
        {
            planet.gameObject.SetActive(false);
            G.Main.TryChoosePlanetPosition(planet);
        }

        public void CancelPlay(PlanetHandView planet)
        {
            planet.gameObject.SetActive(true);
        }
        /*public void ShowPlacement(PlanetHandView planet)
        {
            PlacementPointer.SetActive(true);
            // position to show
            // alright first get the unoccupied orbit
            var orbit = G.Main.Orbits.GetUnoccupiedOrbit;
            if (orbit == null)
                return;
            var position = orbit.CalculatePlanetPosition(orbit.StepsProgress / orbit.MoveSteps);
            PlacementPointer.transform.position = position;
            // now take the right value of the orbit, probably using circumference
        }*/
        
        // public void HidePlacement(PlanetHandView planet)
        // {
        //     PlacementPointer.SetActive(false);
        // }
    }
}