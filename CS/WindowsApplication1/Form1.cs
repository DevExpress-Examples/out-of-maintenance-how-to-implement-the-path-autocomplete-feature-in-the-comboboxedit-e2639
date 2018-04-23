using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.Utils.Win;
using DevExpress.XtraEditors.Popup;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            new AutoCompleteFilesHelper(comboBoxEdit1);
        }
    }

    public class DropDownListHelper
    {
        
      public static List<string> GetDropDownList(string path)
      {
          List<string> result = new List<string>();
          path = CheckPath(path);
          if (!Directory.Exists(path))
              return result;
          result.AddRange(GetFiles(path));
          result.AddRange(GetFolders(path));
          return result;
      }
        public static string CheckPath(string path)
        {
            if (path == string.Empty)
                return path;
            string result = Path.GetDirectoryName(path);
            if (Directory.Exists(result))
                return result;
            result = path + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
            if (Directory.Exists(result))
                return result;
            return path;
        }

        private static string[] GetFiles(string path)
        {
            string[] result = new string[] { };
            try
            {

                result = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }

        private static string[] GetFolders(string path)
        {
            string[] result = new string[] { }; ;
            try
            {

                result = Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }

    public class AutoCompleteFilesHelper
    {

        private ComboBoxEdit _Editor;
        public AutoCompleteFilesHelper(ComboBoxEdit editor)
        {
            _Editor = editor;
            _Editor.EditValueChanged += new EventHandler(_Editor_EditValueChanged);
            _Editor.Properties.ImmediatePopup = true;
            _Editor.KeyDown += new KeyEventHandler(_Editor_KeyDown);
        }

        void _Editor_KeyDown(object sender, KeyEventArgs e)
        {
            TextEdit editor = sender as TextEdit;
            if (e.KeyCode == Keys.End)
            {
                editor.SelectionStart = editor.Text.Length;
                e.Handled = true;
            }
 
        }

        void _Editor_EditValueChanged(object sender, EventArgs e)
        {
            UpdateDataSource();
        }

        private void UpdateDataSource()
        {
            bool needOpen = _Editor.IsPopupOpen;
            RepopulateList();
            if (needOpen)
                _Editor.ShowPopup();
        }
        private void RepopulateList()
        {
            _Editor.Properties.Items.Clear();
            _Editor.Properties.Items.AddRange(DropDownListHelper.GetDropDownList(_Editor.Text));
        }
    }
}