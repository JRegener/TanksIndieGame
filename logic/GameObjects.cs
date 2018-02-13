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
            float x = 0;
            float z = 0;
            float step = 10f;

            string[] lines = mapStructure.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < lines.GetLength(0); i++)
            {
                for(int j = 0; j < lines.GetLength(0); j++)
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
                x = 0;
                z += step;
            }
        }
    }
}
