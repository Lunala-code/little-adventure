using little_adventure.Model;
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

        private Dictionary<PlayerAnimationName, AnimatedSprite> _sprites;

        private Dictionary<ParticuleAnimationName, AnimatedSprite> _particules;
        private Dictionary<StaticEffectName, FinishedAnimation> _staticEffect;

        private AnimatedSprite _actualPlayerSprite;
        private AnimatedSprite _actualParticuleSprite;
        private FinishedAnimation _actualStaticSprite;
        

        private int n;
        private bool _isJumping;
        public Vector2 Position;
        public SpriteEffects effect;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">content manager for sprite loading</param>
        public SpriteManager(){

            this._sprites = new Dictionary<PlayerAnimationName, AnimatedSprite>();
            this._particules = new Dictionary<ParticuleAnimationName, AnimatedSprite>();
            this._staticEffect = new Dictionary<StaticEffectName, FinishedAnimation>();

            this._sprites.Add(PlayerAnimationName.NONE, new NullAnimatedSprite());
            this._particules.Add(ParticuleAnimationName.NONE, new NullAnimatedSprite());
            this._staticEffect.Add(StaticEffectName.NONE, new NullAnimatedSprite());
            

            n = 0;

        }


        /// <summary>
        /// Load multiple texture to make a animation
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="animationPath"></param>
        public void addAnimation(PlayerAnimationName name, ContentManager content, String path) {
            
            this._sprites.Add(name, addTxt(content, path));
                this._sprites[name].ManualAnimation = name == PlayerAnimationName.JUMP;
        }

        public void addAnimation(ParticuleAnimationName name, ContentManager content, String path) {
            this._particules.Add(name, addTxt(content, path));
        }

        public void addStaticAnimation(StaticEffectName  name, ContentManager content, String path) {
            this._staticEffect.Add(name, addSTxt(content, path));
        }

        private AnimatedSprite addTxt(ContentManager content, String path) {
            AnimatedSprite animatedSprite = new AnimatedSprite();
            animatedSprite.loadContent(path, content);
            return animatedSprite;
        }
        
        private FinishedAnimation addSTxt(ContentManager content, String path ){
            FinishedAnimation finishedAnimation = new FinishedAnimation();
            finishedAnimation.loadContent(path, content);
            return finishedAnimation;
        }

        /// <summary>
        /// define the texture/animation to draw
        /// </summary>
        /// <param name="textureName"></param>
        public void setActualTexture(PlayerAnimationName name) {
            this._actualPlayerSprite = this._sprites[name];
            n = 0;
            if (name == PlayerAnimationName.JUMP)
                this._isJumping = true;
            else
                this._isJumping = false;
        }

        public void setActualTexture(ParticuleAnimationName name) {
            this._actualParticuleSprite = this._particules[name];
            
        }

        public void setActualTexture(StaticEffectName name, Vector2 position) {
            this._actualStaticSprite = this._staticEffect[name];
            this._actualStaticSprite.start(position);

        }

        /// <summary>
        /// Actualise les sprites à afficher
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {

            int j = 0;
            n++;

            if (this._isJumping && n < 34)
                j = 0;
            else if (this._isJumping)
                j = 1;
    
            else
                this._actualParticuleSprite.Update(this.Position);
            this._actualPlayerSprite.Update(this.Position, j);
            this._actualStaticSprite.Update();
        }

        /// <summary>
        /// Actualise les sprites
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            
            this._actualPlayerSprite.Draw(spriteBatch, this.effect);
            this._actualParticuleSprite.Draw(spriteBatch, this.effect);
            this._actualStaticSprite.Draw(spriteBatch, this.effect);      
                
        }

        public Sprite NextSprite() {
            return this._actualPlayerSprite.Sprite;
        }

            
    }
}

