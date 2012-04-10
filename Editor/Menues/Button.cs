using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.OpenGl;
using Editor.Figures;
using Editor.Scene;
using Tao.FreeGlut;

namespace Editor.Menues
{
    /// <summary>
    /// Clase que representa la funcionalidad de un botón para editar una propiedad de un objeto
    /// </summary>
    public class Button : BaseMenu
    {
        // Retorna un vector nuevo que es considerado como no cambiar ningún valor
        protected Vector UndefinedInputVector()
        {
            float u = Scene.Scene.UNDEFINED;
            return new Vector(u, u, u, u);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; private set; }
        public int inputCount { get; private set; }
        public int inputIndex { get; set; }
        public void RestartInput() { inputIndex = 0; }
        public Vector currentInput { get; private set; }
        public bool COMPLETED { get; set; }

        private bool numberButton;

        // El nombre que se envía para afuera sobre que propiedad se está editando
        private List<string> propertyList;
        public string CurrentProperty{
            get{return propertyList[inputIndex];}
        }

        public EditPropertyDelegate callback { get; set; }

        public Button(string text, int width, int height, int inputCount, List<string> propertyList, bool numberButton = true)
            // El botón ignora las propiedades offsetX y offsetY ya que se las dan en el Draw
            // Es un desorden más o menos ya que hice el error de poner presentación en el modelo
            // (MVC fue creado para evitar precisamente eso. Es uno de los TODO)
            : base(width, height, 0, 0, 2)
        {
            this.numberButton = numberButton;
            this.Name = text;
            this.inputCount = inputCount;
            this.inputIndex = 0;
            this.currentInput = new Vector();
            this.propertyList = propertyList;

            outerColor = new Vector(0.4, 0.4, 0.4);
            innerColor = new Vector(0.4, 0.7, 0.7);

            // Indica si termino de parsear el input
            COMPLETED = false;
        }

        /// <summary>
        /// Dibuja el botón.
        /// </summary>
        /// <param name="text">Texto a desplegar dentro del botón</param>
        public override void Draw(Scene.Scene scene)
        {
            IntPtr middleFont = Glut.GLUT_BITMAP_HELVETICA_12;

            Gl.glBegin(Gl.GL_QUADS);

            // Dibujamos el margen
            Gl.glColor3fv(MathUtils.GetVector3Array(outerColor));
            Gl.glVertex2d(X, Y);
            Gl.glVertex2d(X + WIDTH, Y);
            Gl.glVertex2d(X + WIDTH, Y + HEIGHT);
            Gl.glVertex2d(X, Y + HEIGHT);

            // El rectángulo que tiene la información
            Gl.glColor3fv(MathUtils.GetVector3Array(innerColor));
            Gl.glVertex2d(X + Margin, Y + Margin);
            Gl.glVertex2d(X + WIDTH - Margin, Y + Margin);
            Gl.glVertex2d(X + WIDTH - Margin, Y + HEIGHT - Margin);
            Gl.glVertex2d(X + Margin, Y + HEIGHT - Margin);

            Gl.glEnd();
            Gl.glColor3d(0, 0, 0);
            Figure.WriteText(middleFont, Name, X+5, Y+HEIGHT/2+5);
        }
		
		public override void KeyboardInput(Scene.Scene scene, byte key, int x, int y)
		{
			throw new NotImplementedException();
		}
		
		public override void MouseInput(Scene.Scene scene, int button, int state, int x, int y)
		{
			// Vemos si el click fue dentro del botón
			if(x > X && x < (X + WIDTH))
			{
				if(y > Y && y < (Y+HEIGHT))
				{
					// Si no estamos editando una propiedad, marcamos que la estamos editando
					if(scene.sceneState == SceneState.Free)
					{
						scene.sceneState = SceneState.Edit;
						// Decimos que este es el botón siendo seleccionado
						scene.SelectedButton = this;
						// Borramos el input
						scene.InputString = "";
                        // Reiniciamos el input
                        currentInput = UndefinedInputVector();
					}
				}
			}
		}
		
		/// <summary>
		/// Intenta parsear el input que le pasaron
		/// </summary>
        public bool ParseInput(string inputString)
        {
            if (numberButton)
            {
                // Le quitamos los posibles espacios
                string input = inputString.Trim(' ');
                input = input.Replace(',', '.');

                // Intentamos parsear el input
                float data = 0;
                if (float.TryParse(input, out data))
                {
                    // El parseo fue exitoso
                    // Aprovechamos el agradable acceso que hizo Sergio Alvarez
                    // (Solo por si no lo saben, el ++ es post-incremento. Esto quiere decir que suma a la
                    // variable después de entregar el valor, por lo que funciona en nuestro caso).
                    currentInput[inputIndex++] = data;

                    // Queremos que a cada paso vaya editando la propiedad
                    callback(Name, currentInput);

                    // Vemos que ocurrió con el índice
                    if (inputIndex == inputCount)
                    {
                        // Reiniciamos el índice e indicamos que se completó el input
                        inputIndex = 0;
                        COMPLETED = true;
                    }

                    // Decimos que este input fue parseado exitosamente
                    return true;
                }

                // El input no fue parseado con éxito
                return false;
            }
            else
            {
                callback(Name, inputString);
                COMPLETED = true;
                return true;
            }
        }
        
    }
}
