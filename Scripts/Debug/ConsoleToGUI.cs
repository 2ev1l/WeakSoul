using UnityEngine;

namespace DebugStuff
{
    public class ConsoleToGUI : MonoBehaviour
    {
#if false
#if !UNITY_EDITOR
#region fields
        private string myLog = "*Press [Space] to open/close developer window";
        private string filename = "";
        private bool doShow = true;
        private int kChars = 700;
        private bool fileLogEnabled = false;
#endregion fields

#region methods
        void OnEnable() 
        { 
            Application.logMessageReceived += Log; 
        }
        void OnDisable() 
        { 
            Application.logMessageReceived -= Log; 
        }
        void Update()
        { 
            if (Input.GetKeyDown(KeyCode.Space)) 
            { 
                doShow = !doShow; 
            } 
        }
        public void Log(string logString, string stackTrace, LogType type)
        {
            myLog = myLog + "\n" + logString;
            if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }

            if (!fileLogEnabled) return;

            if (filename == "")
            {
                string d = System.Environment.GetFolderPath(
                   System.Environment.SpecialFolder.Desktop) + "/WEAK_SOUL_LOGS";
                System.IO.Directory.CreateDirectory(d);
                string r = Random.Range(1000, 9999).ToString();
                filename = d + "/log-" + r + ".txt";
            }
            try { System.IO.File.AppendAllText(filename, logString + "\n"); }
            catch { }
        }

        void OnGUI()
        {
            if (!doShow) return;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
               new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
            GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
        }
#endregion methods
#endif
#endif
    }
}