using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;

using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace DNNGo.Modules.PowerForms
{
    public partial class ajaxCaptcha : System.Web.UI.Page
    {

        #region "==����=="

        private Hashtable _PowerForms_Settings = new Hashtable();
        /// <summary>
        /// ������ģ������
        /// </summary>
        public Hashtable PowerForms_Settings
        {
            get
            {
                if (!(_PowerForms_Settings != null && _PowerForms_Settings.Count > 0))
                {
                    _PowerForms_Settings = new DotNetNuke.Entities.Modules.ModuleController().GetModule(ModuleID).ModuleSettings;
                }
                return _PowerForms_Settings;
            }
        }

        /// <summary>
        /// ģ����
        /// </summary>
        public Int32 ModuleID
        {
            get { return WebHelper.GetIntParam(Request, "ModuleID", 0); }
        }


        /// <summary>
        /// ��֤����ַ���
        /// </summary>
        public String Settings_Characters
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_Characters"] != null ? Convert.ToString(PowerForms_Settings["PowerForms_Recaptcha_Characters"]) : "abcdefghijkmnpqrstuvwxyz23456789@"; }
        }

        /// <summary>
        /// ��֤�����λ��
        /// </summary>
        public Int32 Settings_MaxNumber
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_MaxNumber"] != null ? Convert.ToInt32(PowerForms_Settings["PowerForms_Recaptcha_MaxNumber"]) : 5; }
        }
        /// <summary>
        /// ��֤����Сλ��
        /// </summary>
        public Int32 Settings_MinNumber
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_MinNumber"] != null ? Convert.ToInt32(PowerForms_Settings["PowerForms_Recaptcha_MinNumber"]) : 4; }
        }
        /// <summary>
        /// ��֤��ͼƬ�Ŀ��
        /// </summary>
        public Int32 Settings_imgWidth
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_imgWidth"] != null ? Convert.ToInt32(PowerForms_Settings["PowerForms_Recaptcha_imgWidth"]) : 120; }
        }
        /// <summary>
        /// ��֤��ͼƬ�ĸ߶�
        /// </summary>
        public Int32 Settings_imgHeight
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_imgHeight"] != null ? Convert.ToInt32(PowerForms_Settings["PowerForms_Recaptcha_imgHeight"]) : 30; }
        }

        public Int32 Settings_FontSize
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_FontSize"] != null ? Convert.ToInt32(PowerForms_Settings["PowerForms_Recaptcha_FontSize"]) : 25; }
        }

        /// <summary>
        /// ����������
        /// </summary>
        public Boolean Settings_BackgroundNoiseLine
        {
            get { return PowerForms_Settings["PowerForms_Recaptcha_NoiseLine"] != null ? Convert.ToBoolean(PowerForms_Settings["PowerForms_Recaptcha_NoiseLine"]) : true; }
        }


        #endregion







        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    //���ú�������֤������ͼƬ
                    this.CreateCheckCodeImage(GenerateCheckCode());
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        private string GenerateCheckCode()
        {


            if (!HttpContext.Current.Items.Contains("SessionCaptcha"))
            {
                HttpContext.Current.Items.Add("SessionCaptcha", "true");
            }
            else
            {
                Thread.Sleep(100);
            }



            String SessionCaptcha = String.Format("SessionCaptcha_{0}", ModuleID);

            //������λ������ַ���
            int number;
            string checkCode = String.Empty;
            string Characters = Settings_Characters;
            Int32 MaxNumber = Settings_MaxNumber;
            Int32 MinNumber = Settings_MinNumber;
            Int32 Number = MaxNumber > MinNumber ? rnd.Next(MinNumber, MaxNumber) : MaxNumber;
            for (int i = 0; i < Number; i++)
            {
                number = rnd.Next(0, Characters.Length - 1);
                checkCode += Characters.Substring(number, 1);
            }

            if (HttpContext.Current.Response.Cookies[SessionCaptcha] != null)
            {
                HttpContext.Current.Response.Cookies.Remove(SessionCaptcha); // [SessionCaptcha].Value = checkCode;//���ڿͻ���У����Ƚ�
            }
            HttpCookie c = new HttpCookie(SessionCaptcha, checkCode);
            c.Expires =DateTime.Now.AddMinutes(30);
            HttpContext.Current.Response.Cookies.Add(c);


            return checkCode;
        }

        private void CreateCheckCodeImage(string checkCode)
        {  //����֤������ͼƬ��ʾ
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(Settings_imgWidth, Settings_imgHeight);
            Graphics g = Graphics.FromImage(image);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            try
            {
                //������������� 
                Random random = new Random();

                //���ͼƬ����ɫ 
                g.Clear(Color.White);

                StringFormat sf = new StringFormat(StringFormatFlags.NoClip);
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                //�����С
                Int32 fontsize = Settings_FontSize;

                //List<FontStyle> a = GetRndColor(checkCode.Length);
                FontStyle fontcolor= GetColorList[random.Next(0,GetColorList.Count)];
                //������
                int charX, CharY, charSize, charFontSize;

                //�Ƴ����
                int delwidth = 0;
                //��ת�Ƕ�
                int rotatefont = 0;
                Matrix mat = new Matrix();
                for (int i = 0; i < checkCode.Length; i++)
                {
                    if (i != 0)
                    {
                        delwidth = rnd.Next(fontsize/4, fontsize / 3);
                    }
                    rotatefont = random.Next(-30, 30);
                    charFontSize = rnd.Next(fontsize-5, fontsize);
                    charSize = (Settings_imgWidth - 5) / checkCode.Length;
                    charX = charSize * (i)+rnd.Next(2,15);
                    CharY = rnd.Next(1, Settings_imgHeight - (charFontSize)) - 7;

                    mat.RotateAt(rotatefont, new PointF(charX, fontsize));
                    g.Transform = mat;

                    g.DrawString(checkCode[i].ToString(), new System.Drawing.Font("garamond", charFontSize, (System.Drawing.FontStyle.Bold)), new SolidBrush(fontcolor.FontColor), charX - delwidth, CharY);

                    mat.RotateAt(rotatefont*-1, new PointF(charX, fontsize));
                    g.Transform = mat;
                }

                //��ͼƬ�ı��������� 
                if (Settings_BackgroundNoiseLine)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int x1 = 0;
                        int x2 = image.Width;
                        int y1 = random.Next(image.Height);
                        int y2 = random.Next(image.Height);
                        if (i == 0)
                        {
                            g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
                        }
                    }
                }


                g.Transform = new Matrix();

                //��ͼƬ�ı߿��� 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                Response.ClearContent();
                Response.ContentType = "image/png";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }



        /// <summary>
        /// ��ȡ��ɫ�б�
        /// </summary>
        private List<FontStyle> GetColorList
        {
            get {
                Int32 fontsize = Settings_FontSize;
                List<FontStyle> a = new List<FontStyle>(15);
                a.Add(new FontStyle(Color.Red, fontsize));
                a.Add(new FontStyle(Color.Green, fontsize));
                a.Add(new FontStyle(Color.Blue, fontsize));
                a.Add(new FontStyle(Color.DarkOrange, fontsize));
                a.Add(new FontStyle(Color.Brown, fontsize));
                a.Add(new FontStyle(Color.DarkOliveGreen, fontsize));
                a.Add(new FontStyle(Color.Gold, fontsize));
                a.Add(new FontStyle(Color.DarkSlateBlue, fontsize));
                a.Add(new FontStyle(Color.GreenYellow, fontsize));
                a.Add(new FontStyle(Color.HotPink, fontsize));
                a.Add(new FontStyle(Color.Khaki, fontsize));
                a.Add(new FontStyle(Color.LightBlue, fontsize));
                a.Add(new FontStyle(Color.PaleGreen, fontsize));
                a.Add(new FontStyle(Color.SteelBlue, fontsize));
                a.Add(new FontStyle(Color.Tomato, fontsize));
                return a;
            }
        }

        /// <summary>
        /// ��������ɫ
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private List<FontStyle> GetRndColor(int num)
        {
            List<FontStyle> l = new List<FontStyle>(num);
            
            for (int i = 0; i < num; i++)
            {
                l.Add(GetColorList[rnd.Next(0, GetColorList.Count)]);
            }
            return l;

        }

        Random rnd = new Random();

    }
    

    /// <summary>
    /// ������
    /// </summary>
    public class FontStyle
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="FontColor">��ɫ</param>
        /// <param name="FontSize">�����С</param>
        public FontStyle(Color FontColor, int FontSize)
        {
            _FontColor = FontColor;
            _FontSize = FontSize;
        }
        #region "Private Variables"
        private Color _FontColor;
        private int _FontSize;
        #endregion

        #region "Public Variables"
        /// <summary>
        /// ������ɫ
        /// </summary>
        public Color FontColor
        {
            get {
                return _FontColor;
            }
            set {
                _FontColor = value;
            }
        }
        /// <summary>
        /// �����С
        /// </summary>
        public int FontSize
        {
            get {
                return _FontSize;
            }
            set {
                _FontSize = value;
            }
        }
        #endregion
    }
}