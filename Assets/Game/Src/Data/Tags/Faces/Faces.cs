using System;
using System.Collections;
using Game.Source;
using Game.Src.Gameplay;
using Game.Src.General;
using UnityEngine;

namespace Game.Src.Tags
{
    [Serializable]
    public class TagDamage : EntityComponentDefinition
    {
        public int Delta;
    }

    [Serializable]
    public class TagSadFace : TagFaceDefinition
    {

    }
    [Serializable]
    public class TagPokerFace : TagFaceDefinition
    {
    }
}