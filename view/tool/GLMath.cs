using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.view.tool
{
    public static class GLMath
    {
        

        public static mat4 CreateTransformationMatrix(vec3 translation, vec3 rotation, float scale)
        {
            mat4 matrix = mat4.identity();
            matrix = glm.translate(matrix, translation);
            matrix = glm.rotate(matrix, rotation.x, new vec3(1, 0, 0));
            matrix = glm.rotate(matrix, rotation.y, new vec3(0, 1, 0));
            matrix = glm.rotate(matrix, rotation.z, new vec3(0, 0, 1));
            matrix = glm.scale(matrix, new vec3(scale, scale, scale));
            return matrix;
        }

        public static mat4 CreateViewMatrix(Camera camera)
        {
            mat4 viewMatrix = mat4.identity();
            viewMatrix = glm.rotate(viewMatrix, glm.radians(camera.Pitch), new vec3(1, 0, 0));
            viewMatrix = glm.rotate(viewMatrix, glm.radians(camera.Yaw), new vec3(0, 1, 0));
            vec3 negativeCamPos = new vec3(-camera.Position.x, -camera.Position.y, -camera.Position.z);
            viewMatrix = glm.translate(viewMatrix, negativeCamPos);
            return viewMatrix;
        }

        public static mat4 CreateProjectionMatrix(float fov, float width, float height,
                                                    float near, float far)
        {
            return glm.perspectiveFov(fov, width, height, near, far);
        }

        public static ushort[] ToUShort(uint[] arr)
        {
            ushort[] res = new ushort[arr.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = Convert.ToUInt16(arr[i]);
            }
            return res;
        }

        public static float BarryCentric(vec3 p1, vec3 p2, vec3 p3, vec2 pos)
        {
            float det = (p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z);
            float l1 = ((p2.z - p3.z) * (pos.x - p3.x) + (p3.x - p2.x) * (pos.y - p3.z)) / det;
            float l2 = ((p3.z - p1.z) * (pos.x - p3.x) + (p1.x - p3.x) * (pos.y - p3.z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.y + l2 * p2.y + l3 * p3.y;
        }

        public static bool InCircle(float cx, float cz, float x, float z, float radius)
        {
            return (x - cx) * (x - cx) + (z - cz) * (z - cz) < radius * radius;
        }


        public static vec3 GetNormal(vec3 a, vec3 b, vec3 c)
        {
            vec3 vec1 = b - a;
            vec3 vec2 = c - a;
            vec3 normal = MultVector(vec1, vec2) * -1;
            return glm.normalize(normal);
        }

        public static vec3 MultVector(vec3 a, vec3 b)
        {
            return new vec3(a.y * b.z - a.z * b.y,
                            a.z * b.x - a.x * b.z,
                            a.x * b.y - a.y * b.x);
        }

        public static vec3 GetAverageVector(vec3 v1, vec3 v2)
        {
            return glm.normalize(v1 + v2);
        }
    }
}
