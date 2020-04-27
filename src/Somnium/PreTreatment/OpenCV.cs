using OpenCvSharp;

namespace Somnium.PreTreatment
{
    public class OpenCv
    {
        public static Mat GetMat(string fileName, ImreadModes imreadModes = ImreadModes.Grayscale)
        {
            return new Mat(fileName, imreadModes);
        }

        public static Mat GetMat(int rows, int columns, byte[] array, MatType matType)
        {
            return new Mat(rows, columns, MatType.CV_8S, array);
        }

        public static Mat Resize(InputArray inPutArray, Size size)
        {
            var outPutArray = new Mat();
            Cv2.Resize(inPutArray, outPutArray, size);
            return outPutArray;
        }

    }
}
