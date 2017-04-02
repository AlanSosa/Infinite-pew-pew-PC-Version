using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Infinite_pew_pew.ScreenManager
{
    public enum Log_Type
    {
        ERROR = 0,
        WARNING = 1,
        INFO = 2
    }

    public static class Logger
   {
        static private bool _active;  // In case you want to deactivate the logger
 
        static public void init()
        {
            _active = true;
#if WINDOWS
 
            StreamWriter textOut = new StreamWriter(new FileStream("log.html", FileMode.Create, FileAccess.Write));
            textOut.WriteLine("Log File");
            textOut.WriteLine("");
            textOut.WriteLine("<span style=font-family: &quot;Kootenay&quot;; color: #000000;>");
            textOut.WriteLine("Log started at " + System.DateTime.Now.ToLongTimeString()+"</span><hr />");
            textOut.Close();
#endif
        }
 
        static public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
 
        static public void log(Log_Type type, string text)
        {
#if WINDOWS
            if (!_active) return;
            string begin = "";
            switch (type)
            {
                case Log_Type.ERROR: begin = "<span style=color: #00f000;>"; break;
                case Log_Type.INFO: begin = "<span style=color: #0008f0;>"; break;
                case Log_Type.WARNING: begin = "<span style=color: #00ff00;>"; break;
            }
            text = begin+System.DateTime.Now.ToLongTimeString() + " : " + text + "</span><br>";
            Output(text);
#endif
        }
 
        static private void Output(string text)
        {
#if WINDOWS
            try
            {
                StreamWriter textOut = new StreamWriter(new FileStream("log.html", FileMode.Append, FileAccess.Write));
                textOut.WriteLine(text);
                textOut.Close();
            }
            catch (System.Exception e)
            {
                string error = e.Message;
            }
 
#endif
        }
    }
}
