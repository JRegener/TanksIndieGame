using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;
using TanksIndieGame.view.shaders;
using TanksIndieGame.view.tool;

namespace TanksIndieGame.view.render
{
    public class Renderer
    {
        private uint renderType = OpenGL.GL_TRIANGLES;

        private float fov = 45;
        private float near = 0.1f;
        private float far = 1000;

        private mat4 projectionMatrix;

        public mat4 ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        public Renderer(float fov, float near, float far, int width, int height)
        {
            SetViewProperties(fov, near, far, width, height);
        }

        public void SetViewProperties(float fov, float near, float far, int width, int height)
        {
            this.fov = fov;
            this.near = near;
            this.far = far;
            this.projectionMatrix = GLMath.CreateProjectionMatrix(this.fov, width, height,
                                                                    this.near, this.far);
        }

        /// <summary>
        /// First function call
        /// </summary>
        public void Prepare(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(1, 1, 1, 1);
        }

        /// <summary>
        /// Second function call
        /// </summary>
        public void Render(OpenGL gl, List<Model> models, Camera camera)
        {
            for (int i = 0; i < models.Count; i++)
            {
                SetProjectionMatrix(gl, models[i].BaseShader);
                SetViewMatrix(gl, models[i].BaseShader, camera);

                RenderModel(gl, models[i]);
            }
        }

        private void RenderModel(OpenGL gl, Model model)
        {
            model.BaseShader.Start();

            gl.BindVertexArray(model.ModelView.VaoId);
            gl.EnableVertexAttribArray(0);
            gl.EnableVertexAttribArray(1);
            gl.EnableVertexAttribArray(2);

            model.BaseShader.LoadVariables();

            mat4 transformationMatrix = GLMath.CreateTransformationMatrix(
                model.BaseModel.Position,
                model.BaseModel.Rotation,
                model.BaseModel.Scale);

            model.BaseShader.LoadTransformationMatrix(transformationMatrix);

            gl.ActiveTexture(OpenGL.GL_TEXTURE0);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, model.ModelView.TextureId);

            gl.DrawElements(renderType, model.ModelView.Indices.Length, model.ModelView.Indices);
            gl.DisableVertexAttribArray(0);
            gl.DisableVertexAttribArray(1);
            gl.DisableVertexAttribArray(2);
            gl.BindVertexArray(0);

            model.BaseShader.Stop();
        }


        private void SetProjectionMatrix(OpenGL gl, BaseShader shader)
        {
            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        private void SetViewMatrix(OpenGL gl, BaseShader shader, Camera camera)
        {
            shader.Start();
            shader.LoadViewMatrix(GLMath.CreateViewMatrix(camera));
            shader.Stop();
        }


        //private void RenderTerrain(OpenGL gl, Terrain terrain, TerrainBrush brush)
        //{
        //    terrain.StartShader();

        //    gl.BindVertexArray(terrain.Id);
        //    gl.EnableVertexAttribArray(0);
        //    gl.EnableVertexAttribArray(1);
        //    gl.EnableVertexAttribArray(2);

        //    terrain.LoadShaderVariables();

        //    mat4 transformationMatrix = GLMath.CreateTransformationMatrix(new vec3(0, 0, 0), new vec3(0, 0, 0), 1);
        //    terrain.SetTransformationMatrix(transformationMatrix);

        //    BindTerrainTexturePack(gl, terrain.Textures);
        //    gl.DrawElements(renderType, terrain.Indices.Length, terrain.Indices);
        //    gl.DisableVertexAttribArray(0);
        //    gl.DisableVertexAttribArray(1);
        //    gl.DisableVertexAttribArray(2);
        //    gl.BindVertexArray(0);

        //    terrain.StopShader();
        //}

        //private void BindTerrainTexturePack(OpenGL gl, ModelTexture[] terrainTexturePack)
        //{
        //    gl.ActiveTexture(OpenGL.GL_TEXTURE0);
        //    gl.BindTexture(OpenGL.GL_TEXTURE_2D, terrainTexturePack[0].Id);
        //    gl.ActiveTexture(OpenGL.GL_TEXTURE1);
        //    gl.BindTexture(OpenGL.GL_TEXTURE_2D, terrainTexturePack[1].Id);
        //    gl.ActiveTexture(OpenGL.GL_TEXTURE2);
        //    gl.BindTexture(OpenGL.GL_TEXTURE_2D, terrainTexturePack[2].Id);
        //    gl.ActiveTexture(OpenGL.GL_TEXTURE3);
        //    gl.BindTexture(OpenGL.GL_TEXTURE_2D, terrainTexturePack[3].Id);
        //    gl.ActiveTexture(OpenGL.GL_TEXTURE4);
        //    gl.BindTexture(OpenGL.GL_TEXTURE_2D, terrainTexturePack[4].Id);
        //}


    }
}
