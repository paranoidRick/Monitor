using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager temp = (resourceMan = new ResourceManager("Properties.Resources", typeof(Resources).Assembly));
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap bg_0
	{
		get
		{
			object obj = ResourceManager.GetObject("bg_0", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap bg_01
	{
		get
		{
			object obj = ResourceManager.GetObject("bg_01", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap black
	{
		get
		{
			object obj = ResourceManager.GetObject("black", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap red
	{
		get
		{
			object obj = ResourceManager.GetObject("red", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal Resources()
	{
	}
}
