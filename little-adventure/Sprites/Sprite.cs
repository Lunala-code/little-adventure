using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Sprites {
    public class Sprite {

        protected Texture2D _texture;
        private Texture2D _debugTexture;

        public Vector2 Position;
        public Vector2 Velocity = Vector2.Zero;
        public Color Colour = Color.White;
        public SpriteEffects effect = SpriteEffects.None;

        public Rectangle Rectangle {

            get {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, 
                    this._texture.Width, this._texture.Height);
            }
        }

        public Sprite(Texture2D texture) {
            this._texture = texture;

        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites) {
            
        }

        public Sprite Clone() {
            return (Sprite)this.MemberwiseClone();
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this._texture, this.Position,
                null, Color.White, 0f, Vector2.Zero, 1f, this.effect, 0f);
        }

        #region Colloision
        public bool IsTouchingLeft(Sprite sprite) {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        public bool IsTouchingRight(Sprite sprite) {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
              this.Rectangle.Right > sprite.Rectangle.Right &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        public bool IsTouchingTop(Sprite sprite) {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Top &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        public bool IsTouchingBottom(Sprite sprite) {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
              this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        #endregion


    }

}

