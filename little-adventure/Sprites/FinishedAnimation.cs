
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Sprites {
    class FinishedAnimation : AnimatedSprite {

        private bool _isRunning = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public FinishedAnimation(): base() {
        }

        
        /// <summary>
        /// Start the animation computing
        /// </summary>
        /// <param name="position"> position of sprite</param>
        public void start(Vector2 position) {
            this._isRunning = true;
            this._counter = 0;
            this._positions = position;
            Debug.WriteLine(this._positions);
        }

        /// <summary>
        /// If animation is running, compute the next sprite to draw
        /// </summary>
        public virtual void Update() {
            this._counter++;
            this._isRunning = !(this._counter/this._fpsModuler >= this._sprites.Count);

            if (this._isRunning) {
                this._nextSprite = this._sprites[this._counter / this._fpsModuler];
                this._nextSprite.Position = this._positions;
            }
            else
                return;
        }

        /// <summary>
        /// Drawing sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="effect"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects effect) {
            
            if (this._isRunning)
                base.Draw(spriteBatch, effect);
            else
                return;
        }
    }
}
