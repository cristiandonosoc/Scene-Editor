﻿using System;
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
    /// Decidí definir aquí este enum ya que es aquí donde se utiliza más frecuentemente.
    /// </summary>
    public enum SceneState { Free, Edit, Error, Create, Save, LoadObj };

    /// <summary>
    /// Clase donde se define la funcionalidad para actualizar el estado del programa vía el input del usuario.
    /// 
    /// Es importante notar que, como está en un método estático en una clase separada, este código está pensado
    /// como una manera de ordenar el donde está escrito el programa más que un patrón de diseño per se.
    /// </summary>
    class HandleKeyboardInputClass
    {
        // Algunas teclas
        protected static byte ENTER = 13;
        protected static byte ESCAPE = 27;
        protected static byte TAB = 9;
        protected static byte BACKSPACE = 8;
        protected static byte SUPR = 127;
        protected static byte SPACE = 32;


        static public void KeyboardInput(byte key, int x, int y, Scene.Scene scene, List<BaseMenu> menuList)
        {
            Camera camera = scene.SceneCamera;
            List<Figure> objectList = scene.SceneFigures;
            List<Light> lightList = scene.SceneLights;
            
            if (scene.sceneState == SceneState.Save)
                scene.sceneState = SceneState.Free;

            else if (scene.sceneState == SceneState.Free)
            {
                if (key == SPACE)
                {
                    scene.Save("output.xml");
                    scene.sceneState = SceneState.Save;
                }

                if (key == SUPR)
                {
                    scene.DeleteSelectedFigure();
                }

                if (key == 'l')
                {
                    scene.InputString = "";
                    scene.sceneState = SceneState.LoadObj;
                }

                if (key == 'c')
                    scene.sceneState = SceneState.Create;

                if (key == '1')
                    camera.cameraMode = CameraMode.Z;
                if (key == '2')
                    camera.cameraMode = CameraMode.X;
                if (key == '3')
                    camera.cameraMode = CameraMode.Y;

                // Manejamos el input de la cámara
                scene.SceneCamera.HandleInput(key, x, y);

                // Manejamos el input de los menues
                foreach (BaseMenu menu in menuList)
                    menu.KeyboardInput(scene, key, x, y);
            }

            // ESPERANDO INPUT DE EDICIÓN
            else if (scene.sceneState == SceneState.Edit)
                EditInput(scene, key);

            // EL INPUT TIENE ERROR
            else if (scene.sceneState == SceneState.Error)
                ErrorInput(scene, key);

            // INPUT DE CREACIÓN
            else if (scene.sceneState == SceneState.Create)
                CreateInput(scene, key);

            // INPUT DE CARGA OBJ
            else if (scene.sceneState == SceneState.LoadObj)
                LoadObj(scene, key);
        }

        

        /// <summary>
        /// Método que maneja la lógica de crear nuevos objetos
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="key"></param>
        protected static void CreateInput(Scene.Scene scene, byte key)
        {
            if (key == 't')
                scene.FigureToCreate = FigureEnum.Triangle;
            else if (key == 's')
                scene.FigureToCreate = FigureEnum.Sphere;
            else if (key == ENTER)
            {
                if (scene.FigureToCreate == FigureEnum.Triangle)
                {
                    Triangle triangle = new Triangle();
                    scene.AddFigure(triangle);
                    scene.SelectFigure(triangle);
                    scene.sceneState = SceneState.Free;
                    scene.FigureToCreate = FigureEnum.None;

                }
                else if (scene.FigureToCreate == FigureEnum.Sphere)
                {
                    Sphere sphere = new Sphere();
                    scene.AddFigure(sphere);
                    scene.SelectFigure(sphere);
                    scene.sceneState = SceneState.Free;
                    scene.FigureToCreate = FigureEnum.None;
                }

            }
            if (key == ESCAPE)
            {
                scene.FigureToCreate = FigureEnum.None;
                scene.sceneState = SceneState.Free;
            }
        }

        protected static void EditInput(Scene.Scene scene, byte key)
        {
            // Enter (13) termina con la edicion
            if (key == ENTER)
            {
                // Intentamos parsear el input
                if (scene.SelectedButton.ParseInput(scene.InputString))
                {
                    // Este input fue parseado con éxito
                    // Reiniciamos el input
                    scene.InputString = "";

                    // Vemos si el botón está listo
                    if (scene.SelectedButton.COMPLETED)
                    {
                        scene.SelectedButton.COMPLETED = false;
                        scene.SelectedButton.selected = false;
                        scene.sceneState = SceneState.Free;
                    }
                    else
                    {
                        // Cuando hay que continuar parseando
                    }
                }
                else
                {
                    // El input no es válido
                    scene.sceneState = SceneState.Error;
                }
            }

            // TAB (9) permite pasar el input al siguiente sin cambiar el valor
            else if (key == TAB)
            {
                // Decimos que no queremos cambiar más el input
                scene.SelectedButton.ParseInput(Scene.Scene.UNDEFINED.ToString());
                // Vemos si el botón está listo
                if (scene.SelectedButton.COMPLETED)
                {
                    scene.SelectedButton.COMPLETED = false;
                    scene.SelectedButton.selected = false;
                    scene.sceneState = SceneState.Free;
                }
            }

            // ESCAPE (27) deja el input
            else if (key == ESCAPE)
            {
                

                scene.SelectedButton.COMPLETED = false;
                scene.SelectedButton.selected = false;
                scene.SelectedButton.RestartInput();
                scene.sceneState = SceneState.Free;

            }

            // Sino, capa input que recibimos los tiramos al input string
            else
            {
                if (scene.SelectedFigure is Model && scene.SelectedButton != null && (key == 'a' || key == 's' || key == 'd' || key == 'w' || key == 'i' || key == 'k'))
                {
                    Model m = (Model)scene.SelectedFigure;
                    if (key == 'a')
                        m.EditPropertyByOffset(scene.SelectedButton.Name, new Vector(-0.1, 0, 0));
                    else if (key == 'd')
                        m.EditPropertyByOffset(scene.SelectedButton.Name, new Vector(0.1, 0, 0));
                    else if (key == 'w')
                        m.EditPropertyByOffset(scene.SelectedButton.Name, new Vector(0, 0.1, 0));
                    else if (key == 's')
                        m.EditPropertyByOffset(scene.SelectedButton.Name, new Vector(0, -0.1, 0));
                    else if (key == 'k')
                        m.EditPropertyByOffset(scene.SelectedButton.Name, new Vector(0, 0, -0.1));
                    else if (key == 'i')
                        m.EditPropertyByOffset(scene.SelectedButton.Name, new Vector(0, 0, 0.1));
                }
                else
                {
                    string input = scene.InputString;
                    if (key == BACKSPACE)    // BORRAR
                    {
                        if (input.Length > 0)
                            input = input.Substring(0, input.Length - 1);
                    }
                    else
                        input += ((char)key).ToString();

                    // Actualizamos el input
                    scene.InputString = input;
                }
            }
        }

        protected static void ErrorInput(Scene.Scene scene, byte key)
        {
            // Como hubo un error, solo esperamos un input.
            scene.sceneState = SceneState.Edit;
            scene.InputString = "";
        }


        protected static void LoadObj(Scene.Scene scene, byte key)
        {
            // Enter (13) termina con la edicion
            if (key == ENTER)
            {
              
                scene.LoadObjModel(scene.InputString);               
                scene.sceneState = SceneState.Free;
            }

            // ESCAPE (27) deja el input
            else if (key == ESCAPE)
            {
                scene.SelectedButton.COMPLETED = false;
                scene.SelectedButton.RestartInput();
                scene.sceneState = SceneState.Free;
            }

            // Sino, capa input que recibimos los tiramos al input string
            else
            {
                string input = scene.InputString;
                if (key == BACKSPACE)    // BORRAR
                {
                    if (input.Length > 0)
                        input = input.Substring(0, input.Length - 1);
                }
                else
                    input += ((char)key).ToString();

                // Actualizamos el input
                scene.InputString = input;
            }
        }
        

    }
}
