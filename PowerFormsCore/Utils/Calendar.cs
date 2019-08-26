using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Web.UI;

namespace DNNGo.Modules.PowerForms
{
    public class Calendar
    {
        public static string InvokePopupCal(String FieldClientID,Page FieldPage)
		{
			char[] TrimChars = {
				',',
				' '
			};
			string MonthNameString = "";
			foreach (string Month in DateTimeFormatInfo.CurrentInfo.MonthNames) {
				MonthNameString += Month + ",";
			}
			MonthNameString = MonthNameString.TrimEnd(TrimChars);
			string DayNameString = "";
			foreach (string Day in DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames) {
				DayNameString += Day + ",";
			}
			DayNameString = DayNameString.TrimEnd(TrimChars);
			string FormatString = DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToString();
			if (!DotNetNuke.UI.Utilities.ClientAPI.IsClientScriptBlockRegistered(FieldPage, "PopupCalendar.js")) {
				DotNetNuke.UI.Utilities.ClientAPI.RegisterClientScriptBlock(FieldPage, "PopupCalendar.js", "<script type=\"text/javascript\" src=\"" + DotNetNuke.UI.Utilities.ClientAPI.ScriptPath + "PopupCalendar.js\"></script>");
			}
			string strToday = DotNetNuke.UI.Utilities.ClientAPI.GetSafeJSString(DotNetNuke.Services.Localization.Localization.GetString("Today"));
			string strClose = DotNetNuke.UI.Utilities.ClientAPI.GetSafeJSString(DotNetNuke.Services.Localization.Localization.GetString("Close"));
			string strCalendar = DotNetNuke.UI.Utilities.ClientAPI.GetSafeJSString(DotNetNuke.Services.Localization.Localization.GetString("Calendar"));
			return "javascript:popupCal('Cal','" + FieldClientID + "','" + FormatString + "','" + MonthNameString + "','" + DayNameString + "','" + strToday + "','" + strClose + "','" + strCalendar + "'," + (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + ");";
		}
	}
   
}