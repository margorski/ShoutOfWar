using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Engine
{
    public sealed class Entity
    {
        public string Id { private set; get; }
        public Vector2 position;
        public float rotation;
        public float scale = 1.0f;
        public bool enabled = true;

        private List<Component> components = new List<Component>();
        
        public Entity(string Id)
        {
            this.Id = Id;
        }

        public void AddComponent(Component component)
        {
            component.parent = this;
            components.Add(component);
        }

        public bool HasComponent<T>() where T : Component
        {            
            return components.Exists(c => c.GetType() == typeof(T));
        }

        public T GetComponent<T>(bool onlyEnabled = true) where T : Component
        {
            return components.FindAll(c => c.GetType() == typeof(T) && (!onlyEnabled || c.enabled)).Select(c => c as T).FirstOrDefault();
        }

        public IEnumerable<T> GetComponents<T>(bool onlyEnabled = true) where T : Component
        {
            return components.FindAll(c => c.GetType() == typeof(T) && (!onlyEnabled || c.enabled)).Select(c => c as T);
        }

        // Run init after adding child components
        public void Init()
        {
            foreach (var component in components) component.Init();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var component in components)
            {
                if (component.enabled) component.Draw(gameTime);
            }
        }
        public void DrawDebug(GameTime gameTime)
        {
            foreach (var component in components)
            {
                if (component.enabled) component.DrawDebug(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                if (component.enabled) component.Update(gameTime);
            }
        }
    }
}
