using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic.scripts
{
    public class ShellBehaviour : ObjectBehaviour
    {
        private ObjectBehaviour tankScript;
        private float speed = 0.9f;
        private float velocity = 0;

        private vec3 moveDirection;

        #region properties
        public vec3 MoveDirection
        {
            get
            {
                return moveDirection;
            }

            set
            {
                moveDirection = value;
            }
        }

        public ObjectBehaviour TankScript
        {
            get
            {
                return tankScript;
            }

            set
            {
                tankScript = value;
            }
        }

        #endregion

        public ShellBehaviour(Model parentModel) : base(parentModel)
        { }

        public override void FixedUpdate()
        {
            if (parentModel != null)
            {
                velocity = speed;
                parentModel.BaseObject.Position += velocity * moveDirection;
            }
        }

        public override void Update(float interpolation)
        {
            if (parentModel != null)
                parentModel.BaseObject.ViewPosition = parentModel.BaseObject.Position
                + velocity * moveDirection * interpolation;
        }

        public override void CollisionDetected(Model obj)
        {
            velocity = 0;
            parentModel.BaseObject.Position = parentModel.BaseObject.OldPosition;
            parentModel.BaseObject.OldPosition = parentModel.BaseObject.Position;

            obj.CollisionObjects.AllCollidingObjects.Remove(parentModel);

            if (obj.IsKinematic)
            {
                ((TankBehaviour)tankScript).ShellDestroyed();
            }
            else
            {
                if (!obj.Tag.Equals("shell"))
                {
                    // самоуничтожение вражеского танка
                    ((TankBehaviour)obj.ObjectBehaviour).TankDestroyed();
                    ((TankBehaviour)tankScript).ShellDestroyed();
                    ((TankBehaviour)tankScript).IncreaseDestroyedTankCount();
                }
            }

            DestroyObject(parentModel);
            DestroyScript();
        }
    }
}
