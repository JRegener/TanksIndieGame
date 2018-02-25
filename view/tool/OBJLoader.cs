using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;
using TanksIndieGame.view.render;

namespace TanksIndieGame.view.tool
{
    public class OBJLoader
    {
        #region comment
        public static Model LoadObjModel(string objectTag, bool isKinematic, OpenGL gl, Loader loader, string path, Image textureImg,
            string vertexShaderCode, string fragmentShaderCode, Light lights)
        {
            string line;
            List<Vertex> vertices = new List<Vertex>();
            List<vec2> textures = new List<vec2>();
            List<vec3> normals = new List<vec3>();
            List<uint> indices = new List<uint>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line.StartsWith("v "))
                        {
                            String[] currentLine = line.Split(' ');
                            vec3 vertex = new vec3(float.Parse(currentLine[2], CultureInfo.InvariantCulture.NumberFormat),
                                    float.Parse(currentLine[3], CultureInfo.InvariantCulture.NumberFormat),
                                    float.Parse(currentLine[4], CultureInfo.InvariantCulture.NumberFormat));
                            Vertex newVertex = new Vertex(vertices.Count, vertex);
                            vertices.Add(newVertex);

                        }
                        else if (line.StartsWith("vt "))
                        {
                            String[] currentLine = line.Split(' ');
                            vec2 texture = new vec2(float.Parse(currentLine[1], CultureInfo.InvariantCulture.NumberFormat),
                                    float.Parse(currentLine[2], CultureInfo.InvariantCulture.NumberFormat));
                            textures.Add(texture);
                        }
                        else if (line.StartsWith("vn "))
                        {
                            String[] currentLine = line.Split(' ');
                            vec3 normal = new vec3(float.Parse(currentLine[1], CultureInfo.InvariantCulture.NumberFormat),
                                    float.Parse(currentLine[2], CultureInfo.InvariantCulture.NumberFormat),
                                    float.Parse(currentLine[3], CultureInfo.InvariantCulture.NumberFormat));
                            normals.Add(normal);
                        }
                        else if (line.StartsWith("f "))
                        {
                            break;
                        }
                    }
                    while (line != null && line.StartsWith("f "))
                    {
                        String[] currentLine = line.Split(' ');
                        String[] vertex1 = currentLine[1].Split('/');
                        String[] vertex2 = currentLine[2].Split('/');
                        String[] vertex3 = currentLine[3].Split('/');
                        processVertex(vertex1, vertices, indices);
                        processVertex(vertex2, vertices, indices);
                        processVertex(vertex3, vertices, indices);
                        line = sr.ReadLine();
                    }
                }
            }
            catch (IOException e)
            {

            }
            removeUnusedVertices(vertices);
            float[] verticesArray = new float[vertices.Count * 3];
            float[] texturesArray = new float[vertices.Count * 2];
            float[] normalsArray = new float[vertices.Count * 3];
            float furthest = convertDataToArrays(vertices, textures, normals, verticesArray,
                    texturesArray, normalsArray);
            uint[] indicesArray = convertIndicesListToArray(indices);

            float collisionRadius = GetCollisionRadius(vertices);
            return loader.LoadModel(objectTag, isKinematic, gl, 0, 0, 0, 0, 0, 0, 1f, verticesArray, indicesArray, texturesArray,
                normalsArray, textureImg, collisionRadius, vertexShaderCode, fragmentShaderCode, lights);
        }

        private static void processVertex(String[] vertex, List<Vertex> vertices, List<uint> indices)
        {
            uint index = uint.Parse(vertex[0]) - 1;
            Vertex currentVertex = vertices[Convert.ToInt32(index)];
            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;
            if (!currentVertex.isSet())
            {
                currentVertex.setTextureIndex(textureIndex);
                currentVertex.setNormalIndex(normalIndex);
                indices.Add(index);
            }
            else
            {
                dealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices,
                        vertices);
            }
        }

        private static uint[] convertIndicesListToArray(List<uint> indices)
        {
            uint[] indicesArray = new uint[indices.Count];
            for (int i = 0; i < indicesArray.Length; i++)
            {
                indicesArray[i] = indices[i];
            }
            return indicesArray;
        }

        private static float convertDataToArrays(List<Vertex> vertices, List<vec2> textures,
                List<vec3> normals, float[] verticesArray, float[] texturesArray,
                float[] normalsArray)
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];
                if (currentVertex.getLength() > furthestPoint)
                {
                    furthestPoint = currentVertex.getLength();
                }
                vec3 position = currentVertex.getPosition();
                vec2 textureCoord = textures[currentVertex.getTextureIndex()];
                vec3 normalVector = normals[currentVertex.getNormalIndex()];
                verticesArray[i * 3] = position.x;
                verticesArray[i * 3 + 1] = position.y;
                verticesArray[i * 3 + 2] = position.z;
                texturesArray[i * 2] = textureCoord.x;
                texturesArray[i * 2 + 1] = 1 - textureCoord.y;
                normalsArray[i * 3] = normalVector.x;
                normalsArray[i * 3 + 1] = normalVector.y;
                normalsArray[i * 3 + 2] = normalVector.z;
            }
            return furthestPoint;
        }

        private static void dealWithAlreadyProcessedVertex(Vertex previousVertex, int newTextureIndex,
                int newNormalIndex, List<uint> indices, List<Vertex> vertices)
        {
            if (previousVertex.hasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(Convert.ToUInt32(previousVertex.getIndex()));
            }
            else
            {
                Vertex anotherVertex = previousVertex.getDuplicateVertex();
                if (anotherVertex != null)
                {
                    dealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex,
                            indices, vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.getPosition());
                    duplicateVertex.setTextureIndex(newTextureIndex);
                    duplicateVertex.setNormalIndex(newNormalIndex);
                    previousVertex.setDuplicateVertex(duplicateVertex);
                    vertices.Add(duplicateVertex);
                    indices.Add(Convert.ToUInt32(duplicateVertex.getIndex()));
                }

            }
        }

        private static void removeUnusedVertices(List<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                if (!vertex.isSet())
                {
                    vertex.setTextureIndex(0);
                    vertex.setNormalIndex(0);
                }
            }
        }
        #endregion

        private static float GetCollisionRadius(List<Vertex> vertices)
        {
            float minX = float.MaxValue;
            float maxX = float.MinValue;

            float minZ = float.MaxValue;
            float maxZ = float.MinValue;

            vec3 pos;
            for(int i = 0; i < vertices.Count; i++)
            {
                pos = vertices[i].getPosition();
                if (pos.x < minX)
                    minX = pos.x;
                if (pos.x > maxX)
                    maxX = pos.x;

                if (pos.z < minZ)
                    minZ = pos.z;
                if (pos.z > maxZ)
                    maxZ = pos.z;
            }

            return Math.Min(Math.Abs(maxX - minX), Math.Abs(maxZ - minZ));
        }

        #region first code
        //public static Model LoadObjModel(OpenGL gl, Loader loader, string path, Image texture,
        //    string vertexShaderCode, string fragmentShaderCode, Light[] lights)
        //{
        //    List<vec3> vertices = new List<vec3>();
        //    List<vec2> textures = new List<vec2>();
        //    List<vec3> normals = new List<vec3>();
        //    List<uint> indices = new List<uint>();

        //    float[] verticesArray = null;
        //    float[] textureArray = null;
        //    float[] normalsArray = null;
        //    uint[] indicesArray = null;


        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            string line;
        //            while (true)
        //            {
        //                line = sr.ReadLine();
        //                string[] currentLine = line.Split(' ');
        //                if (line.StartsWith("v "))
        //                {
        //                    vertices.Add(new vec3(float.Parse(currentLine[2], CultureInfo.InvariantCulture.NumberFormat),
        //                        float.Parse(currentLine[3], CultureInfo.InvariantCulture.NumberFormat),
        //                        float.Parse(currentLine[4], CultureInfo.InvariantCulture.NumberFormat)));
        //                }
        //                else if (line.StartsWith("vt "))
        //                {
        //                    textures.Add(new vec2(float.Parse(currentLine[1], CultureInfo.InvariantCulture.NumberFormat),
        //                        float.Parse(currentLine[2], CultureInfo.InvariantCulture.NumberFormat)));

        //                }
        //                else if (line.StartsWith("vn "))
        //                {
        //                    normals.Add(new vec3(float.Parse(currentLine[1], CultureInfo.InvariantCulture.NumberFormat),
        //                        float.Parse(currentLine[2], CultureInfo.InvariantCulture.NumberFormat),
        //                        float.Parse(currentLine[3], CultureInfo.InvariantCulture.NumberFormat)));

        //                }
        //                else if (line.StartsWith("f "))
        //                {
        //                    break;
        //                }
        //            }

        //            textureArray = new float[vertices.Count * 2];
        //            normalsArray = new float[vertices.Count * 3];

        //            while (line != null)
        //            {
        //                if (!line.StartsWith("f "))
        //                {
        //                    line = sr.ReadLine();
        //                    continue;
        //                }

        //                string[] currentLine = line.Split(' ');
        //                string[] vertex1 = currentLine[1].Split('/');
        //                string[] vertex2 = currentLine[2].Split('/');
        //                string[] vertex3 = currentLine[3].Split('/');

        //                ProcessVertex(vertex1, indices, textures, normals, textureArray, normalsArray);
        //                ProcessVertex(vertex2, indices, textures, normals, textureArray, normalsArray);
        //                ProcessVertex(vertex3, indices, textures, normals, textureArray, normalsArray);

        //                line = sr.ReadLine();
        //            }
        //        }
        //    }
        //    catch (IOException e)
        //    {

        //    }

        //    verticesArray = new float[vertices.Count * 3];
        //    indicesArray = new uint[indices.Count];

        //    int vertexPointer = 0;

        //    foreach (vec3 vertex in vertices)
        //    {
        //        verticesArray[vertexPointer++] = vertex.x;
        //        verticesArray[vertexPointer++] = vertex.y;
        //        verticesArray[vertexPointer++] = vertex.z;
        //    }

        //    for (int i = 0; i < indices.Count; i++)
        //    {
        //        indicesArray[i] = indices[i];
        //    }

        //    return loader.LoadModel(gl, 0, 0, 0, 0, 0, 0, 0.2f, verticesArray, indicesArray, textureArray,
        //        normalsArray, texture, vertexShaderCode, fragmentShaderCode, lights);
        //}

        //private static void ProcessVertex(string[] vertexData, List<uint> indices,
        //    List<vec2> textures, List<vec3> normals, float[] textureArray,
        //    float[] normalsArray)
        //{
        //    uint currentVertexPointer = uint.Parse(vertexData[0]) - 1;
        //    indices.Add(currentVertexPointer);

        //    vec2 currentTex = textures[int.Parse(vertexData[1]) - 1];
        //    textureArray[currentVertexPointer * 2] = currentTex.x;
        //    textureArray[currentVertexPointer * 2 + 1] = 1-currentTex.y;

        //    vec3 currentNorm = normals[int.Parse(vertexData[2]) - 1];
        //    normalsArray[currentVertexPointer * 3] = currentNorm.x;
        //    normalsArray[currentVertexPointer * 3 + 1] = currentNorm.y;
        //    normalsArray[currentVertexPointer * 3 + 2] = currentNorm.z;
        //}
        #endregion
    }
}
