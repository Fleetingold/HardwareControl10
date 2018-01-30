using SystemMan.Client.Model;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System;
using System.Threading;
using System.Drawing;

namespace HardwareControl.Sensor
{
    /// <summary>
    /// Interaction logic for SensorView.xaml
    /// </summary>
    public partial class SensorView : Window
    {
        EventTransItem _result;
        bool IsScan;

        static private SensorModel _Sensor;
        static private bool Isinit;
        public SensorView()
        {
            this.Title = "指纹识别";
            IsScan = true;
            _result = new EventTransItem();
            _result.result = false;
            InitializeComponent();

            this.Msg.Content = "请按压指纹";

        }
        #region 指纹仪
        /// <summary>
        /// 初始化并启动指纹识别
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <returns></returns>
        private bool SensorON()
        {

            try
            {
                _Sensor = new Sensor.SensorModel();
                if (!_Sensor.InitSensor() || !_Sensor.OpenSensor())
                { return false; }
                Isinit = true;
            }
            catch (Exception)
            {
                Isinit = false;
            }
            return Isinit;
        }
        /// <summary>
        /// 注册指纹
        /// </summary>
        public bool RegFinger()
        {
            bool res = false;
            if (Isinit)
            {
                res = true;
                _Sensor.SensorStates = 1;
                _Sensor.RegisterCount = 0;
            }
            else
            {
                if (SensorON())
                {
                    res = true;
                    _Sensor.SensorStates = 1;
                    _Sensor.RegisterCount = 0;
                }
            }
            return res;
        }
        /// <summary>
        /// 1:n识别
        /// </summary>
        public bool Check1N()
        {
            bool res = false;
            if (Isinit)
            {
                res = true;
                _Sensor.SensorStates = 2;
            }
            else
            {
                if (SensorON())
                {
                    res = true;
                    _Sensor.SensorStates = 2;
                }
            }
            return res;
        }
        /// <summary>
        /// 1:1识别
        /// </summary>
        public bool Check11()
        {
            bool res = false;
            if (Isinit)
            {
                res = true;
                _Sensor.SensorStates = 3;
            }
            else
            {
                if (SensorON())
                {
                    res = true;
                    _Sensor.SensorStates = 3;
                }
            }
            return res;
        }

        #endregion
        /// <summary>
        /// 获取指纹识别结果
        /// </summary>
        /// <param name="type">0注册，1 1：n识别，2 1:1识别</param>
        /// <returns></returns>
        public EventTransItem GetSensorResult(int type=0)
        {
            bool res = false;
            switch (type)
            {
                case (0):
                    { res=RegFinger(); break; }
                case (1):
                    {
                        res= Check1N(); break;
                    }
                case (2):
                    {
                        res= Check11(); break;
                    }
                default: break;
            }
            if(!res)
            {
                Isinit = false;
                MessageBox.Show(this,"指纹仪启动失败,请使用授权密码或重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }else{
                Scan();
            }
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            ShowDialog();
            IsScan = false;
            return _result;
        }
        /// <summary>
        /// 生成后台线程开始扫描
        /// </summary>
        /// <returns></returns>
        public void Scan()
        {
            BackgroundWorker fingerworker = new BackgroundWorker();
            fingerworker.WorkerReportsProgress = true;
            fingerworker.WorkerSupportsCancellation = true;
            fingerworker.DoWork += Fingerworker_DoWork;
            fingerworker.ProgressChanged += Fingerworker_ProgressChanged;
            fingerworker.RunWorkerAsync();

        }

        private void Fingerworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            EventTransItem eti = e.UserState as EventTransItem;
            int res = Convert.ToInt32(eti.result);
            if (res == 100)
            {
                _result = eti;
                _result.result = true;
                IsScan = false;
                this.Close();
            }
            else
            {
                //Bitmap bitmap = eti.Transitem as Bitmap;
                //if (bitmap != null)
                //{
                //    this.Fimage.Source = BitmapToBitmapImage(bitmap);
                //}

                var bmp = new BitmapImage();

                bmp.BeginInit();
                bmp.StreamSource = eti.Transitem as MemoryStream;
                bmp.EndInit();

                this.Fimage.Source = bmp;
                this.Msg.Content = eti.EventType;
            }
        }
        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }
        private void Fingerworker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (IsScan)
            {
                EventTransItem eti = _Sensor.DoCapture();

                if (Convert.ToInt32(eti.result) > 0)
                {
                    worker.ReportProgress(0, eti);
                }

                Thread.Sleep(500);
            }


        }


        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            string pass = this.stationPass.Password;
            if (pass.Length > 0)
            {
                _result.Transitem = pass;
                _result.result = true;
                _result.EventType = "油站密码";
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void stationPass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key==System.Windows.Input.Key.Enter)
            {
                ButtonOk_Click(null,null);
            }
        }

        //private void Window_Closing(object sender, CancelEventArgs e)
        //{
        //    IsScan = false;
        //}
    }
}
