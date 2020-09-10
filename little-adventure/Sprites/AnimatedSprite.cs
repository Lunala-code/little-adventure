using little_adventure.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure {


    class SpriteManager{

        private Dictionary<String, List<Sprite>> _sprites;
        private Sprite _nextSprite;
        private String _actualTexture;
        private int _animationCounter;
        private int _animationModuler;

        private Dictionary<String, List<Sprite>> _particules;
        private Sprite _nextParticule;
        private String _actualParticule;
        private int _particuleCounter;
        private int _particuleModuler;

        private Dictionary<String, List<Sprite>> _staticEffect;
        private Sprite _nextStaticEffect;
        private String _actualStaticEffect;
        private int _staticEffectCounter;
        private int _staticEffectModuler;
        public Vector2 staticEffectPosition;






        private int n;
        public Vector2 Position;
        public SpriteEffects effect;


        public int Height { get {
                return this._nextSprite.Rectangle.Height;
            }
        }
        public int Width { get {
                return this._nextSprite.Rectangle.Width;
            } }
        
        public Sprite NextSprite {
            get { return this._nextSprite.Clone(); }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">content manager for sprite loading</param>
        public SpriteManager(GraphicsDevice graphicsDevice){

            this._sprites = new Dictionary<string, List<Sprite>>();
            this._particules = new Dictionary<string, List<Sprite>>();
            this._staticEffect = new Dictionary<string, List<Sprite>>();

            Texture2D t = new Texture2D(graphicsDevice, 1, 1);
            List<Sprite> l = new List<Sprite>();
            l.Add(new Sprite(t));

            this._particules.Add("none", l);
            this._sprites.Add("none", l);
            this._staticEffect.Add("none", l);


            //this._content = content;

            this._animationCounter = 0;
            this._animationModuler = 0;

            this._particuleCounter = 0;
            this._particuleModuler = 0;

            this._staticEffectCounter = 0;
            this._staticEffectModuler = 0;

            n = 0;

        }


        /// <summary>
        /// Load multiple texture to make a animation
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="animationPath"></param>
        public void addTexture(ContentManager content, String animationName, String animationPath) {

            List<Sprite> sps = new List<Sprite>();
            Texture2D tx;

            int textureCount = System.IO.Directory.GetFiles(content.RootDirectory + animationPath).Length;

            for(int i=0; i<textureCount; i++) {
                tx = content.Load<Texture2D>(animationPath+"tile" + i.ToString("000"));
                sps.Add(new Sprite(tx));
            }

            this._sprites.Add(animationName, sps);

        }

        /// <summary>
        /// Load multiple particule sprite to male a animation
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="animationPath"></param>
        public void addParticule(ContentManager content, String animationName, String animationPath) {

            List<Sprite> pts = new List<Sprite>();
            Texture2D tx;

            int textureCount = System.IO.Directory.GetFiles(content.RootDirectory + animationPath).Length;

            for (int i = 0; i < textureCount; i++) {
                tx = content.Load<Texture2D>(animationPath + "tile" + i.ToString("000"));
                pts.Add(new Sprite(tx));
            }

            this._particules.Add(animationName, pts);

        }

        public void addStaticEffect(ContentManager content, String effectName, String effectPath) {

            List<Sprite> pts = new List<Sprite>();
            Texture2D tx;

            int textureCount = System.IO.Directory.GetFiles(content.RootDirectory + effectPath).Length;

            for (int i = 0; i < textureCount; i++) {
                tx = content.Load<Texture2D>(effectPath + "tile" + i.ToString("000"));
                pts.Add(new Sprite(tx));
            }

            this._staticEffect.Add(effectName, pts);
        }

        /// <summary>
        /// define the texture/animation to draw
        /// </summary>
        /// <param name="textureName"></param>
        public void setActualTexture(string textureName) {
            this._actualTexture = textureName;
            this._animationCounter = 0;
            this._animationModuler = this._sprites[textureName].Count;
            n = 0;
            
        }

        /// <summary>
        /// define the particule/animation to draw
        /// </summary>
        /// <param name="particuleName"></param>
        public void setActualPaticule(string particuleName) {
            this._actualParticule = particuleName;
            this._particuleCounter = 0;
            this._particuleModuler = this._particules[particuleName].Count;
        }

        public void setActualStaticEffect(string staticEffectName) {
            this._actualStaticEffect = staticEffectName;
            this._staticEffectCounter = 0;
            this._staticEffectModuler = this._staticEffect[staticEffectName].Count;
        }


        /// <summary>
        /// calculate the next texture to draw in functions of the animation position and the animation to draw
        /// </summary>
        /// <returns>next texture to print</returns>
        public void nextSprite() {

            n++;
            
            if(this._actualTexture == "jump" && n<34) {
                this._animationCounter = 0;
            } else if(this._actualTexture == "jump" && n >= 34){
                this._animationCounter =2;
            }

            this._animationCounter = (this._animationCounter + 1) % (this._animationModuler * 2);
            
            this._nextSprite = this._sprites[this._actualTexture][this._animationCounter/2];
        }

        /// <summary>
        /// calculate the next particule texture to draw in functions of the animation position and the animation to draw
        /// </summary>
        /// <returns>next particule texture to print</returns>
        public void nextParticule() {
            
            this._particuleCounter = (this._particuleCounter + 1) % (this._particuleModuler * 2);

            this._nextParticule = this._particules[this._actualParticule][this._particuleCounter / 2];
        }

        public void nextStaticEffect() {

            this._staticEffectCounter = (this._staticEffectCounter + 1) % (this._staticEffectModuler * 2);

            this._nextStaticEffect = this._staticEffect[this._actualStaticEffect][this._staticEffectCounter / 2];

            if (this._nextStaticEffect == this._staticEffect[this._actualStaticEffect].Last())
                this.setActualStaticEffect("none");

        }

        /// <summary>
        /// Actualise les sprites à afficher
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            this.nextParticule();
            this.nextSprite();
            this.nextStaticEffect();

            this._nextParticule.effect=this.effect;
            this._nextSprite.effect = this.effect;

            this._nextSprite.Position = this.Position;
            this._nextParticule.Position = this.Position;
            this._nextStaticEffect.Position = this.staticEffectPosition;
        }

        /// <summary>
        /// Actualise les sprites
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {

            

            this._nextSprite.Draw(spriteBatch);
            
            this._nextParticule.Draw(spriteBatch);

            this._nextStaticEffect.Draw(spriteBatch);
                
        }

            
    }
}

