using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.OpenGl;
using Tao.FreeGlut;
using System.Xml.Linq;
using Editor.Menues;

namespace Editor.Figures
{
    class Sphere : Figure
    {
        public float Radius { get; set; }
        public bool Wireframe { get; set; }

        public Sphere(string name = "", Vector center = null, float Radius = 1)
            : base(translation: center)
        {
            this.Radius = Radius;
            this.Wireframe = false;

            SetUpDelegates();
        }

        protected override void SetUpDelegates()
        {
            Button button;

            // Creamos las listas de nombres de propiedades
            // (Podría perfectamente haber heredado los botones, pero no lo hice... LAZYNESS!)
            List<string> xyzList = new List<string>();
            xyzList.Add("X"); xyzList.Add("Y"); xyzList.Add("Z");

            // Posición
            button = new Button("Center", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Center", button);

            // Radio
            List<string> list = new List<string>();
            list.Add("Radius");
            button = new Button("Radius", 60, 20, 1, list);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Radius", button);

            base.SetUpDelegates();

        }

        public override void Draw(bool select = false)
        {
            Gl.glPushMatrix();
            if (select)
            {
                Gl.glColor3fv(MathUtils.GetVector3Array(Color));
            }
            else if (isSelected)
            {
                Gl.glColor3fv(MathUtils.GetVector3Array(new Vector(1, 1, 1)));   // Seleccionado es color blanco
            }
            else
                Gl.glColor3fv(MathUtils.GetVector3Array(new Vector(1, 0, 1)));
            Gl.glTranslatef(Traslation.x, Traslation.y, Traslation.z);
            if (Wireframe)
                Glut.glutWireSphere(Radius, 16, 16);
            else
                Glut.glutSolidSphere(Radius, 16, 16);

            Gl.glPopMatrix();
        }

        public override void Update(int value)
        {
            //throw new NotImplementedException();
        }
		
		public override void MouseMenuInput(Scene.Scene scene, int button, int state, int x, int y)
		{
            // Iteramos sobre todos los botones, para ver si hicimos click
            // en alguno de ellos.
            foreach (KeyValuePair<string, Button> entry in buttonDictionary)
            {
                entry.Value.MouseInput(scene, button, state, x, y);
            }
		}

        public override void Save(XElement parentNode)
        {
            // Nodo de la esfera
            XElement xmlSphere = new XElement("sphere");
            xmlSphere.SetAttributeValue("radius", Radius);
            xmlSphere.SetAttributeValue("name", Name);
            xmlSphere.SetAttributeValue("material", "Yellow");
            //TODO: Materials

            // Nuestra implementación no usa position, sino center
            xmlSphere.Add(MathUtils.GetXYZElement("position", new Vector()));
            // Rotación de esferas no hace sentido, a menos que está tenga nodos hijos
            // Lo cual aún no está implementado en este programa...
            xmlSphere.Add(MathUtils.GetXYZElement("rotation", new Vector())); 
            xmlSphere.Add(MathUtils.GetXYZElement("scale", Scaling));
            xmlSphere.Add(MathUtils.GetXYZElement("center", Traslation));

            // Agregamos la esfera a la lista de objetos
            parentNode.Add(xmlSphere);

        }

        public override void Print(Scene.Scene scene, int x, int y)
        {
            // Fonts a usar
            IntPtr normalFont = Glut.GLUT_BITMAP_HELVETICA_10;
            IntPtr titleFont = Glut.GLUT_BITMAP_HELVETICA_18;
            IntPtr middleFont = Glut.GLUT_BITMAP_HELVETICA_12;
            Button button;
            int postButtonDelta = 35;
            int interButtonDelta = 20;
            Gl.glColor3d(0, 0, 0);


            // Índices que se usará para escribir en el lugar correcto
            int X = x; int Y = y;
            Gl.glColor3d(0, 0, 0);
            // Título
            button = buttonDictionary["Name"];
            button.X = X - 5; button.Y = Y - 10;
            button.Draw(scene);
            Y += 10;    // Nos vamos moviendo para bajo;
            WriteText(titleFont, Name, X, Y);

            // Posición
            Y += interButtonDelta;
            //WriteText(middleFont, "Posición", X, Y);
            button = buttonDictionary["Center"];
            button.X = X; button.Y = Y;
            button.Draw(scene);
            Y += postButtonDelta;
            WriteText(normalFont, Traslation.ToString(), X + 10, Y);

            // Rotación
            Y += interButtonDelta;
            button = buttonDictionary["Radius"];
            button.X = X; button.Y = Y;
            button.Draw(scene);
            Y += postButtonDelta;
            WriteText(normalFont, Radius.ToString(), X + 10, Y);

        }

        #region DELEGATE METHODS

        protected override void CallbackMethod(string property, Object values)
        {
            // Sacamos la máscara
            Vector mask = new Vector();
            if (values is Vector)
                mask = InputMask((Vector)values);

            switch (property)
            {
                case "Center":
                    EditCenter((Vector)values, mask); break;
                case "Radius":
                    EditRadius((Vector)values, mask); break;
                default:
                    base.CallbackMethod(property, values); break;
            }
        }

        protected void EditCenter(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? Traslation[i] : values[i];

            this.Traslation = result;
        }

        protected void EditRadius(Vector values, Vector mask)
        {
            Radius = mask[0] == 0 ? Radius : values[0];
        }

        #endregion DELEGATE METHODS

        public override string ToString()
        {
            return "SPHERE";
        }
    }
}
