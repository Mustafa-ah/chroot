using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Graphics;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using static Android.Gms.Vision.Detector;
using System.Threading.Tasks;
using Android.Hardware.Camera2;
using Android.Media;
using Scanner.Models;
using Scanner.Repository;
using Android.Gms.Ads;


namespace Scanner.Activities
{
    [Activity(Label = "CameraActivity")]
    public class CameraActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {

        private SurfaceView camView;
        private CameraSource camSource;
        private TextView tv;
        private const int premissionId = 1001;

        ////for flashlight
        //private CameraManager mCameraManager;
        //private String mCameraId;
        //private MediaPlayer mp;

        ////Android.Hardware.Camera2.
        private ImageView flashImage;
        ImageView redBox;
        //private CaptureRequest.Builder mPreviewBuilder;
        //private CameraDevice mCameraDevice;
        //private CameraCaptureSession mPreviewSession;
        bool isFlashOn;
        Card _selectedCard;
        UserSettings _userSetting;
        XmlDb xmlDb;
        CardRepository cardrepo;

        List<Card> _chargedList;

        private bool _isBusy;
       // private bool _isOnthreed;

        string lastchargNumber;
        List<string> chargedNumbers;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_camera);

            xmlDb = new XmlDb(this);
            cardrepo = new CardRepository();
            chargedNumbers = new List<string>();

            camView = FindViewById<SurfaceView>(Resource.Id.MainSurfaceView);
            tv = FindViewById<TextView>(Resource.Id.txt);
            flashImage = FindViewById<ImageView>(Resource.Id.imageViewFlash);
            flashImage.Visibility = ViewStates.Visible;

            TextRecognizer txtRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();

            if (!txtRecognizer.IsOperational)
            {
                //https://stackoverflow.com/questions/38254349/google-mobile-vision-text-api-example
                IntentFilter lowstorageFilter = new IntentFilter(Intent.ActionDeviceStorageLow);
                bool hasLowStorage = RegisterReceiver(null, lowstorageFilter) != null;

                if (hasLowStorage)
                {
                    string ok = GetString(Resource.String.home_ok);
                    var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                    builder.SetIconAttribute
                        (Android.Resource.Attribute.AlertDialogIcon);
                    builder.SetTitle("");
                    builder.SetMessage(GetString(Resource.String.cam_lowStorage));
                    builder.SetPositiveButton(ok, delegate { });
                    builder.Create().Show();
                    //Toast.MakeText(this, " Low Storage, delete some files", ToastLength.Long).Show();
                }
                Toast.MakeText(this, GetString(Resource.String.cam_Detector_not_ready), ToastLength.Short).Show();
                Log.Error("CameraActivity", "Detector not ready");
            }
            else
            {
                camSource = new CameraSource.Builder(ApplicationContext, txtRecognizer)
                    .SetFacing(CameraFacing.Back)
                    .SetRequestedPreviewSize(1280, 1024)
                    .SetRequestedFps(2.0f)
                    .SetAutoFocusEnabled(true)
                    .Build();

                camView.Holder.AddCallback(this);

                txtRecognizer.SetProcessor(this);
            }

            GetChargedList();

            #region Flash light
            // FlashlightIsAvailableOrNot();
            // mCameraManager = (CameraManager)GetSystemService(Context.CameraService);

            //try
            //{
            //    mCameraId = mCameraManager.GetCameraIdList()[0];
            //}
            //catch (CameraAccessException e)
            //{
            //    e.PrintStackTrace();
            //}
            //CameraDeviceCallback callBack = new CameraDeviceCallback();
            //mCameraManager.OpenCamera(mCameraId, callBack, null);
            //mCameraDevice = callBack.Device;

            //mPreviewBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);

            /*
              mPreviewBuilder = mCameraDevice.createCaptureRequest(CameraDevice.TEMPLATE_PREVIEW);

            Surface previewSurface = new Surface(texture);
            mPreviewBuilder.addTarget(previewSurface);
             */

            flashImage.Click += FlashImage_Click;
            #endregion
            redBox = FindViewById<ImageView>(Resource.Id.imageViewRedBox);
            

            #region banner ads
            var adView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
            #endregion


        }

        private void FlashImage_Click(object sender, EventArgs e)
        {
            if (isFlashOn)
            {
                flashImage.SetImageResource(Resource.Drawable.f_off);
                isFlashOn = false;
            }
            else
            {
                flashImage.SetImageResource(Resource.Drawable.f_on);
                isFlashOn = true;
            }
            SwitchFlash(isFlashOn);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            // base.OnSaveInstanceState(outState);
        }
        #region Flash Light Functions
        /*
        private void FlashImage_Click(object sender, EventArgs e)
        {
            //https://www.codeproject.com/Articles/1112813/Android-Flash-Light-Application-Tutorial-Using-Cam
            try
            {

                if (isFlashOn)
                {
                    isFlashOn = false;

                    TurnOffFlashLight();
                }
                else
                {
                    isFlashOn = false;
                    CaptureRequest.Builder builder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
                    builder.Set(CaptureRequest.FlashMode, true);
                    CaptureRequest request = builder.Build();
                    //TurnOnFlashLight();

                }
            }
            catch (Exception ex)
            {
                Log.Error("FlashImage_Click", ex.Message);
            }
        }

        public void TurnOnFlashLight()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                mCameraManager.SetTorchMode(mCameraId, true);
            }
        }

        public void TurnOffFlashLight()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                mCameraManager.SetTorchMode(mCameraId, false);
            }
        }
    
        private void FlashlightIsAvailableOrNot()
        {

            bool hasFeature = this.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFlash);
            if (hasFeature)
            {
                flashImage.Visibility = ViewStates.Visible;
            }
        }
        */
        #endregion

        protected async override void OnResume()
        {
            base.OnResume();
            try
            {
                string userJson = xmlDb.getSavedUserSetting();
                _userSetting = await cardrepo.GetUserSetting(userJson);
                _selectedCard = _userSetting.UserCard;
                // lastchargNumber = "";
                #region show red box

                if (_userSetting.ShowRedBox)
                {
                    redBox.Visibility = ViewStates.Visible;
                }
                else
                {
                    redBox.Visibility = ViewStates.Invisible;
                }
                #endregion
                _isBusy = false;
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == premissionId)
            {
                if (grantResults[0] == Permission.Granted)
                {
                    camSource.Start(camView.Holder);
                }
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            Log.Info("SurfaceChanged", $"{width} . ... {height}");
            //throw new NotImplementedException();
            
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new string[]
                    {
                        Manifest.Permission.Camera,
                        Manifest.Permission.CallPhone
                    }
                    , premissionId);

                    return;
                }
                camSource.Start(camView.Holder);
            }
            catch (Exception ex)
            {
                Log.Error("Detector", ex.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            camSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            try
            {
                if (_isBusy)
                {
                    return;
                }
                SparseArray items = detections.DetectedItems;
                if (items.Size() != 0)
                {
                    tv.Post(() =>
                    {
                        StringBuilder bulder = new StringBuilder();
                        for (int i = 0; i < items.Size(); i++)
                        {
                            string det = ((TextBlock)items.ValueAt(i)).Value;
                            Log.Debug("......ReceiveDetections.....", det);
                            GetNumber(det);
                            bulder.Append(det);
                            bulder.Append("\n");
                        }
                        tv.Text = bulder.ToString();
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error("Detector", ex.Message);
            }
        }

        /// <summary>
        /// valid number must have :
        /// spaces between digits
        /// 16 digit if vodafone
        /// 15 digit if etisalat
        /// </summary>
        /// <param name="detction"></param>
        private void GetNumber(string detction)
        {
            try
            {

                string chargNumber = "";
                bool haveSpacs = false;
                bool haveDash = false;
                int spaceCount = 0;
                int _dashCount = 0;

                bool _isFourDigitsThenSpace = false;
                bool _isFourDigitsThenDash = false;

                bool _isThreeDigitsThenSpace = false;
                bool _isThreeDigitsThenDash = false;

                    foreach (char item in detction)
                    {
                        if (char.IsDigit(item))
                        {
                            chargNumber = chargNumber + item;
                        }
                        else if(char.IsWhiteSpace(item))
                        {
                            haveSpacs = true;
                            spaceCount++;
                            if (chargNumber.Length == 3)
                            {
                                _isThreeDigitsThenSpace = true;
                            }
                            else if (chargNumber.Length == 4)
                            {
                                _isFourDigitsThenSpace = true;
                            }
                        }
                        else if (item == '-')
                        {
                            haveDash = true;
                            _dashCount++;
                            if (chargNumber.Length == 3)
                            {
                                _isThreeDigitsThenDash = true;
                            }
                            else if (chargNumber.Length == 4)
                            {
                                _isFourDigitsThenDash = true;
                            }
                        }
                    }
                    if ((chargNumber.Length == 16 || chargNumber.Length == 15 || chargNumber.Length == 14) &&
                        (haveSpacs || haveDash) &&
                        (spaceCount > 2  || _dashCount > 2) &&
                        (_isFourDigitsThenSpace || _isFourDigitsThenDash || _isThreeDigitsThenDash || _isThreeDigitsThenSpace))
                    {
                       // camSource.Stop();
                        RunOnUiThread(()=> tv.Text = chargNumber);
                        // System.Threading.Thread.Sleep(1000);
                        //PuaseScanning();
                        MakeDial(chargNumber);
                        
                    }
               
            }
            catch (Exception ex)
            {
                Log.Info("Release", "txtRecognizer.SetProcessor");
            }
        }

        private void MakeDial(string chargNumber)
        {
            try
            {
                //Log.Info("MakeDial ", "..................MakeDial...............");
                if (_isBusy)
                {
                      Log.Info("MakeDial _isBusy 1........... ", _isBusy.ToString());
                    return;
                }
                _isBusy = true;

                if (chargNumber == lastchargNumber)
                {
                    //  camSource.Start();
                    //  Log.Info("MakeDial  if (chargNumber == lastchargNumber) 2........... ", lastchargNumber);
                    _isBusy = false;
                    return;
                }
                else if (chargedNumbers.Contains(chargNumber))
                {
                    Toast.MakeText(this, "charged!", ToastLength.Short).Show();
                    _isBusy = false;
                    return;
                }
                lastchargNumber = chargNumber;
                ////Log.Info("MakeDial lastchargNumber 3 ........... ", lastchargNumber);

                Intent call;
                //I found a solution for this issue by replacing # in 
                string preix = _selectedCard.Prefix.Replace("#", "%23");
                string suffix = _selectedCard.Suffix.Replace("#", "%23");
                //https://stackoverflow.com/questions/7280209/sending-action-call-intent-in-android-containing-hash-sign
                //string formatedNumber = string.Format("tel:*556*{0}{1}", chargNumber,
                //    Android.Net.Uri.Encode("#"));
                  string formatedNumber = $"tel:{preix}{chargNumber}{suffix}";
                Android.Net.Uri num = Android.Net.Uri.Parse(formatedNumber);

                if (_userSetting.ShowFirst)
                {
                    call = new Intent(Intent.ActionDial, num);
                    string forClip = _selectedCard.Prefix + chargNumber + _selectedCard.Suffix;
                    CopyToClipboard(forClip);
                }
                else
                {
                    call = new Intent(Intent.ActionCall, num);
                }

                #region save charged to local
                _selectedCard.CardNumber = chargNumber;
                _selectedCard.ChargingDate = DateTime.Now;
                _chargedList?.Add(_selectedCard);
                #endregion
                SaveChargedList();

                // Finish();
                // camSource.Stop();
                StartActivity(call);
                chargedNumbers.Add(chargNumber);
                //camSource.Start();
                // _isBusy = false;
            }
            catch (Exception es)
            {
                _isBusy = false;
                Log.Info("Release", "txtRecognizer.SetProcessor " + es.Message);
            }
        }

        public void Release()
        {
            Log.Info("Release", "txtRecognizer.SetProcessor");
            //throw new NotImplementedException();
        }

        private void CopyToClipboard(string tex)
        {
            RunOnUiThread(() =>
            {
                ClipboardManager clipboard = (ClipboardManager)GetSystemService(Context.ClipboardService);
                ClipData clip = ClipData.NewPlainText("label", tex);
                clipboard.PrimaryClip = clip;
                Toast.MakeText(this, Resource.String.CopiedToClipbord, ToastLength.Long).Show();
            });
           
        }

        private async void GetChargedList()
        {
            try
            {
                string chargedJson = xmlDb.GetChargsList();
                _chargedList = await cardrepo.GetChargedList(chargedJson);
            }
            catch (Exception es)
            {
                Log.Info("Release", "txtRecognizer.SetProcessor " + es.Message);
            }
        }

        private async void SaveChargedList()
        {
            try
            {
                if (_chargedList != null)
                {
                    string chargedJson = await cardrepo.SerializeObject(_chargedList);
                    xmlDb.SaveChargsList(chargedJson);
                }
            }
            catch (Exception es)
            {
                Log.Info("Release", "txtRecognizer.SetProcessor " + es.Message);
            }
        }

        private async void PuaseScanning()
        {
            _isBusy = true;
            await Task.Delay(5000);
            _isBusy = false;
        }

        #region Flashlight
        private Android.Hardware.Camera GetCamera()
        {
            try
            {
                var javaHero = camSource.JavaCast<Java.Lang.Object>();
                var fields = javaHero.Class.GetDeclaredFields();
                foreach (var field in fields)
                {
                    if (field.Type.CanonicalName.Equals("android.hardware.camera",StringComparison.OrdinalIgnoreCase))
                    {
                        field.Accessible = true;
                        var camera = field.Get(javaHero);
                        var cCamera = (Android.Hardware.Camera)camera;
                        return cCamera;
                    }
                }
                return null;
            }
            catch (Exception es)
            {
               
                Log.Info("Release", "txtRecognizer.SetProcessor " + es.Message);
                return null;
            }
        }

        private void SwitchFlash(bool On)
        {
            try
            {
                Android.Hardware.Camera _camer = GetCamera();
                if (On)
                {
                    if (_camer != null)
                    {
                        var prms = _camer.GetParameters();
                        prms.FlashMode = Android.Hardware.Camera.Parameters.FlashModeTorch;
                        _camer.SetParameters(prms);
                    }
                }
                else
                {
                    if (_camer != null)
                    {
                        var prms = _camer.GetParameters();
                        prms.FlashMode = Android.Hardware.Camera.Parameters.FlashModeOff;
                        _camer.SetParameters(prms);
                    }
                }
                
            }
            catch (Exception es)
            {
                Log.Info("Release", "txtRecognizer.SetProcessor " + es.Message);
            }
            
        }
        #endregion
    }

}