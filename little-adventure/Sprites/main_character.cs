using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using little_adventure.Sprites;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;

namespace little_adventure {

    public static class Define {
        public const byte ParameterNumber = 2;
        public const byte MovementTextuePosition = 0;
        public const byte StaticTexturePosition = 1;
    }
    


    class MainCharacter {
        
        private SpriteManager _spriteManager;
        private Vector2 _velocity;
        private Vector2 _positions;
        private Directions _actualDirection;
        private bool _isJumping;
        private bool _wasFalling;
        private bool _isDoubleJumping;
        private int _doubleJumpTempo;
        private bool _isFalling;
        private Vector2 gravity;

        /// <summary>
        /// Constucte and initialize the class
        /// </summary>
        /// <param name="game"> Game who call this class</param>
        /// <param name="initial_position"> Initial position of this sprite</param>
        public MainCharacter(Vector2 initial_position) {
            
            //this._spriteManager.inflation = 1f;
            this._velocity = new Vector2(0, 0);
            this._positions = initial_position;
            this._actualDirection = Directions.NONE;
            this._isDoubleJumping = false;
            this._isJumping = false;
            this._isFalling = false;
            this._wasFalling = false;
            this.gravity = Vector2.Zero;
        }

        /// <summary>
        /// Load all texture of main character. 
        /// </summary>
        /// <param name="path">directory poth who contains texture</param>
        public void LoadContent(string path, GraphicsDevice graphicsDevice, ContentManager content) {

            this._spriteManager = new SpriteManager(graphicsDevice);

            this._spriteManager.addTexture(content, "static", path+"/static/");

            this._spriteManager.addTexture(content, "walk", path + "/walk/");

            this._spriteManager.addParticule(content, "dust", "./particules/dust/");

            this._spriteManager.addTexture(content, "jump", path + "/jump/");

            this._spriteManager.addTexture(content, "fall", path + "/falling/");

            this._spriteManager.addStaticEffect(content, "dump", path + "/doubleump/");

            this._spriteManager.setActualTexture("static");
            this._spriteManager.setActualPaticule("none");
            this._spriteManager.setActualStaticEffect("none");
            this._spriteManager.nextSprite();

        }

        /// <summary>
        /// Movment and collision manager
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, PlateformerSprite lv) {

            var lastDirection = this._actualDirection;
            
            KeyboardGestion();
            
            this._velocity.Y += 0.15f;
            
            //gestion des collisions

            collisionManager(lv);

            //gestion de l'animation
            AnimationGestion(lastDirection);

            //mise à jour des données
            this._positions += this._velocity;
            this._spriteManager.Position = this._positions;

            this._spriteManager.Update(gameTime);



        }

        /// <summary>
        /// Gestion des collisions avec l'environnement
        /// </summary>
        /// <param name="lv">niveau actuel</param>
        private void collisionManager(PlateformerSprite lv) {

            Sprite newSprite = this._spriteManager.NextSprite;
            newSprite.Velocity = this._velocity;

            var bot = lv.collisionBot(newSprite);

            if (bot) { //collision su les pieds
                if (this._isJumping || this._isFalling)
                    this._wasFalling = true;
                else
                    this._wasFalling = false;

                this._isJumping = false;
                this._isFalling = false;
                this._velocity.Y = 0f;
                this._positions = newSprite.Position;
                this._doubleJumpTempo = 0;
                this._isDoubleJumping = false;
            }
            else if (!bot && !this._isJumping && !this._isDoubleJumping) { //gestion de la chutte
                this._isFalling = true;
                this._actualDirection = Directions.FALL;
            }
            if (lv.collisionLeft(newSprite)) // collision à gauche
                this._velocity.X = 0;
            if (lv.collisionRight(newSprite)) // collision à doite
                this._velocity.X = 0;
            if (lv.collisionTop(newSprite)) // collision à la tête
                this._velocity.Y = 1;
        }

        /// <summary>
        /// Gestion du clavie
        /// </summary>
        private void KeyboardGestion() {

            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) && !this._isJumping) { //gestion du saut simple
                this._velocity.Y = -6f;
                this._isJumping = true;
                this._positions.Y -= 20f;
                this._actualDirection = Directions.JUMP;
                this._doubleJumpTempo = 0;
                return;
            }

            if (state.IsKeyDown(Keys.Space) && !this._isDoubleJumping && this._doubleJumpTempo > 34) {
                this._velocity.Y = -6f;
                this._isDoubleJumping = true;
                this._positions.Y -= 20f;
                this._actualDirection = Directions.JUMP;
                this._spriteManager.setActualStaticEffect("dump");
                this._spriteManager.staticEffectPosition = this._positions;
                return;
            }

            //gestion des entrés claviers
            if (state.IsKeyDown(Keys.Left)) { //déplacement à gauche
                this._velocity.X = -3;
                this._spriteManager.effect = SpriteEffects.FlipHorizontally;
                this._actualDirection = Directions.LEFT;
            } 
            else if (state.IsKeyDown(Keys.Right)) { // déplacement à droite
                this._velocity.X = 3;
                this._spriteManager.effect = SpriteEffects.None;
                this._actualDirection = Directions.RIGTH;
            }
            else { //aucune touche
                this._actualDirection = Directions.NONE;
                this._velocity.X = 0;
            }

            if (this._isJumping)
                this._doubleJumpTempo++;
        }

        /// <summary>
        /// Gestion de l'animation et des modifications d'animations
        /// </summary>
        /// <param name="lastDirection">précédente direction pour détecter les changement de directions</param>
        private void AnimationGestion(Directions lastDirection) {
            //gestion de l'animation

            if (this._actualDirection == Directions.JUMP) { //
                this._spriteManager.setActualPaticule("none");
                this._spriteManager.setActualTexture("jump");
            }
            else if (this._isJumping)
                return;
            else if(this._actualDirection == Directions.FALL) {
                this._spriteManager.setActualPaticule("none");
                this._spriteManager.setActualTexture("fall");
            }
            else if ((lastDirection == Directions.LEFT && this._actualDirection == Directions.RIGTH) ||
                (lastDirection == Directions.RIGTH && this._actualDirection == Directions.LEFT) ||
                (this._actualDirection == Directions.RIGTH && this._wasFalling) ||
                (this._actualDirection == Directions.LEFT && this._wasFalling)) {
                this._spriteManager.setActualPaticule("dust");
                this._spriteManager.setActualTexture("walk");
            }
            else if (this._actualDirection == Directions.NONE) {
                this._spriteManager.setActualTexture("static");
                this._spriteManager.setActualPaticule("none");
            }
            else if ((this._actualDirection == Directions.LEFT || this._actualDirection == Directions.RIGTH)
                && lastDirection == Directions.NONE) {

                this._spriteManager.setActualPaticule("dust");
                this._spriteManager.setActualTexture("walk");
            }
        }

        


        /// <summary>
        /// Darwing charracter sprite on screen
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch) {

            this._spriteManager.Draw(batch);

        }

    }
}
