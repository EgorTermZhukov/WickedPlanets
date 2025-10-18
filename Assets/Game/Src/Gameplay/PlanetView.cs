using System.Collections.Generic;
using Game.Src.Tags;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Src.Gameplay
{
    public class PlanetState
    {
        public CMSEntity Model;
        public List<EntityComponentDefinition> Components = new List<EntityComponentDefinition>();
        public T Get<T>() where T : EntityComponentDefinition, new()
        {
            return Components.Find(m => m is T) as T;
        }
        public bool Is<T>(out T unknown) where T : EntityComponentDefinition , new()
        {
            unknown = Get<T>();
            return unknown != null;
        }
        public void AddFromEntityList(List<CMSEntity> entities)
        {
            foreach(var entity in entities)
            {
                foreach (var component in entity.components)
                {
                    if(!Components.Contains(component))
                        Components.Add(component);
                }
            }
        }
        public void CopyFromEntity(CMSEntity toCopy)
        {
            Model = toCopy;
            foreach (var component in toCopy.components)
            {
                var copy = component.DeepCopy();
                Components.Add(copy);
            }
        }

        public PlanetView View;
    }
    public class PlanetView : MonoBehaviour
    {
        public SpriteRenderer Shape;
        public SpriteRenderer Face;
        
        public void SetData(PlanetState data)
        {
            Face.sprite = data.Get<TagFaceDefinition>().FaceSprite.Face;
            Shape.sprite = data.Get<TagShapeDefinition>().ShapeSprite.Shape;
            Shape.color = data.Get<TagColorDefinition>().Color.Color;
        }
    }
}