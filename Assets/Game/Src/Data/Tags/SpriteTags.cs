using System;
using UnityEngine;

namespace Game.Src.Tags
{
    [Serializable]
    public class TagFaceSprite : EntityComponentDefinition
    {
        public Sprite Face;
    }
    [Serializable]
    public class TagShapeSprite : EntityComponentDefinition
    {
        public Sprite Shape;
    }

    [Serializable]
    public class TagColor : EntityComponentDefinition
    {
        public Color Color;
    }
}