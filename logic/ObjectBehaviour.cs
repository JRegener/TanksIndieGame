using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;
using TanksIndieGame.view.render;

namespace TanksIndieGame.logic
{
    public class ObjectBehaviour
    {
        protected GameObjects gameObjects = GameObjects.Instance;

        protected Model parentModel;

        public ObjectBehaviour(Model parentModel)
        {
            this.parentModel = parentModel;
        }

        #region properties
        public Model ParentModel
        {
            get { return this.parentModel; }
            set { this.parentModel = value; }
        }
        #endregion

        public virtual void FixedUpdate() { }

        public virtual void Update(float interpolation) { }

        public virtual void CollisionDetected(Model obj) { }

        protected Model InitializeObject(Model obj)
        {
            gameObjects.GameModels.Add((Model)obj.Clone());
            
            return gameObjects.GameModels.Last();
        }

        protected void DestroyObject(Model obj)
        {

            int idx = gameObjects.GameModels.IndexOf(obj);

            for (int i = 0; i < gameObjects.GameModels[idx].CollisionObjects.Areas.Count; i++)
            {
                gameObjects.RemoveObjectFromArea(gameObjects.GameModels[idx].CollisionObjects.Areas[i],
                    gameObjects.GameModels[idx].CollisionObjects.IdsInAreas[i], 
                    gameObjects.GameModels[idx]);
            }
            gameObjects.GameModels[idx].Destroy();
            gameObjects.GameModels[idx] = null;
            gameObjects.GameModels.RemoveAt(idx);
        }

        public void DestroyScript()
        {
            parentModel = null;
            gameObjects = null;
        }

    }
}
