using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic
{
    enum GameModel
    {
        Empty = '0',
        Wall = '1',
        Player = '2',
        Bot = '3'
    }

    public sealed class GameObjects
    {

        private static readonly Lazy<GameObjects> instanceHolder =
            new Lazy<GameObjects>(() => new GameObjects());

        public static GameObjects Instance
        {
            get { return instanceHolder.Value; }
        }

        public readonly vec3 ShellShiftPosition = new vec3(-4.7f, 3.22f, -0.06f);

        private vec2 mapStartPosition = new vec2(0, 0);
        private float mapWidth = 0; // X
        private float mapLength = 0; // Z

        private float mapGridAreaWidth = 0;
        private float mapGridAreaLength = 0;

        private int horizontalAreasCount = 4;
        private int verticalAreasCount = 4;

        private List<Model>[,] objectsInArea;
        private List<vec2> overlappingAreas = new List<vec2>();

        private List<Model> gameModels = new List<Model>();
        private Light lights = SceneSettings.BASE_LIGHT;


        private Model defaultWall = null;
        private Model playerTank = null;
        private Model defaultShell = null;
        private Model defaultBotTank = null;



        public List<Model> GameModels
        {
            get
            {
                return gameModels;
            }

        }

        public Light Lights
        {
            get
            {
                return lights;
            }

        }

        public Model DefaultWall
        {
            get
            {
                return defaultWall;
            }

            set
            {
                defaultWall = value;
            }
        }

        public Model PlayerTank
        {
            get
            {
                return playerTank;
            }

            set
            {
                playerTank = value;
            }
        }

        public Model DefaultShell
        {
            get
            {
                return defaultShell;
            }

            set
            {
                defaultShell = value;
            }
        }

        public Model DefaultBotTank
        {
            get
            {
                return defaultBotTank;
            }

            set
            {
                defaultBotTank = value;
            }
        }

        private GameObjects() { }

        public void LoadMap(string mapStructure)
        {
            float x = mapStartPosition.x;
            float z = mapStartPosition.y;
            float step = 10f;

            string[] lines = mapStructure.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.GetLength(0); i++)
            {
                for (int j = 0; j < lines.GetLength(0); j++)
                {
                    if (lines[i][j].Equals((char)GameModel.Empty))
                    {

                    }
                    else if (lines[i][j].Equals((char)GameModel.Wall))
                    {
                        if (defaultWall != null)
                        {
                            Model newWall = (Model)defaultWall.Clone();
                            newWall.BaseObject.Position = new vec3(x, 0, z);
                            gameModels.Add(newWall);
                        }
                    }
                    else if (lines[i][j].Equals((char)GameModel.Player))
                    {
                        if (playerTank != null)
                        {
                            playerTank.BaseObject.Position = new vec3(x, 0, z);
                            gameModels.Add(playerTank);
                        }
                    }
                    else if (lines[i][j].Equals((char)GameModel.Bot))
                    {

                    }

                    x += step;
                }
                mapWidth = x;
                x = 0;
                z += step;
            }

            mapLength = z;


            // Prepare collision
            mapGridAreaWidth = mapWidth / horizontalAreasCount;
            mapGridAreaLength = mapLength / verticalAreasCount;

            objectsInArea = new List<Model>[horizontalAreasCount, verticalAreasCount];

            for (int i = 0; i < verticalAreasCount; i++)
            {
                for (int j = 0; j < horizontalAreasCount; j++)
                    objectsInArea[j, i] = new List<Model>();
            }


        }

        public void CheckCollision()
        {
            vec2 overlap;
            for (int z = 0; z < verticalAreasCount; ++z)
            {
                for (int x = 0; x < horizontalAreasCount; ++x)
                {
                    List<Model> objInArea = objectsInArea[x, z];
                    for (int i = 0; i < objInArea.Count - 1; ++i)
                    {
                        Model obj1 = objInArea[i];
                        for (int j = i + 1; j < objInArea.Count; ++j)
                        {
                            Model obj2 = objInArea[j];

                            if (!obj1.Tag.Equals("wall") || !obj2.Tag.Equals("wall"))
                            {
                                if (OverlapsSigned(obj1, obj2, out overlap) && HasCollisionDataFor(obj1, obj2))
                                {
                                    obj1.CollisionObjects.AllCollidingObjects.Add(new CollisionData(obj2, overlap,
                                        new vec2(obj1.BaseObject.Position), new vec2(obj2.BaseObject.Position)));
                                    obj2.CollisionObjects.AllCollidingObjects.Add(new CollisionData(obj1, new vec2(-overlap.x, -overlap.y),
                                        new vec2(obj2.BaseObject.Position), new vec2(obj1.BaseObject.Position)));

                                }
                            }
                        }
                    }
                }
            }
        }

        private bool OverlapsSigned(Model first, Model second, out vec2 overlap)
        {
            overlap = new vec2();

            if (Math.Abs(first.BaseObject.Position.x - second.BaseObject.Position.x) > first.ModelCollision.HalfWeight + second.ModelCollision.HalfWeight
                || Math.Abs(first.BaseObject.Position.z - second.BaseObject.Position.z) > first.ModelCollision.HalfLength + second.ModelCollision.HalfLength)
                return false;

            overlap = new vec2(Math.Sign(first.BaseObject.Position.x - second.BaseObject.Position.x)
                * ((first.ModelCollision.HalfWeight + second.ModelCollision.HalfWeight)
                - Math.Abs(first.BaseObject.Position.x - second.BaseObject.Position.x)),
                Math.Sign(first.BaseObject.Position.z - second.BaseObject.Position.z)
                * ((first.ModelCollision.HalfLength + second.ModelCollision.HalfLength)
                - Math.Abs(first.BaseObject.Position.z - second.BaseObject.Position.z)));

            return true;
        }

        private bool HasCollisionDataFor(Model first, Model second)
        {
            for (int i = 0; i < first.CollisionObjects.AllCollidingObjects.Count; ++i)
            {
                if (first.CollisionObjects.AllCollidingObjects[i].other == second)
                    return true;
            }

            return false;
        }

        private vec2 GetMapObjectPosition(vec2 position)
        {
            return new vec2(position.x - mapStartPosition.x,
                position.y - mapStartPosition.y);
        }

        public void UpdateAreas(Model obj)
        {
            //Get point in local map coordinates

            vec2 topLeft = GetMapObjectPosition(new vec2(obj.BaseObject.PosX - obj.ModelCollision.HalfWeight,
                obj.BaseObject.PosZ - obj.ModelCollision.HalfLength));
            vec2 topRight = GetMapObjectPosition(new vec2(obj.BaseObject.PosX + obj.ModelCollision.HalfWeight,
                obj.BaseObject.PosZ - obj.ModelCollision.HalfLength));
            vec2 bottomLeft = GetMapObjectPosition(new vec2(obj.BaseObject.PosX - obj.ModelCollision.HalfWeight,
                obj.BaseObject.PosZ + obj.ModelCollision.HalfLength));
            vec2 bottomRight = new vec2();

            topLeft.x = (int)(topLeft.x / mapGridAreaWidth);
            topLeft.y = (int)(topLeft.y / mapGridAreaLength);

            topRight.x = (int)(topRight.x / mapGridAreaWidth);
            topRight.y = (int)(topRight.y / mapGridAreaLength);

            bottomLeft.x = (int)(bottomLeft.x / mapGridAreaWidth);
            bottomLeft.y = (int)(bottomLeft.y / mapGridAreaLength);

            bottomRight.x = topRight.x;
            bottomRight.y = bottomLeft.y;

            //see how many different areas we have
            if (topLeft.x == topRight.x && topLeft.y == bottomLeft.y)
            {
                overlappingAreas.Add(topLeft);
            }
            else if (topLeft.x == topRight.x)
            {
                overlappingAreas.Add(topLeft);
                overlappingAreas.Add(bottomLeft);
            }
            else if (topLeft.y == bottomLeft.y)
            {
                overlappingAreas.Add(topLeft);
                overlappingAreas.Add(topRight);
            }
            else
            {
                overlappingAreas.Add(topLeft);
                overlappingAreas.Add(bottomLeft);
                overlappingAreas.Add(topRight);
                overlappingAreas.Add(bottomRight);
            }

            List<vec2> areas = obj.CollisionObjects.Areas;
            List<int> ids = obj.CollisionObjects.IdsInAreas;

            for (int i = 0; i < areas.Count; ++i)
            {
                if (!overlappingAreas.Contains(areas[i]))
                {
                    RemoveObjectFromArea(areas[i], ids[i], obj);
                    //object no longer has an index in the area
                    areas.RemoveAt(i);
                    ids.RemoveAt(i);
                    --i;
                }
            }

            for (var i = 0; i < overlappingAreas.Count; ++i)
            {
                vec2 area = overlappingAreas[i];
                if (!areas.Contains(area))
                    AddObjectToArea(area, obj);
            }

            overlappingAreas.Clear();

        }

        private void AddObjectToArea(vec2 areaIndex, Model obj)
        {
            List<Model> area = objectsInArea[(int)areaIndex.x, (int)areaIndex.y];

            //save the index of  the object in the area
            obj.CollisionObjects.Areas.Add(areaIndex);
            obj.CollisionObjects.IdsInAreas.Add(area.Count);

            //add the object to the area
            area.Add(obj);
        }

        private void RemoveObjectFromArea(vec2 areaIndex, int objIndexInArea, Model obj)
        {
            List<Model> area = objectsInArea[(int)areaIndex.x, (int)areaIndex.y];

            //swap the last item with the one we are removing
            Model tmp = area[area.Count - 1];
            area[area.Count - 1] = obj;
            area[objIndexInArea] = tmp;

            List<int> tmpIds = tmp.CollisionObjects.IdsInAreas;
            List<vec2> tmpAreas = tmp.CollisionObjects.Areas;
            for (int i = 0; i < tmpAreas.Count; ++i)
            {
                if (tmpAreas[i].x == areaIndex.x && tmpAreas[i].y == areaIndex.y)
                {
                    tmpIds[i] = objIndexInArea;
                    break;
                }
            }

            //remove the last item
            area.RemoveAt(area.Count - 1);
        }
    }
}
