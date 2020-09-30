using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using little_adventure.Sprites;
using little_adventure.Physics;
using little_adventure.Model;

namespace little_adventure {

    class PlateformerSprite {

        private string _texturePath;
        private Texture2D platform;
        public List<Body> collisionMap;
        private int _width;
        private int _height;
        public int Width { get => _width; }
        public int Height { get => _height; }
        private int tileWidth = 32;
        private int tileHeight = 32;
        private int tileCountWith;
        private int tileCountHeight;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="levelPath"></param>
        public PlateformerSprite(string levelPath) {
            this._texturePath = levelPath;
        }

        public void loadTexture(ContentManager content) {

            platform = content.Load<Texture2D>(this._texturePath);

            this._width = this.platform.Width;
            this._height = this.platform.Height;

            this.tileCountHeight = this._height / this.tileHeight;
            this.tileCountWith = this._width / this.tileWidth;
            
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch) {
            batch.Draw(this.platform, new Vector2(0, 0), Color.White);
        }

        /// <summary>
        /// Génère la carte des collisions
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void getColisionMap(World world) {
            
            
            bool[,] col = new bool[tileCountHeight, tileCountWith];

            Rectangle r = new Rectangle(0, 0, 32, 32);
            Color[] buffer = new Color[32 * 32];

            for (int h = 0; h < tileCountHeight; h++) {
                for (int w = 0; w < tileCountWith; w++) {
                    r.X = tileWidth * w;
                    r.Y = tileHeight * h;
                    this.platform.GetData(0, r, buffer, 0, 32 * 32);
                    col[h, w] = !buffer.All(c => c == Color.Transparent);
                }
            }

            bool le = true, ri = true, to = true, bo = true;

            for (int h = 0; h < tileCountHeight; h++) {

                for (int w = 0; w < tileCountWith; w++) {

                    if (h - 1 > 0)
                        to = col[h - 1, w];
                    else
                        to = true;
                    
                    if (h + 1 < tileCountHeight)
                        bo = col[h + 1, w];
                    else
                        bo = true;

                    if (w - 1 > 0)
                        le = col[h, w - 1];
                    else
                        le = true;

                    if (w + 1 < tileCountWith)
                        ri = col[h, w + 1];
                    else
                        ri = true;

                    if((!ri | !le | !to | !bo) & col[h, w])
                        world.CreateBody(32, 32, new Vector2(tileWidth * w, tileHeight * h), BodyType.STATIC);
                }

            }

        }

    }

        
}
