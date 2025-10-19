using System;
using Game.Src.General;
using UnityEngine;

namespace Game.Src.Tags
{
    [Serializable]
    public class TagEnemyView : EntityComponentDefinition
    {
        [SerializeReference]
        public EnemyView View;
    }
}