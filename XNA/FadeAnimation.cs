using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameName4.Interactive
{
    class FadeAnimation : Animation
    {
        bool increase, stopUpdating;
        float fadeSpeed, activateValue, defaultAlpha;
        TimeSpan defaultTime, timer;

        public override float Alpha {
            set { 
                alpha = value;
                if (alpha == 1.0f) increase = false;
                else if (alpha == 0.0f) increase = true;
            }
            get { return alpha; }
        }

        public float ActivateValue {
            set { activateValue = value; }
            get { return activateValue; }
        }

        public float FadeSpeed {
            set { fadeSpeed = value; }
            get { return fadeSpeed; }
        }
        public TimeSpan Timer {
            set { timer = defaultTime; defaultTime = value; }
            get { return timer; }
        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position) {
            base.LoadContent(Content, image, text, position);
            increase = false;
            fadeSpeed = 5.0f;
            defaultTime = new TimeSpan(0, 0, 0, 0, 500); // 500ms
            timer = defaultTime;
            activateValue = 0.0f;
            stopUpdating = false;
            defaultAlpha = alpha;
        }

        public override void Update(GameTime gameTime) {
            if (IsActive) {
                if (!stopUpdating) {
                    if (!increase) {
                        alpha -= fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    } else {
                        alpha += fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    // bound the alpha by limits
                    if (alpha <= 0.0f) alpha = 0.0f; else if (alpha >= 1) alpha = 1.0f;
                }

                if (alpha == activateValue) {
                    stopUpdating = true;
                    timer -= gameTime.ElapsedGameTime;
                    if (timer.TotalSeconds <= 0) {
                        increase = !increase; //if false, set to true
                        timer = defaultTime;
                        stopUpdating = false;
                    }
                }
                
            } else {
                alpha = defaultAlpha;
            }
        }
    }
}
