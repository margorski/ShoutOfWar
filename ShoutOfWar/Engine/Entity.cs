using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Engine
{
    sealed class Entity
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

        public IEnumerable<T> GetComponent<T>() where T : Component
        {
            var t = components.FindAll(c => { return c.GetType() == typeof(T); });
            var b = t.Select(c => c as T);

            return components.FindAll(c => c.GetType() == typeof(T)).Select(c => c as T);
        }

        // Run init after adding child components
        public void Init()
        {
            foreach (var component in components) component.Init();
        }

        public void Draw(GameTime gameTime)
        {
            if (!enabled) return;
            foreach (var component in components) component.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (!enabled) return;
            foreach (var component in components) component.Update(gameTime);            
        }
    }
}
