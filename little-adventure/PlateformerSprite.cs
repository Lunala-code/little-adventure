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

namespace little_adventure {

    class PlateformerSprite {

        private string _texturePath;
        private Texture2D platform;
        public List<Sprite> collisionMap;
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


            this.collisionMap = new List<Sprite>();
            
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
        public void getColisionMap(GraphicsDevice graphicsDevice) {

            this.collisionMap = new List<Sprite>();
            Sprite s;

            Rectangle r = new Rectangle(0, 0, 32, 32);
            Color[] buffer = new Color[32 * 32];

            for (int h = 0; h < tileCountHeight; h++) {
                for (int w = 0; w < tileCountWith; w++) {
                    r.X = tileWidth * w;
                    r.Y = tileHeight * h;
                    this.platform.GetData(0, r, buffer, 0, 32 * 32);

                    if (!buffer.All(c => c == Color.Transparent)) {
                        s = new Sprite(new Texture2D(graphicsDevice, 32, 32));
                        s.Position = new Vector2(r.X, r.Y);
                        this.collisionMap.Add(s);
                        Debug.WriteLine(s.Rectangle);
                    }
                }
                
            }

        }

        /// <summary>
        /// Détecte une collision entre une plateform de la carte et le bas du sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public bool collisionBot(Sprite sprite) {
            foreach(var s in this.collisionMap) {
                if (sprite.IsTouchingTop(s)) {
                    sprite.Position = new Vector2(sprite.Position.X, s.Position.Y-sprite.Rectangle.Height);
                    return true;
                }
                    
            }
            return false;
        }

        /// <summary>
        /// Détecte une collision enter une plateform de la carte et la tête du sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public bool collisionTop(Sprite sprite) {
            foreach (var s in this.collisionMap) {
                if (sprite.IsTouchingBottom(s))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Détecte une collision enter une plateform de la carte et le coté gauche du sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public bool collisionLeft(Sprite sprite) {
            foreach (var s in this.collisionMap) {
                if (sprite.IsTouchingRight(s))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Détecte une collision enter une plateform de la carte et le coté droit du sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public bool collisionRight(Sprite sprite) {
            foreach (var s in this.collisionMap) {
                if (sprite.IsTouchingLeft(s))
                    return true;
            }
            return false;
        }



    }

        
}
