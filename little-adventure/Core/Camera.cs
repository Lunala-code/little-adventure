using little_adventure.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace little_adventure.Core {
    public class Camera {

        private static Camera instance;

        private float lastY = float.MinValue;
        
        public static Camera Instance {
            get {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        public Matrix viewMatrix { get; private set; }
        private Vector2 positions;

        public void setFocalPoinst(Vector2 focalPosition) {
            positions = new Vector2(focalPosition.X - Game1.ScreenWidth / 4,
                focalPosition.Y - Game1.ScreenHeight/3 );

            if (positions.X < 0)
                positions.X = 0;
            if (positions.Y < 0)
                positions.Y = 0;

                

            
            Debug.WriteLine(positions.Y);
        }

        public void Update() {
            
            viewMatrix =  Matrix.CreateTranslation(new Vector3(-positions, 0));
            
        }
    }
}
