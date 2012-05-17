using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;
using Utilities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace Editor.Figures
{
    class Cube : Figure
    {
        Vector color;
        List<Vector> vertices = new List<Vector>(8);
        // Pueden ser bytes, pero [F*CK IT], estamos en 2012, donde tenemos memoria para gastar ;)
        List<Array> indices = new List<Array>(6);
        List<Vector> normals = new List<Vector>(6);
        List<double[]> texCoords = new List<double[]>();

        bool hasTexture = false;
        uint[] texture;

        public Cube(Vector position, Vector rotation, Vector scaling, float size, Vector color = null, String textureName = "") :
            base(translation: position, rotation: rotation, scaling: scaling)
        {
            if (textureName != "")
                LoadTexture(textureName);


            // Creamos los vértices ocupando nuestros conocimientos binarios
            vertices.Add(new Vector(size / 2, size / 2, size / 2));     // 0
            vertices.Add(new Vector(size / 2, size / 2, -size / 2));    // 1
            vertices.Add(new Vector(size / 2, -size / 2, size / 2));    // 2
            vertices.Add(new Vector(size / 2, -size / 2, -size / 2));   // 3
            vertices.Add(new Vector(-size / 2, size / 2, size / 2));    // 4
            vertices.Add(new Vector(-size / 2, size / 2, -size / 2));   // 5
            vertices.Add(new Vector(-size / 2, -size / 2, size / 2));   // 6
            vertices.Add(new Vector(-size / 2, -size / 2, -size / 2));  // 7

            /**
             * Creamos los índices para cada tríangulo, según cara
             * Agregamos además las normales a cada cara. El último índice indica la normal.
             * 
             * Hagan el dibujo del cubo y vean porque se ocupan estos índices y normales
             **/
            indices.Add(new int[] { 0, 1, 2, 0 });
            indices.Add(new int[] { 3, 1, 2, 0 });
            normals.Add(new Vector(1, 0, 0));

            indices.Add(new int[] { 7, 5, 3, 1 });
            indices.Add(new int[] { 1, 5, 3, 1 });
            normals.Add(new Vector(0, 0, -1));

            indices.Add(new int[] { 4, 5, 0, 2 });
            indices.Add(new int[] { 1, 5, 0, 2 });
            normals.Add(new Vector(0, 1, 0));

            indices.Add(new int[] { 6, 7, 4, 3 });
            indices.Add(new int[] { 5, 7, 4, 3 });
            normals.Add(new Vector(-1, 0, 0));

            indices.Add(new int[] { 6, 7, 2, 4 });
            indices.Add(new int[] { 3, 7, 2, 4 });
            normals.Add(new Vector(0, -1, 0));

            indices.Add(new int[] { 6, 4, 2, 5 });
            indices.Add(new int[] { 0, 4, 2, 5 });
            normals.Add(new Vector(0, 0, 1));

            this.color = color != null ? color : new Vector();
        }

        private void LoadTexture(String textureName)
        {
            hasTexture = true;

            // Deberiamos agarrar el caso en que la textura no se encuentre
            Bitmap image = new Bitmap(textureName);

            // Creamos el bitmapData que nos permitira pasar la información a OpenGL
            BitmapData bitmapData;
            bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            
            // Inicializamos el puntero de textura, ya que si es null, OpenGL intentará escribir 
            // sobre un puntero nulo, efectivamente intentanto escribir en memoria protegida... o peor (Skynet)
            texture = new uint[1];

            // Generamos punteros de OpenGL a las texturas (en este caso necesitamos 1, pero aún así
            // OpenGL pide un arreglo para guardar los punteros.
            Gl.glGenTextures(1, texture);

            // Indicamos que el puntero de textura que pedimos es una textura 2D.
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR); 
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            // Guardamos la textura en OpenGL
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D,               // El tipo de textura que estamos cargando
                            0,                              // El nivel de detalle (0 es el nivel base)
                            Gl.GL_RGBA,                      // En que formato de color será guardada la textura en OpenGL
                            image.Width, image.Height,      // El tamaño de la textura     
                            0,                              // El borde de la textura
                            Gl.GL_BGR,                      // En que formato de color viene la textura
                                                            // ¡OJO CON ESTO, ME DEMORE BASTANTE EN CACHAR PORQUE ME MOSTRABA MAL
                                                            // LOS COLORES!
                            Gl.GL_UNSIGNED_BYTE,            // En que tipo de dato viene la información
                            bitmapData.Scan0);              // El puntero a los pixeles

            // Liberamos la memoria, ya que la textura está guardada en OpenGL
            image.UnlockBits(bitmapData);
            image.Dispose();
        }

        public override void Draw(bool select = false)
        {
            Gl.glPushMatrix();
   

            Gl.glTranslatef(Traslation.x, Traslation.y, Traslation.z);
            Gl.glRotatef(Rotation.x, 1f, 0f, 0f);
            Gl.glRotatef(Rotation.y, 0f, 1f, 0f);
            Gl.glRotatef(Rotation.z, 0f, 0f, 1f);

            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glColor3fv(MathUtils.GetVector3Array(hasTexture ? new Vector(1, 1, 1) : color));

            // Para texturizar, aprovechamos la forma en que agregamos los vértices
            bool upTriangle = true;
            List<double[]> texCoords = new List<double[]>(4);
            texCoords.Add(new double[]{0,0});
            texCoords.Add(new double[]{1,0});
            texCoords.Add(new double[]{0,1});
            texCoords.Add(new double[]{1,1});

            foreach (int[] index in indices)
            {
                // Agregamos los vértices  y normales de cada triángulo del cubo
                Gl.glNormal3fv(MathUtils.GetVector3Array(normals[index[3]]));
                if (hasTexture) Gl.glTexCoord2dv(upTriangle ? texCoords[1] : texCoords[2]);
                Gl.glVertex3fv(MathUtils.GetVector3Array(vertices[index[0]]));

                Gl.glNormal3fv(MathUtils.GetVector3Array(normals[index[3]]));
                if (hasTexture) Gl.glTexCoord2dv(texCoords[3]);
                Gl.glVertex3fv(MathUtils.GetVector3Array(vertices[index[1]]));

                Gl.glNormal3fv(MathUtils.GetVector3Array(normals[index[3]]));
                if (hasTexture) Gl.glTexCoord2dv(texCoords[0]);
                Gl.glVertex3fv(MathUtils.GetVector3Array(vertices[index[2]]));

                upTriangle = !upTriangle;
            }

            Gl.glEnd();

            Gl.glPopMatrix();
        }

        protected override void SetUpDelegates() { }

        public override void Update(int value)
        {
            // Rotamos el cubo en el eje z
            Rotation += new Vector(-0.3f, 1f, 2f);
            float x = 0f;
            if (Rotation.x < 0) x = 360f;
            else if (Rotation.x > 360) x = -360f;
            float y = 0f;
            if (Rotation.y < 0) y = 360f;
            else if (Rotation.y > 360) y = -360f;
            float z = 0f;
            if (Rotation.z < 0) z = 360f;
            else if (Rotation.z > 360) z = -360f;

            Rotation += new Vector(x, y, z);
            
        }
		
		public override void MouseMenuInput(Scene.Scene scene, int button, int state, int x, int y)
		{
			throw new NotImplementedException();
		}


        public override void Save(XElement parentNode)
        {
            throw new NotImplementedException();
        }

        public override void Print(Scene.Scene scene, int x, int y)
        {
            throw new NotImplementedException();
        }

        protected override void CallbackMethod(string property, object values)
        {
            throw new NotImplementedException();
        }

    }
}
