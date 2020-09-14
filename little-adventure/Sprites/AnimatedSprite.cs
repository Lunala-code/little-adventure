using little_adventure.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Sprites {

    


    class AnimatedSprite {

        protected int _moduler;
        protected int _counter;
        protected int _fpsModuler;
        protected List<Sprite> _sprites;
        public Vector2 _positions;
        protected Sprite _nextSprite;
        private bool _manualAnimation;
        protected bool _isStatic;

        public Sprite Sprite { get { return _nextSprite; } }
        
        public bool ManualAnimation { set { _manualAnimation = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public AnimatedSprite() {
            this._fpsModuler = 2;
            this._moduler = 1;
            this._counter = 0;
            this._sprites = new List<Sprite>();
            this._positions = Vector2.Zero;
            this._manualAnimation = false;
            this._isStatic = false;
        }

        /// <summary>
        /// Crée la liste des sprite qui compose l'animation
        /// </summary>
        /// <param name="path"> chemin du dossier qui contient les sprites</param>
        /// <param name="content"> gestionnaire des content du gens</param>
        public void loadContent(String path, ContentManager content) {
            
            Texture2D tx;

            int textureCount = System.IO.Directory.GetFiles(content.RootDirectory + path).Length;

            for (int i = 0; i < textureCount; i++) {
                tx = content.Load<Texture2D>(path + "tile" + i.ToString("000"));
                this._sprites.Add(new Sprite(tx));
            }

            this._moduler = this._sprites.Count;
            this._nextSprite = this._sprites[0];
        }

        public void setStaticAnimation(Vector2 position) {
            this._isStatic = true;
            this._positions = position;
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"> position du prochain sprite</param>
        /// <param name="c">sprite à afficher en mode manuel</param>
        public virtual void Update(Vector2 position, int c=0) {

            if (this._manualAnimation)
                this._counter = c*this._fpsModuler;
            else
                this._counter = (this._counter + 1) % (this._moduler * this._fpsModuler);
            

            this._nextSprite = this._sprites[this._counter / this._fpsModuler];

            if(!this._isStatic)
                this._positions = position;

            this._nextSprite.Position = this._positions;
            
        }
        
        /// <summary>
        /// Dessine le sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="effect"></param>
        public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effect) {
            this._nextSprite.effect = effect;
            this._nextSprite.Draw(spriteBatch);
        }
        
    }
}