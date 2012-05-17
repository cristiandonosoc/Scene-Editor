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

namespace Editor.OpenGL_Methods
{
    /// <summary>
    /// Clase donde está la lógica de manejo del input del mouse.
    /// </summary>
    class HandleMouseInputClass
    {
        /// <summary>
        /// Punto de entrada de un click del mouse
        /// </summary>
        /// <param name="button">Que botón fue usado</param>
        /// <param name="state">Cual es el estado de este botón (que evento fue gatillado)</param>
        /// <param name="x">Coordenada X del mouse</param>
        /// <param name="y">Coordenada Y del mouse</param>
        /// <param name="scene">La escena</param>
        /// <param name="menuList">Los menus</param>
        static public void MouseInput(int button, int state, int x, int y, Scene.Scene scene, List<BaseMenu> menuList)
        {
            // Vemos que tipo de evento obtenemos
            // Si apretamos el botón izquierdo
            if (button == Glut.GLUT_LEFT_BUTTON)
            {
                // Si lo apretamos (no release)
                if (state == Glut.GLUT_DOWN)
                {
                    // Tenemos que ver donde cayó el click
                    if (x > menuList[0].WIDTH)
                    {
                        // El click fue en la escena	
                        ClickScene(scene, menuList, x, y);
                    }
                    else 	// El click fue en el menú
                    {
                        foreach (BaseMenu menu in menuList)
                            menu.MouseInput(scene, button, state, x, y);
                    }



                }
            }
        }

        protected static void ClickScene(Scene.Scene scene, List<BaseMenu> menuList, int x, int y)
        {
            /**
                * Vamos a ver que objeto se seleccionó. Hay varias formas de hacer esto. Un approach más matemático
                * (e inclusive más intuitivo, pero no por eso el más simple) es proyectar el mouse como un rayo y ver  
                * que objeto choca con ese rayo (al más puro estilo de un ray tracer).
                * 
                * No obstante, una solución mucho más fácil (kudos Joaquín Jaramillo por mencionarlo) es renderiar 
                * cada objeto con un color único (provisto por la escena) el cual cual funciona como ID. 
                * Luego simplemente necesitamos ver el color del pixel para saber que objeto ha sido seleccionado. 
                * Hay que tener cuidado de no asignar como ID el color del fondo, 
                * con tal de poder detectar que no se ha seleccionado ningún objeto.
                **/

            // Deshabilitamos cualquier efecto que haría que un objeto no tuviese un color uniforme
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_TEXTURE_2D);

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            // Dibujamos la escena con el modo especial
            DrawClass.Run(scene, true, menuList);

            // Pixel para obtener el color
            byte[] pixel = new byte[3];

            // Leemos el color del pixel en cuestión
            // La coordenada Y es (WIDTH - yMouse) debido a una disparidad de cual es el (0,0) de la ventana
            // entre OpenGL (esquina inferior izquierda) y Windows (esquina superior izquierda).
            // TODO: Generalizar offsets
            Gl.glReadPixels(x, scene.HEIGHT + menuList[1].HEIGHT - y, 1, 1, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixel);

            // Ahora, si tenemos un color no fondo (negro 0,0,0), buscamos el objeto en cuestión
            if (pixel[0] != 0 || pixel[1] != 0 || pixel[2] != 0)
            {
                Figure selectedFigure = scene.FindFigure(pixel);
                if (selectedFigure != null)
                {
                    scene.SelectFigure(selectedFigure);
                }
            }

            // Independiente de lo ocurrido, reestablecemos los modos
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_TEXTURE_2D);

        }

        public static void MouseWheelHandle(Scene.Scene scene, List<BaseMenu> menuList, int wheel, int dir, int x, int y)
        {
            // Por ahora hacemos el zoom
            scene.SceneCamera.HandleMouseWheel(wheel, dir, x, y);
        }
    }
}
