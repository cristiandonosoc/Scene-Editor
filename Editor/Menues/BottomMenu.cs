using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.OpenGl;
using Editor.Scene;
using Tao.FreeGlut;

namespace Editor.Menues
{
    class BottomMenu:BaseMenu
    {


        public BottomMenu(int width, int height, int offsetX, int offsetY)
            : base(width, height, offsetX, offsetY, 10)
        {
            outerColor = new Vector(0.4, 0.4, 0.4);
            innerColor = new Vector(0.7, 0.7, 0.7);
        }

        public override void Draw(Scene.Scene scene)
        {
            DrawMenuBox();

            DrawMenuInfo(scene);
        }

        /// <summary>
        /// Este es el método importante de esta clase, ya que este menú es netamente informativo de 
        /// que está haciendo el usuario en este momento.
        /// </summary>
        /// <param name="scene"></param>
        protected void DrawMenuInfo(Scene.Scene scene)
        {
            // Mostramos que hubo un error
            IntPtr font = Glut.GLUT_BITMAP_HELVETICA_12;
            Gl.glColor3d(0, 0, 0);
            Gl.glRasterPos2d(OffsetX + 20, OffsetY + HEIGHT / 2 + 4);
            string text = "";
            if (scene.sceneState == SceneState.Edit)
                text = "Editando " + scene.SelectedButton.CurrentProperty + ": " + scene.InputString;
            else if (scene.sceneState == SceneState.Error)
                text = "NO SE PUDO PARSEAR EL INPUT. APRETE CUALQUIER TECLA PARA CONTINUAR.";
            else if (scene.sceneState == SceneState.Create)
            {
                text = "CREATE ";
                if (scene.FigureToCreate == Figures.FigureEnum.Triangle)
                    text += "TRIANGLE";
                else if (scene.FigureToCreate == Figures.FigureEnum.Sphere)
                    text += "SPHERE";

            }
            else if (scene.sceneState == SceneState.Save)
                text = "ESCENA FUE EXPORTADA CON ÉXITO";
            
            Glut.glutBitmapString(font, text);

        }

        protected void DrawMenuBox()
        {
            // Vamos a estar haciendo un menú, por lo cual usamos un modo 2D
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(0, WIDTH, HEIGHT, 0, 0, 1);  // Proyección Ortogonal para no preocuparnos de la profundidad

            // Cambiamos el viewport al pedazo que nos corresponde
            Gl.glViewport(OffsetX, OffsetY, WIDTH, HEIGHT);

            // Dibujamos el layout del menu
            Gl.glBegin(Gl.GL_QUADS);

            // Dibujamos el margen
            Gl.glColor3fv(MathUtils.GetVector3Array(outerColor));
            Gl.glVertex2d(0, 0);
            Gl.glVertex2d(WIDTH, 0);
            Gl.glVertex2d(WIDTH, HEIGHT);
            Gl.glVertex2d(0, HEIGHT);

            // El rectángulo que tiene la información
            Gl.glColor3fv(MathUtils.GetVector3Array(innerColor));
            Gl.glVertex2d(Margin, Margin);
            Gl.glVertex2d(WIDTH - Margin, Margin);
            Gl.glVertex2d(WIDTH - Margin, HEIGHT - Margin);
            Gl.glVertex2d(Margin, HEIGHT - Margin);

            Gl.glEnd();
        }

        public override void KeyboardInput(Scene.Scene scene, byte key, int x, int y)
        {
            //throw new NotImplementedException();
        }

        public override void MouseInput(Scene.Scene scene, int button, int state, int x, int y)
        {
            //throw new NotImplementedException();
        }
    }
}
