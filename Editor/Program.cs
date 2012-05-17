using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;
using Utilities;
using Editor.Figures;
using Editor.Lighting;
using Editor.Scene;
using System.Threading;
using Editor.Menues;
using Editor.OpenGL_Methods;

namespace Editor
{
    /// <summary>
    /// SCENE EDITOR
    /// 
    /// Clase donde inicia el programa y en este caso, se especifica la funcionalidad general de OpenGL.
    /// Prácticamente TODO el código está comentado de manera bastante explicativa, por lo que debiese ser fácil
    /// leerlo y entender que se está haciendo en cada momento.
    /// 
    /// UPDATE: Los últimos cambios han sido hechos bastante rápidos y, si bien siguen comentados, ya no siguen un
    /// patrón de diseño tan pensado como antes (shame on me >> Las figuras tienen información sobre sus menus de
    /// edición... Oh, the humanity).
    /// 
    /// Para ver como funcionan distintos partes del editor, referirse a:
    /// 
    /// ESCENA:     Scene/Scene.cs (El alto y ancho de la ventana están definidos en el constructor aquí en Init())
    /// CÁMARA:     Scene/Camera.cs
    /// LUZ:        Lighting/Light.cs
    /// OBJETOS:    Figures/Figure.cs (Es el lugar de inicio. Todas las figuras (objetos renderiables) heredan de esta clase)
    ///             Figures/* (Todas las figuras están en esta carpeta)
    /// DRAW, 
    /// UPDATE, 
    /// HANDLEINPUT: OpenGL Methods/*   (Con el fin de ordenar un poco el código, decidí separar un poco el código. Cada uno
    ///                                 de estos métodos están implementados como métodos estáticos de una clase separada, 
    ///                                 con el fin de que cada una de las funcionalidades del loop estén bien separadas entre sí).
    ///                                 
    /// La lógica de separar esta funcionalidad en distintas clases implica que necesito un objeto que permita coordinar
    /// el estado del programa entre si. Esta función la cumple la clase Scene. Esto implica que cumple dos roles: tiene el
    /// estado de la escena como tal (figuras y su información) y además mantiene la información del estado del programa.
    /// Esto tiene sus ventajas y sus muchas desventajas.
    ///  
    /// </summary>
    class Program
    {
        
        // Alcance de nombres weon... Pero ya no lo cambié (you win this time gravity)
        static Editor.Scene.Scene scene;
        static List<BaseMenu> menuList = new List<BaseMenu>();
        
        //static Camera camera;
        //static List<Figure> sceneObjects;
		
		enum CameraPerspective { Perspective, Ortho };
		static CameraPerspective cameraPerspective = CameraPerspective.Perspective;

        static void Main(string[] args)
        {

            
            // Creamos la escena
            scene = new Scene.Scene(800, 600);

            // Creamos los menues
            // ES IMPORTANTE QUE <InfoMenu> VAYA PRIMERO
            int bottomHeight = 50;
            menuList.Add(new InfoMenu(300, scene.HEIGHT - 20, 0, bottomHeight + 20));
            // TODO: Generalizar los offsets
            menuList.Add(new BottomMenu(scene.WIDTH+menuList[0].WIDTH, bottomHeight, 0, 0));

            // Inicializamos GLUT
            Glut.glutInit();
            // Inicializamos un display, y le indicamos con OR los distintos flags
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGBA | Glut.GLUT_DEPTH);

            // Hacemos la ventana algo más grande que el viewport de escena para tener
            // como mostrar información extra
            Glut.glutInitWindowSize(scene.WIDTH + menuList[0].WIDTH, scene.HEIGHT + menuList[1].HEIGHT);
            Glut.glutInitWindowPosition(100, 10);

            // Creamos el display
            int id = Glut.glutCreateWindow("Scene Editor");
            
            // Inicializamos el programa
            Init();
            
            /**
             * Linkeamos a GLUT los handlers que creamos para los eventos.
             * 
             * OJO: Solo tenemos permiso a dibujar en la llamada que definimos para DisplayFunc
             **/
            Glut.glutDisplayFunc(new Glut.DisplayCallback(Draw));                   // Dibujar
            Glut.glutKeyboardFunc(new Glut.KeyboardCallback(KeyboardHandle));       // Input Teclado
            Glut.glutMouseFunc(new Glut.MouseCallback(MouseHandle));                // Input Mouse
            Glut.glutMouseWheelFunc(new Glut.MouseWheelCallback(MouseWheelHandle)); // Rueda del mouse
            //Glut.glutReshapeFunc(new Glut.ReshapeCallback(Resize));                 // Resize de la ventana
         
            // glutTimerFunc llama a la función entregada en el tiempo indicado en ms, y le entrega a
            // tal función el value, para poner identificar en caso de ser necesario.
            // Ojo que no se pueden cancelar callbacks. En tal caso hay que ignorarlos vía estados.
            // 34 ms = ~30 FPS
            Glut.glutTimerFunc(34, Update, 0);

            // Iniciamos el Loop gráfico
            Glut.glutMainLoop();
        }

        /// <summary>
        /// Método que inicializa OpenGL
        /// </summary>
        static void Init()
        {
            // Inicializamos la escena desde archivo
            scene.Load("Scene/sceneTest.xml");

            //scene.Save("Scene/output.xml");

            // GL_DEPTH_TEST usa la profundidad para dibujar lo que esta enfrente 
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            //Gl.glClearColor(1, 0, 0.9f, 1);

            // Colores y texturas
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            

            // Iluminacíón
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_LIGHT1);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glEnable(Gl.GL_SMOOTH);

			
			// Perspectiva
			Resize(scene.WIDTH, scene.HEIGHT);

            //setShaders("Toon");
        }

        static int program;

        static void setShaders(String shader)
        {
            // Cargamos y compilamos el vertex shader
            int vs = Gl.glCreateShader(Gl.GL_VERTEX_SHADER);
            String[] vsSource = new String[] { System.IO.File.ReadAllText("Shaders/" + shader + ".vert") };
            Gl.glShaderSource(vs, 1, vsSource, null);
            Gl.glCompileShader(vs);

            // Cargamos y compilamos el fragment shader
            int fs = Gl.glCreateShader(Gl.GL_FRAGMENT_SHADER);
            String[] fsSource = new String[] { System.IO.File.ReadAllText("Shaders/" + shader + ".frag") };
            Gl.glShaderSource(fs, 1, fsSource, null);
            Gl.glCompileShader(fs);

            // Linkeamos los shaders a un programa entendible por OpenGL
            program = Gl.glCreateProgram();
            Gl.glAttachShader(program, vs);
            Gl.glAttachShader(program, fs);
            Gl.glLinkProgram(program);
            
            // Finalmente le decimos a OpenGL que use nuestro programa de shaders
            Gl.glUseProgram(program);

        }

        /// <summary>
        /// Método llamado para actualizar el estado de la escena. Se crea creadno un timer en GLUT.
        /// </summary>
        /// <param name="value">Entregado por GLUT para reconocer la llamada en caso de hacer varios timers inscritos.</param>
        static void Update(int value)
        {
            // Hacemos update del estado de la escena
            //UpdateClass.Run(value, camera: camera, objects: sceneObjects, lights: sceneLights);
            UpdateClass.Run(value, camera: scene.SceneCamera,
                            objects: scene.SceneFigures,
                            lights: scene.SceneLights);
            // Indicamos a GLUT que la escena cambió y por tanto hay que repintarla
            Glut.glutPostRedisplay();

            // Rellamamos al timer para hacer efectivamente un loop
            Glut.glutTimerFunc(34, Update, value);
        }

        /// <summary>
        /// Método que hace el render de la escena.
        /// Este es el único lugar donde se pueden hacer llamadas para dibujar,
        /// ya que este es el método que GLUT usa para hacer tales llamadas.
        /// </summary>
        static void Draw()
        {
            // Mandamos a dibujar la escena
            //DrawClass.Run(camera, objects: sceneObjects, lights: sceneLights);
            DrawClass.Run(scene: scene, select: false, menuList: menuList);
        }

        /// <summary>
        /// Método que maneja un cambio de tamaño de la ventana. Debido a que el flujo eventualmente cambió a múltiples
        /// viewports, el resize funciona de manera nominal (la aplicación aún se vé en proporción, pero la funcionalidad
        /// de select se perdió. Ese fue el punto donde comenzó el hardcodeo...
        /// </summary>
        /// <param name="w">Ancho de la ventana</param>
        /// <param name="h">Altura de la venta</param>
        public static void Resize(int w, int h)
        {
            // TODO: Generalizar offsets
            Gl.glViewport(menuList[0].WIDTH, menuList[1].HEIGHT, w, h);                      // Actualizamos el tamaño del viewport

            Gl.glMatrixMode(Gl.GL_PROJECTION);              // Vamos a actualizar la matriz de proyección
            Gl.glLoadIdentity();

            // Cargamos la cámara de la escena
            Camera camera = scene.SceneCamera;

			if(cameraPerspective == CameraPerspective.Perspective)
            	Glu.gluPerspective(camera.FOV,              // Ángulo FOV   
                                (double)w / (double)h,      // Razón Ancho:Alto
                                camera.NearClip,                // z Near
                                camera.FarClip);                // z Far
			else if(cameraPerspective == CameraPerspective.Ortho)
			{
				double zDif = camera.FarClip - camera.NearClip;
				//Gl.glOrtho(-w/2, w/2, -h/2, h/2, -0, 0);
				double div = 50;
				Gl.glOrtho(-w/div, w/div, -h/div, h/div, 1, 200);
			}
			
			// Actualizamos el tamaño de la pantalla
			scene.WIDTH = w;
			scene.HEIGHT = h;
			
        }

        /// <summary>
        /// Manejamos el input del teclado
        /// </summary>
        /// <param name="key">Caracter recibido por el evento de input</param>
        /// <param name="x">Coordenada x del mouse al momento de apretar la tecla</param>
        /// <param name="y">Coordenada y del mouse al momento de apretar la tecla</param>
        static void KeyboardHandle(byte key, int x, int y)
        {
            /* FUNCIONALIDAD BUGGEADA DE CREAR PERSPECTIVA ORTOGONAL
			// Manejamos el cambio de cámara en este contexto
			if(key == 'c')
			{
				if(cameraPerspective == CameraPerspective.Perspective)
					cameraPerspective = CameraPerspective.Ortho;
				else if(cameraPerspective == CameraPerspective.Ortho)
					cameraPerspective = CameraPerspective.Perspective;
				
                // Reutilización de código algo trucha ya que tecnicamente no estamos haciendo un resize, pero
                // aprovecho el hecho de que se calcula la perspectiva en este método.
				Resize (scene.WIDTH, scene.HEIGHT);
			}
            
            */
			
            // Manejamos nuestro input
            HandleKeyboardInputClass.KeyboardInput(key, x, y, scene, menuList);
        }

        /// <summary>
        /// Manejamos el input del mouse
        /// </summary>
        /// <param name="button">Boton apretado (ID de OpenGL)</param>
        /// <param name="state">Estado del boton (Press o Release)</param>
        /// <param name="x">Coordenada X del evento</param>
        /// <param name="y">Coordenada Y del evento</param>
        static void MouseHandle(int button, int state, int x, int y)
        {
            // Llamamos a nuestra función estática para que se encargue de todo
            HandleMouseInputClass.MouseInput(button, state, x, y, scene, menuList);
        }

        /// <summary>
        /// Maneja un evento de la rueda del mouse
        /// </summary>
        /// <param name="wheel">Identificador de la rueda</param>
        /// <param name="direction">Dirección de cambio ed la rueda</param>
        /// <param name="x">Coordenada X del mouse</param>
        /// <param name="y">Coordenada Y del mouse</param>
        static void MouseWheelHandle(int wheel, int direction, int x, int y)
        {
            // Llamamos al método que tiene la lógica
            HandleMouseInputClass.MouseWheelHandle(scene, menuList, wheel, direction, x, y);
        }

    }
}
