using little_adventure.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Physics {
    public class Body {

        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Position;
        private int _height;
        private int _width;
        private BodyType _type;

        private bool _botCollision;
        private bool _topCollision;
        private bool _leftCollision;
        private bool _rightCollision;

        public bool BotCollision { get { return this._botCollision; } }
        public bool TopCollision { get { return this._topCollision; } }
        public bool LeftCollision { get { return this._leftCollision; } }
        public bool RightCollision { get { return this._rightCollision; } }

        public BodyType Type { get { return this._type; } }

        public Rectangle Rectangle {

            get {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y,
                    this._width, this._height);
            }
        }

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="heigth">height of body</param>
        /// <param name="width">width of body</param>
        /// <param name="position">initial position of body</param>
        public Body(int heigth, int width, Vector2 position, BodyType type) {
            this.Position = position;
            this._height = heigth;
            this._width = width;
            this._type = type;
        }

        /// <summary>
        /// Apply force to the body to move it
        /// </summary>
        /// <param name="velocity"></param>
        public void applyForce(Vector2 velocity) {
            this.Velocity = velocity;
        }

        public void preUpdate() {
            this._botCollision = false;
            this._topCollision = false;
            this._leftCollision = false;
            this._rightCollision = false;
        }
        /// <summary>
        /// Update velocity and collision var
        /// </summary>
        public void Update() {
            this.Position += this.Velocity;
            
        }

        /// <summary>
        /// Get collision between this and another body
        /// </summary>
        /// <param name="body"></param>
        public void Collision(Body body) {
            

            /// compute the collision
            bool bot = this.IsTouchingTop(body);
            bool top = this.IsTouchingBottom(body);
            bool left = this.IsTouchingRight(body);
            bool right = this.IsTouchingLeft(body);
            
            /// velocity management
            if (bot) {
                this.Velocity.Y = 0;
                this.Position.Y = body.Position.Y - this._height; //
                    
            }
            if (top)
                this.Velocity.Y = 1;
            if (left || right)
                this.Velocity.X = 0;

            this._botCollision |= bot;
            this._topCollision |= top;
            this._leftCollision |= left;
            this._rightCollision |= right;
            
        }


        #region Colloision
        private bool IsTouchingLeft(Body body) {
            return this.Rectangle.Right + this.Velocity.X > body.Rectangle.Left &&
              this.Rectangle.Left < body.Rectangle.Left &&
              this.Rectangle.Bottom > body.Rectangle.Top &&
              this.Rectangle.Top < body.Rectangle.Bottom;
        }

        private bool IsTouchingRight(Body body) {
            return this.Rectangle.Left + this.Velocity.X < body.Rectangle.Right &&
              this.Rectangle.Right > body.Rectangle.Right &&
              this.Rectangle.Bottom > body.Rectangle.Top &&
              this.Rectangle.Top < body.Rectangle.Bottom;
        }

        private bool IsTouchingTop(Body body) {
            return this.Rectangle.Bottom + this.Velocity.Y > body.Rectangle.Top &&
              this.Rectangle.Top < body.Rectangle.Top &&
              this.Rectangle.Right > body.Rectangle.Left &&
              this.Rectangle.Left < body.Rectangle.Right;
        }

        private bool IsTouchingBottom(Body body) {
            return this.Rectangle.Top + this.Velocity.Y < body.Rectangle.Bottom &&
              this.Rectangle.Bottom > body.Rectangle.Bottom &&
              this.Rectangle.Right > body.Rectangle.Left &&
              this.Rectangle.Left < body.Rectangle.Right;
        }

        #endregion
    }
}
