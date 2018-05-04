using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace x600d1dea.stubs.utils
{

	public class MockADSProvier : IADSProvider
	{
		public bool CouldShow(string ID)
		{
			return true;
		}
		public void Show(string ID)
		{
		}
	}
}