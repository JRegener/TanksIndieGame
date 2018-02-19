using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;
using TanksIndieGame.view.render;

namespace TanksIndieGame.view.entities
{
    public class GroundControl
    {
        private float[] vertices =
            {
                0f, 0, 0f,
                0f, 0, 1f,
                1f, 0, 1f,
                1f, 0, 0f
            };

        private uint[] indices =
        {
                0,1,3,
                3,1,2
            };

        private float[] textureCoords =
        {
                0,0,
                0,1,
                1,1,
                1,0
            };
        private float[] normals =
        {
                0,1,0,
                0,1,0,
                0,1,0,
                0,1,0
            };

        private Model groundModel;

        public Model GroundModel
        {
            get
            {
                return groundModel;
            }
        }

        public GroundControl(OpenGL gl, Loader loader, Image textureImg, string vertexShaderCode, string fragmentShaderCode, Light lights)
        {
            groundModel = loader.LoadModel("ground", gl, 0, 0, 0, 0, 0, 0, 1f,
                vertices, indices, textureCoords, normals,
                textureImg, 0, 0, vertexShaderCode, fragmentShaderCode, lights);
        }


    }
}
