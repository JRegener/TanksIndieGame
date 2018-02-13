using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksIndieGame.logic;
using TanksIndieGame.logic.scripts;
using TanksIndieGame.view.models;
using TanksIndieGame.view.render;
using TanksIndieGame.view.tool;

namespace TanksIndieGame
{
    public partial class Form1 : Form
    {
        private bool isMouseDown;
        private Point oldPosition;

        private OpenGL gl;
        private Renderer renderer;
        private Loader loader;
        private Camera camera;

        private GameObjects gameObjects;
        private MainGameLoopThread mainGameLoop;

        private PlayerTankBehaviour player;

        Model shell;

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl_Resized(object sender, EventArgs e)
        {
            if (renderer == null)
            {
                return;
            }
            renderer.SetViewProperties(SceneSettings.FOV, SceneSettings.NEAR,
                SceneSettings.FAR, glControl.Width, glControl.Height);
        }

        private void glControl_OpenGLInitialized(object sender, EventArgs e)
        {

            gl = glControl.OpenGL;
            renderer = new Renderer(SceneSettings.FOV, SceneSettings.NEAR,
                SceneSettings.FAR, glControl.Width, glControl.Height);
            loader = new Loader();
            camera = new Camera();
            gameObjects = GameObjects.Instance;

            mainGameLoop = new MainGameLoopThread(glControl);

            shell = OBJLoader.LoadObjModel("shell", gl, loader,
                @"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\shell.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\shell.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            Model tank = OBJLoader.LoadObjModel("tank", gl, loader,
                @"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\tank.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\tank.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            //tank.BaseObject.PosX = 0f;

            //player = new PlayerTankBehaviour(tank);
            //tank.ObjectBehaviour = player;

            Model wall = OBJLoader.LoadObjModel("wall", gl, loader,
                @"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\wall.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\wall.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            //tank.BaseObject.Scale = 1f;
            //shell.BaseObject.Scale = 1f;
            //wall.BaseObject.Scale = 1f;

            player = new PlayerTankBehaviour(tank);
            tank.ObjectBehaviour = player;

            gameObjects.DefaultWall = wall;
            gameObjects.DefaultShell = shell;
            gameObjects.PlayerTank = tank;
            gameObjects.LoadMap(Resource.map);
            //wall.BaseObject.PosZ = 5f;

            shell.BaseObject.Rotation = new vec3(0, 3.14f, 0);

            //gameObjects.GameModels.Add(wall);
            //gameObjects.GameModels.Add(shell);
            //gameObjects.GameModels.Add(tank);

            mainGameLoop.Start();
        }

        private void glControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (renderer == null || camera == null)
            {
                return;
            }
            renderer.Prepare(gl);
            renderer.Render(gl, gameObjects.GameModels, camera);

            lblInfo.Text = camera.GetInfo();
        }

        #region key/mouse control
        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isMouseDown = true;
                oldPosition = e.Location;
                //player.Move(false);
            }
            if (e.Button == MouseButtons.Left)
            {
                player.Fire();
            }

        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isMouseDown = false;
            }
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!isMouseDown)
                {
                    return;
                }

                int dx = oldPosition.X - e.Location.X;
                int dy = oldPosition.Y - e.Location.Y;

                if (Math.Abs(dy) > Math.Abs(dx))
                {
                    if (dy < 0)
                    {
                        camera.IncreasePitch(1);
                    }
                    else
                    {
                        camera.DecreasePitch(1);
                    }
                    oldPosition = e.Location;
                }
                if (Math.Abs(dy) < Math.Abs(dx))
                {
                    if (dx < 0)
                    {
                        camera.IncreaseAroundPoint(1);
                    }
                    else
                    {
                        camera.DecreaseAroundPoint(1);
                    }
                    oldPosition = e.Location;
                }

            }




        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.MoveForward();
            }

            if (e.KeyCode == Keys.S)
            {
                camera.MoveBack();
            }

            if (e.KeyCode == Keys.A)
            {
                camera.MoveLeft();
            }

            if (e.KeyCode == Keys.D)
            {
                camera.MoveRight();
            }

        }

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                camera.DecreaceDistance();
            }
            else
            {
                camera.IncreaceDistance();
            }

        }

        #endregion

    }
}
