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
    public class PlayerTankBehaviour : ObjectBehaviour
    {
        private GameObjects gameObjects = GameObjects.Instance;

        private float velocity = 0.7f;
        private vec3 moveDirection = new vec3(-1, 0, 0);
        private float rotationAngle = 0;
        private float shiftAngle = (float)Math.PI;

        private bool isMove = false;

        public PlayerTankBehaviour(Model parentModel) : base(parentModel)
        { }

        public override void FixedUpdate()
        {
            if (isMove)
            {
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

        public void Move(bool isMove)
        {
            this.isMove = isMove;
        }

        public void Fire()
        {
            Model newShell = (Model)gameObjects.DefaultShell.Clone();
            newShell.BaseObject.Position = parentModel.BaseObject.Position + GLMath.RotateVector(rotationAngle, gameObjects.ShellShiftPosition);
            newShell.BaseObject.Rotation = new vec3(0, rotationAngle + (float)Math.PI, 0);
            ShellBehaviour shellBehaviour = new ShellBehaviour(newShell);
            shellBehaviour.MoveDirection = new vec3(moveDirection);
            newShell.ObjectBehaviour = shellBehaviour;

            gameObjects.GameModels.Add(newShell);
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
    }
}
