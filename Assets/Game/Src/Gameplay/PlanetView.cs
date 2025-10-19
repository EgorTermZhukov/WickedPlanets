using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Src.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public void CopyFromEntityList(List<CMSEntity> entities)
        {
            foreach(var entity in entities)
            {
                foreach (var component in entity.components)
                {
                    if (!Components.Contains(component))
                        Components.Add(component.DeepCopy());
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

        public int Damage;
        public PlanetView View;
    }
    public class PlanetView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public PlanetState PlanetState;
        
        public Transform Point;
        public GameObject PlacementShower;
        
        public SpriteRenderer Shape;
        public SpriteRenderer Face;

        public Vector3 InitialScale; 
        
        public event Action<PlanetView> OnClicked;
        public void SetData(PlanetState data)
        {
            Face.sprite = data.Get<TagFaceDefinition>().FaceSprite.Face;
            Shape.sprite = data.Get<TagShapeDefinition>().ShapeSprite.Shape;
            Shape.color = data.Get<TagColorDefinition>().Color.Color;
            
            PlanetState = data;
            InitialScale = transform.localScale;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
        public void Ghost()
        {
            PlacementShower.SetActive(true);
            var shapeColor = Shape.color;
            shapeColor.a = 0.5f;
            Shape.color = shapeColor;
            var faceColor = Color.white;
            faceColor.a = 0.5f;
            Face.color = faceColor;
        }
        public void UnGhost()
        {
            transform.DOKill();
            transform.localScale = InitialScale;
            PlacementShower.SetActive(false);
            var normalColor = Shape.color;
            normalColor.a = 1;
            Shape.color = normalColor;
            Face.color = Color.white;
        }
        // i dunno, do something when can't be moved
        public void FlashRed()
        {
            
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill();
            transform.localScale = InitialScale;
            transform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill();
            transform.localScale = InitialScale;
        }
    }
}