using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;
using TanksIndieGame.view.tool;

namespace TanksIndieGame.logic.scripts
{
    public class TankBehaviour : ObjectBehaviour
    {

        private float speed = 0.7f;
        private float velocity = 0;
        private vec3 moveDirection = new vec3(-1, 0, 0);
        private float rotationAngle = 0;
        private float shiftAngle = (float)Math.PI;

        private bool isMove = false;

        private bool isShellExist = false;

        private bool isAlive = true;

        private int countDestroyedTanks = 0;

        public TankBehaviour(Model parentModel) : base(parentModel)
        {
        }

        public override void FixedUpdate()
        {
            if (isMove)
            {
                velocity = speed;
                parentModel.BaseObject.Position += velocity * moveDirection;
            }
        }

        public override void Update(float interpolation)
        {

            if (isMove)
            {
                parentModel.BaseObject.ViewPosition = parentModel.BaseObject.Position
                    + velocity * moveDirection * interpolation;
            }
        }

        public override void CollisionDetected(Model obj)
        {
            if (obj.IsKinematic)
            {
                velocity = 0;
                parentModel.BaseObject.Position = parentModel.BaseObject.OldPosition;
                parentModel.BaseObject.OldPosition = parentModel.BaseObject.Position;
            }
            else
            {
                if (!obj.Tag.Equals("shell"))
                {
                    velocity = 0;
                    parentModel.BaseObject.Position = parentModel.BaseObject.OldPosition;
                    parentModel.BaseObject.OldPosition = parentModel.BaseObject.Position;
                }
            }
        }

        public void SetViewDirection(vec3 mouseCoord)
        {
            vec3 startDirection = new vec3(1, 0, 0);
            moveDirection = mouseCoord - parentModel.BaseObject.Position;
            moveDirection = glm.normalize(moveDirection);

            vec3 scalarProduct = startDirection * moveDirection;
            float cos = scalarProduct.x + scalarProduct.y + scalarProduct.z /
                GLMath.GetLength(startDirection) * GLMath.GetLength(moveDirection);

            if (moveDirection.z > 0)
            {
                rotationAngle = glm.acos(cos) + shiftAngle;
            }
            else
            {
                rotationAngle = 2 * (float)Math.PI - glm.acos(cos) + shiftAngle;
            }

            rotationAngle = 2 * (float)Math.PI - rotationAngle;
            parentModel.BaseObject.Rotation = new vec3(0, rotationAngle, 0);
        }

        public void Move(bool isMove)
        {
            this.isMove = isMove;
        }

        public void Fire()
        {
            if (isShellExist)
            {
                return;
            }

            isShellExist = true;

            Model shell = InitializeObject(gameObjects.DefaultShell);

            shell.BaseObject.Position = parentModel.BaseObject.Position + GLMath.RotateVector(rotationAngle, gameObjects.ShellShiftPosition);
            shell.BaseObject.Rotation = new vec3(0, rotationAngle + (float)Math.PI, 0);
            shell.BaseObject.Scale = 2f;

            ShellBehaviour shellBehaviour = new ShellBehaviour(shell);
            shellBehaviour.MoveDirection = new vec3(moveDirection);
            shellBehaviour.TankScript = this;
            shell.ObjectBehaviour = shellBehaviour;

        }

        public void IncreaseDestroyedTankCount()
        {
            countDestroyedTanks++;
        }

        public void ShellDestroyed()
        {
            isShellExist = false;
        }

        public void TankDestroyed()
        {
            isAlive = false;

            DestroyObject(parentModel);
            DestroyScript();
        }



    }
}
