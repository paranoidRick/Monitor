using System;
using System.Collections.Generic;
using System.Reflection;
using NPOI.XWPF.UserModel;

namespace Monitor;

internal class NPOIHleper
{
	public static void ReplaceKey(XWPFParagraph para, object model)
	{
		string text = para.ParagraphText;
		IList<XWPFRun> runs = para.Runs;
		string styleid = para.Style;
		for (int i = 0; i < runs.Count; i++)
		{
			XWPFRun run = runs[i];
			text = run.ToString();
			Type t = model.GetType();
			PropertyInfo[] pi = t.GetProperties();
			PropertyInfo[] array = pi;
			foreach (PropertyInfo p in array)
			{
				if (text.Contains("{$" + p.Name + "}"))
				{
					text = text.Replace("{$" + p.Name + "}", p.GetValue(model, null).ToString());
				}
			}
			runs[i].SetText(text, 0);
		}
	}
}
