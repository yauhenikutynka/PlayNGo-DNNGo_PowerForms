
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
namespace DNNGo.Modules.PowerForms
{
    [ToolboxBitmap(typeof(ExportDotNet)), DefaultProperty("Text"), ToolboxData("<{0}:ExportDotNet runat=server></{0}:ExportDotNet>")]
    public class ExportDotNet : WebControl
    {
        private Color _AlternateRowBackColor;
        private DisplaySetting _ColumnHeader = new DisplaySetting();
        private string _ContentType = "UTF-8";
        private DisplaySetting _Data = new DisplaySetting();
        private Color _DataBorderColor;
        private int _DataBorderSize = 1;
        private bool _DataSuppressZero = false;
        private string _Delimeter = "";
        private DataSet _ds;
        private string _ExportFileName = "";
        private StreamWriter _ExportWriter;
        private DisplaySetting _Footer = new DisplaySetting();
        private bool _isDemo = false;
        private string _msg = ("This file is generated from a free Web control from http://www.invenmanager.com on " + DateTime.Now.ToString());
        //private string _msg = "";
        private string _PopupURL = "";
        private bool _ShowBorder = true;
        private bool _ShowPopupWindow = true;
        private string _text = "";
        private DisplaySetting _Title = new DisplaySetting();

        private DataTable ConvertDataReaderToDataTable(OleDbDataReader reader)
        {
            DataTable table;
            try
            {
                DataTable table2 = new DataTable();
                DataTable schemaTable = reader.GetSchemaTable();
                int num3 = schemaTable.Rows.Count - 1;
                int num = 0;
                while (num <= num3)
                {
                    DataRow row = schemaTable.Rows[num];
                  
                    DataColumn column = new DataColumn(StringType.FromObject(row["ColumnName"]), (Type)row["DataType"]);
                    table2.Columns.Add(column);
                    num++;
                }
                while (reader.Read())
                {
                    DataRow row2 = table2.NewRow();
                    int num2 = reader.FieldCount - 1;
                    for (num = 0; num <= num2; num++)
                    {
                        //row2.set_Item(num, RuntimeHelpers.GetObjectValue(reader.GetValue(num)));
                        row2[num] = RuntimeHelpers.GetObjectValue(reader.GetValue(num));
                    }
                    table2.Rows.Add(row2);
                }
                table = table2;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return table;
        }

        private DataTable ConvertDataReaderToDataTable(SqlDataReader reader)
        {
            DataTable table;
            try
            {
                DataTable table2 = new DataTable();
                DataTable schemaTable = reader.GetSchemaTable();
                int num3 = schemaTable.Rows.Count - 1;
                int num = 0;
                while (num <= num3)
                {
                    DataRow row = schemaTable.Rows[num];
                    DataColumn column = new DataColumn(StringType.FromObject(row["ColumnName"]), (Type)row["DataType"]);
                    table2.Columns.Add(column);
                    num++;
                }
                while (reader.Read())
                {
                    DataRow row2 = table2.NewRow();
                    int num2 = reader.FieldCount - 1;
                    for (num = 0; num <= num2; num++)
                    {
                        row2[num] = RuntimeHelpers.GetObjectValue(reader.GetValue(num));
                        //row2.set_Item(num, RuntimeHelpers.GetObjectValue(reader.GetValue(num)));
                    }
                    table2.Rows.Add(row2);
                }
                table = table2;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return table;
        }

        private string DataTableToHtml(DataTable dt)
        {
            string str = String.Empty;
            try
            {
                StringBuilder builder = new StringBuilder();
                int num = dt.Columns.Count;
                this._ExportWriter.WriteLine("<HTML><HEAD>");
                if (StringType.StrCmp(this._ContentType, "", false) != 0) this._ExportWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html\"; charset=\"" + this._ContentType + "\">");
                this.WriteStyle();
                this._ExportWriter.WriteLine("</HEAD>");
                this._ExportWriter.WriteLine("<BODY>");
                if (!this._ShowBorder) this._DataBorderSize = 0;
                this._ExportWriter.Write("<table border='" + this._DataBorderSize.ToString() + "'");
                if (!this._DataBorderColor.IsEmpty)
                {
                    WebColorConverter converter = new WebColorConverter();
                    this._ExportWriter.Write(" bordercolor='" + converter.ConvertToString(this._DataBorderColor) + "'");
                }
                this._ExportWriter.WriteLine(">");
                //if (this._isDemo)
                //{
                //    this._ExportWriter.Write("<tr><td colspan='" + num.ToString() + "'>");
                //    this._ExportWriter.Write(this._msg);
                //    this._ExportWriter.WriteLine("</td></tr>");
                //}
                if (StringType.StrCmp(this._Title.Description, "", false) != 0)
                {
                    this._ExportWriter.Write("<tr class=\"title\"><td colspan='" + num.ToString() + "' ");
                    this._ExportWriter.Write(">");
                    this._ExportWriter.Write(this._Title.Description);
                    this._ExportWriter.WriteLine("</td></tr>");
                }
                if (this._ColumnHeader.Show)
                {
                    this._ExportWriter.Write("<tr valign=\"middle\" class=\"columnheader\">");
                    foreach (DataColumn column in dt.Columns)
                    {
                        this._ExportWriter.Write("<td>" + column.ToString() + "</td>");
                    }
                    this._ExportWriter.WriteLine("</tr>");
                }
                bool flag = false;
                foreach (DataRow row in dt.Rows)
                {
                    if (flag)
                        this._ExportWriter.Write("<tr class=\"alternaterowdata\">");
                    else
                        this._ExportWriter.Write("<tr class=\"data\">");
                    object[] objArray = row.ItemArray;
                    for (int i = 0; i < objArray.Length; i++)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(objArray[i]);
                        this._ExportWriter.Write("<td");
                        if (objectValue == DBNull.Value) objectValue = "";
                        if (StringType.StrCmp(objectValue.ToString().Trim(), "0", false) == 0 & this.DataSuppressZero) objectValue = "";
                        string str2 = StringType.FromObject(objectValue);
                        this._ExportWriter.Write(">");
                        this._ExportWriter.Write(str2);
                        this._ExportWriter.Write("</td>");
                    }
                    this._ExportWriter.WriteLine("</tr>");
                    flag = !flag;
                }
                if (this._isDemo)
                {
                    this._ExportWriter.Write("<tr><td colspan='" + num.ToString() + "'>");
                    this._ExportWriter.Write(this._msg);
                    this._ExportWriter.WriteLine("</td></tr>");
                }

                if (StringType.StrCmp(this._Footer.Description, "", false) != 0)
                {
                    this._ExportWriter.Write("<tr class=\"footer\">");
                    this._ExportWriter.Write("<td colspan='" + num.ToString() + "'>");
                    this._ExportWriter.Write(this._Footer.Description);
                    this._ExportWriter.WriteLine("</td></tr>");
                }
                this._ExportWriter.WriteLine("</TABLE></BODY></HTML>");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataTableToText(DataTable dt)
        {
            string str = String.Empty;
            try
            {
                IEnumerator enumerator = null;
                int num = dt.Columns.Count;
                //if (this._isDemo) this._ExportWriter.WriteLine(this._msg);
                if (StringType.StrCmp(this._Delimeter, "", false) == 0) this._Delimeter = "\t";
                if (this._ColumnHeader.Show)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        this._ExportWriter.Write(column.ToString());
                        this._ExportWriter.Write(this._Delimeter);
                    }
                    this._ExportWriter.WriteLine("");
                }
                try
                {
                    enumerator = dt.Rows.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        object[] objArray = ((DataRow)enumerator.Current).ItemArray;
                        for (int i = 0; i < objArray.Length; i++)
                        {
                            object objectValue = RuntimeHelpers.GetObjectValue(objArray[i]);
                            if (objectValue == DBNull.Value) objectValue = "";
                            string str2 = StringType.FromObject(objectValue);
                            this._ExportWriter.Write(str2);
                            this._ExportWriter.Write(this._Delimeter);
                        }
                        this._ExportWriter.WriteLine("");
                    }
                    if (this._isDemo) this._ExportWriter.WriteLine(this._msg);
                }
                finally
                {
                    if (enumerator is IDisposable) ((IDisposable)enumerator).Dispose();
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataTableToXml(DataTable dt)
        {
            string str = String.Empty;
            try
            {
                int num = dt.Columns.Count;
                if (this._Title.Description.Length == 0) this._Title.Description = "Data";
                this._Title.Description = this._Title.Description.Replace(" ", "_");
                this._ExportWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                this._ExportWriter.WriteLine("<" + this._Title.Description + ">");
                foreach (DataRow row in dt.Rows)
                {
                    this._ExportWriter.WriteLine("  <Record>");
                    int num3 = num - 1;
                    for (int i = 0; i <= num3; i++)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(row[i]);
                        if (objectValue == DBNull.Value) objectValue = "";
                        if (StringType.StrCmp(objectValue.ToString().Trim(), "0", false) == 0 & this.DataSuppressZero) objectValue = "";
                        if (StringType.StrCmp(objectValue.ToString(), "", false) == 0)
                            this._ExportWriter.WriteLine("    <" + dt.Columns[i].ToString() + " />");
                        else
                        {
                            this._ExportWriter.Write("    <" + dt.Columns[i].ToString() + ">");
                            this._ExportWriter.Write(objectValue.ToString().Replace("&", "&amp;"));
                            this._ExportWriter.WriteLine("</" + dt.Columns[i].ToString() + ">");
                        }
                    }
                    this._ExportWriter.WriteLine("  </Record>");
                }
                this._ExportWriter.WriteLine("</" + this._Title.Description + ">");
                if (this._isDemo) this._ExportWriter.WriteLine("<!-- " + this._msg + " -->");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataTableToXmlWithAttributes(DataTable dt)
        {
            string str = String.Empty;
            try
            {
                int num = dt.Columns.Count;
                if (this._Title.Description.Length == 0) this._Title.Description = "Data";
                this._Title.Description = this._Title.Description.Replace(" ", "_");
                this._ExportWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                this._ExportWriter.WriteLine("<" + this._Title.Description + ">");
                foreach (DataRow row in dt.Rows)
                {
                    this._ExportWriter.Write("  <Record");
                    int num3 = num - 1;
                    for (int i = 0; i <= num3; i++)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(row[i]);
                        if (objectValue == DBNull.Value) objectValue = "";
                        if (StringType.StrCmp(objectValue.ToString().Trim(), "0", false) == 0 & this.DataSuppressZero) objectValue = "";
                        this._ExportWriter.Write(" " + dt.Columns[i].ToString() + "=\"");
                        this._ExportWriter.Write(objectValue.ToString().Replace("&", "&amp;"));
                        this._ExportWriter.Write("\"");
                    }
                    this._ExportWriter.WriteLine(" />");
                }
                this._ExportWriter.WriteLine("</" + this._Title.Description + ">");
                if (this._isDemo) this._ExportWriter.WriteLine("<!-- " + this._msg + " -->");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataViewToHtml(DataView dv)
        {
            string str = String.Empty;
            try
            {
                int num = 0;
                if (dv.Count == 0) return "";
                num = dv.Table.Columns.Count;
                this._ExportWriter.WriteLine("<HTML><HEAD>");
                if (StringType.StrCmp(this._ContentType, "", false) != 0) this._ExportWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html\"; charset=\"" + this._ContentType + "\">");
                this.WriteStyle();
                this._ExportWriter.WriteLine("</HEAD>");
                this._ExportWriter.WriteLine("<BODY>");
                if (!this._ShowBorder) this._DataBorderSize = 0;
                this._ExportWriter.Write("<table border='" + this._DataBorderSize.ToString() + "'");
                if (!this._DataBorderColor.IsEmpty)
                {
                    WebColorConverter converter = new WebColorConverter();
                    this._ExportWriter.Write(" bordercolor='" + converter.ConvertToString(this._DataBorderColor) + "'");
                }
                this._ExportWriter.WriteLine(">");
                if (this._isDemo)
                {
                    this._ExportWriter.Write("<tr><td colspan='" + num.ToString() + "'>");
                    this._ExportWriter.Write(this._msg);
                    this._ExportWriter.WriteLine("</td></tr>");
                }
                if (StringType.StrCmp(this._Title.Description, "", false) != 0)
                {
                    this._ExportWriter.Write("<tr class=\"title\"><td colspan='" + num.ToString() + "' ");
                    this._ExportWriter.Write(">");
                    this._ExportWriter.Write(this._Title.Description);
                    this._ExportWriter.WriteLine("</td></tr>");
                }
                if (this._ColumnHeader.Show)
                {
                    this._ExportWriter.Write("<tr valign=\"middle\" class=\"columnheader\">");
                    foreach (DataColumn column in dv.Table.Columns)
                    {
                        this._ExportWriter.Write("<td>" + column.ToString() + "</td>");
                    }
                    this._ExportWriter.WriteLine("</tr>");
                }
                if (this._AlternateRowBackColor.IsEmpty) this._AlternateRowBackColor = this._Data.BackColor;
                bool flag = false;
                int num5 = dv.Count - 1;
                for (int i = 0; i <= num5; i++)
                {
                    if (flag)
                        this._ExportWriter.Write("<tr class=\"alternaterowdata\">");
                    else
                        this._ExportWriter.Write("<tr class=\"data\">");
                    int num4 = num - 1;
                    for (int j = 0; j <= num4; j++)
                    {
                        this._ExportWriter.Write("<td");
                        object objectValue = RuntimeHelpers.GetObjectValue(dv[i][j]);
                        if (objectValue == DBNull.Value) objectValue = "";
                        if (StringType.StrCmp(objectValue.ToString().Trim(), "0", false) == 0 & this.DataSuppressZero) objectValue = "";
                        string str2 = StringType.FromObject(objectValue);
                        this._ExportWriter.Write(">");
                        this._ExportWriter.Write(str2);
                        this._ExportWriter.Write("</td>");
                    }
                    this._ExportWriter.WriteLine("</tr>");
                    flag = !flag;
                }
                if (StringType.StrCmp(this._Footer.Description, "", false) != 0)
                {
                    this._ExportWriter.Write("<tr class=\"footer\">");
                    this._ExportWriter.Write("<td colspan='" + num.ToString() + "'>");
                    this._ExportWriter.Write(this._Footer.Description);
                    this._ExportWriter.WriteLine("</td></tr>");
                }
                this._ExportWriter.WriteLine("</TABLE></BODY></HTML>");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataViewToText(DataView dv)
        {
            string str = String.Empty;
            try
            {
                int num = 0;
                if (StringType.StrCmp(this._Delimeter, "", false) == 0) this._Delimeter = "\t";
                if (dv.Count == 0) return "";
                num = dv.Table.Columns.Count;
                if (this._isDemo) this._ExportWriter.WriteLine(this._msg);
                if (this._ColumnHeader.Show)
                {
                    foreach (DataColumn column2 in dv.Table.Columns)
                    {
                        this._ExportWriter.Write(column2.ToString());
                        this._ExportWriter.Write(this._Delimeter);
                    }
                    this._ExportWriter.WriteLine("");
                }
                int num5 = dv.Count - 1;
                for (int i = 0; i <= num5; i++)
                {
                    int num4 = num - 1;
                    for (int j = 0; j <= num4; j++)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(dv[i][j]);
                        if (objectValue == DBNull.Value) objectValue = "";
                        string str2 = StringType.FromObject(objectValue);
                        this._ExportWriter.Write(str2);
                        this._ExportWriter.Write(this._Delimeter);
                    }
                    this._ExportWriter.WriteLine("");
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataViewToXml(DataView dv)
        {
            string str = String.Empty;
            try
            {
                int num = 0;
                if (dv.Count == 0) return "";
                num = dv.Table.Columns.Count;
                if (this._Title.Description.Length == 0) this._Title.Description = "Data";
                this._Title.Description = this._Title.Description.Replace(" ", "_");
                this._ExportWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                this._ExportWriter.WriteLine("<" + this._Title.Description + ">");
                int num5 = dv.Count - 1;
                for (int i = 0; i <= num5; i++)
                {
                    this._ExportWriter.WriteLine("  <Record>");
                    int num4 = num - 1;
                    for (int j = 0; j <= num4; j++)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(dv[i][j]);
                        if (objectValue == DBNull.Value) objectValue = "";
                        if (StringType.StrCmp(objectValue.ToString(), "", false) == 0)
                            this._ExportWriter.WriteLine("    <" + dv.Table.Columns[j].ToString() + " />");
                        else
                        {
                            this._ExportWriter.Write("    <" + dv.Table.Columns[j].ToString() + ">");
                            this._ExportWriter.Write(objectValue.ToString().Replace("&", "&amp;"));
                            this._ExportWriter.WriteLine("</" + dv.Table.Columns[j].ToString() + ">");
                        }
                    }
                    this._ExportWriter.WriteLine("  </Record>");
                }
                this._ExportWriter.WriteLine("</" + this._Title.Description + ">");
                if (this._isDemo) this._ExportWriter.WriteLine("<!-- " + this._msg + " -->");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        private string DataViewToXmlWithAttributes(DataView dv)
        {
            string str = String.Empty;
            try
            {
                int num = 0;
                if (dv.Count == 0) return "";
                num = dv.Table.Columns.Count;
                if (this._Title.Description.Length == 0) this._Title.Description = "Data";
                this._Title.Description = this._Title.Description.Replace(" ", "_");
                this._ExportWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                this._ExportWriter.WriteLine("<" + this._Title.Description + ">");
                int num5 = dv.Count - 1;
                for (int i = 0; i <= num5; i++)
                {
                    this._ExportWriter.Write("  <Record");
                    int num4 = num - 1;
                    for (int j = 0; j <= num4; j++)
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(dv[i][j]);
                        if (objectValue == DBNull.Value) objectValue = "";
                        this._ExportWriter.Write(" " + dv.Table.Columns[j].ToString() + "=\"");
                        this._ExportWriter.Write(objectValue.ToString().Replace("&", "&amp;"));
                        this._ExportWriter.WriteLine("\"");
                    }
                    this._ExportWriter.WriteLine(" />");
                }
                this._ExportWriter.WriteLine("</" + this._Title.Description + ">");
                if (this._isDemo) this._ExportWriter.WriteLine("<!-- " + this._msg + " -->");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return str;
        }

        public bool ExportToCSV(DataSet ds) { return this.ExportToCSV(ds.Tables[0]); }

        public bool ExportToCSV(DataTable dt)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "csv", false) != 0) throw new Exception(this.ExportFileName + " is not a valid CSV file name.");
                this.Initialize();
                this._Delimeter = ",";
                this.DataTableToText(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToCSV(DataView dv)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "csv", false) != 0) throw new Exception(this.ExportFileName + " is not a valid CSV file name.");
                this.Initialize();
                this._Delimeter = ",";
                this.DataViewToText(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToCSV(OleDbDataReader dr) { return this.ExportToCSV(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToCSV(SqlDataReader dr) { return this.ExportToCSV(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToDoc(DataSet ds) { return this.ExportToDoc(ds.Tables[0]); }

        public bool ExportToDoc(DataTable dt)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "doc", false) != 0) throw new Exception(this.ExportFileName + " is not a valid word document file name.");
                this.Initialize();
                this.DataTableToHtml(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToDoc(DataView dv)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "doc", false) != 0) throw new Exception(this.ExportFileName + " is not a valid word document file name.");
                this.Initialize();
                this.DataViewToHtml(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToDoc(OleDbDataReader dr) { return this.ExportToDoc(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToDoc(SqlDataReader dr) { return this.ExportToDoc(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToExcel(DataSet ds) { return this.ExportToExcel(ds.Tables[0]); }

        public bool ExportToExcel(DataTable dt)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "xls", false) != 0) throw new Exception(this.ExportFileName + " is not a valid Excel file name.");
                this.Initialize();
                this.DataTableToHtml(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToExcel(DataView dv)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "xls", false) != 0) throw new Exception(this.ExportFileName + " is not a valid Excel file name.");
                this.Initialize();
                this.DataViewToHtml(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToExcel(OleDbDataReader dr) { return this.ExportToExcel(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToExcel(SqlDataReader dr) { return this.ExportToExcel(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToHtml(DataSet ds) { return this.ExportToHtml(ds.Tables[0]); }

        public bool ExportToHtml(DataTable dt)
        {
            bool flag = false;
            try
            {
                if (!(StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "htm", false) == 0 | StringType.StrCmp(Strings.Right(this.ExportFileName, 4).ToLower(), "html", false) == 0)) throw new Exception(this.ExportFileName + " is not a valid Html file name.");
                this.Initialize();
                this.DataTableToHtml(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToHtml(DataView dv)
        {
            bool flag= false;
            try
            {
                if (!(StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "htm", false) == 0 | StringType.StrCmp(Strings.Right(this.ExportFileName, 4).ToLower(), "html", false) == 0)) throw new Exception(this.ExportFileName + " is not a valid Html file name.");
                this.Initialize();
                this.DataViewToHtml(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToHtml(OleDbDataReader dr) { return this.ExportToHtml(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToHtml(SqlDataReader dr) { return this.ExportToHtml(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToTextFile(DataSet ds) { return this.ExportToTextFile(ds.Tables[0]); }

        public bool ExportToTextFile(DataTable dt)
        {
            bool flag =false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "txt", false) != 0) throw new Exception(this.ExportFileName + " is not a valid text file name.");
                this.Initialize();
                this.DataTableToText(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToTextFile(DataView dv)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "txt", false) != 0) throw new Exception(this.ExportFileName + " is not a valid text file name.");
                this.Initialize();
                this.DataViewToText(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToTextFile(OleDbDataReader dr) { return this.ExportToTextFile(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToTextFile(SqlDataReader dr) { return this.ExportToTextFile(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToXml(DataSet ds) { return this.ExportToXml(ds.Tables[0]); }

        public bool ExportToXml(DataTable dt)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "xml", false) != 0) throw new Exception(this.ExportFileName + " is not a valid XML file name.");
                this.Initialize();
                this.DataTableToXml(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToXml(DataView dv)
        {
            bool flag=false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "xml", false) != 0) throw new Exception(this.ExportFileName + " is not a valid XML file name.");
                this.Initialize();
                this.DataViewToXml(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToXml(OleDbDataReader dr) { return this.ExportToXml(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToXml(SqlDataReader dr) { return this.ExportToXml(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToXmlWithAttributes(DataSet ds) { return this.ExportToXmlWithAttributes(ds.Tables[0]); }

        public bool ExportToXmlWithAttributes(DataTable dt)
        {
            bool flag=false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "xml", false) != 0) throw new Exception(this.ExportFileName + " is not a valid XML file name.");
                this.Initialize();
                this.DataTableToXmlWithAttributes(dt);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToXmlWithAttributes(DataView dv)
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(Strings.Right(this.ExportFileName, 3).ToLower(), "xml", false) != 0) throw new Exception(this.ExportFileName + " is not a valid XML file name.");
                this.Initialize();
                this.DataViewToXmlWithAttributes(dv);
                this.Finish();
                flag = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        public bool ExportToXmlWithAttributes(OleDbDataReader dr) { return this.ExportToXmlWithAttributes(this.ConvertDataReaderToDataTable(dr)); }

        public bool ExportToXmlWithAttributes(SqlDataReader dr) { return this.ExportToXmlWithAttributes(this.ConvertDataReaderToDataTable(dr)); }

        private void Finish()
        {
            try
            {
                this._ExportWriter.Close();
                this.PopUpWindow();
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
        }

        private string GetStyle(Font f, Color c, Color bc, EnumAlignment a)
        {
            StringBuilder builder = new StringBuilder();
            if (f != null)
            {
                builder.Append("font-family:");
                builder.Append(f.Name);
                builder.Append(";");
                if (f.Bold) builder.Append(" font-weight:bold;");
                if (f.Italic) builder.Append(" font-style:italic;");
                if (f.Underline)
                {
                    builder.Append(" text-decoration:underline");
                    if (f.Strikeout) builder.Append(" line-through");
                    builder.Append(";");
                }
                builder.Append(" font-size:");
                builder.Append(f.Size.ToString());
                builder.Append("pt;");
            }
            WebColorConverter converter = new WebColorConverter();
            if (!c.IsEmpty)
            {
                builder.Append(" color:");
                builder.Append(converter.ConvertToString(c));
                builder.Append(";");
            }
            if (!bc.IsEmpty)
            {
                builder.Append(" background-color:");
                builder.Append(converter.ConvertToString(bc));
                builder.Append(";");
            }
            if (a != EnumAlignment.Left)
            {
                builder.Append(" text-align:");
                builder.Append(a.ToString());
                builder.Append(";");
            }
            return builder.ToString();
        }

        private string GetStyleFromDisplaySetting(DisplaySetting d)
        {
            string style;
            if (d.Style.Length == 0)
                style = this.GetStyle(d.Font, d.ForeColor, d.BackColor, d.Alignment);
            else
                style = d.Style;
            if (style.IndexOf("vertical-align") <= 0) style = style + " vertical-align: middle;";
            return style;
        }

        private bool Initialize()
        {
            bool flag = false;
            try
            {
                if (StringType.StrCmp(this._ExportFileName, "", false) != 0)
                {
                    this._ExportWriter = new StreamWriter(this._ExportFileName, false, Encoding.UTF8);
                    return true;
                }
                flag = false;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
            return flag;
        }

        private void PopUpWindow()
        {
            if (this.ShowPopupWindow & this.PopupURL.Length > 0) this._text = "<script language='javascript'>window.open('" + this.PopupURL + "')</script>";
        }

        protected override void Render(HtmlTextWriter output) { output.Write(this._text); }

        private void WriteStyle()
        {
            this._ExportWriter.WriteLine("<style>");
            this._ExportWriter.WriteLine(".title{" + this.GetStyleFromDisplaySetting(this._Title) + "}");
            this._ExportWriter.WriteLine(".columnheader{" + this.GetStyleFromDisplaySetting(this._ColumnHeader) + "}");
            this._ExportWriter.WriteLine(".data{" + this.GetStyleFromDisplaySetting(this._Data) + "}");
            WebColorConverter converter = new WebColorConverter();
            if (!this._AlternateRowBackColor.IsEmpty)
                this._ExportWriter.WriteLine(".alternaterowdata{" + this.GetStyleFromDisplaySetting(this._Data) + " background-color:" + converter.ConvertToString(this._AlternateRowBackColor) + "}");
            else
                this._ExportWriter.WriteLine(".alternaterowdata{" + this.GetStyleFromDisplaySetting(this._Data) + "}");
            this._ExportWriter.WriteLine(".footer{" + this.GetStyleFromDisplaySetting(this._Footer) + "}");
            this._ExportWriter.WriteLine("</style>");
        }

        [Bindable(true), Category("Display"), Description("Specify the alternate row's back ground color to for display data.")]
        public Color AlternateRowBackColor { get { return this._AlternateRowBackColor; } set { this._AlternateRowBackColor = value; } }

        [Description("Specify the font to use for column header."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Display"), Bindable(true)]
        public Font ColomnHeaderFont { get { return this._ColumnHeader.Font; } set { this._ColumnHeader.Font = value; } }

        [Description("Specify the column header's alignment."), Category("Display"), DefaultValue("1"), Bindable(true)]
        public EnumAlignment ColumnHeaderAlignment { get { return this._ColumnHeader.Alignment; } set { this._ColumnHeader.Alignment = value; } }

        [Category("Display"), Bindable(true), Description("Specify the backcolor to use for column header."), TypeConverter("System.Web.UI.WebControls.WebColorConverter")]
        public Color ColumnHeaderBackColor { get { return this._ColumnHeader.BackColor; } set { this._ColumnHeader.BackColor = value; } }

        [Description("Specify the forecolor to use for column header."), Category("Display"), TypeConverter("System.Web.UI.WebControls.WebColorConverter"), Bindable(true)]
        public Color ColumnHeaderForeColor { get { return this._ColumnHeader.ForeColor; } set { this._ColumnHeader.ForeColor = value; } }

        [Bindable(true), Description("Specify the custom style to be used to display the column header, this will overide the ColumnHeaderFont and ColumnHeaderColor specified."), Category("Display")]
        public string ColumnHeaderStyle { get { return this._ColumnHeader.Style; } set { this._ColumnHeader.Style = value; } }

        [Description("Set the content type for the data to be exported."), Bindable(true), Category("ExportData")]
        public string ContentType { get { return this._ContentType; } set { this._ContentType = value; } }

        [Category("Display"), DefaultValue("0"), Bindable(true), Description("Specify the data's alignment.")]
        public EnumAlignment DataAlignment { get { return this._Data.Alignment; } set { this._Data.Alignment = value; } }

        [TypeConverter("System.Web.UI.WebControls.WebColorConverter"), Bindable(true), Category("Display"), Description("Specify the backcolor to use for data.")]
        public Color DataBackColor { get { return this._Data.BackColor; } set { this._Data.BackColor = value; } }

        [Category("Display"), Bindable(true), Description("Specify the border color for generated data."), TypeConverter("System.Web.UI.WebControls.WebColorConverter")]
        public Color DataBorderColor { get { return this._DataBorderColor; } set { this._DataBorderColor = value; } }

        [Category("Display"), Bindable(true), DefaultValue(1), Description("Specify the border size for generated data.")]
        public int DataBorderSize { get { return this._DataBorderSize; } set { this._DataBorderSize = value; } }

        [Category("Display"), Description("Specify the font to use for data."), Bindable(true)]
        public Font DataFont { get { return this._Data.Font; } set { this._Data.Font = value; } }

        [TypeConverter("System.Web.UI.WebControls.WebColorConverter"), Description("Specify the forecolor to use for data."), Bindable(true), Category("Display")]
        public Color DataForeColor { get { return this._Data.ForeColor; } set { this._Data.ForeColor = value; } }

        [Category("Display"), Bindable(true), Description("Specify the custom style to be used to display the data, this will overide the DataFont and DataColor specified.")]
        public string DataStyle { get { return this._Data.Style; } set { this._Data.Style = value; } }

        [Category("Display"), Description("Do not display if data in a cell is numeric type and value is 0."), Bindable(true)]
        public bool DataSuppressZero { get { return this._DataSuppressZero; } set { this._DataSuppressZero = value; } }

        [Category("ExportData"), Description("Specify the delimiter to use when export to delimeted file."), Bindable(true)]
        public string Delimeter { get { return this._Delimeter; } set { this._Delimeter = value; } }

        [Category("ExportData"), Description("Specify the file name which data will be exported."), Bindable(true)]
        public string ExportFileName { get { return this._ExportFileName; } set { this._ExportFileName = value; } }

        [Bindable(true), Description("Specify the footer to be displayed together with the exported data."), Category("Display")]
        public string Footer { get { return this._Footer.Description; } set { this._Footer.Description = value; } }

        [DefaultValue("Center"), Bindable(true), Category("Display"), Description("Specify the footer's alignment.")]
        public EnumAlignment FooterAlignment { get { return this._Footer.Alignment; } set { this._Footer.Alignment = value; } }

        [Category("Display"), Description("Specify the backcolor to use for footer."), Bindable(true), TypeConverter("System.Web.UI.WebControls.WebColorConverter")]
        public Color FooterBackColor { get { return this._Footer.BackColor; } set { this._Footer.BackColor = value; } }

        [Description("Specify the font to use for footer."), Category("Display"), Bindable(true)]
        public Font FooterFont { get { return this._Footer.Font; } set { this._Footer.Font = value; } }

        [Bindable(true), Description("Specify the forecolor to use for footer."), Category("Display"), TypeConverter("System.Web.UI.WebControls.WebColorConverter")]
        public Color FooterForeColor { get { return this._Footer.ForeColor; } set { this._Footer.ForeColor = value; } }

        [Category("Display"), Description("Specify the custom style to be used to display the footer, this will overide the FooterFont and FooterColor specified."), Bindable(true)]
        public string FooterStyle { get { return this._Footer.Style; } set { this._Footer.Style = value; } }

        [Description("Specify the URL which popup window will show exported file."), Category("ExportData"), Bindable(true)]
        public string PopupURL { get { return this._PopupURL; } set { this._PopupURL = value; } }

        [Description("Display border or not for the generated data."), Bindable(true), Category("Display")]
        public bool ShowBorder { get { return this._ShowBorder; } set { this._ShowBorder = value; } }

        [Description("Display column header or not for the generated data."), Category("Display"), Bindable(true)]
        public bool ShowColumnHeader { get { return this._ColumnHeader.Show; } set { this._ColumnHeader.Show = value; } }

        [Bindable(true), DefaultValue("True"), Category("ExportData"), Description("After export, show exported file in a popup window or not.")]
        public bool ShowPopupWindow { get { return this._ShowPopupWindow; } set { this._ShowPopupWindow = value; } }

        [Category("Display"), Bindable(true), Description("Specify the title to be displayed together with the exported data.")]
        public string Title { get { return this._Title.Description; } set { this._Title.Description = value; } }

        [Bindable(true), Description("Specify the title's alignment."), Category("Display")]
        public EnumAlignment TitleAlignment { get { return this._Title.Alignment; } set { this._Title.Alignment = value; } }

        [Description("Specify the backcolor to use for title."), Bindable(true), Category("Display"), TypeConverter("System.Web.UI.WebControls.WebColorConverter")]
        public Color TitleBackColor { get { return this._Title.BackColor; } set { this._Title.BackColor = value; } }

        [Description("Specify the font to use for title."), Bindable(true), Category("Display")]
        public Font TitleFont { get { return this._Title.Font; } set { this._Title.Font = value; } }

        [TypeConverter("System.Web.UI.WebControls.WebColorConverter"), Bindable(true), Category("Display"), Description("Specify the forecolor to use for title.")]
        public Color TitleForeColor { get { return this._Title.ForeColor; } set { this._Title.ForeColor = value; } }

        [Category("Display"), Description("Specify the custom style to be used to display the title, this will overide the TitleFont and TitleColor specified."), Bindable(true)]
        public string TitleStyle { get { return this._Title.Style; } set { this._Title.Style = value; } }

        private class DisplaySetting
        {
            private ExportDotNet.EnumAlignment _Alignment = ExportDotNet.EnumAlignment.Left;
            private Color _BackColor;
            private string _Description;
            private System.Drawing.Font _Font;
            private Color _ForeColor;
            private bool _Show = true;
            private string _Style = "";

            private System.Drawing.Font CheckFontFormat(System.Drawing.Font fontId, string propName)
            {
                int num = 0;
                System.Drawing.Font font2 = null;
                if (!Information.IsNothing(fontId))
                {
                    System.Drawing.Font font3 = fontId;
                    if (font3.Bold) num++;
                    if (font3.Italic) num++;
                    if (font3.Underline) num++;
                    if (font3.Strikeout) num++;
                    if (num > 1)
                    {
                        if (MessageBox.Show("You have more than one style for the " + propName + " property. Our testing has shown this to cause errors. Would you like to reduce to a single style?", "Too Many Styles", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==  DialogResult.Yes)
                        {
                           System.Drawing. FontStyle style = 0;
                            if (font3.Bold)
                                style =  System.Drawing.FontStyle.Bold;
                            else if (font3.Italic)
                                style = System.Drawing.FontStyle.Italic;
                            else if (font3.Underline)
                                style = System.Drawing.FontStyle.Underline;
                            else if (font3.Strikeout) style = System.Drawing.FontStyle.Strikeout;
                            font2 = new System.Drawing.Font(fontId, style);
                        }
                    }
                    else
                        font2 = fontId;
                    font3 = null;
                }
                return font2;
            }

            public ExportDotNet.EnumAlignment Alignment { get { return this._Alignment; } set { this._Alignment = value; } }

            public Color BackColor { get { return this._BackColor; } set { this._BackColor = value; } }

            public string Description { get { return this._Description; } set { this._Description = value; } }

            public System.Drawing.Font Font { get { return this._Font; } set { this._Font = this.CheckFontFormat(value, "Font"); } }

            public Color ForeColor { get { return this._ForeColor; } set { this._ForeColor = value; } }

            public bool Show { get { return this._Show; } set { this._Show = value; } }

            public string Style { get { return this._Style; } set { this._Style = value; } }
        }

        public enum EnumAlignment
        {
            Left,
            Center,
            Right
        }
    }
}
