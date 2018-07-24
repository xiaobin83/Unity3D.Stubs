using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;


namespace x600d1dea.stubs 
{
	public class Cmd
	{
		static string cwd = Path.GetDirectoryName(Application.dataPath);

		public static string ChangeWorkingDir(string dir)
		{
			UnityEngine.Debug.Log("Change Working Dir " + dir);
			var old = cwd;
			cwd = dir;
			return old;
		}

		public static void Execute(string cmdLine, bool useShell = false)
		{
			CustomEditorApp.AddTask(() => UnityEngine.Debug.LogFormat("executing {0}", cmdLine));
			var proc = new Process();

			var processInfo = new ProcessStartInfo("cmd.exe", "/c " + cmdLine);
			proc.StartInfo = processInfo;

			processInfo.WorkingDirectory = cwd;
			if (useShell)
			{
				processInfo.CreateNoWindow = false;
				processInfo.UseShellExecute = true;
				processInfo.RedirectStandardInput = false;
				processInfo.RedirectStandardError = false;
			}
			else
			{
				processInfo.CreateNoWindow = true;
				processInfo.UseShellExecute = false;
				processInfo.RedirectStandardOutput = true;
				processInfo.RedirectStandardError = true;

				proc.OutputDataReceived += (s, e) =>
				{
					if (!string.IsNullOrEmpty(e.Data))
					{
						CustomEditorApp.AddTask(() => UnityEngine.Debug.Log(e.Data));
					}
				};
				proc.ErrorDataReceived += (s, e) =>
				{
					if (!string.IsNullOrEmpty(e.Data))
						CustomEditorApp.AddTask(() => UnityEngine.Debug.LogError(e.Data));
				};
			}
			proc.Start();
			if (!useShell)
			{
				proc.BeginOutputReadLine();
				proc.BeginErrorReadLine();
			}
			proc.WaitForExit();
			var exitCode = proc.ExitCode;
			CustomEditorApp.AddTask(() => UnityEngine.Debug.LogFormat("{0} end with {1}", cmdLine, exitCode));
			proc.Close();
		}


		public static void CreateDirectoryLink(string link, string target)
		{
			Execute(string.Format("mklink /D {0} {1}", link.Replace("/", "\\"), target.Replace("/", "\\")));
		}

		public static void DeleteDirectoryLink(string link)
		{
			Execute(string.Format("rmdir {0}", link.Replace("/", "\\")));
		}


		static void CopyDirectoryInternal(string srcDir, string destDir, Regex[] patterns)
		{
			var dir = new DirectoryInfo(srcDir);
			if (dir.Exists)
			{
				if (!Directory.Exists(destDir))
				{
					Directory.CreateDirectory(destDir);
				}

				var files = dir.GetFiles();
				foreach (FileInfo file in files)
				{
					string temppath = Path.Combine(destDir, file.Name);
					if (patterns.Length > 0)
					{
						foreach (var re in patterns)
						{
							if (re.IsMatch(file.Name))
							{
								file.CopyTo(temppath, true);
								break;
							}
						}
					}
					else
					{
						file.CopyTo(temppath, true);
					}

				}
				
				DirectoryInfo[] dirs = dir.GetDirectories();
				foreach (var subdir in dirs)
				{
					string temppath = Path.Combine(destDir, subdir.Name);
					CopyDirectory(subdir.FullName, temppath);
				}
			}
		}

		public static void CopyDirectory(string srcDir, string destDir, params string[] patterns)
		{
			Regex[] rePatterns = new Regex[patterns.Length];
			for (int i = 0; i < patterns.Length; ++i)
			{
				rePatterns[i] = new Regex(patterns[i]);
			}
			CopyDirectoryInternal(srcDir, destDir, rePatterns);

		}
	}
}
