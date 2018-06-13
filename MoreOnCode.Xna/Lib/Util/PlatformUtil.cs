using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MoreOnCode.Lib.Util
{
	public enum Platforms {
		Unknown,
		Android,
		Angle,
		iOS,
		Linux,
		MacOS,
		Ouya,
		PSMobile,
		Windows,
		Windows8,
		WindowsGL,
		WindowsPhone,
		Web,
	};

	public class PlatformUtil
	{
		private static readonly Dictionary<Platforms, string> PlatformPersonalFolder;
		private static readonly Dictionary<Platforms, string> PlatformApplicationDataFolder;
		private static readonly Dictionary<Platforms, string> PlatformLocalApplicationDataFolder;

		public static readonly Platforms CurrentPlatform;

		static PlatformUtil() {
			PlatformPersonalFolder = 
				new Dictionary<Platforms, string>() {
				{ Platforms.Android, @"^\/data\/data\/[^\/]+\/files$" },
				{ Platforms.Angle, @"" },
				{ Platforms.iOS, @"" },
				{ Platforms.Linux, @"" },
				{ Platforms.MacOS, @"^\/Users\/[^\/]+$" },
				{ Platforms.Ouya, @"" },
				{ Platforms.PSMobile, @"" },
				{ Platforms.Web, @"" },
				{ Platforms.Windows, @"" },
				{ Platforms.Windows8, @"" },
				{ Platforms.WindowsGL, @"" },
				{ Platforms.WindowsPhone, @"" },
			};

			PlatformApplicationDataFolder = 
				new Dictionary<Platforms, string>(){
				{ Platforms.Android, @"^\/data\/data\/[^\/]+\/files\/\.config$" },
				{ Platforms.Angle, @"" },
				{ Platforms.iOS, @"" },
				{ Platforms.Linux, @"" },
				{ Platforms.MacOS, @"^\/Users\/[^\/]+\/\.config+$" },
				{ Platforms.Ouya, @"" },
				{ Platforms.PSMobile, @"" },
				{ Platforms.Web, @"" },
				{ Platforms.Windows, @"" },
				{ Platforms.Windows8, @"" },
				{ Platforms.WindowsGL, @"" },
				{ Platforms.WindowsPhone, @"" },
			};

			PlatformLocalApplicationDataFolder = 
				new Dictionary<Platforms, string>() {
				{ Platforms.Android, @"^\/data\/data\/[^\/]+\/files\/\.local\/share$" },
				{ Platforms.Angle, @"" },
				{ Platforms.iOS, @"" },
				{ Platforms.Linux, @"" },
				{ Platforms.MacOS, @"^\/Users\/[^\/]+\/\.local\/share+$" },
				{ Platforms.Ouya, @"" },
				{ Platforms.PSMobile, @"" },
				{ Platforms.Web, @"" },
				{ Platforms.Windows, @"" },
				{ Platforms.Windows8, @"" },
				{ Platforms.WindowsGL, @"" },
				{ Platforms.WindowsPhone, @"" },
			};

			CurrentPlatform = InitCurrentPlatform ();
		}


		private static Platforms InitCurrentPlatform() {
			var detected = Platforms.Unknown;

			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			foreach (string pattern in PlatformPersonalFolder.Values) {
				if (!String.IsNullOrEmpty (pattern)) {
					var match = Regex.IsMatch (path, pattern);
					if (match) {
						foreach (Platforms key in PlatformPersonalFolder.Keys) {
							if (PlatformPersonalFolder [key] == pattern) {
								detected = key;
								break;
							}
						}
						break;
					}
				}
			}

			try {
				var agreesWithApplicationDataFolder = Regex.IsMatch (
					Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), 
					PlatformApplicationDataFolder [detected]);
				var agreesWithLocalApplicationDataFolder = Regex.IsMatch (
					Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData), 
					PlatformLocalApplicationDataFolder [detected]);
				
				return 
					agreesWithApplicationDataFolder &&
					agreesWithLocalApplicationDataFolder ? 
					detected : Platforms.Unknown;
			} catch { }
				
			return Platforms.Unknown;
		}

		public static string PlatformString {
			get { return Environment.OSVersion.Platform.ToString(); }
		}

		public static bool IsWindows { 
			get { 
				return 
					CurrentPlatform == Platforms.Windows || 
					CurrentPlatform == Platforms.WindowsGL || 
					CurrentPlatform == Platforms.Windows8; 
			} 
		}

		public static bool IsMacOS { 
			get{ return CurrentPlatform == Platforms.MacOS; } 
		}

		public static bool IsLinux { 
			get{ return CurrentPlatform == Platforms.Linux; } 
		}

		public static bool IsDesktop { 
			get { return IsLinux || IsMacOS || IsWindows; } 
		}

		public static bool IsMobile { 
			get { 
				return 
					CurrentPlatform == Platforms.WindowsPhone || 
					CurrentPlatform == Platforms.iOS || 
					CurrentPlatform == Platforms.Android ||
					CurrentPlatform == Platforms.PSMobile; 
			} 
		}

		public static bool IsConsole { 
			get { 
				return 
					CurrentPlatform == Platforms.Ouya ||
					CurrentPlatform == Platforms.PSMobile;
			} 
		}

		public static bool IsDirectX { 
			get { 
				#if DIRECTX
				return true; 
				#else
				return false;
				#endif
			} 
		}

		public static bool IsOpenGL { 
			get { 
				#if OPENGL || GLES
				return true; 
				#else
				return false;
				#endif
			} 
		}

		public static bool IsOpenGLES { 
			get { 
				#if GLES
				return true; 
				#else
				return false;
				#endif
			} 
		}

		public static bool IsOpenAL { 
			get { 
				#if OPENAL
				return true; 
				#else
				return false;
				#endif
			} 
		}

		public static bool IsDebug {
			get { 
				#if DEBUG
				return true; 
				#else
				return false;
				#endif
			} 
		}

		public static bool IsTrace {
			get { 
				#if TRACE
				return true; 
				#else
				return false;
				#endif
			} 
		}
	}
}

