﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace AgOpenGPS
{
    public partial class FormFieldDir : Form
    {
        //class variables
        private FormGPS mf = null;


        public FormFieldDir(Form callingForm)
        {

            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            //fill something in
            if (String.IsNullOrEmpty(tboxFieldName.Text)) tboxFieldName.Text = "XX";

            //append date time to name
            mf.currentFieldDirectory = tboxFieldName.Text.Trim() +
                String.Format("{0}", DateTime.Now.ToString("_MMMdd"));
            try
            {
                //get the directory and make sure it exists, create if not
                string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                               "\\AgOpenGPS\\Fields\\" + mf.currentFieldDirectory + "\\";

                //make sure directory exists, or create it for first save
                string directoryName = Path.GetDirectoryName(dirField);

                if ((directoryName.Length > 0) && (Directory.Exists(directoryName)))
                {
                    MessageBox.Show("Choose a different name", "Directory Exists", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                else
                {
                    //reset the offsets
                    mf.pn.utmEast = (int)mf.pn.actualEasting;
                    mf.pn.utmNorth = (int)mf.pn.actualNorthing;

                    mf.worldGrid.CreateWorldGrid(0, 0);

                    //make sure directory exists, or create it
                    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
                    { Directory.CreateDirectory(directoryName); }

                    //if you can't save here, you're screwed
                    mf.FileSaveField();
                    mf.FileSaveContour();
                    mf.FileSaveFlags();
                }
            }

            catch (Exception err)
            {
                MessageBox.Show("Error", err.ToString());
                mf.currentFieldDirectory = "";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9a-zA-Z ]", "");
            textboxSender.SelectionStart = cursorPosition;

        }

      
    }
}
