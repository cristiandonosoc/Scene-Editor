using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.OpenGl;
using Tao.FreeGlut;

namespace Editor.Figures
{
    class Teapot : Figure
    {
        public float Size { get; private set; }
        public Teapot(Vector position, float size)
            : base(translation: position)
        {
            this.Size = size;
        }

        public override void Update(int value)
        {
			/*
            // Rotamos el cubo en el eje z
            rotationVector += new Vector(-0.3f, 1f, 2f);
            float x = 0f;
            if (rotationVector.x < 0) x = 360f;
            else if (rotationVector.x > 360) x = -360f;
            float y = 0f;
            if (rotationVector.y < 0) y = 360f;
            else if (rotationVector.y > 360) y = -360f;
            float z = 0f;
            if (rotationVector.z < 0) z = 360f;
            else if (rotationVector.z > 360) z = -360f;

            rotationVector += new Vector(x, y, z);
            */

        }

        public override void Draw(bool select = false)
        {
            Gl.glPushMatrix();
          

            Gl.glTranslatef(Traslation.x, Traslation.y, Traslation.z);
            Gl.glRotatef(Rotation.x, 1f, 0f, 0f);
            Gl.glRotatef(Rotation.y, 0f, 1f, 0f);
            Gl.glRotatef(Rotation.z, 0f, 0f, 1f);

            Glut.glutSolidTeapot(Size);

            Gl.glPopMatrix();
        }
		
		public override void MouseMenuInput(Scene.Scene scene, int button, int state, int x, int y)
		{
			throw new NotImplementedException();
		}

		
        public override void Save(System.Xml.Linq.XElement parentNode)
        {
            //throw new NotImplementedException();
        }

        public override void Print(Scene.Scene scene, int x, int y)
        {
            throw new NotImplementedException();
        }

        protected override void SetUpDelegates()
        {
            throw new NotImplementedException();
        }

        protected override void CallbackMethod(string property, object values)
        {
            throw new NotImplementedException();
        }


    }


}
