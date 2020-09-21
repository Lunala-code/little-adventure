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


            this.collisionMap = new List<Body>();
            
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

            this.collisionMap = new List<Body>();
            Body b;

            Rectangle r = new Rectangle(0, 0, 32, 32);
            Color[] buffer = new Color[32 * 32];

            for (int h = 0; h < tileCountHeight; h++) {
                for (int w = 0; w < tileCountWith; w++) {
                    r.X = tileWidth * w;
                    r.Y = tileHeight * h;
                    this.platform.GetData(0, r, buffer, 0, 32 * 32);

                    if (!buffer.All(c => c == Color.Transparent)) {
                        b = world.CreateBody(32, 32, new Vector2(r.X, r.Y), BodyType.STATIC);
                        this.collisionMap.Add(b);
                    }
                }
                
            }

        }

        public void collision(Body body) {
            foreach (var b in this.collisionMap) {
                body.Collision(b);

            }
        }

        



    }

        
}
