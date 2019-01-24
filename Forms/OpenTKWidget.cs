using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK;
using L3DS.Engine.Views;
using System.Diagnostics;

namespace L3DS.Forms
{
    public partial class OpenTKWidget : UserControl
    {
        // Constants:
        private static readonly float velocitySpeed = 50.0f;
        private static readonly float panSpeed = 40.0f;

        // Globals:
        private Camera cam;
        private Vector3 scale;
        public float xrot, yrot, sdepth;
        public int xcursor, ycursor;
        public List<Vector3> pointClouds;
        public Vector3 mainPosition;
        public bool changed = false;
        public Matrix4 view, proj;

        private Form parentForm;
        private float zoomSpeed;

        public OpenTKWidget()
        {
            InitializeComponent();
            scale = new Vector3(1.0f);
            pointClouds = new List<Vector3>();
            xrot = yrot = 0.0f;
            mainPosition = new Vector3();
            cam = new Camera();
            cam.Width = glControl.Width;
            cam.Height = glControl.Height;
            changed = true;
            zoomSpeed = 50.0f;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (ParentForm != null)
            {
                ParentForm.Closing -= parentForm_Closing;
            }

            parentForm = FindForm();

            if (ParentForm != null)
                ParentForm.Closing += parentForm_Closing;
        }

        void parentForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parentForm.Closing -= parentForm_Closing;
            parentForm = null;
            OnClosing(e);
        }

        public void ClearData()
        {
            pointClouds.Clear();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            glControl.MouseWheel += GlControl_MouseWheel;
            glControl.KeyDown += glControl_KeyDown;
            glControl.Resize += glControl_Resize;
            glControl.MouseMove += GlControl_MouseMove;

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);

            Application.Idle += Application_Idle;

            glControl_Resize(glControl, EventArgs.Empty);
        }

        private float DegreeToRadian(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }

        private void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float dx = velocitySpeed * (e.X - xcursor) / Width;
                float dy = velocitySpeed * (e.Y - ycursor) / Height;

                cam.Rotate(DegreeToRadian(dx), DegreeToRadian(dy));

            } else if(e.Button == MouseButtons.Right)
            {
                float dx = panSpeed * (e.X - xcursor) / Width;
                float dy = panSpeed * (e.Y - ycursor) / Height;

                cam.Pan(dx, dy, 0.0f);
            }


            xcursor = e.X;
            ycursor = e.Y;
        }

        private void GlControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta > 0)
                cam.ZoomCamera(-zoomSpeed);
            else if (e.Delta < 0)
                cam.ZoomCamera(zoomSpeed);
        }

        protected void OnClosing(CancelEventArgs e)
        {
            Application.Idle -= Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                Render();
            }
        }

       public void SetOffset(Vector3 offset)
        {
            cam.ViewCenter = offset;
        }

        private void BuildMatrices()
        {
            cam.UpdateCamera();
            proj = cam.ProjectionMatrix;
            view = cam.ViewMatrix;

            GL.MatrixMode(MatrixMode.Projection); //Load Perspective
            GL.LoadIdentity();
            GL.Viewport(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview); //Load Camera
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);
        }

        private void RenderAxis()
        {
            GL.Begin(BeginMode.Lines);
            GL.Color3(Color.Green);
            GL.Vertex3(cam.ViewCenter.X - 2, cam.ViewCenter.Y, cam.ViewCenter.Z);
            GL.Vertex3(cam.ViewCenter.X + 2, cam.ViewCenter.Y, cam.ViewCenter.Z);
            GL.Color3(Color.Blue);
            GL.Vertex3(cam.ViewCenter.X, cam.ViewCenter.Y - 2, cam.ViewCenter.Z);
            GL.Vertex3(cam.ViewCenter.X, cam.ViewCenter.Y + 2, cam.ViewCenter.Z);
            GL.Color3(Color.Red);
            GL.Vertex3(cam.ViewCenter.X, cam.ViewCenter.Y, cam.ViewCenter.Z - 2);
            GL.Vertex3(cam.ViewCenter.X, cam.ViewCenter.Y, cam.ViewCenter.Z + 2);
            GL.End();
        }

        private void RenderPointClouds()
        {
            GL.PointSize(1.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.GreenYellow);

            for (int i = 0; i < pointClouds.Count(); i++)
                GL.Vertex3(pointClouds[i].X, pointClouds[i].Y, pointClouds[i].Z);

            GL.End();
        }

        private void Render()
        {
            BuildMatrices();

            // Settings GL Render
            GL.Viewport(0, 0, this.ClientSize.Width, this.ClientSize.Height); //Size of window
            GL.Enable(EnableCap.DepthTest); //Enable correct Z Drawings
            GL.DepthFunc(DepthFunction.Less); //Enable correct Z Drawings
            GL.CullFace(CullFaceMode.Back);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render Axis.
            RenderAxis();

            // Point cloud render.
            RenderPointClouds();


            glControl.SwapBuffers();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);

            cam.OnResize(c.ClientSize.Width, c.ClientSize.Height);
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.OemMinus)
                cam.ZoomCamera(-zoomSpeed);
            else if(e.KeyCode == Keys.Oemplus)
                cam.ZoomCamera(zoomSpeed);   
        }
    }
}
