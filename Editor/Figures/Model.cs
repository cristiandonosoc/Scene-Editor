using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.OpenGl;
using System.Xml.Linq;
using Editor.Menues;
using Tao.FreeGlut;

namespace Editor.Figures
{
    class Model : Figure
    {
        private Vector _color;
        public override Vector Color
        {
            get { return _color; }
            set { _color = value; foreach (Figure f in triangleList) { f.Color = value; } }

        }
        private bool _isSelected;
        public override bool isSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; foreach (Figure f in triangleList) { f.isSelected = _isSelected; } }

        }

        public List<Triangle> triangleList;
        public string Path { get; private set; }

        public Model(string name = "unnamed", string path = "unnamed", Vector traslation = null, Vector rotation = null, Vector scaling = null)
            : base(name)
        {
            this.Path = path;
            triangleList = new List<Triangle>();
            SetUpDelegates();
        }

        public void addTriangle(Triangle triangle)
        {
            triangleList.Add(triangle);
        }

        public override void Draw(bool select = false)
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(Traslation.x, Traslation.y, Traslation.z);
            //Gl.glTranslatef(Traslation.x, Traslation.y, Traslation.z);
            Gl.glRotatef(Rotation.x, 1f, 0f, 0f);
            Gl.glRotatef(Rotation.y, 0f, 1f, 0f);
            Gl.glRotatef(Rotation.z, 0f, 0f, 1f);
            Gl.glScalef(Scaling.x, Scaling.y, Scaling.z);

            foreach (Triangle triangle in triangleList)
                triangle.Draw(select);

            Gl.glPopMatrix();
        }

        public override void Save(System.Xml.Linq.XElement parentNode)
        {
            List<XElement> elementList = new List<XElement>();

            // Escalamiento
            XElement scale = new XElement("scale");
            scale.SetAttributeValue("x", Scaling.x);
            scale.SetAttributeValue("y", Scaling.y);
            scale.SetAttributeValue("z", Scaling.z);
            elementList.Add(scale);

            // Rotación
            XElement rotation = new XElement("rotation");
            rotation.SetAttributeValue("x", Rotation.x);
            rotation.SetAttributeValue("y", Rotation.y);
            rotation.SetAttributeValue("z", Rotation.z);
            elementList.Add(rotation);

            // Posición
            XElement position = new XElement("position");
            position.SetAttributeValue("x", Traslation.x);
            position.SetAttributeValue("y", Traslation.y);
            position.SetAttributeValue("z", Traslation.z);
            elementList.Add(position);

            XElement modelNode = new XElement("model");
            modelNode.SetAttributeValue("name", Name);
            modelNode.SetAttributeValue("path", Path);

            foreach (XElement element in elementList)
                modelNode.Add(element);

            parentNode.Add(modelNode);

            //Creamos los archivos para cada modelo
            XDocument xmlDoc = new XDocument();
            xmlDoc.Declaration = new XDeclaration("1.0", "UTF-8", "yes");

            XElement xmlModel = new XElement("model");
            SaveModel(xmlModel);

            // Agregamos la escena al documento
            xmlDoc.Add(xmlModel);
            xmlDoc.Save(Path);
        }

        public void SaveModel(XElement parentNode)
        {
            XElement triangleNode = new XElement("triangle_list");
            foreach (Triangle triangle in triangleList)
                triangle.Save(triangleNode);

            XElement modelNode = new XElement("model");
            parentNode.Add(triangleNode);
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

            // Índices que se usará para escribir en el lugar correcto
            int X = x; int Y = y;
            Gl.glColor3d(0, 0, 0);
            // Título
            button = buttonDictionary["Name"];
            button.X = X - 5; button.Y = Y - 10;
            button.Draw(scene);
            Y += 10;    // Nos vamos moviendo para bajo;
            WriteText(titleFont, Name, X, Y);

            Y += postButtonDelta;
            WriteText(normalFont, "Seleccionnar propiedad y modificar con a-d, s-w, i-k o", X + 5, Y);
            Y += postButtonDelta / 2;
            WriteText(normalFont, "ingresando valores nuevos directamente en consola.", X + 5, Y);
            Y += postButtonDelta / 2;
            WriteText(normalFont, "Presionar ESC para terminar edición actual.", X + 5, Y);
            Y += postButtonDelta / 2;
            WriteText(normalFont, "Presionar TAB para seleccionar siguiente coordenada.", X + 5, Y);


            
            // Posición
            Y += interButtonDelta;
            //WriteText(middleFont, "Posición", X, Y);
            button = buttonDictionary["Position"];
            button.X = X; button.Y = Y;
            button.Draw(scene);
            Y += postButtonDelta;
            WriteText(normalFont, Traslation.ToString(), X + 10, Y);

            // Rotación
            Y += interButtonDelta;
            button = buttonDictionary["Rotation"];
            button.X = X; button.Y = Y;
            button.Draw(scene);
            Y += postButtonDelta;
            WriteText(normalFont, Rotation.ToString(), X+10, Y);

            // Escalamiento
            Y += interButtonDelta;
            button = buttonDictionary["Scale"];
            button.X = X; button.Y = Y;
            button.Draw(scene);
            Y += postButtonDelta;
            WriteText(normalFont, Scaling.ToString(), X+10, Y);

            


        }

        protected override void SetUpDelegates()
        {

            base.SetUpDelegates();
            Button button;

            // Creamos las listas de nombres de propiedades
            // (Podría perfectamente haber heredado los botones, pero no lo hice... LAZYNESS!)
            List<string> xyzList = new List<string>();
            xyzList.Add("X"); xyzList.Add("Y"); xyzList.Add("Z");

            // Posición
            button = new Button("Position", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Position", button);
            // Rotación
            button = new Button("Rotation", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Rotation", button);
            // Escala
            button = new Button("Scale", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Scale", button);
        }

        protected override void CallbackMethod(string property, Object values)
        {


            // Sacamos la máscara
            // Por default una máscara que no hace nada
            Vector mask = new Vector();
            if (values is Vector)
                mask = InputMask((Vector)values);

            switch (property)
            {
                case "Position":
                    EditPosition((Vector)values, mask); break;
                case "Rotation":
                    EditRotation((Vector)values, mask); break;
                case "Scale":
                    EditScaling((Vector)values, mask); break;
                default:
                    base.CallbackMethod(property, values); break;
            }

        }

        public void EditPropertyByOffset(string property, Vector offset)
        {
            Vector mask = new Vector(1,1,1);
            
            switch (property)
            {
                case "Position":
                    EditPosition(Traslation + offset, mask); break;
                case "Rotation":
                    EditRotation(Rotation + 10*offset, mask); break;
                case "Scale":
                    EditScaling(Scaling + offset, mask); break;
            }

        }

        protected void EditPosition(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? Traslation[i] : values[i];

            this.Traslation = result;
        }

        protected void EditRotation(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? Rotation[i] : values[i];

            this.Rotation = result;
        }

        protected void EditScaling(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? Scaling[i] : values[i];

            this.Scaling = result;
        }


        public override void Update(int value)
        {

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
    }
}
