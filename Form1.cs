﻿using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksIndieGame.logic;
using TanksIndieGame.logic.scripts;
using TanksIndieGame.view.entities;
using TanksIndieGame.view.models;
using TanksIndieGame.view.render;
using TanksIndieGame.view.tool;

namespace TanksIndieGame
{
    public partial class Form1 : Form
    {
        //желательный fps
        private const int MAX_FPS = 50;
        //максимальное число кадров, которые можно пропустить
        private const int MAX_FRAME_SKIPS = 5;
        //период, который занимает кадр(последовательность обновление-рисование)
        private const int FRAME_PERIOD = 1000 / MAX_FPS;

        private long currentTime;     // время начала цикла
        private float interpolation = 0;
        private int loops;

        private MousePicker mousePicker;
        private bool isMouseDown;
        private Point oldPosition;

        private OpenGL gl;
        private Renderer renderer;
        private Loader loader;
        private Camera camera;

        private GameObjects gameObjects;

        private Stopwatch stopwatch = null;

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

            if (glControl.Width == 0 || glControl.Height == 0)
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
            mousePicker = new MousePicker(camera, renderer.ProjectionMatrix);

            stopwatch = Stopwatch.StartNew();
            currentTime = stopwatch.ElapsedMilliseconds;

            GroundControl ground = new GroundControl(gl, loader, Image.FromFile(@"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\ground.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            Model shell = OBJLoader.LoadObjModel("shell", false, gl, loader,
                @"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\shell.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\shell.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            shell.BaseObject.Scale = 50f;

            Model tank = OBJLoader.LoadObjModel("tank", false, gl, loader,
                @"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\tank.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\tank.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            Model enemyTank = OBJLoader.LoadObjModel("enemyTank", false, gl, loader,
                @"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\enemy_tank.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\enemy_tank.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            Model wall = OBJLoader.LoadObjModel("wall", true, gl, loader,
                @"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\wall.obj",
                Image.FromFile(@"C:\Users\Regener\Documents\visual studio 2015\Projects\TanksIndieGame\TanksIndieGame\objects\wall_stone.png"),
                Resource.vertexModelShader, Resource.fragmentModelShader, gameObjects.Lights);

            gameObjects.PlayerTankBehaviour = new PlayerTankBehaviour(tank);
            tank.ObjectBehaviour = gameObjects.PlayerTankBehaviour;

            gameObjects.GameModels.Add(ground.GroundModel);

            gameObjects.DefaultWall = wall;
            gameObjects.DefaultShell = shell;
            gameObjects.DefaultPlayerTank = tank;
            gameObjects.DefaultEnemyTank = enemyTank;
            gameObjects.LoadMap(Resource.map);

            gameLoopTimer.Start();
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
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                oldPosition = e.Location;
                //player.Move(false);
            }

        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    isMouseDown = false;
            //}
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            mousePicker.Update(e.X, e.Y, glControl.Width, glControl.Height);
            ((PlayerTankBehaviour)gameObjects.PlayerTankBehaviour).SetViewDirection(mousePicker.CurGroundPoint);
            //if (e.Button == MouseButtons.Left)
            //{
            //    if (!isMouseDown)
            //    {
            //        return;
            //    }

            //    int dx = oldPosition.X - e.Location.X;
            //    int dy = oldPosition.Y - e.Location.Y;

            //    if (Math.Abs(dy) > Math.Abs(dx))
            //    {
            //        if (dy < 0)
            //        {
            //            camera.IncreasePitch(1);
            //        }
            //        else
            //        {
            //            camera.DecreasePitch(1);
            //        }
            //        oldPosition = e.Location;
            //    }
            //    if (Math.Abs(dy) < Math.Abs(dx))
            //    {
            //        if (dx < 0)
            //        {
            //            camera.IncreaseAroundPoint(1);
            //        }
            //        else
            //        {
            //            camera.DecreaseAroundPoint(1);
            //        }
            //        oldPosition = e.Location;
            //    }

            //}




        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.W)
            //{
            //    camera.MoveForward();
            //}

            //if (e.KeyCode == Keys.S)
            //{
            //    camera.MoveBack();
            //}

            //if (e.KeyCode == Keys.A)
            //{
            //    camera.MoveLeft();
            //}

            //if (e.KeyCode == Keys.D)
            //{
            //    camera.MoveRight();
            //}



            if(e.KeyCode == Keys.X)
            {
                ((PlayerTankBehaviour)gameObjects.PlayerTankBehaviour).Move(true);
            }

            if(e.KeyCode == Keys.F)
            {
                ((PlayerTankBehaviour)gameObjects.PlayerTankBehaviour).Fire();
            }

        }


        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X)
            {
                ((PlayerTankBehaviour)gameObjects.PlayerTankBehaviour).Move(false);
            }
        }

        #endregion

        #region MAIN GAME LOOP
        private void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            loops = 0;
            while (stopwatch.ElapsedMilliseconds > currentTime && loops < MAX_FRAME_SKIPS)
            {
                UpdateGame();

                currentTime += FRAME_PERIOD;
                loops++;
            }

            interpolation = (stopwatch.ElapsedMilliseconds + FRAME_PERIOD - currentTime)
                            / (float)(FRAME_PERIOD);
            UpdateDisplay(interpolation);
        }

        private void UpdateGame()
        {
            // Update object logic
            for (int i = 0; i < gameObjects.GameModels.Count; i++)
            {
                if (gameObjects.GameModels[i].ObjectBehaviour != null)
                {
                    gameObjects.GameModels[i].ObjectBehaviour.FixedUpdate();
                }
                gameObjects.UpdateAreas(gameObjects.GameModels[i]);
                gameObjects.GameModels[i].CollisionObjects.AllCollidingObjects.Clear();
            }

            gameObjects.CheckCollision();

            gameObjects.UpdateCollision();
        }

        private void UpdateDisplay(float interpolation)
        {
            for (int i = 0; i < gameObjects.GameModels.Count; i++)
            {
                if (gameObjects.GameModels[i].ObjectBehaviour != null)
                    gameObjects.GameModels[i].ObjectBehaviour.Update(interpolation);
            }

            glControl.DoRender();
        }
        #endregion
    }
}
