﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;
using Utilities;
using System.Xml.Linq;
using Tao.FreeGlut;
using Editor.Menues;

namespace Editor.Figures
{
    /// <summary>
    /// Enum con todas las figuras que se pueden crear o editar
    /// </summary>
    public enum FigureEnum { None, Triangle, Sphere };

    public delegate void EditPropertyDelegate(string property, Object values);

    public abstract class Figure
    {

        public string Name { get; set; }
        public Vector Traslation { get; set; }
        public Vector Rotation { get; set; }
        public Vector Scaling { get; set; }
        public Vector Program { get; set; }
        public virtual Vector Color { get; set; }
        public byte[] ColorByte {get; set;}
        public virtual bool isSelected { get; set; }
        public Dictionary<string, Button> buttonDictionary { get; private set; }


        #region Constructores

        public Figure(string name = "", Vector translation = null, Vector rotation = null, Vector scaling = null)
        {
            this.Name = name;
            this.Traslation = translation != null ? translation : new Vector();
            this.Rotation = rotation != null ? rotation : new Vector();
            this.Scaling = scaling != null ? scaling : new Vector(1, 1, 1);
            buttonDictionary = new Dictionary<string, Button>();
        }


        #endregion

        /// <summary>
        /// Método que dibuja la figura. Se preocupa de guardar el estado anterior y moverse relativo a el.
        /// Al terminar, devuelve a OpenGL a su estado anterior.
        /// </summary>
        public abstract void Draw(bool select = false);

        /// <summary>
        /// Método que imprime la información sobre el objeto en el menu.
        /// </summary>
        public abstract void Print(Scene.Scene scene, int x, int y);

        protected virtual void SetUpDelegates()
        {
            Button button;

            List<string> propList = new List<string>();
            propList.Add("Name");
            button = new Button("", 200, 30, 1, propList, false);
            button.callback = new EditPropertyDelegate(CallbackMethod);
            buttonDictionary.Add("Name", button);
        }


        /// <summary>
        /// Método que actualiza el estado de la figura en cuestión.
        /// </summary>
        /// <param name="value">value entregado por GLUT</param>
        public abstract void Update(int value);

        /// <summary>
        /// Método que serializa una escena a un nodo de XML
        /// </summary>
        /// <param name="parentNode">El nodo padre</param>
        public abstract void Save(XElement parentNode);
		
		public abstract void MouseMenuInput(Scene.Scene scene, int button, int state, int x, int y);

        /// <summary>
        /// Imprime a pantalla texto (Usado para que la figura imprima su información en el menú)
        /// </summary>
        /// <param name="font">El font a ser usado</param>
        /// <param name="text">El texto a imprimir</param>
        /// <param name="x">Coordenada X del texto</param>
        /// <param name="y">Coordenada Y del texto</param>
        public static void WriteText(IntPtr font, string text, int x, int y, int r = 0, int g = 0, int b = 0)
        {
            Gl.glRasterPos2d(x, y);
            Gl.glColor3d(r, g, b);
            Glut.glutBitmapString(font, text);
        }

        /// <summary>
        /// Genera una máscara que dice que datos deben tomar el valor original, usando un valor
        /// predeterminado que funciona como UNDEFINED o UNCHANGED.
        /// Este valor está definido en la clase Scene.
        /// </summary>
        /// <param name="values">Los valores del vector</param>
        /// <returns>La máscara correspondiente</returns>
        protected Vector InputMask(Vector values)
        {
            Vector result = new Vector();
            for (int i = 0; i < 4; i++)
                result[i] = values[i] == Scene.Scene.UNDEFINED ? 0 : 1;

            return result;
        }

        protected virtual void CallbackMethod(string property, Object values)
        {
            if (values is string)
            {
                string value = values as string;

                if (property == "")
                    Name = value;
            }
        }
    }
}
