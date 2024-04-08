namespace TemplateScripts
{
	public static class StringLoggingExtensions
	{
		public static string Colored(this string message, Colors color)
		{
			return string.Format("<color={0}>{1}</color>", color.ToString(), message);
		}
		
		public static string Colored(this string message, string colorCode)
		{
			return string.Format("<color={0}>{1}</color>", colorCode, message);
		}
		
		public static string Sized(this string message, int size)
		{
			return string.Format ("<size={0}>{1}</size>", size, message);
		}
		
		public static string Bold(this string message)
		{
			return string.Format ("<b>{0}</b>", message);
		}
		
		public static string Italics(this string message)
		{
			return string.Format ("<i>{0}</i>", message);
		}
	}

	public enum Colors
	{
		aqua,
		black,
		blue,
		brown,
		cyan,
		darkblue,
		fuchsia,
		green,
		grey,
		lightblue,
		lime,
		magenta,
		maroon,
		navy,
		olive,
		purple,
		red,
		silver,
		teal,
		white,
		yellow
	}
}