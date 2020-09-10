using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure {
    class Ligne {

        private Vector2 _start;
        private Vector2 _end;
        private Texture2D t;
        private int _distance;
        private bool _isVertical=false;
        private bool _isToTheTop = false;

        public Vector2 Start { get { return this._start; } }
        public Vector2 End {  get { return this._end; } }

        public Ligne(Vector2 start, Vector2 end, GraphicsDevice graphics) {
            this._start = start;
            this._end = end;

            t = new Texture2D(graphics, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.LightPink });// fill the texture with white

            this._distance = (int)(Math.Sqrt(
                Math.Pow((this._start.X - this._end.X), 2) +
                 Math.Pow((this._start.Y - this._end.Y), 2)));

            if(this._start.X == this._end.X) {
                this._isVertical = true;
                if (this._start.Y > this._end.Y)
                    this._isToTheTop = true;
                else
                    this._isToTheTop = false;
            }
        }

        public void DrawLine(SpriteBatch spriteBatch) {

            Rectangle r;

            if (this._isVertical && !this._isToTheTop)
                r = new Rectangle((int)this._start.X, (int)this._start.Y, 3, this._distance);
            else if (this._isVertical && this._isToTheTop) {
                r = new Rectangle((int)this._end.X, (int)this._end.Y, 3, this._distance);
                spriteBatch.Draw(this.t, this._end, r, Color.DeepPink);
                return;
            }
                
            else
                r = new Rectangle((int)this._start.X, (int)this._start.Y, this._distance, 3);


            spriteBatch.Draw(this.t, this._start, r, Color.DeepPink);
        }

        public bool isTouchingFromTop(Vector2 pts) {
            return (pts.X > this._start.X && pts.X < this._end.X) && pts.Y >= this._start.Y;
        }

        public bool isTouchingFromTheRight(Vector2 pts) {

            if (pts.X >= this._start.X + 5)
                return false;
            return ((pts.Y < this._start.Y && pts.Y > this._end.Y) || 
                (pts.Y > this._start.Y && pts.Y < this._end.Y) )
                && pts.X >= this._start.X;
        }

        public bool isTouchingFromTheLeft(Vector2 pts) {
            Debug.WriteLine(pts);

            if (pts.X <= this._start.X - 10)
                return false;
            return ((pts.Y < this._start.Y && pts.Y > this._end.Y) ||
                (pts.Y > this._start.Y && pts.Y < this._end.Y))
                && pts.X <= this._start.X;
        }


    }
}
