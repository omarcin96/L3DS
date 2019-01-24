using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Cloo;
using System.IO;
using System.Diagnostics;
using System.Numerics;

namespace L3DS.Engine.Processing
{
    public class Gaussian
    {
        public static float[,] Calculate1DSampleKernel(float deviation, int size)
        {
            float[,] ret = new float[size, 1];
            float sum = 0;
            int half = size / 2;
            for (int i = 0; i < size; i++)
            {
                ret[i, 0] = 1 / ((float)Math.Sqrt(2 * Math.PI) * deviation) * (float)Math.Exp(-(i - half) * (i - half) / (2 * deviation * deviation));
                sum += ret[i, 0];
            }
            return ret;
        }
        public static float[,] Calculate1DSampleKernel(float deviation)
        {
            int size = (int)Math.Ceiling(deviation * 3) * 2 + 1;
            return Calculate1DSampleKernel(deviation, size);
        }
        public static float[,] CalculateNormalized1DSampleKernel(float deviation)
        {
            return NormalizeMatrix(Calculate1DSampleKernel(deviation));
        }
        public static float[,] NormalizeMatrix(float[,] matrix)
        {
            float[,] ret = new float[matrix.GetLength(0), matrix.GetLength(1)];
            float sum = 0;
            for (int i = 0; i < ret.GetLength(0); i++)
            {
                for (int j = 0; j < ret.GetLength(1); j++)
                    sum += matrix[i, j];
            }
            if (sum != 0)
            {
                for (int i = 0; i < ret.GetLength(0); i++)
                {
                    for (int j = 0; j < ret.GetLength(1); j++)
                        ret[i, j] = matrix[i, j] / sum;
                }
            }
            return ret;
        }
        public static float[,] GaussianConvolution(float[,] matrix, float deviation)
        {
            float[,] kernel = CalculateNormalized1DSampleKernel(deviation);
            float[,] res1 = new float[matrix.GetLength(0), matrix.GetLength(1)];
            float[,] res2 = new float[matrix.GetLength(0), matrix.GetLength(1)];
            //x-direction
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    res1[i, j] = processPoint(matrix, i, j, kernel, 0);
            }
            //y-direction
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    res2[i, j] = processPoint(res1, i, j, kernel, 1);
            }
            return res2;
        }
        private static float processPoint(float[,] matrix, int x, int y, float[,] kernel, int direction)
        {
            float res = 0;
            int half = kernel.GetLength(0) / 2;
            for (int i = 0; i < kernel.GetLength(0); i++)
            {
                int cox = direction == 0 ? x + i - half : x;
                int coy = direction == 1 ? y + i - half : y;
                if (cox >= 0 && cox < matrix.GetLength(0) && coy >= 0 && coy < matrix.GetLength(1))
                {
                    res += matrix[cox, coy] * kernel[i, 0];
                }
            }
            return res;
        }

    }



    //https://stackoverflow.com/questions/32255440/how-can-i-get-and-set-pixel-values-of-an-emgucv-mat-image?noredirect=1&lq=1
    public static class MatExtension
    {
        public static dynamic GetValue(this Mat mat, int row, int col)
        {
            var value = CreateElement(mat.Depth);
            Marshal.Copy(mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, value, 0, 1);
            return value[0];
        }

        public static void SetValue(this Mat mat, int row, int col, dynamic value)
        {
            var target = CreateElement(mat.Depth, value);
            Marshal.Copy(target, 0, mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, 1);
        }
        private static dynamic CreateElement(DepthType depthType, dynamic value)
        {
            var element = CreateElement(depthType);
            element[0] = value;
            return element;
        }

        private static dynamic CreateElement(DepthType depthType)
        {
            if (depthType == DepthType.Cv8S)
            {
                return new sbyte[1];
            }
            if (depthType == DepthType.Cv8U)
            {
                return new byte[1];
            }
            if (depthType == DepthType.Cv16S)
            {
                return new short[1];
            }
            if (depthType == DepthType.Cv16U)
            {
                return new ushort[1];
            }
            if (depthType == DepthType.Cv32S)
            {
                return new int[1];
            }
            if (depthType == DepthType.Cv32F)
            {
                return new float[1];
            }
            if (depthType == DepthType.Cv64F)
            {
                return new double[1];
            }
            return new float[1];
        }
    }

    public sealed class Laser
    {
        // Stałe.
        private static readonly string THRESHOLD_KEY = "threshold";
        private static readonly string SIGMA_KEY = "sigma";
        private static readonly string WINDOW_KEY = "window";
        private static readonly string LASER_ANGLE_KEY = "laserAngle";
        private static readonly string LASER_DELAY_MS = "laserDelay";
        private static readonly string OFFSET_Z_KEY = "offsetZ";
        private static readonly string LASER_CL_PATH = "Kernels/laser.cl";

        // Singleton:
        private static Laser m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        // Static.
        private Matrix<Int32> weightMatrix = null;
        private ComputeContext clooCtx = null;
        private ComputeProgram ctxLaserCL = null;
        private ComputeKernel ctxMinMaxKernel = null;
        private ComputeKernel ctxMaskImageKernel;
        private ComputeKernel ctxCenterMassKernel;
        private ComputeKernel ctxTransform3DKernel;

        // Globals:
        Mat leftLaserSubstracted = new Mat();
        Mat rightLaserSubstracted = new Mat();
        Mat blurLeftLaserThreshold = new Mat();
        Mat blurRightLaserThreshold = new Mat();
        Mat imageSubstracted = new Mat();
        Mat imageThreshold = new Mat();
        Mat imageBlur = new Mat();
        Mat imageBlurThreshold = new Mat();
        Mat binarizedLeftLaser = new Mat();
        Mat binarizedRightLaser = new Mat();
        Mat maskLeftLaser = null;
        Mat maskRightLaser = null;
        Mat imageR = new Mat();
        Mat laserR = new Mat();
        ComputeCommandQueue queue = null;
        ComputeEventList events = null;

        // Handlers:
        #region Handlers
        public Mat Left_Substract_Image
        {
            get
            {
                return leftLaserSubstracted;
            }
        }

        public Mat Left_Mask_Image
        {
            get
            {
                return maskLeftLaser;
            }
        }

        public Mat Right_Mask_Image
        {
            get
            {
                return maskRightLaser;
            }
        }

        public Mat Right_Substract_Image
        {
            get
            {
                return rightLaserSubstracted;
            }
        }

        public Mat Threshold_Image
        {
            get
            {
                return imageThreshold;
            }
        }

        public Matrix<Int32> WeightMatrix
        {
            get
            {
                return weightMatrix;
            }
        }

        public Mat Blur_Threshold_Image
        {
            get
            {
                return imageBlurThreshold;
            }
        }

        public Mat Left_Binarized_Image
        {
            get
            {
                return binarizedLeftLaser;
            }
        }

        public Mat Right_Binarized_Image
        {
            get
            {
                return binarizedRightLaser;
            }
        }

        // Pobieranie instancji klasy.
        public static Laser Instance
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new Laser();
                    }

                    return m_oInstance;
                }
            }
        }

        // Wartość progowania.
        private int _threshold;
        public int Threshold
        {
            get
            {
                return _threshold;
            }

            set
            {
                if (value >= 0 && value <= 255)
                {
                    _threshold = value;
                }
            }
        }


        // Wartość progowania.
        private float _offsetz;
        public float OffsetZ
        {
            get
            {
                return _offsetz;
            }

            set
            {
                _offsetz = value;
            }
        }

        // kat lasera.
        private int _laserAngle;
        public int LaserAngle
        {
            get
            {
                return _laserAngle;
            }

            set
            {
                if (value >= 0 && value <= 90)
                {
                    _laserAngle = value;
                }
            }
        }

        // Odchylenie.
        private int _sigma;
        public int Sigma
        {
            get
            {
                return _sigma;
            }

            set
            {
                _sigma = value;
            }
        }

        // Korekcja.
        private int _window;
        public int Window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        // Zwloka odczytu klatki.
        private int _DelayLaserMs;
        public int DelayLaserMS
        {
            get
            {
                return _DelayLaserMs;
            }

            set
            {
                _DelayLaserMs = value;
            }
        }
        #endregion

        private void InitClooApi()
        {
            try
            {
                CvInvoke.UseOpenCL = true;

                // pick first platform
                ComputePlatform platform = ComputePlatform.Platforms[1];

                // create context with all gpu devices
                clooCtx = new ComputeContext(ComputeDeviceTypes.Gpu,
                new ComputeContextPropertyList(platform), null, IntPtr.Zero);

                // load opencl source
                StreamReader streamReader = new StreamReader(LASER_CL_PATH);
                string clSource = streamReader.ReadToEnd();
                streamReader.Close();

                // build program.
                ctxLaserCL = new ComputeProgram(clooCtx, clSource);

                // compile opencl source
                ctxLaserCL.Build(null, null, null, IntPtr.Zero);

                // load chosen kernel from program
                ctxMinMaxKernel = ctxLaserCL.CreateKernel("minMaxValues");
                ctxMaskImageKernel = ctxLaserCL.CreateKernel("maskImage");
                ctxCenterMassKernel = ctxLaserCL.CreateKernel("centerMass");
                ctxTransform3DKernel = ctxLaserCL.CreateKernel("transformPixelsTo3D");

                // create a command queue with first gpu found
                queue = new ComputeCommandQueue(clooCtx,
                clooCtx.Devices[0], ComputeCommandQueueFlags.None);

                // execute kernel
                events = new ComputeEventList();

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ctxLaserCL.GetBuildLog(clooCtx.Devices[0]));
            }
        }

        private Laser()
        {
            LoadConfiguration();
            InitClooApi();
        }

        // Pobierz wartość z pliku konfiguracyjnego.
        private static string GetConfigValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public void GenerateWeightMatrix(int width, int height)
        {

            if (weightMatrix != null)
            {
                if (weightMatrix.Width == width && weightMatrix.Height == height)
                    return;

                weightMatrix.Dispose();
            }

            weightMatrix = new Matrix<Int32>(height, width);

            // utworz macierz z wagami dla kolumn {0,1,2... width-1} oraz dla wierszy {0,1,2,... , height-1}
            for (int y = 0; y < weightMatrix.Cols; y++)
            {
                for (int x = 0; x < weightMatrix.Rows; x++)
                {
                    weightMatrix.Data[x, y] = x;
                }
            }
        }

        // Zapisz konfiguracje.
        private static void SetConfigValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            configFile.AppSettings.Settings[key].Value = value;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        // Zapisywanie konfiguracji.
        public void SaveConfiguration()
        {
            SetConfigValue(THRESHOLD_KEY, String.Format("{0}", Threshold));
            SetConfigValue(WINDOW_KEY, String.Format("{0}", Window));
            SetConfigValue(SIGMA_KEY, String.Format("{0}", Sigma));
            SetConfigValue(LASER_ANGLE_KEY, String.Format("{0}", LaserAngle));
            SetConfigValue(LASER_DELAY_MS, String.Format("{0}", DelayLaserMS));
            SetConfigValue(OFFSET_Z_KEY, String.Format("{0}", OffsetZ));
        }

        // Dzieli ramke na dwie czesci na wysokosc.
        public void SplitFrameR(Mat frame, out Mat a, out Mat b)
        {
            a = null;
            b = null;

            try
            {
                if (frame == null)
                    throw new InvalidOperationException("frame == null");

                int index = 0;

                if (frame.NumberOfChannels == 3)
                {
                    index = 2;
                }

                Mat[] splits = frame.Split();


                a = new Mat(splits[index], new System.Drawing.Rectangle(0, 0, (int)frame.Width, (int)frame.Height / 2));
                b = new Mat(splits[index], new System.Drawing.Rectangle(0, (int)(frame.Height / 2), (int)frame.Width, (int)frame.Height / 2));

                foreach(var item in splits)
                {
                    if (item != null)
                        item.Dispose();
                }

            } catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static class MinMaxCL
        {
            public static byte[] frameData = null;
            public static int[] maxBuffer = null;
            public static int[] minBuffer = null;
            public static ComputeBuffer<Byte> frameBuffer = null;
            public static ComputeBuffer<int> maxBufferCB = null;
            public static ComputeBuffer<int> minBufferCB = null;
            public static GCHandle frameDataHandler;
            public static int Length = 0;

            public static void UpdateArguments(Mat frame, ComputeContext ctx, ComputeKernel k, int WindowValue)
            {

                if(frame.Width * frame.Height != Length)
                {
                    // alokuj pamiec.
                    maxBuffer = new int[frame.Cols];
                    minBuffer = new int[frame.Cols];

                    minBufferCB = new ComputeBuffer<int>(ctx, ComputeMemoryFlags.WriteOnly, minBuffer.Length);
                    maxBufferCB = new ComputeBuffer<int>(ctx, ComputeMemoryFlags.WriteOnly, maxBuffer.Length);

                    Length = frame.Width * frame.Height;
                }

                if (frameDataHandler.IsAllocated)
                    frameDataHandler.Free();

                if (frameBuffer != null)
                    frameBuffer.Dispose();

                frameData = new byte[frame.Width * frame.Height];
                frameDataHandler = GCHandle.Alloc(frameData, GCHandleType.Pinned);

                // ustaw parametry alokacji pamieci.
                frameBuffer = new ComputeBuffer<byte>(ctx,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, frame.Width * frame.Height, frameDataHandler.AddrOfPinnedObject());


                frame.CopyTo(frameData);
                k.SetMemoryArgument(0, frameBuffer);
                k.SetValueArgument<int>(1, frame.Rows);
                k.SetValueArgument<int>(2, frame.Cols);
                k.SetMemoryArgument(3, maxBufferCB);
                k.SetMemoryArgument(4, minBufferCB);
                k.SetValueArgument<int>(5, WindowValue);
            }
        }

        private static class MaskImageCL
        {
            public static byte[] frameData = null;
            public static int[] maxBuffer = null;
            public static int[] minBuffer = null;
            public static ComputeBuffer<Byte> frameBuffer = null;
            public static ComputeBuffer<int> maxBufferCB = null;
            public static ComputeBuffer<int> minBufferCB = null;
            public static ComputeBuffer<byte> frameDataOutBuffer = null;
            public static GCHandle frameDataHandler;
            public static GCHandle minHandler;
            public static GCHandle maxHandler;
            public static int Length = 0;

            public static void UpdateArguments(Mat frame, int[] max, int[] min, ComputeContext ctx, ComputeKernel k)
            {
                if (frame.Width * frame.Height != Length)
                {
                    frameDataOutBuffer = new ComputeBuffer<byte>(ctx, ComputeMemoryFlags.WriteOnly, frame.Width * frame.Height);

                    Length = frame.Width * frame.Height;
                }

                if(frameBuffer != null)
                {
                    frameBuffer.Dispose();
                    frameBuffer = null;
                }

                if(minBufferCB != null)
                {
                    minBufferCB.Dispose();
                    minBufferCB = null;
                }

                if(maxBufferCB != null)
                {
                    maxBufferCB.Dispose();
                    maxBufferCB = null;
                }

                if (frameDataHandler.IsAllocated)
                    frameDataHandler.Free();
                if (minHandler.IsAllocated)
                    minHandler.Free();
                if (maxHandler.IsAllocated)
                    maxHandler.Free();

                frameData = new byte[frame.Width * frame.Height];
                frameDataHandler = GCHandle.Alloc(frameData, GCHandleType.Pinned);

                // ustaw parametry alokacji pamieci.
                frameBuffer = new ComputeBuffer<byte>(ctx,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, frame.Width * frame.Height, frameDataHandler.AddrOfPinnedObject());

                // alokuj pamiec.
                maxBuffer = new int[frame.Cols];
                maxHandler = GCHandle.Alloc(maxBuffer, GCHandleType.Pinned);

                minBuffer = new int[frame.Cols];
                minHandler = GCHandle.Alloc(minBuffer, GCHandleType.Pinned);

                minBufferCB = new ComputeBuffer<int>(ctx, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, minBuffer.Length, minHandler.AddrOfPinnedObject());
                maxBufferCB = new ComputeBuffer<int>(ctx, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, maxBuffer.Length, maxHandler.AddrOfPinnedObject());

                frame.CopyTo(frameData);
                max.CopyTo(maxBuffer, 0);
                min.CopyTo(minBuffer, 0);

                k.SetMemoryArgument(0, frameBuffer);
                k.SetMemoryArgument(1, maxBufferCB);
                k.SetMemoryArgument(2, minBufferCB);
                k.SetValueArgument<int>(3, frame.Rows);
                k.SetValueArgument<int>(4, frame.Cols);
                k.SetMemoryArgument(5, frameDataOutBuffer);
            }
        }

        private static class CenterMassCL
        {
            public static byte[] frameData = null;
            public static int[] mulData = null;
            public static int[] maxBuffer = null;
            public static int[] minBuffer = null;
            public static ComputeBuffer<Byte> frameBuffer = null;
            public static ComputeBuffer<int> mulBuffer = null;
            public static ComputeBuffer<float> uCoordCB = null;
            public static ComputeBuffer<float> vCoordCB = null;
            public static ComputeBuffer<float> sCoordCB = null;
            public static ComputeBuffer<int> cCB = null;
            public static GCHandle frameDataHandler;
            public static GCHandle mulFrameDataHandler;

            public static int Length = 0;

            public static void UpdateArguments(Mat frame, Mat mulFrame, ComputeContext ctx, ComputeKernel k)
            {
                if (frame.Width * frame.Height != Length)
                {
                    uCoordCB = new ComputeBuffer<float>(ctx,
                        ComputeMemoryFlags.WriteOnly, frame.Cols);

                    vCoordCB = new ComputeBuffer<float>(ctx,
                        ComputeMemoryFlags.WriteOnly, frame.Cols);

                  //  cCB = new ComputeBuffer<int>(ctx,
                  //      ComputeMemoryFlags.WriteOnly, 1);

                    sCoordCB = new ComputeBuffer<float>(ctx, ComputeMemoryFlags.WriteOnly, frame.Cols);

                    Length = frame.Width * frame.Height;
                }

                if (frameBuffer != null)
                {
                    frameBuffer.Dispose();
                    frameBuffer = null;
                }


                if (mulBuffer != null)
                {
                    mulBuffer.Dispose();
                    mulBuffer = null;
                }


                if (frameDataHandler.IsAllocated)
                    frameDataHandler.Free();

                if (mulFrameDataHandler.IsAllocated)
                    mulFrameDataHandler.Free();

                frameData = new byte[frame.Width * frame.Height];
                frameDataHandler = GCHandle.Alloc(frameData, GCHandleType.Pinned);

                mulData = new int[frame.Width * frame.Height];
                mulFrameDataHandler = GCHandle.Alloc(mulData, GCHandleType.Pinned);


                // ustaw parametry alokacji pamieci.
                frameBuffer = new ComputeBuffer<byte>(ctx,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, frame.Width * frame.Height, frameDataHandler.AddrOfPinnedObject());

                mulBuffer = new ComputeBuffer<int>(ctx,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, mulData.Length, mulFrameDataHandler.AddrOfPinnedObject());

                mulFrame.CopyTo(mulData);
                frame.CopyTo(frameData);

                k.SetMemoryArgument(0, frameBuffer);
                k.SetMemoryArgument(1, mulBuffer);
                k.SetValueArgument<int>(2, frame.Rows);
                k.SetValueArgument<int>(3, frame.Cols);
                k.SetMemoryArgument(4, uCoordCB);
                k.SetMemoryArgument(5, vCoordCB);
                k.SetMemoryArgument(6, sCoordCB);
            }
        }

        // Wartosci min, max po OpenCL.
        public void GetMinMaxValuesCL(Mat frame, out int[] maxValues, out int[] minValues, int windowValue)
        {
            maxValues = null;
            minValues = null;


            try
            {
                MinMaxCL.UpdateArguments(frame, clooCtx, ctxMinMaxKernel, windowValue);

                // execute kernel
                queue.Execute(ctxMinMaxKernel, null, new long[] { frame.Cols }, null, null);

                // max Values.
                maxValues = new int[frame.Cols];
                GCHandle maxHandle = GCHandle.Alloc(maxValues, GCHandleType.Pinned);
                queue.Read(MinMaxCL.maxBufferCB, true, 0, maxValues.Length, maxHandle.AddrOfPinnedObject(), null);

                // min Values.
                minValues = new int[frame.Cols];
                GCHandle minHandle = GCHandle.Alloc(minValues, GCHandleType.Pinned);
                queue.Read(MinMaxCL.minBufferCB, true, 0, minValues.Length, minHandle.AddrOfPinnedObject(), null);

                // end opencl compute.
                queue.Finish();

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Wartości min,max dla danej ramki.
        // DZIAŁA.
        public void GetMinMaxValues(Mat frame, out int[] maxValues, out int[] minValues, int windowValue)
        {
            maxValues = null;
            minValues = null;
            try
            {
                if (frame == null)
                    throw new InvalidOperationException("frame == null");

                int value = 0;
                maxValues = new int[frame.Cols];
                minValues = new int[frame.Cols];

                for (int y = 0; y < frame.Cols; y++)
                {
                    for (int x = 0; x < frame.Rows; x++)
                    {
                        if(frame.GetValue(x,y) > (byte)value)
                        {
                            value = x;
                        }
                    }

                    maxValues[y] = value + windowValue;
                    minValues[y] = value - windowValue;
                }

            } catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static class Transform3DPixelCL
        {
            public static float[] fBuffer = null;
            public static float[] vBuffer = null;
            public static ComputeBuffer<float> fBufferCB = null;
            public static ComputeBuffer<float> vBufferCB = null;
            public static ComputeBuffer<float> xBufferCB = null;
            public static ComputeBuffer<float> yBufferCB = null;
            public static ComputeBuffer<float> zBufferCB = null;
            public static GCHandle fDataHandler;
            public static GCHandle vDataHandler;
            public static int Length = 0;

            public static void UpdateArguments(float[] f, float[] v, float kx, float ky, float kz, float angleTan, float xOffset, ComputeContext ctx, ComputeKernel k)
            {
                if (f.Length <= 0)
                    return;

                if (f.Length != Length)
                {
                    if(xBufferCB != null)
                    {
                        xBufferCB.Dispose();
                        xBufferCB = null;
                    }

                    if (yBufferCB != null)
                    {
                        yBufferCB.Dispose();
                        yBufferCB = null;
                    }

                    if (zBufferCB != null)
                    {
                        zBufferCB.Dispose();
                        zBufferCB = null;
                    }

                    // Write only.
                    xBufferCB = new ComputeBuffer<float>(ctx, ComputeMemoryFlags.WriteOnly, f.Length);
                    yBufferCB = new ComputeBuffer<float>(ctx, ComputeMemoryFlags.WriteOnly, f.Length);
                    zBufferCB = new ComputeBuffer<float>(ctx, ComputeMemoryFlags.WriteOnly, f.Length);

                    Length = f.Length;
                }

                // Set values.
                if (fBufferCB != null)
                {
                    fBufferCB.Dispose();
                    fBufferCB = null;
                }

                if (vBufferCB != null)
                {
                    vBufferCB.Dispose();
                    vBufferCB = null;
                }

                if (fDataHandler.IsAllocated)
                    fDataHandler.Free();
                if (vDataHandler.IsAllocated)
                    vDataHandler.Free();

                fBuffer = new float[f.Length];
                fDataHandler = GCHandle.Alloc(fBuffer, GCHandleType.Pinned);

                vBuffer = new float[v.Length];
                vDataHandler = GCHandle.Alloc(vBuffer, GCHandleType.Pinned);

                // ustaw parametry alokacji pamieci.
                fBufferCB = new ComputeBuffer<float>(ctx,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, f.Length, fDataHandler.AddrOfPinnedObject());

                vBufferCB = new ComputeBuffer<float>(ctx,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, v.Length, vDataHandler.AddrOfPinnedObject());

                // Copy data.
                f.CopyTo(fBuffer, 0);
                v.CopyTo(vBuffer, 0);

                k.SetMemoryArgument(0, fBufferCB);
                k.SetMemoryArgument(1, vBufferCB);
                k.SetValueArgument<float>(2, kx);
                k.SetValueArgument<float>(3, ky);
                k.SetValueArgument<float>(4, kz);
                k.SetValueArgument<float>(5, angleTan);
                k.SetValueArgument<float>(6, xOffset);
                k.SetMemoryArgument(7, xBufferCB);
                k.SetMemoryArgument(8, yBufferCB);
                k.SetMemoryArgument(9, zBufferCB);
            }
        }

        // Funkcja tworzy maskę w postaci prostokąta otaczającego główną linię i wycina resztę zakłóceń.
        // DZIAŁA
        public void ValidWindowMask(Mat thresholdImage, Mat imageDiff, out Mat binaryImage, out Mat mask)
        {
            binaryImage = null;
            mask = null;

            try
            {
                if (thresholdImage == null)
                    throw new InvalidOperationException("BinaryImage == null");

                // granice przedziałów pikseli.
                int[] minValuesCL, maxValuesCL;

                // Przedział binaryzacji pikseli.
                GetMinMaxValuesCL(thresholdImage, out maxValuesCL, out minValuesCL, Window);

                // tworzenie maski.
                Matrix<Byte> mask_cl;
                GenerateImageMaskCL(thresholdImage, minValuesCL, maxValuesCL, out mask_cl);

                // wycinanie niepotrzebnych śmieci.
                binaryImage = new Mat();
                CvInvoke.BitwiseAnd(imageDiff, mask_cl, binaryImage);
                mask = mask_cl.Mat.Clone();

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateImageMaskCL(Mat frame, int[] minValues, int[] maxValues, out Matrix<byte> mask_cl)
        {
            mask_cl = null;

            try
            {
                MaskImageCL.UpdateArguments(frame, maxValues, minValues, clooCtx, ctxMaskImageKernel);

                // execute kernel
                queue.Execute(ctxMaskImageKernel, null, new long[] { frame.Cols }, null, null);

                // mask image read.
                byte[] mask_array = new byte[frame.Cols * frame.Rows];
                GCHandle maskHandle = GCHandle.Alloc(mask_array, GCHandleType.Pinned);
                queue.Read(MaskImageCL.frameDataOutBuffer, true, 0, mask_array.Length, maskHandle.AddrOfPinnedObject(), null);

                // end opencl compute.
                queue.Finish();

                mask_cl = new Matrix<byte>(frame.Rows, frame.Cols, maskHandle.AddrOfPinnedObject());

                maskHandle.Free();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ImageToCenterOfMassPointsCL(Mat image,Mat mulImage, out float[] u, out float[] v, out float[] s)
        {
            u = null;
            s = null;
            v = null;

            try
            {
                CenterMassCL.UpdateArguments(image, mulImage, clooCtx, ctxCenterMassKernel);

                // execute kernel
                queue.Execute(ctxCenterMassKernel, null, new long[] { image.Cols }, null, null);

                // get u coord.
                int LengthUV = image.Cols;

                u = new float[LengthUV];
                GCHandle uHandler = GCHandle.Alloc(u, GCHandleType.Pinned);
                queue.Read(CenterMassCL.uCoordCB, true, 0, u.Length, uHandler.AddrOfPinnedObject(), null);

                // get v coord.
                v = new float[LengthUV];
                GCHandle vHandler = GCHandle.Alloc(v, GCHandleType.Pinned);
                queue.Read(CenterMassCL.vCoordCB, true, 0, v.Length, vHandler.AddrOfPinnedObject(), null);

                // sum array.
                s = new float[image.Cols];
                GCHandle sHandler = GCHandle.Alloc(s, GCHandleType.Pinned);
                queue.Read(CenterMassCL.sCoordCB, true, 0, s.Length, sHandler.AddrOfPinnedObject(), null);


                queue.Finish();
                uHandler.Free();
                vHandler.Free();
                sHandler.Free();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Tworzy segmenty z danych.
        public void DetectSegments(float[] sum, float[] u, float[] v, out float[] f, out float[] v2)
        {
            Matrix<float> tempBlurBuffer = null;
            Mat outBlurBuffer = null;

            List<float> temp = new List<float>();
            List<float> temp2 = new List<float>();
            List<float> slices = new List<float>();
            List<float> slices2 = new List<float>();

            for (int i = 0; i < sum.Length; i++)
            {
                if(sum[i] > 0)
                {
                    temp.Add(u[i]);
                    temp2.Add(v[i]);
                }
                else
                {
                    if(temp.Count > 0)
                    {
                        // gaussian blur matrices.
                        tempBlurBuffer = new Matrix<float>(temp.ToArray());
                        outBlurBuffer = new Mat();

                        CvInvoke.GaussianBlur(tempBlurBuffer, outBlurBuffer, new Size(3, 3), Sigma, Sigma);

                        // convert matrix to float[].
                        float[] buffer = new float[temp.Count];
                        outBlurBuffer.CopyTo(buffer);
                        slices.AddRange(buffer);

                        slices2.AddRange(temp2);

                        temp.Clear();
                        temp2.Clear();
                    }
                }
            }

            if(temp.Count > 0)
            {
                // gaussian blur matrices.
                tempBlurBuffer = new Matrix<float>(temp.ToArray());
                outBlurBuffer = new Mat();

                CvInvoke.GaussianBlur(tempBlurBuffer, outBlurBuffer, new Size(3, 3), Sigma, Sigma);

                // convert matrix to float[].
                float[] buffer = new float[temp.Count];
                outBlurBuffer.CopyTo(buffer);
                slices.AddRange(buffer);
                slices2.AddRange(temp2);

                temp.Clear();
                temp2.Clear();
                outBlurBuffer.Dispose();
                tempBlurBuffer.Dispose();
            }

            f = slices.ToArray();
            v2 = slices2.ToArray();
        }

        

        public void CalculateCenterOfMassCL(Mat binaryImage, out float[] f, out float[] v)
        {
            f = v = null;

            try
            {
                // exception handler.
                if (binaryImage == null)
                    throw new InvalidOperationException("binaryImage == null");

                // pobierz szerokosc wysokosc ramki zdjecia.
                int height = binaryImage.Height;
                int width = binaryImage.Width;

                // Wymnóż aby uzyskać tablicę gotową do sumowania.
                Matrix<Int32> mul = new Matrix<int>(binaryImage.Rows, binaryImage.Cols, 1);

                CvInvoke.Multiply(WeightMatrix, binaryImage, mul, 1, DepthType.Cv32S);

                // Oblicz pozycję punktów.
                float[] u, s;
                ImageToCenterOfMassPointsCL(binaryImage, mul.Mat, out u, out v, out s);

                // Oblicz segmenty.
                DetectSegments(s, u, v, out f, out v);

                if(mul != null)
                {
                    mul.Dispose();
                    mul = null;
                }

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ShowImage(string id, Mat splitR, Mat binarized, Mat mask, float[] u, float[] v)
        {
            CvInvoke.Imshow("SplitR" + id, splitR);
            CvInvoke.Imshow("Binarized" + id, binarized);
            CvInvoke.Imshow("Mask" + id, mask);

            MCvScalar[] scalars = { new MCvScalar(255, 255, 255), new MCvScalar(0, 0, 0), new MCvScalar(0, 255, 0), new MCvScalar(255, 0, 255), new MCvScalar(255, 255, 255) };
            int idx = 0;

            int k = 0;

            for(int i = 0; i < u.Length; i++)
            {
                    CvInvoke.Circle(binarized, new Point((int)v[i], (int)u[i]), 1, scalars[idx]);

            }

            CvInvoke.Imshow("Points" + id, binarized);

            CvInvoke.WaitKey(2000);
        }

        public void TransformPixelsTo3DPointCL(float[] f, float [] v, float kx, float ky, float kz, float xOffset, out float[] x, out float[] y, out float[] z)
        {

            x = new float[1];
            y = new float[1];
            z = new float[1];

            if (v.Length <= 0)
                return;

            try
            {
                float laserAngle = (float)Math.Tan((Math.PI * LaserAngle) / 180.0);

                Transform3DPixelCL.UpdateArguments(f, v, kx, ky, kz, laserAngle, xOffset, clooCtx, ctxTransform3DKernel);

                // execute kernel
                queue.Execute(ctxTransform3DKernel, null, new long[] { v.Length }, null, null);

                // get x,y,z coord.
                x = new float[f.Length];
                GCHandle xHandler = GCHandle.Alloc(x, GCHandleType.Pinned);
                queue.Read(Transform3DPixelCL.xBufferCB, true, 0, x.Length, xHandler.AddrOfPinnedObject(), null);

                y = new float[f.Length];
                GCHandle yHandler = GCHandle.Alloc(y, GCHandleType.Pinned);
                queue.Read(Transform3DPixelCL.yBufferCB, true, 0, y.Length, yHandler.AddrOfPinnedObject(), null);

                z = new float[f.Length];
                GCHandle zHandler = GCHandle.Alloc(z, GCHandleType.Pinned);
                queue.Read(Transform3DPixelCL.zBufferCB, true, 0, z.Length, zHandler.AddrOfPinnedObject(), null);

                queue.Finish();
                xHandler.Free();
                yHandler.Free();
                zHandler.Free();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ProcessingImageCL(Mat lasers, Mat backgrounds, out float[] lf, out float [] lv, out float[] rf, out float[] rv)
        {
            lf = lv = rf = rv = null;

            if(imageR != null)
            {
                imageR.Dispose();
                imageR = null;
            }

            if (laserR != null)
            {
                laserR.Dispose();
                laserR = null;
            }
            
            if (leftLaserSubstracted != null)
            {
                leftLaserSubstracted.Dispose();
                leftLaserSubstracted = null;
            }

            if (rightLaserSubstracted != null)
            {
                rightLaserSubstracted.Dispose();
                rightLaserSubstracted = null;
            }

            if (binarizedLeftLaser != null)
            {
                binarizedLeftLaser.Dispose();
                binarizedLeftLaser = null;
            }

            if (binarizedRightLaser != null)
            {
                binarizedRightLaser.Dispose();
                binarizedRightLaser = null;
            }

            if (maskLeftLaser != null)
            {
                maskLeftLaser.Dispose();
                maskLeftLaser = null;
            }

            if (maskRightLaser != null)
            {
                maskRightLaser.Dispose();
                maskRightLaser = null;
            }


            // Pobierz tylko czerwony kolor.
         //   Mat[] bg_splits = backgrounds.Split();
            Mat[] laser_splits = lasers.Split();

          //  imageR = bg_splits[2];
            laserR = laser_splits[2];

            // Odejmowanie obrazow od siebie.

      //      CvInvoke.Subtract(laserR, imageR, imageSubstracted);
            SplitFrameR(laserR, out leftLaserSubstracted, out rightLaserSubstracted);

            // Progowanie.
            CvInvoke.Threshold(laserR, imageThreshold, Threshold, 255, ThresholdType.ToZero);

            // Rozmycie ostrych krawedzi, nieraz pomaga usunac reszte zaklocen.
            CvInvoke.Blur(imageThreshold, imageBlur, new Size(5, 5), new Point(-1, -1));

            // Ponowne progrowanie.
            CvInvoke.Threshold(imageBlur, imageBlurThreshold, Threshold, 255, ThresholdType.ToZero);

            Mat redChannelL, redChannelR;
            redChannelL = redChannelR = null;
            SplitFrameR(imageBlurThreshold, out redChannelL, out redChannelR);

            // wyznaczanie tzw. masek ograniczajacych pole wykrywania wiazki lasera.
            ValidWindowMask(redChannelL, leftLaserSubstracted, out binarizedLeftLaser, out maskLeftLaser);
            ValidWindowMask(redChannelR, rightLaserSubstracted, out binarizedRightLaser, out maskRightLaser);

            // obliczanie srodkow ciezkosci.
            CalculateCenterOfMassCL(binarizedLeftLaser, out lf, out lv);
            CalculateCenterOfMassCL(binarizedRightLaser, out rf, out rv);

            if(redChannelL != null)
            {
                redChannelL.Dispose();
                redChannelL = null;
            }

            if (redChannelR != null)
            {
                redChannelR.Dispose();
                redChannelR = null;
            }

            foreach(var img in laser_splits)
                if(img != null)
                    img.Dispose();

            //foreach (var img in bg_splits)
          //      if (img != null)
             //       img.Dispose();

          //   ShowImage("left", imageBlurThreshold, binarizedLeftLaser, maskLeftLaser, lf, lv);
             //ShowImage("right", redChannelR, binarizedRightLaser, maskRightLaser, rf, rv, rs);

            return true;
        }


        // Ladowanie konfiguracji z pliku.
        public void LoadConfiguration()
        {
            bool isInitialized = false;
            int temp;

            //
            isInitialized = int.TryParse(GetConfigValue(THRESHOLD_KEY), out temp);
            if (isInitialized)
            {
                Threshold = temp;
            }

            //
            isInitialized = int.TryParse(GetConfigValue(WINDOW_KEY), out temp);
            if (isInitialized)
            {
                Window = temp;
            }

            //
            isInitialized = int.TryParse(GetConfigValue(SIGMA_KEY), out temp);
            if (isInitialized)
            {
                Sigma = temp;
            }

            //
            isInitialized = int.TryParse(GetConfigValue(LASER_ANGLE_KEY), out temp);
            if (isInitialized)
            {
                LaserAngle = temp;
            }


            //
            isInitialized = int.TryParse(GetConfigValue(LASER_DELAY_MS), out temp);
            if (isInitialized)
            {
                DelayLaserMS = temp;
            }

            //
            float ftemp;
            isInitialized = float.TryParse(GetConfigValue(OFFSET_Z_KEY), out ftemp);
            if (isInitialized)
            {
                OffsetZ = ftemp;
            }

        }


    }
}
