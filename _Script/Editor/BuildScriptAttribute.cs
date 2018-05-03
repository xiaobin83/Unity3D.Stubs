using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace x600d1dea.stubs
{
	public enum BuildStage
	{
		PreBuild,
		PostBuild,
	}

	public enum BuildOrder
	{
		Init,
		Default,
		Later,
	}

	public class BuildScriptAttribute : Attribute 
	{
		public delegate bool Builder();

		public BuildOrder order;
		public int priority;
		public BuildStage stage;
		const int maxPriority = 10000;
		public BuildScriptAttribute(BuildStage stage, BuildOrder order = BuildOrder.Default, int priority = 0)
		{
			this.stage = stage;
			this.order = order;
			this.priority = Math.Sign(priority) * Math.Min(Math.Abs(priority), maxPriority);
		}

		public static List<Builder> GetBuilders(BuildStage stage)
		{
			var b = new List<Builder>();
			IEnumerable<Type> allTypes = null;
			try
			{
				allTypes = Assembly.Load("Assembly-CSharp-Editor").GetTypes().AsEnumerable();
			}
			catch
			{ }
			try
			{
				var typesInPlugins = Assembly.Load("Assembly-CSharp-Editor-firstpass").GetTypes();
				if (allTypes != null)
					allTypes = allTypes.Union(typesInPlugins);
				else
					allTypes = typesInPlugins;
			}
			catch
			{ }
			if (allTypes == null)
			{
				return b;
			}

			var methods = allTypes.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				.Where(m =>
				{
					var attrs = m.GetCustomAttributes(typeof(BuildScriptAttribute), false);
					if (attrs.Length > 0)
					{
						var attr = (BuildScriptAttribute)attrs[0];
						return attr.stage == stage;
					}
					return false;
				})
				.OrderBy(m =>
				{
					var attr = (BuildScriptAttribute)m.GetCustomAttributes(typeof(BuildScriptAttribute), false)[0];
					return (int)attr.order * maxPriority + attr.priority;
				})
				.ToArray();
			for (int i = 0; i < methods.Length; ++i)
			{
				var m = methods[i];
				var builder = (Builder)Delegate.CreateDelegate(typeof(Builder), m);
				b.Add(builder);
			}
			return b;
		}

	}
}
