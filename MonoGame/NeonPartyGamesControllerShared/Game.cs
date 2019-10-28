using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController
{
	public class NeonPartyGamesControllerGame : EngineGame
	{
		public const int ScreenHeight = 720;

#if ANDROID
        public static Microsoft.Devices.Sensors.Accelerometer Accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
        public static Android.OS.Vibrator Vibrator;
#endif

		public NeonPartyGamesControllerGame() : base(1280, NeonPartyGamesControllerGame.ScreenHeight, 0, 0) {
			this.CanvasWidth = NeonPartyGamesControllerGame.GetScreenWidth();
			this.Graphics.PreferredBackBufferWidth = this.CanvasWidth;
			this.Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

#if NETFX_CORE
            this.Graphics.SynchronizeWithVerticalRetrace = true;
#else
			this.Graphics.SynchronizeWithVerticalRetrace = false;
#endif
			this.IsFixedTimeStep = false;
			this.Content.RootDirectory = "Content";

			this.Graphics.IsFullScreen = false;
#if !ANDROID && !IOS && !PLAYSTATION4
			this.Graphics.HardwareModeSwitch = false;
			this.IsMouseVisible = true;
#endif

#if NETFX_CORE
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(this.CanvasWidth, NeonPartyGamesControllerGame.ScreenHeight);
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode = Windows.UI.ViewManagement.FullScreenSystemOverlayMode.Minimal;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, Windows.UI.Core.BackRequestedEventArgs args) => { args.Handled = true; };
#endif
#if !PLAYSTATION4
			this.Window.AllowUserResizing = true;
#endif
			this.Window.AllowAltF4 = true;
		}

		private static int GetScreenWidth()
		{
			float screen_pixel_width;
			float screen_pixel_height;

#if ANDROID
            Android.Util.DisplayMetrics displayMetrics = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            screen_pixel_width = (float)displayMetrics.WidthPixels;
            screen_pixel_height = (float)displayMetrics.HeightPixels;
#else
			screen_pixel_width = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			screen_pixel_height = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#endif

			float resolution_percent = (float)NeonPartyGamesControllerGame.ScreenHeight / screen_pixel_height;
			return (int)(screen_pixel_width * resolution_percent);
		}

		private static int GetScreenWidth(int fake_display_width, int fake_display_height)
		{
			float resolution_percent = (float)NeonPartyGamesControllerGame.ScreenHeight / (float)fake_display_height;
			int screen_width = (int)((float)fake_display_width * resolution_percent);

			Debug.WriteLine("----- Fake Screen Size Enabled ------");
			Debug.WriteLine("Width: " + screen_width);
			Debug.WriteLine("Height: " + NeonPartyGamesControllerGame.ScreenHeight);
			Debug.WriteLine("----- Fake Screen Size Enabled ------");
			return screen_width;
		}

		protected override void Initialize() {
			base.Initialize();
#if ANDROID
            if (NeonPartyGamesControllerGame.Accelerometer.State != Microsoft.Devices.Sensors.SensorState.Ready)
            {
                NeonPartyGamesControllerGame.Accelerometer.Start();
            }
#endif
		}

		protected override void LoadContent() {
			base.LoadContent();

			Engine.ChangeRoom<RoomMain>();
		}
	}
}
