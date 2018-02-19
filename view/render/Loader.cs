using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;
using TanksIndieGame.view.shaders;
using TanksIndieGame.view.shaders.objects;
using TanksIndieGame.view.tool;

namespace TanksIndieGame.view.render
{
    public class Loader
    {
        private List<uint> vaos = new List<uint>();
        private List<uint> vbos = new List<uint>();
        private List<uint> textures = new List<uint>();

        private uint BindVao(OpenGL gl)
        {
            uint[] ids = new uint[1];
            gl.GenVertexArrays(1, ids);
            uint vaoId = ids[0];
            gl.BindVertexArray(vaoId);

            return vaoId;
        }

        private void UnbindVao(OpenGL gl)
        {
            gl.BindVertexArray(0);
        }

        private void DeleteVao(OpenGL gl, uint id)
        {
            gl.DeleteVertexArrays(1, new uint[] { id });
        }

        private uint SetVbo(OpenGL gl, uint attributeIndex, int stride, float[] data)
        {
            uint[] ids = new uint[1];
            gl.GenBuffers(1, ids);
            uint vboId = ids[0];
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vboId);

            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, data, OpenGL.GL_STATIC_DRAW);
            gl.VertexAttribPointer(attributeIndex, stride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(attributeIndex);

            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);

            return vboId;
        }


        private void DeleteVbo(OpenGL gl, uint id)
        {
            gl.DeleteBuffers(1, new uint[] { id });
        }

        private uint SetIndexBuffer(OpenGL gl, uint[] indices)
        {
            uint[] ids = new uint[1];
            gl.GenBuffers(1, ids);
            uint indexId = ids[0];
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, indexId);

            gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, GLMath.ToUShort(indices), OpenGL.GL_STATIC_DRAW);

            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, 0);

            return indexId;
        }

        private uint LoadTexture(OpenGL gl, Image texture)
        {
            uint[] ids = new uint[1];
            gl.GenTextures(1, ids);
            uint textureId = ids[0];
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureId);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

            Bitmap bitmap = new Bitmap(texture);

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGBA,
                bitmap.Width, bitmap.Height, 0, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE,
                bmpData.Scan0);
            bitmap.UnlockBits(bmpData);

            bitmap.Dispose();

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);


            return textureId;
        }


        private void DeleteTexture(OpenGL gl, uint id)
        {
            gl.DeleteTextures(1, new uint[] { id });
        }


        private BaseObject LoadBaseModel(float posX, float posY, float posZ,
            float rotX, float rotY, float rotZ, float scale)
        {
            return new BaseObject(posX, posY, posZ, rotX, rotY, rotZ, scale);
        }

        private ModelView LoadModelView(OpenGL gl, float[] vertices, uint[] indices, 
            float[] uv, float[] normals, Image texture)
        {
            uint vaoId = BindVao(gl);
            uint indicesId = SetIndexBuffer(gl, indices);
            uint verticesId = SetVbo(gl, 0, 3, vertices);
            uint uvId = SetVbo(gl, 1, 2, uv);
            uint normalsId = SetVbo(gl, 2, 3, normals);
            UnbindVao(gl);

            uint textureId = LoadTexture(gl, texture);

            return new ModelView(vaoId, textureId, indicesId,
                verticesId, uvId, normalsId, texture,
                vertices, indices, uv, normals);
        }

        private BaseShader LoadModelShader(OpenGL gl, string vertexShaderCode, string fragmentShaderCode, Light lights)
        {
            return new ModelShader(gl, vertexShaderCode, fragmentShaderCode, lights);
        }

        private ModelCollision LoadModelCollision( float weight, float length)
        {
            return new ModelCollision(weight, length);
        }

        public Model LoadModel(string objectTag, OpenGL gl, float posX, float posY, float posZ,
            float rotX, float rotY, float rotZ, float scale,
            float[] vertices, uint[] indices, float[] uv, float[] normals, Image texture,
            float widthCollision, float lengthCollision,
            string vertexShaderCode, string fragmentShaderCode, Light lights)
        {
            BaseObject baseModel = LoadBaseModel(posX, posY, posZ, rotX, rotY, rotZ, scale);
            ModelView modelView = LoadModelView(gl, vertices, indices, uv, normals, texture);
            BaseShader modelShader = LoadModelShader(gl, vertexShaderCode, fragmentShaderCode, lights);
            ModelCollision modelCollision = LoadModelCollision(widthCollision, lengthCollision);

            return new Model(gl, this, objectTag, baseModel, modelView, modelShader, modelCollision);
        }

        //public void FreeModel(OpenGL gl, Model model)
        //{
        //    DeleteVbo(gl, model.IndicesId);
        //    DeleteVbo(gl, model.VerticesId);
        //    DeleteVbo(gl, model.UvId);
        //    DeleteVbo(gl, model.NormalsId);

        //    DeleteTexture(gl, model.GetTextureId());

        //    DeleteVao(gl, model.Id);
        //}

        //private void UpdateVbo(OpenGL gl, uint id, uint attributeIndex, int stride, float[] data)
        //{
        //    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, id);

        //    gl.BufferData(OpenGL.GL_ARRAY_BUFFER, data, OpenGL.GL_STATIC_DRAW);
        //    gl.VertexAttribPointer(attributeIndex, stride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
        //    gl.EnableVertexAttribArray(attributeIndex);

        //    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
        //}

        //private void UpdateIndexBuffer(OpenGL gl, uint id, uint[] indices)
        //{
        //    gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, id);

        //    gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, GLMath.ToUShort(indices), OpenGL.GL_STATIC_DRAW);

        //    gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, 0);
        //}





        //public Terrain LoadTerrainToVao(OpenGL gl, float[] vertices, uint[] indices, float[] uv, float[] normals,
        //                            float posX, float posZ, float terrainSize, Image[] textures, Light[] lights,
        //                            string vertexShader, string fragmentShader, TerrainBrush brush)
        //{
        //    uint vaoId = BindVao(gl);
        //    uint indicesId = SetIndexBuffer(gl, indices);
        //    uint verticesId = SetVbo(gl, 0, 3, vertices);
        //    uint uvId = SetVbo(gl, 1, 2, uv);
        //    uint normalsId = SetVbo(gl, 2, 3, normals);
        //    UnbindVao(gl);

        //    ModelTexture[] mTextures = new ModelTexture[textures.Length];
        //    for (int i = 0; i < mTextures.Length; i++)
        //    {
        //        mTextures[i] = LoadTexture(gl, textures[i]);
        //    }

        //    BaseShader shader = LoadTerrainShader(gl, vertexShader, fragmentShader, lights, brush, terrainSize);

        //    return new Terrain(vaoId, verticesId, indicesId, uvId, normalsId,
        //        vertices, indices, uv, normals, terrainSize,
        //        1f, posX, posZ, mTextures, shader, brush);
        //}

        //public void UpdateModel(OpenGL gl, Model model)
        //{

        //    EnableVao(gl, model.Id);

        //    UpdateIndexBuffer(gl, model.IndicesId, model.Indices);

        //    UpdateVbo(gl, model.VerticesId, 0, 3, model.Vertices);
        //    UpdateVbo(gl, model.UvId, 1, 2, model.Uv);
        //    UpdateVbo(gl, model.NormalsId, 2, 3, model.Normals);

        //    UnbindVao(gl);

        //    UpdateTexture(gl, model.Texture);
        //}

        //public void UpdateTerrain(OpenGL gl, Terrain terrain)
        //{

        //    EnableVao(gl, terrain.Id);

        //    UpdateIndexBuffer(gl, terrain.IndicesId, terrain.Indices);

        //    UpdateVbo(gl, terrain.VerticesId, 0, 3, terrain.Vertices);
        //    UpdateVbo(gl, terrain.UvId, 1, 2, terrain.Uv);
        //    UpdateVbo(gl, terrain.NormalsId, 2, 3, terrain.Normals);

        //    UnbindVao(gl);
        //}



    }
}
