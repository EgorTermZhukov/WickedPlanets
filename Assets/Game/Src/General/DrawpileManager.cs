using System.Collections.Generic;
using Game.Src.Gameplay;

namespace Game.Src.General
{
    public class DrawpileManager
    {
        public List<PlanetState> draw = new List<PlanetState>();
        public List<PlanetState> discard = new List<PlanetState>();
    }
}