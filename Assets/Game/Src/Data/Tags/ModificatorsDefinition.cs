using System;
using UnityEngine;

namespace Game.Src.Tags
{
    [Serializable]
    public class TagFaceDefinition : EntityComponentDefinition
    {
        [SerializeReference][SubclassSelector]
        public TagFaceSprite FaceSprite;
    }
    [Serializable]
    public class TagShapeDefinition : EntityComponentDefinition
    {
        [SerializeReference][SubclassSelector]
        public TagShapeSprite ShapeSprite;
    }
    [Serializable]
    public class TagColorDefinition : EntityComponentDefinition
    {
        [SerializeReference][SubclassSelector]
        public TagColor Color;
    }
}