using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace x600d1dea.stubs.utils
{
	public class GetMethodInfo
	{
		public static IEnumerable<MethodInfo> WithAttr<TAttr>(bool fromEditor = false) 
		{
			IEnumerable<Type> allTypes = null;
			try
			{
				if (fromEditor)
					allTypes = Assembly.Load("Assembly-CSharp-Editor").GetTypes().AsEnumerable();
				else
					allTypes = Assembly.Load("Assembly-CSharp").GetTypes().AsEnumerable();
			}
			catch
			{ }
			try
			{
				IEnumerable<Type> typesInPlugins;
				if (fromEditor)
					typesInPlugins = Assembly.Load("Assembly-CSharp-Editor-firstpass").GetTypes();
				else
					typesInPlugins = Assembly.Load("Assembly-CSharp-firstpass").GetTypes();
				if (allTypes != null)
					allTypes = allTypes.Union(typesInPlugins);
				else
					allTypes = typesInPlugins;
			}
			catch
			{ }
			if (allTypes == null)
			{
				return new MethodInfo[0];
			}
			var methods = allTypes.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				.Where(m => m.GetCustomAttributes(typeof(TAttr), false).Length > 0);
			return methods;
		}
	}
}
