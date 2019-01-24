using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace L3DS.Engine.Views
{
    public class Camera
    {
        // Globals:
        public float MinimumZoom, MaximumZoom, Zoom;
        public float Theta;
        public float Phi;
        public float Angle;
        public float Width;
        public float Height;
        private bool changed;
        private Vector3 camPos;
        private Vector3 viewCenter;
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;
        private Vector3 directionPos;

        public Camera()
        {
            viewCenter = new Vector3(0, 0, 0);
            MinimumZoom = 10.0f;
            MaximumZoom = 1000.0f;
            Zoom = 100.0f;
            Theta = Phi = 0;
            Angle = 15.0f * (float)Math.PI / 180f;
            Width = 640;
            Height = 480;
            changed = true;
            camPos = new Vector3();
            UpdateCamera();
        }
        
        public void OnResize(int _Width, int _Height)
        {
            Width = _Width;
            Height = _Height;
            changed = true;
        }

        public void UpdateCamera()
        {
            if(changed)
            {
                // view matrix.
                camPos.X = viewCenter.X + (float)(Zoom * Math.Cos(Theta) * Math.Sin(Phi));
                camPos.Y = viewCenter.Y + (float)(Zoom * Math.Sin(Theta) * Math.Sin(Phi));
                camPos.Z = viewCenter.Z + (float)(Zoom * Math.Cos(Phi));

                viewMatrix = Matrix4.LookAt(CameraPosition[0], CameraPosition[1], CameraPosition[2],
                                                viewCenter[0], viewCenter[1], viewCenter[2],
                                                0, 0, 1.0f);

                // direction view.
                directionPos.X = (float)(-Math.Cos(Theta) * Math.Sin(Phi));
                directionPos.Y = (float)(-Math.Sin(Theta) * Math.Sin(Phi));
                directionPos.Z = (float)(-Math.Cos(Phi));

                // projection matrix.
                Vector3 CameraPos = CameraPosition;
                Vector3 dir = new Vector3();
                Vector3.Subtract(ref viewCenter, ref CameraPos, out dir);
                dir.Normalize();
                float dist;
                Vector3.Dot(ref dir, ref CameraPos, out dist);
                dist = -dist;

                float ratio = (float)Width / (float)Height;
                float nearDist = Math.Max(0.1f, dist - 100);
                float farDist = Math.Max(100 * 2, dist + 100000);
                float nearHeight = 2.0f * (float)Math.Tan(Angle) * dist;

                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)(Angle * 2.0), (float)ratio, nearDist, farDist);

                changed = false;
            }
        }

        // get view matrix
        public Vector3 ViewCenter
        {
            get
            {
                return viewCenter;
            }

            set
            {
                changed = true;
                viewCenter = value;
            }
        }

        // generate eye position.
        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public Vector3 CameraPosition
        {
            get
            {
                return camPos;
            }
        }

        // generate look at matrix
        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public Matrix4 ViewMatrix
        {
            get
            {
                return viewMatrix;
            }
        }

        // generate projection matrix.
        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public Matrix4 ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }
        }

        // generate view direction.
        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public Vector3 ViewDirection()
        {
            return directionPos;
        }


        // zoom camera control.
        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public void ZoomCamera(float dis)
        {
            Zoom += dis;
            if (Zoom < MinimumZoom)
                Zoom = MinimumZoom;
            if (Zoom > 6 * MaximumZoom)
                Zoom = 6 * MaximumZoom;
            if (Zoom > 1)
            {
                Angle = 15.0f * (float)Math.PI / 180f;
            }
            else
            {
                Angle = (float)Math.Atan(Zoom * Math.Tan(15.0 * Math.PI / 180.0));
            }

            changed = true;
        }

        // rotate camera control
        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public void Rotate(float side, float updown)
        {
            Theta += side;
            Phi += updown;
            while (Theta > Math.PI)
                Theta -= 2 * (float)Math.PI;
            while (Theta < -(float)Math.PI)
                Theta += 2 * (float)Math.PI;
            while (Phi > Math.PI)
                Phi = (float)Math.PI - 1e-5f;
            while (Phi < 0)
                Phi = 1e-5f;

            changed = true;
        }

        // Based: https://github.com/repetier/Repetier-Host/blob/master/src/RepetierHost/view/ThreedCamera.cs
        public void RotateDegrees(float rotX, float rotZ)
        {
            Theta += rotX * (float)Math.PI / 180.0f;
            Phi += rotZ * (float)Math.PI / 180.0f;
            while (Theta > (float)Math.PI)
                Theta -= 2 * (float)Math.PI;
            while (Theta < -(float)Math.PI)
                Theta += 2 * (float)Math.PI;
            while (Phi > (float)Math.PI)
                Phi = (float)Math.PI - 1e-5f;
            if (Phi < 0)
                Phi = 1e-5f;

            changed = true;

        }
        public void Pan(float leftRight, float upDown, float dist)
        {
            if (dist < 0) dist = Zoom;
            leftRight *= Math.Max(1, dist) * (float)Math.Tan(Angle) * 2.0f;
            upDown *= -Math.Max(1, dist) * (float)Math.Tan(Angle) * 2.0f;
            Vector3 ud = new Vector3(0, 0, 1);
            Vector3 camCenter = new Vector3();
            Vector3 cp = CameraPosition;
            Vector3.Subtract(ref viewCenter, ref cp, out camCenter);
            Vector3 lr = new Vector3();
            Vector3.Cross(ref camCenter, ref ud, out lr);
            Vector3.Cross(ref lr, ref camCenter, out ud);
            lr.Normalize();
            ud.Normalize();
            viewCenter.X += (float)(leftRight * lr.X + upDown * ud.X);
            viewCenter.Y += (float)(leftRight * lr.Y + upDown * ud.Y);
            viewCenter.Z += (float)(leftRight * lr.Z + upDown * ud.Z);

            changed = true;
        }


    }
}
