using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace x600d1dea.stubs
{
	public class BuildPath
	{

		/// <summary>
		/// ${ProjectDir} with Assets folder in it
		/// </summary>
		public static string projectDir
		{
			get
			{

				return Path.GetDirectoryName(Application.dataPath);
			}
		}

		/// <summary>
		///	${ProjectDir}/Assets/_Staging
		/// </summary>
		public static string stagingAssetsDir
		{
			get
			{
				return Path.Combine(Application.dataPath, "_Staging");
			}
		}

		/// <summary>
		/// ${ProjectDir}/Assets/_Staging/Resources
		/// </summary>
		public static string stagingResourcesDir
		{
			get
			{
				return Path.Combine(stagingAssetsDir, "Resources");
			}
		}


		public static string externalLibsName = "Unity3D.ExternalLibs";

		/// <summary>
		/// ${ProjectDir}/../${ExternalLibsName} (default ${ProjectDir}/../Unity3D.ExternalLibs )
		/// </summary>
		public static string externalLibsDir
		{
			get
			{
				return Path.Combine(Path.GetDirectoryName(projectDir), externalLibsName);
			}
		}

		/// <summary>
		/// ${ProjectDir}/Assets/Plugins
		/// </summary>
		public static string pluginsDir
		{
			get
			{
				return Path.Combine(Application.dataPath, "Plugins");
			}
		}

		/// <summary>
		/// ${ProjectDir}/Assets/Plugins/ExternalLibs
		/// </summary>
		public static string externalLibsPluginsDir
		{
			get
			{
				return Path.Combine(pluginsDir, "ExternalLibs");
			}
		}


		public const string androidBuildDirName = "_build_android";

		/// <summary>
		/// ${ProjectDir}/_build_android, Assets folder placed besides it
		/// </summary>
		public static string androidProjectDir = Path.Combine(projectDir, androidBuildDirName);


		/// <summary>
		/// ${ProjectDir}/_build_android/${ProductName}
		/// </summary>
		public static string androidSrcDir
		{
			get
			{
				return Path.Combine(androidProjectDir, Application.productName);
			}
		}

		/// <summary>
		/// ${ProjectDir}/Assets/Plugins/Android
		/// </summary>
		public static string androidPluginsDir
		{
			get
			{
				return Path.Combine(pluginsDir, "Android");
			}
		}

		/// <summary>
		/// ${ProjectDir}/../${ExternalLibsName}/Android
		/// </summary>
		public static string androidExternalLibsDir
		{
			get
			{
				return Path.Combine(externalLibsDir, "Android");
			}
		}

		/// <summary>
		/// ${ProjectDir}/../${ExternalLibsName}/Android/_Android
		/// </summary>
		public static string androidExternalLibsBaseCodeDir
		{
			get
			{
				return Path.Combine(androidExternalLibsDir, "_Android");
			}
		}


		/// <summary>
		/// ${ProjectDir}/Assets/Plugins/ExternalLibs/Android
		/// </summary>
		public static string androidExternalLibsPlugins
		{
			get
			{
				return Path.Combine(externalLibsPluginsDir, "Android");
			}

		}



	}
}
