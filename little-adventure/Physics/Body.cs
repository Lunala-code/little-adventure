using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Physics {
    class Body {

        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Position;
        private int _height;
        private int _width;

        private bool _botCollision;
        private bool _topCollision;
        private bool _leftCollision;
        private bool _rightCollision;

        public bool BotCollision { get { return this._botCollision; } }
        public bool TopCollision { get { return this._topCollision; } }
        public bool LeftCollision { get { return this._leftCollision; } }
        public bool RightCollision { get { return this._rightCollision; } }

        public Rectangle Rectangle {

            get {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y,
                    this._width, this._height);
            }
        }

        public Body(int heigth, int width, Vector2 position) {
            this.Position = position;
            this._height = heigth;
            this._width = width;
        }

        public void applyForce(Vector2 velocity) {
            this.Velocity = velocity;
        }

        public void Update() {
            this.Position += this.Velocity;
            this._botCollision = false;
            this._topCollision = false;
            this._leftCollision = false;
            this._rightCollision = false;
        }

        public void Collision(Body body) {
                        

            this._botCollision |= this.IsTouchingTop(body);
            this._topCollision |= this.IsTouchingBottom(body);
            this._leftCollision |= this.IsTouchingRight(body);
            this._rightCollision |= this.IsTouchingLeft(body);

            Debug.WriteLineIf(this._rightCollision, "coucou");

            if (this._botCollision)
                this.Velocity.Y = 0;
            if (this._topCollision)
                this.Velocity.Y = 1;
            if ((this._leftCollision) || (this._rightCollision))
                this.Velocity.X = 0;

            
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
