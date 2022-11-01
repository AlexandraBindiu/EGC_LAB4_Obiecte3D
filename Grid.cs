using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obiecte3D
{
    class Grid
    {
        private readonly Color colorisation;
        private bool visibility;

        private readonly Color GRIDCOLOR = Color.WhiteSmoke;
        private const int GRIDSTEP = 10;
        private const int UNITS = 10;
        private const int POINT_OFFSET = GRIDSTEP * UNITS;

        private const int MICRO_OFFSET = 1;
        public Grid()
        {
            colorisation = GRIDCOLOR;
            visibility = true;
        }

        public void Show()
        {
            visibility = true;
        }

        public void Hide()
        {
            visibility = false;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void Draw()
        {
            if (visibility)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(colorisation);
                for (int i = -1 * GRIDSTEP * UNITS; i <= GRIDSTEP * UNITS; i += GRIDSTEP)
                {
                    // XZ plan drawing: parallel with Oz
                    GL.Vertex3(i + MICRO_OFFSET, 0, POINT_OFFSET);
                    GL.Vertex3(i + MICRO_OFFSET, 0, -1 * POINT_OFFSET);

                    // XZ plan drawing: parallel with Ox
                    GL.Vertex3(POINT_OFFSET, 0, i + MICRO_OFFSET);
                    GL.Vertex3(-1 * POINT_OFFSET, 0, i + MICRO_OFFSET);
                }
                GL.End();
            }
        }



    }
}
