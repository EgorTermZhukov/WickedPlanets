using System.Collections.Generic;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class Hand : MonoBehaviour
    {
        public List<PlanetView> Planets = new List<PlanetView>();
        public Transform Anchor;
        public float Offset = 1f;

        public void Add(PlanetView planet)
        {
            Planets.Add(planet);
            planet.transform.position = Anchor.transform.position + Vector3.right * Planets.Count * Offset;
        }
    }
}