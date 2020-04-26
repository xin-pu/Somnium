using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;


namespace Somnium.Tests.OpenCVTest
{
    [TestClass]
    public class OpenCvTest
    {

        public List<byte> GetArrayStreamData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allData = new List<byte>();
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.ForEach(line => { allData.AddRange(line.ToCharArray().Select(a =>
            {
                var data= int.Parse(a.ToString());
                return data == 1 ? (byte) 0 : (byte) 255;
            })); });
  
            return allData;
        }

        [TestMethod]
        public void TestRead()
        {
            var mat1 = new Mat("D:\\test.jpg", ImreadModes.Grayscale);
            using (new Window("", mat1))
            {
                Cv2.WaitKey();
            }

        }

        [TestMethod]
        public void TestThreshold()
        {
            var mat1 = new Mat("D:\\test.jpg", ImreadModes.ReducedGrayscale2);
            var mat2 = new Mat();
            Cv2.Threshold(mat1, mat2, 0, 255, ThresholdTypes.Binary);
            using (new Window("", mat2))
            {
                Cv2.WaitKey();
            }

        }

        [TestMethod]
        public void TesResize()
        {
            var filename = $@"D:\Document Code\Code Somnium\Somnium\datas\trainingDigits\0_0.txt";
            var data = GetArrayStreamData(filename).ToArray();
            var mat1 = new Mat(32, 32, MatType.CV_8UC1, data);
            var mat2 = new Mat();
            Cv2.Resize(mat1, mat2, new Size(16, 16));
            using (new Window("", mat2))
            {
                Cv2.WaitKey();
            }

            mat2.GetArray(out byte[] data2);
        }


    }
}
