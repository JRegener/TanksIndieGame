using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class ModelView : ICloneable
    {
        private uint vaoId;

        private uint textureId;

        private uint indicesId;
        private uint verticesId;
        private uint uvId;
        private uint normalsId;

        private Image texture;
        private float[] vertices;
        private uint[] indices;
        private float[] uv;
        private float[] normals;


        public ModelView(uint vaoId, uint textureId, uint indicesId, uint verticesId, uint uvId, uint normalsId, Image texture, float[] vertices, uint[] indices, float[] uv, float[] normals)
        {
            this.vaoId = vaoId;
            this.textureId = textureId;
            this.indicesId = indicesId;
            this.verticesId = verticesId;
            this.uvId = uvId;
            this.normalsId = normalsId;

            this.texture = texture;
            this.vertices = vertices;
            this.indices = indices;
            this.uv = uv;
            this.normals = normals;
        }

        #region properties
        public uint VaoId
        {
            get
            {
                return vaoId;
            }

            set
            {
                vaoId = value;
            }
        }

        public uint VerticesId
        {
            get
            {
                return verticesId;
            }

            set
            {
                verticesId = value;
            }
        }

        public uint IndicesId
        {
            get
            {
                return indicesId;
            }

            set
            {
                indicesId = value;
            }
        }

        public uint UvId
        {
            get
            {
                return uvId;
            }

            set
            {
                uvId = value;
            }
        }

        public uint NormalsId
        {
            get
            {
                return normalsId;
            }

            set
            {
                normalsId = value;
            }
        }

        public float[] Vertices
        {
            get
            {
                return vertices;
            }

            set
            {
                vertices = value;
            }
        }

        public uint[] Indices
        {
            get
            {
                return indices;
            }

            set
            {
                indices = value;
            }
        }

        public float[] Uv
        {
            get
            {
                return uv;
            }

            set
            {
                uv = value;
            }
        }

        public float[] Normals
        {
            get
            {
                return normals;
            }

            set
            {
                normals = value;
            }
        }

        public uint TextureId
        {
            get
            {
                return textureId;
            }

            set
            {
                textureId = value;
            }
        }

        public Image Texture
        {
            get
            {
                return texture;
            }

            set
            {
                texture = value;
            }
        }

        #endregion

        public object Clone()
        {

            ModelView modelView = (ModelView)this.MemberwiseClone();

            modelView.Texture = (Image)this.texture.Clone();
            modelView.Indices = (uint[])this.indices.Clone();
            modelView.Vertices = (float[])this.vertices.Clone();
            modelView.Uv = (float[])this.uv.Clone();
            modelView.Normals = (float[])this.normals.Clone();

            return modelView;
        }
    }
}
