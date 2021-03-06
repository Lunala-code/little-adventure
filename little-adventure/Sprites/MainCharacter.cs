﻿using Microsoft.Xna.Framework.Graphics;
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
using little_adventure.Model;
using little_adventure.Physics;

namespace little_adventure.Sprites {   


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
        private Body _body;
        private float _speed = 3f;
        private float _jumpSpeed = -6f;
        private float _jumpPosition = -20f;

        /// <summary>
        /// Constucte and initialize the class
        /// </summary>
        /// <param name="game"> Game who call this class</param>
        /// <param name="initial_position"> Initial position of this sprite</param>
        public MainCharacter(Vector2 initial_position) {
            

            this._velocity = new Vector2(0, 0);
            this._positions = initial_position;
            this._actualDirection = Directions.NONE;
            this._isDoubleJumping = false;
            this._isJumping = false;
            this._isFalling = false;
            this._wasFalling = false;

        }

        /// <summary>
        /// Load all texture of main character. 
        /// </summary>
        /// <param name="path">directory poth who contains texture</param>
        public void LoadContent(string path, ContentManager content, World world) {

            this._spriteManager = new SpriteManager();

            this._spriteManager.addAnimation(PlayerAnimationName.STATIC, content, path + "/static/");

            this._spriteManager.addAnimation(PlayerAnimationName.WALK, content, path + "/walk/");

            this._spriteManager.addAnimation(PlayerAnimationName.JUMP, content, path + "/jump/");

            this._spriteManager.addAnimation(PlayerAnimationName.FALL, content, path + "/falling/");

            this._spriteManager.addAnimation(ParticuleAnimationName.WALK, content, "./particules/dust/");

            this._spriteManager.addStaticAnimation(StaticEffectName.DOUBLEJUMP, content, path + "/doubleump/");

            this._spriteManager.setActualTexture(PlayerAnimationName.STATIC);
            this._spriteManager.setActualTexture(ParticuleAnimationName.NONE);
            this._spriteManager.setActualTexture(StaticEffectName.NONE, Vector2.Zero);

            this._body = world.CreateBody(this._spriteManager.NextSprite().Rectangle.Height, 
                this._spriteManager.NextSprite().Rectangle.Width, this._positions, BodyType.DYNAMIC);
            //this._body.Position = this._positions;

        }

        /// <summary>
        /// Movment and collision manager
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, PlateformerSprite lv) {

            var lastDirection = this._actualDirection;
            
            KeyboardGestion();
            
            
            collisionManager();

            //gestion de l'animation
            AnimationGestion(lastDirection);
            

            //mise à jour des données
            this._spriteManager.Position = this._body.Position;

            this._spriteManager.Update(gameTime);
            

        }

        /// <summary>
        /// Gestion des collisions avec l'environnement
        /// </summary>
        /// <param name="lv">niveau actuel</param>
        private void collisionManager() {
                        
            if (this._body.BotCollision) { //collision su les pieds
                if (this._isJumping || this._isFalling)
                    this._wasFalling = true;
                else
                    this._wasFalling = false;

                this._isJumping = false;
                this._isFalling = false;
                this._doubleJumpTempo = 0;
                this._isDoubleJumping = false;
            }
            else if (!this._body.BotCollision && !this._isJumping && !this._isDoubleJumping) { //gestion de la chutte
                this._isFalling = true;
                this._actualDirection = Directions.FALL;
            }
        }

        /// <summary>
        /// Gestion du clavie
        /// </summary>
        private void KeyboardGestion() {

            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) && !this._isJumping) { //gestion du saut simple
                this._body.Velocity.Y = this._jumpSpeed;
                this._isJumping = true;
                this._body.Position.Y += this._jumpPosition;
                this._actualDirection = Directions.JUMP;
                this._doubleJumpTempo = 0;
                return;
            }

            if (state.IsKeyDown(Keys.Space) && !this._isDoubleJumping && this._doubleJumpTempo > 34) {
                this._body.Velocity.Y = this._jumpSpeed;
                this._isDoubleJumping = true;
                this._body.Position.Y += this._jumpPosition;
                this._actualDirection = Directions.JUMP;
                this._spriteManager.setActualTexture(StaticEffectName.DOUBLEJUMP, this._body.Position);
                return;
            }

            //gestion des entrés claviers
            if (state.IsKeyDown(Keys.Left)) { //déplacement à gauche
                this._body.Velocity.X = -this._speed;
                this._spriteManager.effect = SpriteEffects.FlipHorizontally;
                this._actualDirection = Directions.LEFT;
            } 
            else if (state.IsKeyDown(Keys.Right)) { // déplacement à droite
                this._body.Velocity.X = this._speed;
                this._spriteManager.effect = SpriteEffects.None;
                this._actualDirection = Directions.RIGHT;
            }
            else { //aucune touche
                this._actualDirection = Directions.NONE;
                this._body.Velocity.X = 0;
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
                this._spriteManager.setActualTexture(ParticuleAnimationName.NONE);
                this._spriteManager.setActualTexture(PlayerAnimationName.JUMP);
            }
            else if (this._isJumping)
                return;
            else if(this._actualDirection == Directions.FALL) {
                this._spriteManager.setActualTexture(ParticuleAnimationName.NONE);
                this._spriteManager.setActualTexture(PlayerAnimationName.FALL);
            }
            else if ((lastDirection == Directions.LEFT && this._actualDirection == Directions.RIGHT) ||
                (lastDirection == Directions.RIGHT && this._actualDirection == Directions.LEFT) ||
                (this._actualDirection == Directions.RIGHT && this._wasFalling) ||
                (this._actualDirection == Directions.LEFT && this._wasFalling)) {
                this._spriteManager.setActualTexture(ParticuleAnimationName.WALK);
                this._spriteManager.setActualTexture(PlayerAnimationName.WALK);
            }
            else if (this._actualDirection == Directions.NONE) {
                this._spriteManager.setActualTexture(PlayerAnimationName.STATIC);
                this._spriteManager.setActualTexture(ParticuleAnimationName.NONE);
            }
            else if ((this._actualDirection == Directions.LEFT || this._actualDirection == Directions.RIGHT)
                && lastDirection == Directions.NONE) {
                
                this._spriteManager.setActualTexture(PlayerAnimationName.WALK);
                this._spriteManager.setActualTexture(ParticuleAnimationName.WALK);
            }
        }

        


        /// <summary>
        /// Darwing charracter sprite on screen
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch) {

            this._spriteManager.Position = this._body.Position;

            this._spriteManager.Draw(batch);

        }

        public Vector2 getPosition() {
            return this._body.Position;
        }

    }
}
