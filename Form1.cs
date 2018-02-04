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

        private GameControl gameControl;


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
            gameControl = GameControl.Instance;

            Model model = OBJLoader.LoadObjModel("shell", gl, loader, 
                @"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\shell.obj", 
                Image.FromFile(@"C:\Users\Regener\Documents\GameProgramming\Models\Tanks\shell.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameControl.Lights);
            gameControl.RenderModels.Add(model);
        }

        private void glControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if(renderer == null || camera == null)
            {
                return;
            }
            renderer.Prepare(gl);
            renderer.Render(gl, gameControl.RenderModels, camera);

            lblInfo.Text = camera.GetInfo();
        }

        #region key/mouse control
        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isMouseDown = true;
                oldPosition = e.Location;
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
