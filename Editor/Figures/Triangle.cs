using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.OpenGl;
using System.Xml.Linq;
using Tao.FreeGlut;
using Editor.Menues;

namespace Editor.Figures
{
    /// <summary>
    /// La clase tríangulo, si bien hereda de Figure, no funciona como el resto.
    /// Esto es porque es una primitiva más que una figura en sí, por lo que datos como
    /// posición, rotación y escalamiento no son facilmente interpretables. La razón
    /// hacerla figura es para integrarla facilmente al flujo del programa.
    /// 
    /// Es por esta razón que el triángulo funciona independientemente.
    /// </summary>
    class Triangle : Figure
    {
        public List<Vector> VertexList { get; private set; }
        public List<Vector> NormalList { get; private set; }

        
        

        /// <summary>
        /// Se asume que los vértices y normales están en el mismo orden.
        /// </summary>
        /// <param name="vertexList">Lista con los vértices del triángulo</param>
        /// <param name="normalList">Lista de las normales del triángulo</param>
        public Triangle(string name = "", List<Vector> vertexList = null, List<Vector> normalList = null)
            : base(name: name)
        {
            if (vertexList == null)
            {
                // Para crear triánglulos estándar
                // TODO: Mejorar esta funcionalidad
                vertexList = new List<Vector>();
                vertexList.Add(new Vector(1, 0, 0));
                vertexList.Add(new Vector(0, 1, 0));
                vertexList.Add(new Vector(0, 0, 1));
            }
            if (normalList == null)
            {
                // Para crear triánglulos estándar
                // TODO: Mejorar esta funcionalidad
                normalList = new List<Vector>();
                normalList.Add(new Vector());
                normalList.Add(new Vector());
                normalList.Add(new Vector());
            }

            this.VertexList = vertexList;
            this.NormalList = normalList;

            


            

            SetUpDelegates();
        }

        /// <summary>
        /// Este método tiene una falla conceptual gigante, y es que está creando en el modelo elementos de la vista.
        /// En si esto no es tan terrible, si es que todos los datos necesarios son provistos de la vista,
        /// lo cual no es el caso.
        /// TODO: Separar lógica modelo-vista.
        /// </summary>
        protected override void SetUpDelegates()
        {
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
            // Vertex 0
            button = new Button("Vertex 0", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Vertex 0", button);
            // Vertex 1
            button = new Button("Vertex 1", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Vertex 1", button);
            // Vertex 2
            button = new Button("Vertex 2", 60, 20, 3, xyzList);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Vertex 2", button);

            base.SetUpDelegates();

        }

        public override void Draw(bool select = false)
        {
            Gl.glPushMatrix();
            List<Vector> colors = new List<Vector>();

            if (select)
            {
                Gl.glColor3fv(MathUtils.GetVector3Array(Color));
            }
            else if (isSelected)
                Gl.glColor3fv(MathUtils.GetVector3Array(new Vector(1, 1, 1)));   // Seleccionado es color blanco
            else
            {
                colors.Add(new Vector(1, 0, 0));
                colors.Add(new Vector(0, 1, 0));
                colors.Add(new Vector(0, 0, 1));
            }
        

            Gl.glBegin(Gl.GL_TRIANGLES);
            for (int i = 0; i < 3; i++)
            {
                if(!select && !isSelected)
                    Gl.glColor3fv(MathUtils.GetVector3Array(colors[i]));
                Gl.glNormal3fv(MathUtils.GetVector3Array(NormalList[i]));
                Gl.glVertex3fv(MathUtils.GetVector3Array(VertexList[i]));
            }
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        /// <summary>
        /// Este método es una oda al hardcoding
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
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
            button.X = X-5; button.Y = Y-10;
            button.Draw(scene);
            Y += 10;    // Nos vamos moviendo para bajo;
            WriteText(titleFont, Name, X, Y);

            #region BOTONES NO USADOS

            /*
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
             */

            #endregion BOTONES NO USADOS

            // Vértice 1
            for (int i = 0; i < 3; i++)
            {
                Y += interButtonDelta;
                button = buttonDictionary["Vertex "+i];
                button.X = X; button.Y = Y;
                button.Draw(scene);
                Y += postButtonDelta;
                WriteText(normalFont, VertexList[i].ToString(), X+10, Y);
            }
            




        }

        public override void Update(int value)
        {
            //throw new NotImplementedException();
        }
		
		public override void MouseMenuInput(Scene.Scene scene, int button, int state, int x, int y)
		{
			// Iteramos sobre todos los botones, para ver si hicimos click
			// en alguno de ellos.
			foreach(KeyValuePair<string, Button> entry in buttonDictionary)
			{
				entry.Value.MouseInput(scene, button, state, x, y);	
			}
		}
		
        public override void Save(XElement parentNode)
        {
            // Creamos los elementos del triángulo a ser agregados
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

            // Este es el punto preciso donde me aburrí de escribir siempre lo mismo y
            // finalmente atiné a escribir el helper
            for (int i = 0; i < 3; i++)
            {
                XElement vertexNode = new XElement("vertex");
                vertexNode.SetAttributeValue("index", i);
                vertexNode.SetAttributeValue("material", "Yellow");

                vertexNode.Add(MathUtils.GetXYZElement("position", VertexList[i]));
                vertexNode.Add(MathUtils.GetXYZElement("normal", NormalList[i]));
                XElement textNode = new XElement("texture");
                textNode.SetAttributeValue("u", 0.0f);
                textNode.SetAttributeValue("v", 0.0f);
                vertexNode.Add(textNode);
                //TODO: UV Mapping

                // Agregamos el nodo a la lista de elementos
                elementList.Add(vertexNode);
            }

            // Finalmente agregamos todos los elementos al nodo padre del triángulo
            XElement triangleNode = new XElement("triangle");
            triangleNode.SetAttributeValue("name", Name);   // Siempre exportamos nombre, aunque no exista
            foreach (XElement element in elementList)
                triangleNode.Add(element);

            // Agregamos el triángulo a la escena
            parentNode.Add(triangleNode);
        }

        #region Delegate Methods

        protected override void CallbackMethod(string property, Object values)
        {


            // Sacamos la máscara
            // Por default una máscara que no hace nada
            Vector mask = new Vector();
            if(values is Vector)
                mask = InputMask((Vector)values);

            switch (property)
            {
                case "Position":
                    EditPosition((Vector)values, mask); break;
                case "Rotation":
                    EditRotation((Vector)values, mask); break;
                case "Scale":
                    EditScaling((Vector)values, mask); break;
                case "Vertex 0":
                    EditVertex0((Vector)values, mask); break;
                case "Vertex 1":
                    EditVertex1((Vector)values, mask); break;
                case "Vertex 2":
                    EditVertex2((Vector)values, mask); break; 
                default:
                    base.CallbackMethod(property, values); break;
            }

        }

        // METODOS QUE EDITAN LAS PROPIEDADES
        // Estoy convencido que hay una manera de generalizar esto,
        // pero no la he investigado.
        

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

        protected void EditVertex0(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? VertexList[0][i] : values[i];

            this.VertexList[0] = result;
        }

        protected void EditVertex1(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? VertexList[1][i] : values[i];

            this.VertexList[1] = result;
        }

        protected void EditVertex2(Vector values, Vector mask)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = mask[i] == 0 ? VertexList[2][i] : values[i];

            this.VertexList[2] = result;
        }

        #endregion Delegate Methods

        public override string ToString()
        {
            return "TRIANGLE";
        }
    }
}
