using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PWF_Report_Updater
{
    public partial class Form1 : Form
    {
        ExcelHelper oXL;
        PWFAPIHelper oAPI;
        public Form1()
        {
            InitializeComponent();

            oXL = new ExcelHelper();
            oAPI = new PWFAPIHelper();

            dateToPicker.MaxDate = DateTime.Today;
            dateFromPicker.MaxDate = DateTime.Today;

            string timeFrom = DateTime.Today.ToString("yyyy-MM");
            timeFrom = timeFrom + "-01";
            string timeTo = DateTime.Today.ToString("yyyy-MM-dd");
            string reportName = dateFromPicker.Value.Date.ToString("yyyy-MM");
            // Populate DataGridView with Projects
            List<JsonResultTasks> oTasks = new List<JsonResultTasks>();
            JsonResultProjects oProjects = oAPI.GetProjects();
            JsonResultTimes oTimes = oAPI.GetTime(timeFrom, timeTo);


            foreach (JsonResultProject project in oProjects.projects)
            {
                JsonResultTasks tasks = oAPI.GetTasks(project.id.ToString());
                oTasks.Add(tasks);
            }

            oXL.PopulateProjects(oProjects, oTasks);
            oXL.PopulateTime(oTimes, reportName);
            //oXL.PopulateBilling(oTimes);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release helper class instances
            oAPI = null;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            string timeFrom = dateFromPicker.Value.Date.ToString("yyyy-MM-dd");
            string timeTo = dateToPicker.Value.Date.ToString("yyyy-MM-dd");
            string reportName = dateFromPicker.Value.Date.ToString("yyyy-MM");
            // Populate DataGridView with Projects
            List<JsonResultTasks> oTasks = new List<JsonResultTasks>();
            JsonResultProjects oProjects = oAPI.GetProjects();
            JsonResultTimes oTimes = oAPI.GetTime(timeFrom, timeTo);


            foreach (JsonResultProject project in oProjects.projects)
            {
                JsonResultTasks tasks = oAPI.GetTasks(project.id.ToString());
                oTasks.Add(tasks);
            }

            oXL.PopulateProjects(oProjects, oTasks);
            oXL.PopulateTime(oTimes, reportName);
            //oXL.PopulateBilling(oTimes);
        }
    }
}
