using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;

namespace FirstPersonGameExample.Geometry
{
    // Load .map Quake 3 / Half Life files. 
    // Only loads simple blocks.
    class Map
    {
        // TOP          1--2--------->
        // BOTTOM       |
        // FRONT        3
        // BACK         |
        // RIGHT        |
        // LEFT

        public static List<BlockBrush> Load(string file)
        {
            List<BlockBrush> map = new List<BlockBrush>();

            string mapData = File.ReadAllText(file);

            for (int i = 2; i < mapData.Split('{').Length; i++)
            {
                string mapDataSplit = mapData.Split('{')[i].Split('}')[0].Trim();
                string[] planeData = mapDataSplit.Split('\n');

                if (planeData.Length != 6)
                    continue;

                if (!planeData[0].Trim().StartsWith("("))
                    continue;

                Face planeDataTop = ConvertToBrush(planeData[0]);
                Face planeDataBottom = ConvertToBrush(planeData[1]);
                Face planeDataFront = ConvertToBrush(planeData[2]);
                Face planeDataBack = ConvertToBrush(planeData[3]);
                Face planeDataRight = ConvertToBrush(planeData[4]);
                Face planeDataLeft = ConvertToBrush(planeData[5]);

                Plane planeRight = new Plane(planeDataRight.P1, planeDataRight.P2, planeDataRight.P3);
                Plane planeFront = new Plane(planeDataFront.P1, planeDataFront.P2, planeDataFront.P3);
                Plane planeTop = new Plane(planeDataTop.P1, planeDataTop.P2, planeDataTop.P3);

                float lengthValue = GetDistance(planeDataLeft, planeRight);
                float widthValue = GetDistance(planeDataBack, planeFront);
                float heightValue = GetDistance(planeDataBottom, planeTop);

                Vector3 brushSize = new Vector3(widthValue * -1, heightValue * -1,
                    lengthValue * -1);

                map.Add(new BlockBrush(planeDataTop, planeDataBottom,
                    planeDataFront, planeDataBack, planeDataRight, planeDataLeft, planeDataLeft.P2, brushSize));
            }

            return map;
        }

        private static Face ConvertToBrush(string brushData)
        {
            string brushDataP1 = brushData.Split('(')[1].Split(')')[0].Trim();
            string brushDataP2 = brushData.Split('(')[2].Split(')')[0].Trim();
            string brushDataP3 = brushData.Split('(')[3].Split(')')[0].Trim();
            string textureName = brushData.Split(new string[] { brushDataP3 }, StringSplitOptions.None)[1].Split('[')[0].Trim().Substring(1).Trim();

            Vector3 p1 = new Vector3(Convert.ToInt32(brushDataP1.Split(' ')[0]),
                Convert.ToInt32(brushDataP1.Split(' ')[1]), Convert.ToInt32(brushDataP1.Split(' ')[2]));

            Vector3 p2 = new Vector3(Convert.ToInt32(brushDataP2.Split(' ')[0]),
                Convert.ToInt32(brushDataP2.Split(' ')[1]), Convert.ToInt32(brushDataP2.Split(' ')[2]));

            Vector3 p3 = new Vector3(Convert.ToInt32(brushDataP3.Split(' ')[0]),
                Convert.ToInt32(brushDataP3.Split(' ')[1]), Convert.ToInt32(brushDataP3.Split(' ')[2]));

            // Rotate plane X Z Y
            return new Face(
                new Vector3(p1.X, p1.Z, p1.Y),
                new Vector3(p2.X, p2.Z, p2.Y),
                new Vector3(p3.X, p3.Z, p3.Y), textureName);
        }

        private static float GetDistance(Face brushPlane, Plane plane)
        {
            float dotLength = Vector3.Dot(plane.Normal, brushPlane.P1);
            float distance = dotLength + plane.D;

            if (distance == 0)
            {
                dotLength = Vector3.Dot(plane.Normal, brushPlane.P2);
                distance = dotLength + plane.D;

                if (distance == 0)
                {
                    dotLength = Vector3.Dot(plane.Normal, brushPlane.P3);
                    distance = dotLength + plane.D;
                }
            }

            return distance;
        }
    }
}
