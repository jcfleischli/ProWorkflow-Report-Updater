using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;

namespace PWF_Report_Updater
{
    class ExcelHelper
    {
        public void PopulateProjects(JsonResultProjects oProjects, List<JsonResultTasks> oTasks)
        {
            JsonResultProject oProject;

            SLDocument sl = new SLDocument();

            int row = 1;

            try
            {
                for (int i = 0; i < oProjects.projects.Count; i++)
                {
                    // Extract a project
                    oProject = oProjects.projects.ElementAt(i);

                    for (int j = 0; j < oTasks[i].tasks.Count; j++)
                    {
                        sl.SetCellValue("A" + row, oTasks[i].tasks[j].companyname);
                        sl.SetCellValue("B" + row, oProject.title);
                        sl.SetCellValue("C" + row, oProject.categoryname);
                        sl.SetCellValue("D" + row, oProject.managername);
                        sl.SetCellValue("E" + row, oProject.startdate.Split('T')[0]);
                        sl.SetCellValue("F" + row, oProject.duedate.Split('T')[0]);
                        sl.SetCellValue("G" + row, oTasks[i].tasks[j].completedate.Split('T')[0]);
                        sl.SetCellValue("H" + row, oTasks[i].tasks[j].name);
                        sl.SetCellValue("I" + row, oTasks[i].tasks[j].status);
                        sl.SetCellValue("J" + row, oTasks[i].tasks[j].ordernumber);
                        row++;
                    }
                }
                FormatProjectReport(sl);

                sl.SaveAs("\\\\bigdell\\Clients\\Omega\\00 Analytics\\Project Status\\Data\\Active-Projects-Tasks.xlsx");
            }
            catch(Exception e)
            {
                SlackClient client = new SlackClient("");
                client.PostMessage("PWF API Tool Error: " + e.ToString(), "PWF API Admin", "C9X3YGW5B");
                Environment.Exit(1);
            }
            
        }

        public void PopulateTime(JsonResultTimes oTimes, string reportName)
        {
            JsonResultTime oTime;
            SLDocument sl = new SLDocument();
            int row = 1;

            try
            {
                for (int i = 0; i < oTimes.timerecords.Count; i++)
                {
                    oTime = oTimes.timerecords.ElementAt(i);

                    sl.SetCellValue("A" + row, oTime.companyname);
                    sl.SetCellValue("B" + row, oTime.projecttitle);
                    sl.SetCellValue("C" + row, oTime.categoryname);
                    sl.SetCellValue("D" + row, oTime.contactname);
                    sl.SetCellValue("E" + row, oTime.taskname);
                    sl.SetCellValue("F" + row, oTime.timetracked / 60);
                    sl.SetCellValue("G" + row, oTime.starttime.ToString("MM-dd-yyy"));
                    row++;
                }
                FormatTimeReport(sl);

                sl.SaveAs("\\\\bigdell\\Clients\\Omega\\00 Analytics\\Hours\\Data\\Actual\\" + reportName + ".xlsx");
            }
            catch(Exception e)
            {
                SlackClient client = new SlackClient("");
                client.PostMessage("PWF API Tool Error: " + e.ToString(), "PWF API Admin", "C9X3YGW5B");
                Environment.Exit(1);
            }
            
        }

        public void PopulateBilling(JsonResultTimes oTimes)
        {
            JsonResultTime oTime;
            SLDocument sl = new SLDocument();
            int row = 1;
        
            try
            {
                for (int i = 0; i < oTimes.timerecords.Count; i++)
                {
                    oTime = oTimes.timerecords.ElementAt(i);
        
                    sl.SetCellValue("A" + row, oTime.projecttitle);
                    sl.SetCellValue("B" + row, oTime.companyname);
                    sl.SetCellValue("C" + row, oTime.taskname);
                    sl.SetCellValue("D" + row, oTime.starttime.ToString("MM-dd-yyy"));
                    sl.SetCellValue("E" + row, oTime.contactname);
                    sl.SetCellValue("F" + row, oTime.timetracked / 60);
                    sl.SetCellValue("G" + row, oTime.notes);
                    row++;
                }
                FormatBillingReport(sl);
                
              sl.SaveAs("C:\\Users\\John Fleischli\\Downloads\\Billing Report.xlsx");
            }
            catch(Exception e)
            {
                SlackClient client = new SlackClient("");
                client.PostMessage("PWF API Tool Error: " + e.ToString().Substring(0, 100), "PWF API Admin", "C9X3YGW5B");
                Environment.Exit(1);
            }
            
        }

        public void FormatTimeReport(SLDocument sl)
        {
            SLStyle style = sl.CreateStyle();
            style.FormatCode = "0.00";
            sl.SetColumnStyle(6, style);

            sl.InsertRow(1, 1);

            sl.SetCellValue("A1", "Client");
            sl.SetCellValue("B1", "Title");
            sl.SetCellValue("C1", "Category");
            sl.SetCellValue("D1", "User");
            sl.SetCellValue("E1", "TaskName");
            sl.SetCellValue("F1", "TimeSpent3 (Hrs)");
            sl.SetCellValue("G1", "Date");

            sl.AutoFitColumn(1, 6);
            sl.Filter("A1", "G1");
        }

        public void FormatProjectReport(SLDocument sl)
        {
            sl.InsertRow(1, 1);

            sl.SetCellValue("A1", "name");
            sl.SetCellValue("B1", "Title");
            sl.SetCellValue("C1", "Category");
            sl.SetCellValue("D1", "ProjectManager");
            sl.SetCellValue("E1", "Started");
            sl.SetCellValue("F1", "Due");
            sl.SetCellValue("G1", "Completed");
            sl.SetCellValue("H1", "TaskName");
            sl.SetCellValue("I1", "Status");
            sl.SetCellValue("J1", "Ord");

            sl.AutoFitColumn(1, 10, 75.0);
            sl.Filter("A1", "J1");
        }

        public void FormatBillingReport(SLDocument sl)
        {
            SLStyle style = sl.CreateStyle();
            style.FormatCode = "0.00";
            sl.SetColumnStyle(6, style);

            sl.InsertRow(1, 1);

            sl.SetCellValue("A1", "Title");
            sl.SetCellValue("B1", "Client");
            sl.SetCellValue("C1", "TaskName");
            sl.SetCellValue("D1", "Date");
            sl.SetCellValue("E1", "User");
            sl.SetCellValue("F1", "TimeSpent2");
            sl.SetCellValue("G1", "TimeRecord");

            sl.AutoFitColumn(1, 10, 75.0);
            sl.Filter("A1", "G1");
        }
    }
} 