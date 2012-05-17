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

namespace Editor
{
    class UpdateClass
    {
        static public void Run(int value, Camera camera, List<Figure> objects, List<Light> lights)
        {
            foreach (Figure figure in objects)
                figure.Update(value);

            camera.Update(value);

        }
    }
}
