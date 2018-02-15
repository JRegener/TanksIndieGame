using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.entities;
using TanksIndieGame.view.models;

namespace TanksIndieGame.view.tool
{
    public class MousePicker
    {
        private const int RECURSION_COUNT = 200;
        private const float RAY_RANGE = 600;
        private vec3 curRay;
        private vec3 curGroungPoint;

        private mat4 projectionMatrix;
        private Camera camera;

        public MousePicker(Camera camera, mat4 projection)
        {
            this.camera = camera;
            this.projectionMatrix = projection;
        }

        public vec3 CurRay { get { return curRay; } }

        public vec3 CurGroundPoint { get { return curGroungPoint; } }

        public void Update(float mouseX, float mouseY, float width, float height)
        {
            curRay = CalculateMouceRay(mouseX, mouseY, width, height);
            float t = -camera.Position.y / curRay.y;
            float x = camera.Position.x + curRay.x * t;
            float z = camera.Position.z + curRay.z * t;
            curGroungPoint = new vec3(x, 0, z);
        }

        private vec3 CalculateMouceRay(float mouseX, float mouseY, float width, float height)
        {
            vec2 normalizedCoords = GetNormalizedDeviceCoords(mouseX, mouseY, width, height);
            vec4 clipCoords = new vec4(normalizedCoords.x, normalizedCoords.y, -1f, -1f);
            vec4 eyeCoords = ToEyeCoords(clipCoords);
            vec3 worldRay = ToWorldCoords(eyeCoords);
            return worldRay;
        }

        private vec3 ToWorldCoords(vec4 eyeCoords)
        {
            mat4 invertedView = glm.inverse(GLMath.CreateViewMatrix(camera));
            vec4 rayWorld = invertedView * eyeCoords;
            vec3 mouseRay = new vec3(rayWorld.x, rayWorld.y, rayWorld.z);
            mouseRay = glm.normalize(mouseRay);
            return mouseRay;
        }

        private vec4 ToEyeCoords(vec4 clipCoords)
        {
            mat4 invertedProjection = glm.inverse(projectionMatrix);
            vec4 eyeCoords = invertedProjection * clipCoords;
            return new vec4(eyeCoords.x, eyeCoords.y, -1f, 0);
        }

        private vec2 GetNormalizedDeviceCoords(float mouseX, float mouseY, float width, float height)
        {
            float x = (2f * mouseX) / width - 1;
            float y = 1 - (2f * mouseY) / height;
            return new vec2(x, y);
        }


        
    }
}
