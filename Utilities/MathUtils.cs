using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Utilities
{
    public class MathUtils
    {
        public static float[] GetVector3Array(Vector v)
        {
            float[] array = new float[3];
            array[0] = v.x;
            array[1] = v.y;
            array[2] = v.z;
            return array;
        }

        public static float[] GetVector4Array(Vector v)
        {
            float[] array = new float[4];
            array[0] = v.x;
            array[1] = v.y;
            array[2] = v.z;
            array[3] = 1;
            return array;
        }

        public static XElement GetXYZElement(string name, Vector data)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("x", data.x);
            element.SetAttributeValue("y", data.y);
            element.SetAttributeValue("z", data.z);

            return element;
        }

        public static XElement GetRGBElement(string name, Vector data)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("red", data.x);
            element.SetAttributeValue("green", data.y);
            element.SetAttributeValue("blue", data.z);

            return element;
        }
    }
}
