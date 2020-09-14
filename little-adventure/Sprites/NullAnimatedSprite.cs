using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Sprites {
    class NullAnimatedSprite :FinishedAnimation{

        public NullAnimatedSprite() : base() {

        }

        public override void Update(Vector2 position, int c=0) {
            return;
        }
        public override void Update() {

        }
        

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects effects) {
            return;
        }
    }
}
