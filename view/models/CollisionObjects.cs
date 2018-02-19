using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{

    public class CollisionObjects
    {
        private List<vec2> areas = new List<vec2>();
        private List<int> idsInAreas = new List<int>();

        private List<CollisionData> allCollidingObjects = new List<CollisionData>();

        #region properties
        public List<vec2> Areas
        {
            get
            {
                return areas;
            }

            set
            {
                areas = value;
            }
        }

        public List<int> IdsInAreas
        {
            get
            {
                return idsInAreas;
            }

            set
            {
                idsInAreas = value;
            }
        }

        public List<CollisionData> AllCollidingObjects
        {
            get
            {
                return allCollidingObjects;
            }

            set
            {
                allCollidingObjects = value;
            }
        }


        #endregion

        
    }
}
