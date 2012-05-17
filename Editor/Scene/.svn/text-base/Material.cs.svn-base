using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utilities;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace Editor.Scene
{
    public class Material
    {
        public Bitmap TextureImage;
        public IntPtr bitmapPtr { get; set; }

        public string Name { get; set; }
        public string FileName { get; set; }
        public Vector Diffuse { get; set; }
        public Vector Specular { get; set; }
        public Vector Transparent { get; set; }
        public Vector Reflective { get; set; }
        public Vector RefractionIndex { get; set; }
        public float Shininess { get { return Specular.w; } }

        public Material(string name)
        {
            this.Name = name;
            Specular = new Vector();
            Diffuse = new Vector();
        }

        public Vector GetTexturePixelColor(int x, int y)
        {
            if (TextureImage == null)
                return null;

            Color c = TextureImage.GetPixel(x, y);
            Vector color = new Vector(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f);
            return color;
        }

        public void CreatePointer()
        {
            BitmapData data = this.TextureImage.LockBits(new Rectangle(0, 0, TextureImage.Width, TextureImage.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            bitmapPtr = data.Scan0;

            this.TextureImage.UnlockBits(data);

        }

        public void Save(XElement parentNode)
        {
            XElement xmlMaterial = new XElement("material");
            xmlMaterial.SetAttributeValue("name", Name);

            XElement texture = new XElement("texture");
            texture.SetAttributeValue("filename", FileName);
            xmlMaterial.Add(texture);


            xmlMaterial.Add(MathUtils.GetRGBElement("diffuse", Diffuse));
            XElement specular = MathUtils.GetRGBElement("specular", Specular);
            specular.SetAttributeValue("shininess", Shininess);
            xmlMaterial.Add(specular);

            if(Transparent != null)
                xmlMaterial.Add(MathUtils.GetRGBElement("transparent", Transparent));
            if(Reflective != null)
                xmlMaterial.Add(MathUtils.GetRGBElement("reflective", Reflective));
            if(RefractionIndex != null)
                xmlMaterial.Add(MathUtils.GetRGBElement("refraction_index", RefractionIndex));

            parentNode.Add(xmlMaterial);
            
        }
    }
}
