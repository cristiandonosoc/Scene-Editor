using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Tao.FreeGlut;

namespace Editor.Menues
{
    public abstract class BaseMenu
    {

        public int WIDTH { get; private set; }
        public int HEIGHT { get; private set; }
        public int OffsetX { get; private set; }
        public int OffsetY { get; private set; }

        public int Margin { get; private set; }
        public IntPtr Font = Glut.GLUT_BITMAP_HELVETICA_10;

        protected Vector outerColor;
        protected Vector innerColor;

        public BaseMenu(int width, int height, int offsetX, int offsetY, int margin)
        {
            this.WIDTH = width;
            this.HEIGHT = height;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.Margin = margin;
        }
		
		/// <summary>
		/// Draw the specified scene.
		/// </summary>
		/// <param name='scene'>
		/// Scene.
		/// </param>
        public abstract void Draw(Scene.Scene scene);
		
		/// <summary>
		/// Handles the input.
		/// </summary>
		/// <param name='scene'>
		/// Scene.
		/// </param>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='x'>
		/// Coordenada X
		/// </param>
		/// <param name='y'>
		/// Coordenada Y	
		/// </param>
		public abstract void KeyboardInput(Scene.Scene scene, byte key, int x, int y);
		
		public abstract void MouseInput(Scene.Scene scene, int button, int state, int x, int y);


    }
}
