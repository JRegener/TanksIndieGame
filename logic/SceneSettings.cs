using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic
{

    public static class SceneSettings
    {
        // Render
        public static readonly float FOV = 45f;
        public static readonly float NEAR = 0.1f;
        public static readonly float FAR = 1000f;

        // Camera

        // Light
        public static readonly Light BASE_LIGHT = new Light(new vec3(-500, 1000, -500), new vec3(1, 1, 1));

        // Map

    }
}
