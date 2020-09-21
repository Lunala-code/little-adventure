using little_adventure.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Physics {
    public class World {
        private List<Body> _env;
        private Vector2 _gravity;


        public World() {
            this._env = new List<Body>();
            this._gravity = Vector2.Zero;
        }

        public World(Vector2 gravity) {
            this._env = new List<Body>();
            this._gravity = gravity;
        }

        public void setGravity(Vector2 gravity) {
            this._gravity = gravity;
        }

        public Body CreateBody(int heigth, int width, Vector2 position, BodyType type) {
            Body body = new Body(heigth, width, position, type);
            this._env.Add(body);
            return body;
        }

        public void Update() {
            foreach (var b in this._env) {
                
                if (b.Type != Model.BodyType.STATIC) {
                    b.preUpdate();
                    b.Velocity += _gravity;
                    foreach(var b2 in this._env) {
                        if (b != b2)
                            b.Collision(b2);
                    }
                    b.Update();
                }
                    
            }

        }

    }
}
