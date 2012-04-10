using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace Editor.Figures
{
    class Axis : Figure
    {
        public float Size { get; private set; }
        public Axis(float size = 10)
        {
            this.Size = size;
        }

        public override void Update(int value)
        {
            // Nothing to do here
        }

        public override void Draw(bool select = false)
        {
            if (!select)
            {
                Gl.glPushMatrix();


                Gl.glBegin(Gl.GL_LINES);

                // Eje X
                Gl.glColor3d(1, 0, 0);
                Gl.glVertex3d(0, 0, 0);
                Gl.glVertex3d(Size, 0, 0);

                // Eje Y
                Gl.glColor3d(0, 1, 0);
                Gl.glVertex3d(0, 0, 0);
                Gl.glVertex3d(0, Size, 0);

                // Eje Z
                Gl.glColor3d(0, 0, 1);
                Gl.glVertex3d(0, 0, 0);
                Gl.glVertex3d(0, 0, Size);

                Gl.glEnd();

                Gl.glPopMatrix();
            }
        }

        protected override void SetUpDelegates() { }
		
		public override void MouseMenuInput(Scene.Scene scene, int button, int state, int x, int y)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Los ejes no se exportan debido a que no es una figura soporta por la especificación 
        /// original de SceneLib.
        /// </summary>
        /// <param name="parentNode"></param>
        public override void Save(System.Xml.Linq.XElement parentNode)
        {
            //throw new NotImplementedException();
        }

        public override void Print(Scene.Scene scene, int x, int y)
        {
            //throw new NotImplementedException();
        }

        protected override void CallbackMethod(string property, object values)
        {
            throw new NotImplementedException();
        }
    }
}
