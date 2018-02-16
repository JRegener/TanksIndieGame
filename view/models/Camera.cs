using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class Camera
    {
        private vec3 direction = new vec3(0, 0, 0);
        private vec3 position = new vec3(0, 0, 0);
        private float pitch = 45f;
        private float yaw = -45f;
        private float distance = 150f;
        private float projectionDistance;
        private float angelAround = 45f;

        private float step = 1f;


        public vec3 Direction
        {
            get { return direction; }
        }

        public vec3 Position
        {
            get { return position; }
        }

        public float Pitch
        {
            get { return pitch; }
        }

        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        private void CalculateDirection()
        {
            float x = step * glm.sin(glm.radians(180 - yaw));
            float z = step * glm.cos(glm.radians(180 - yaw));
            direction.x += x;
            direction.z += z;
        }

        public Camera()
        {
            CalculateNewLocation();
        }

        public void MoveForward()
        {
            CalculateDirection();
            CalculateNewLocation();
        }
        public void MoveBack()
        {

            CalculateNewLocation();
        }
        public void MoveRight()
        {

            CalculateNewLocation();
        }
        public void MoveLeft()
        {

            CalculateNewLocation();
        }
        public void MoveUp()
        {
            direction.y -= 0.05f;
            CalculateNewLocation();
        }
        public void MoveDown()
        {
            direction.y += 0.05f;
            CalculateNewLocation();
        }

        public void IncreasePitch(int deltha)
        {
            pitch += deltha;
            CalculateNewLocation();
        }

        public void DecreasePitch(int deltha)
        {
            pitch -= deltha;
            CalculateNewLocation();
        }

        public void IncreaseAroundPoint(int deltha)
        {
            angelAround += deltha;
            CalculateNewLocation();
            yaw = 180 + (angelAround + 90);
        }

        public void DecreaseAroundPoint(int deltha)
        {
            angelAround -= deltha;
            CalculateNewLocation();
            yaw = 180 + (angelAround + 90);
        }

        public void ChangePosition(int delthaX, int delthaZ)
        {
            direction.x += delthaX * 0.0005f;
            direction.z += delthaZ * 0.0005f;
            CalculateNewLocation();
        }

        public void IncreaceDistance()
        {
            distance += 0.5f;
            CalculateNewLocation();
        }

        public void DecreaceDistance()
        {
            distance -= 0.5f;
            CalculateNewLocation();
        }


        public void CalculateNewLocation()
        {
            projectionDistance = distance * glm.cos(glm.radians(pitch));
            position.y = direction.y + distance * glm.sin(glm.radians(pitch));

            position.x = direction.x + projectionDistance * glm.cos(glm.radians(angelAround));
            position.z = direction.z + projectionDistance * glm.sin(glm.radians(angelAround));
        }

        public string GetInfo()
        {
            return "pos X " + Position.x.ToString() + Environment.NewLine +
                "pos Y " + Position.y.ToString() + Environment.NewLine +
                "pos Z " + Position.z.ToString() + Environment.NewLine +
                "dir X " + Direction.z.ToString() + Environment.NewLine +
                "dir Y " + Direction.z.ToString() + Environment.NewLine +
                "dir Z " + Direction.z.ToString() + Environment.NewLine +
                "around " + angelAround.ToString() + Environment.NewLine +
                "yaw " + Yaw.ToString() + Environment.NewLine +
                "pitch " + Pitch.ToString() + Environment.NewLine +
                "R " + distance.ToString();
        }
    }
}
