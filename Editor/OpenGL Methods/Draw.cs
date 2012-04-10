using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.Figures;
using Editor.Lighting;
using Editor.Scene;
using Tao.OpenGl;
using Tao.FreeGlut;
using Utilities;
using Editor.Menues;

namespace Editor
{
    /// <summary>
    /// Clase donde va toda la lógica que se refiere al dibujo de la escena. 
    /// El funcionamiento general está basado en polimorfismo, donde se le dice a cada objeto de la escena
    /// que se dibuje, sin tener que tener un seguimiento de qué tipo de figura es, ya que se asume
    /// que esta sabe como hacerlo.
    /// 
    /// Esto quiere decir que si se quiere agregar un nuevo tipo de figura, esta debe heredar de la clase
    /// Figure eventualmente, para así poder agregarla al arreglo objects.
    /// </summary>
    class DrawClass
    {
        static public void Run(Scene.Scene scene, bool select, List<BaseMenu> menuList)
        {
            /**
             * Borramos la escena anterior. Hacemos Bitwise OR entre las distintas máscaras que indican que 
             * buffers queremos borrar. Los distintos tipos de buffers son:
             * 
             * GL_COLOR_BUFFER_BIT:     Los buffers que contienen la información de color.
             * GL_DEPTH_BUFFER_BIT:     El buffer de profundidad.
             * GL_ACCUM_BUFFER_BIT:     EL buffer de acumulación.
             * GL_STENCIL_BUFFER_BIT:   El buffer de stencil.
             **/
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            /**
             * La ventana está dividida en dos viewports. Uno designado para la proyección de la ventana y 
             * otro para el menú con la información
             * 
             **/
            // Primero dibujamos la escena
            // Sabemos que el primer menú de la lista de manues es el de la franja izquierda, el cual
            // lo usamos para ubicar el viewport de render de la escena.
            DrawScene(scene, select, leftMenu: menuList[0]);

            // Dibujamos los menues
            DrawMenu(scene, select, menuList);

            /**
             * Método que manda todo lo que está en el backbuffer a la ventana (buffer secundario donde se renderia la escena
             * sin afectar lo mostrado en la ventana, con el fin de simplemente cambiarlo cuando esté listo).
             * El backbuffer queda indefinido después de esto. Esta llamada no se corre de inmediato, sino que generalmente
             * espera al al retrace vertical del monitor. Las siguientes llamadas quedan encoladas pero no se ejecutan hasta que
             * SwapBuffers haya terminado.
             * Si no hay double buffer, esta llamada no hace nada y simplemente llama a glFlush, que es la llamada que 
             * efectivamente dibuja, solamente que esta vez al buffer principal. Esto es porque glutSwapBuffers 
             * tiene una llamada de glFlush implicita (hay que dibujar en el BackBuffer).
             **/
            if (!select)
                Glut.glutSwapBuffers();
            
        }

        

        #region DIBUJO ESCENA

        /// <summary>
        /// Método que contiene la lógica para el dibujo de la escena a editar.
        /// </summary>
        /// <param name="scene">Clase que contiene la información de la escena</param>
        /// <param name="select">Si se debe dibujar en modo select (ver OpenGL Methods/HandleInput.cs)</param>
        private static void DrawScene(Scene.Scene scene, bool select, BaseMenu leftMenu)
        {
            // La escena no ocupa toda la ventana, por lo que hay que modificar el viewport
            //Gl.glViewport(200, 0, scene.WIDTH, scene.HEIGHT);

            // Obtenemos el viewport que nos corresponde
            //Gl.glViewport(leftMenu.WIDTH, 0, scene.WIDTH, scene.HEIGHT);
            Program.Resize(scene.WIDTH, scene.HEIGHT);

            Camera camera = scene.SceneCamera;
            List<Figure> objects = scene.SceneFigures;
            List<Light> lights = scene.SceneLights;

            Gl.glMatrixMode(Gl.GL_MODELVIEW);           // Cambiamos a la información de dibujo
            Gl.glLoadIdentity();                        // Reiniciamos la matriz

            Glu.gluLookAt(camera.Position.x, camera.Position.y, camera.Position.z,
                          camera.Target.x, camera.Target.y, camera.Target.z,
                          camera.Up.x, camera.Up.y, camera.Up.z);

            // Dibujamos las figuras en la escena
            foreach (Figure figure in objects)
                figure.Draw(select);

            // Seteamos la iluminación de la escena
            SetLighting(lights);

            
        }

        /// <summary>
        /// Método que establece la iluminación de la escena.
        /// Es llamada en cada pasada de Draw.
        /// </summary>
        private static void SetLighting(List<Light> sceneLights)
        {
            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, new float[] { 0.7f, 0.7f, 0.7f, 1f });

            int lightCount = 0;
            foreach (Light light in sceneLights)
                light.RegisterLight(Gl.GL_LIGHT0 + lightCount++);

        }

        #endregion DIBUJO ESCENA

        #region DIBUJO MENU

        private static void DrawMenu(Scene.Scene scene, bool select, List<BaseMenu> menuList)
        {
            

            

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // Deshabilitamos la profundidad
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glDisable(Gl.GL_LIGHTING);

            // Dibujamos los menues
            foreach (BaseMenu menu in menuList)
                menu.Draw(scene);

            // Volvemos a dejar el estado del programa como lo tomamos
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);

        }

        #endregion DIBUJO MENU
    }
}
