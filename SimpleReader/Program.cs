using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vub.Etro.IO;

namespace SimpleReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Wrong number of arguments");
                return;
            }
            try
            {
                C3dReader reader = new C3dReader();

                if (!reader.Open(args[0])) {
                    Console.Error.WriteLine("Error: Unable to open file " + args[0]);
                    return;
                }

                for (int i = 0; i < reader.GetParameter<Int16>("POINT:FRAMES"); i++)
                {
                    // returns array of all points, it is necessary to call this method in each cycle
                    Vector3[] array = reader.ReadFrame(); 

                    // we can ask for specific point
                    Vector3 spine = reader["Spine"];
 
                    // get analog data for this frame
                    float[,] analogData = reader.AnalogData;

                    Console.WriteLine("Frame " + i + ": Spine.X " + spine.X + ",  Spine.Y " + spine.Y + ": Spine.Z " + spine.Z);
                }

                // Don't forget to close everithing 
                // - it updates the frames count information in the c3d file header and parameters section
                reader.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }
    }
}
