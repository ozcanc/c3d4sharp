using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vub.Etro.IO;

namespace SimpleWriter
{
    public enum SkeletonMarkers // | Kinect | OpenNI | IISU | Vicon |
    {                           // +---+---+---+---+
        HipCenter = 0,          // | X | - | X | X | 
        Spine = 1,              // | X | X | X | X | 
        ShoulderCenter = 2,     // | X | X | X | X | 
        Head = 3,               // | X | X | X | X | 
        ShoulderLeft = 4,       // | X | X | X | X |  
        ElbowLeft = 5,          // | X | X | X | X | 
        WristLeft = 6,          // | X | - | - | X |  
        HandLeft = 7,           // | X | X | X | X | 
        ShoulderRight = 8,      // | X | X | X | X | 
        ElbowRight = 9,         // | X | X | X | X | 
        WristRight = 10,        // | X | - | - | X | 
        HandRight = 11,         // | X | X | X | X | 
        HipLeft = 12,           // | X | X | X | X |
        KneeLeft = 13,          // | X | X | X | X |
        AnkleLeft = 14,         // | X | - | - | X |
        FootLeft = 15,          // | X | X | X | X |
        HipRight = 16,          // | X | X | X | X |
        KneeRight = 17,         // | X | X | X | X |
        AnkleRight = 18,        // | X | - | - | X |
        FootRight = 19,         // | X | X | X | X |
        Sternum = 20,           // | - | - | X | X |
        Count = 21,             // +---+---+---+---+
    }

    internal static class ArrayCopyHelper
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    class Program
    {


        static void Main(string[] args)
        {
            C3dWriter _writer = new C3dWriter();
            Int16[] _analogData = null;
            Vector3[] _currentData = null;
            _writer.SetParameter<string[]>("POINT:DATA_TYPE_LABELS", new string[] {
               "Skeleton",
                "Accelerometer",
                "BalanceBoard"
            });
            _writer.Header.AnalogChannels = (short)(7);
            _analogData = new Int16[_writer.Header.AnalogChannels];
            _writer.Header.AnalogSamplesPerFrame = 1;
            _writer.SetParameter<string>("SUBJECTS:MARKER_SET", "Using ETRO extended marker set");
            _writer.SetParameter<string>("INFO:SYSTEM", "ict4rehab");
            _writer.SetParameter<string>("INFO:EVENT", "test");
            _writer.SetParameter<string>("INFO:GAME", "C3D TEST");
            _writer.SetParameter<Int16>("ANALOG:USED", _writer.Header.AnalogChannels);

            string[] alabels = new string[] { 
                "year      ", 
                "month     ",
                "day       ",
                "hour      ", 
                "minute    ",
                "second    ",
                "milisecond"};
            _writer.SetParameter<string[]>("ANALOG:LABELS", alabels.ToArray<string>());

            string[] labels = Enum.GetNames(typeof(SkeletonMarkers));
            labels = ArrayCopyHelper.SubArray<string>(labels, 0, labels.Length - 1);

            string[] angleLabels = new string[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                angleLabels[i] = labels[i] + "Adsadadsadsdsdsd";
            }
            string[] qualityLabels = new string[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                qualityLabels[i] = labels[i] + "Quality";
            }

            _writer.SetParameter<string[]>("POINT:LABELS", labels.Union<string>(angleLabels).ToArray<string>());//.Union<string>(qualityLabels).ToArray<string>());
            _writer.SetParameter<Int16>("POINT:DATA_TYPE", 0);
            _writer.SetParameter<float>("ANALOG:RATE", 30);
            _writer.PointsCount = (short)(((int)SkeletonMarkers.Count) + angleLabels.Length);//  + qualityLabels.Length);
            _currentData = new Vector3[_writer.PointsCount];
            _writer.Open("datafile.c3d");

            for (int i = 0; i < (int)SkeletonMarkers.Count - 1; i++)
            {
                _currentData[i] = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < (int)SkeletonMarkers.Count - 1; i++)
            {

                _currentData[i + (int)SkeletonMarkers.Count-1] = new Vector3(0, 0, 0);
            }

            //for (int i = 0; i < (int)SkeletonMarkers.Count - 1; i++)
            //{

            //    _currentData[i + 2 * (int)SkeletonMarkers.Count] = new Vector3(0, 0, 0);
            //}
            
            _writer.WriteIntFrame(_currentData);
            



            int pos = 0;
            _analogData[pos++] = (Int16)DateTime.Now.Year;
            _analogData[pos++] = (Int16)DateTime.Now.Month;
            _analogData[pos++] = (Int16)DateTime.Now.Day;
            _analogData[pos++] = (Int16)DateTime.Now.Hour;
            _analogData[pos++] = (Int16)DateTime.Now.Minute;
            _analogData[pos++] = (Int16)DateTime.Now.Second;
            _analogData[pos++] = (Int16)DateTime.Now.Millisecond;
            _writer.WriteIntAnalogData(_analogData);
        



            _writer.Close();
        }

        //static void Main(string[] args)
        //{
        //    C3dWriter _writer = new C3dWriter();
        //    Int16[] _analogData = null;
        //    Vector3[] _currentData = null;
        //    _writer.SetParameter<string[]>("POINT:DATA_TYPE_LABELS", new string[] {
        //        "Skeleton",
        //        "Accelerometer",
        //        "BalanceBoard"
        //    });
        //    _writer.Header.AnalogChannels = (short)(7);
        //    _analogData = new Int16[_writer.Header.AnalogChannels];
        //    _writer.Header.AnalogSamplesPerFrame = 1;
        //    _writer.SetParameter<string>("SUBJECTS:MARKER_SET", "Using ETRO extended marker set");
        //    _writer.SetParameter<string>("INFO:SYSTEM", "ict4rehab");
        //    _writer.SetParameter<string>("INFO:EVENT", "test");
        //    _writer.SetParameter<string>("INFO:GAME", "C3D TEST");
        //    _writer.SetParameter<Int16>("ANALOG:USED", _writer.Header.AnalogChannels);

        //    string[] alabels = new string[] { 
        //        "year      ", 
        //        "month     ",
        //        "day       ",
        //        "hour      ", 
        //        "minute    ",
        //        "second    ",
        //        "milisecond"};
        //    _writer.SetParameter<string[]>("ANALOG:LABELS", alabels.ToArray<string>());

        //    string[] labels = Enum.GetNames(typeof(SkeletonMarkers));
        //    labels = ArrayCopyHelper.SubArray<string>(labels, 0, labels.Length - 1);

        //    string[] angleLabels = new string[labels.Length];
        //    for (int i = 0; i < labels.Length; i++)
        //    {
        //        angleLabels[i] = labels[i] + "Angles";
        //    }
        //    string[] qualityLabels = new string[labels.Length];
        //    for (int i = 0; i < labels.Length; i++)
        //    {
        //        qualityLabels[i] = labels[i] + "Quality";
        //    }

        //    _writer.SetParameter<string[]>("POINT:LABELS", labels.Union<string>(angleLabels).Union<string>(qualityLabels).ToArray<string>());
        //    _writer.SetParameter<Int16>("POINT:DATA_TYPE", 0);
        //    _writer.SetParameter<float>("ANALOG:RATE", 30);
        //    _writer.PointsCount = (short)(((int)SkeletonMarkers.Count) + angleLabels.Length + qualityLabels.Length);
        //    _currentData = new Vector3[_writer.PointsCount];
        //    _writer.Open("datafile.c3d");

        //    for (int i = 0; i < (int)SkeletonMarkers.Count - 1; i++)
        //    {
        //        _currentData[i] = new Vector3(0, 0, 0);
        //    }

        //    for (int i = 0; i < (int)SkeletonMarkers.Count - 1; i++)
        //    {

        //        _currentData[i + (int)SkeletonMarkers.Count] = new Vector3(0, 0, 0);
        //    }

        //    for (int i = 0; i < (int)SkeletonMarkers.Count - 1; i++)
        //    {

        //        _currentData[i + 2 * (int)SkeletonMarkers.Count] = new Vector3(0, 0, 0);
        //    }

        //    _writer.WriteIntFrame(_currentData);




        //    //   internal void WriteAnalogData(IGame game)
        //    {
        //        int pos = 0;
        //        _analogData[pos++] = (Int16)DateTime.Now.Year;
        //        _analogData[pos++] = (Int16)DateTime.Now.Month;
        //        _analogData[pos++] = (Int16)DateTime.Now.Day;
        //        _analogData[pos++] = (Int16)DateTime.Now.Hour;
        //        _analogData[pos++] = (Int16)DateTime.Now.Minute;
        //        _analogData[pos++] = (Int16)DateTime.Now.Second;
        //        _analogData[pos++] = (Int16)DateTime.Now.Millisecond;
        //        _writer.WriteIntAnalogData(_analogData);
        //    }



        //    _writer.Close();
        //}

        
    }
}
