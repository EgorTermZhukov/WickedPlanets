using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Src.General
{
    public enum CursorType
    {
        Regular,
        Examine,
        Interact
    }
    public class PlayerCursor : MonoBehaviour
    {
        public CursorType Cursor;

        public bool IsVisible
        {
            get => SpriteRenderer.enabled;
            set => SpriteRenderer.enabled = value;
        }
        public SpriteRenderer SpriteRenderer;
        public List<Sprite> Sprites;
        private void Awake()
        {
            if (G.PlayerCursor != null)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                
                UnityEngine.Cursor.visible = false;
                IsVisible = true;
                
                Sprites = Resources.LoadAll<Sprite>("Graphics/Cursor").ToList();
                SetCursorType(CursorType.Regular);
                
                G.PlayerCursor = this;
            }
        }
        public void SetCursorType(CursorType cursor)
        {
            Sprite sprite = null;
            switch (cursor)
            {
                case CursorType.Regular:
                    sprite = Sprites.Find(s => s.name == cursor.ToString());
                    break;
                case CursorType.Examine:
                    sprite = Sprites.Find(s => s.name == cursor.ToString());
                    break;
                case CursorType.Interact:
                    sprite = Sprites.Find(s => s.name == cursor.ToString());
                    break;
            }
            SpriteRenderer.sprite = sprite;
            Cursor = cursor;
        }
        private void Update()
        {
            var mousePos = Input.mousePosition;
            var position = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}