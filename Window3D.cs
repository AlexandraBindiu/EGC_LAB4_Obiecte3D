using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obiecte3D
{
    class Window3D : GameWindow
    {
        private KeyboardState previousKeyboard;
        private MouseState previousMouse;
        private readonly Randomizer rando;
        private readonly Axes ax;
        private readonly Grid grid;
        private readonly Camera3DIsometric cam;

        private bool GRAVITY = true;

        private List<Objectoid> rainOfobjects;

        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);


        public Window3D() : base (1280, 768, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            rando = new Randomizer();
            ax = new Axes();
            grid = new Grid();
            cam = new Camera3DIsometric();

            rainOfobjects = new List<Objectoid>();
            DisplayHelp();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //set background
            GL.ClearColor(DEFAULT_BKG_COLOR);

            // set viewpoint
            GL.Viewport(0, 0, this.Width, this.Height);

            //set perspective
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 256);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            //setare camera
            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Width/(float)this.Width, 1, 256);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);
            cam.SetCamera();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //logic code

            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            if(currentKeyboard[Key.Escape])
            {
                Exit();
            }
            if (currentKeyboard[Key.N] && !previousKeyboard[Key.N])
            {
                DisplayHelp();
            }
            if (currentKeyboard[Key.R] && !previousKeyboard[Key.R] )
            {
                GL.ClearColor(DEFAULT_BKG_COLOR);
                ax.Show();
                grid.Show();

            }
            if (currentKeyboard[Key.K] && !previousKeyboard[Key.K])
            {
               ax.ToggleVisibility();
            }
            if (currentKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(rando.RandomColor());
            }
            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                grid.ToggleVisibility();
            }

            ///camera contol
            if(currentKeyboard[Key.W])
            {
                cam.MoveForward();
            }
            if (currentKeyboard[Key.S])
            {
                cam.MoveBackward();
            }
            if (currentKeyboard[Key.A])
            {
                cam.MoveLeft();
            }
            if (currentKeyboard[Key.D])
            {
                cam.MoveRight();
            }
            if (currentKeyboard[Key.Q])
            {
                cam.MoveUp();
            }
            if (currentKeyboard[Key.E])
            {
                cam.MoveDown();
            }

            //object spam
            if (currentMouse[MouseButton.Left] && !previousMouse[MouseButton.Left])
            {
                rainOfobjects.Add(new Objectoid(GRAVITY));
            }
            //pbject spam cleanup
            if (currentMouse[MouseButton.Right] && !previousMouse[MouseButton.Right])
            {
                rainOfobjects.Clear();
            }
            //switch gravity
            if (currentKeyboard[Key.G] && !previousKeyboard[Key.G])
            { 

                foreach(  Objectoid obj in rainOfobjects)

                { obj.ToggleGravity(); }
        
            }

            //update falling logic
            foreach (Objectoid obj in rainOfobjects)
            {
                obj.UpdatePosition(GRAVITY);
            }

            previousKeyboard = currentKeyboard;
            previousKeyboard = currentKeyboard;

            //end logic code
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            //render code
            grid.Draw();
            ax.Draw();

           // Objectoid obj = new Objectoid();
           //obj.Draw();
           foreach (Objectoid obj in rainOfobjects)
            {
                obj.Draw();
            }


            SwapBuffers();
        }
       

       
        private void DisplayHelp()
        {
            Console.WriteLine("\n  MENIU");
            Console.WriteLine("H -   meniu");
            Console.WriteLine("ESC -   parasire aplicatie");
            Console.WriteLine("K -   schimbare vizibilitate sistem de axe");
            Console.WriteLine("R -   reseteaza scena la valori implicite");
            Console.WriteLine("B -   schimbare culoare de fundal");
            Console.WriteLine("V -   schimbare vizibilitate");
            Console.WriteLine("W, A, S, D, Q, E -   deplasare camera");
            Console.WriteLine("G -   manipuleaza gravitatia");
            Console.WriteLine("Mouse clic stanga - genereaza un nou obiect");
            Console.WriteLine("Mouse clic dreapta - genereaza un nou obiect");


        }
    }
}
