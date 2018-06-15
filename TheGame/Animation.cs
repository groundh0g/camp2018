﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame
{
    public class Animation
    {
        public List<Texture2D> Images { get; set; }
        public float FrameDuration = 1.0f / 15.0f;
        public bool Loop = false;
        public Point Location = Point.Zero;

        private int currentFrame;
        public void Start(float initElapsed = 0)
        {
            currentFrame = 0;
            elapsed = initElapsed;
            FrameDuration = FrameDuration == 0 ? 1.0f / 15.0f : FrameDuration;
        }

        private double elapsed;
        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if(elapsed >= FrameDuration)
            {
                currentFrame++;
                if (IsDone && Loop) { currentFrame = 0; }
                elapsed = 0;
            }
        }

        public bool IsDone { get { return currentFrame >= Images.Count; } }
        public Texture2D CurrentFrame
        {
            get
            {
                if(elapsed < 0)
                {
                    return null;
                }
                else if(currentFrame < Images.Count)
                {
                    return Images[currentFrame];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
