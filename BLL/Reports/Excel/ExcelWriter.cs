﻿using BLL.Reports.Structs.ReportData;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BLL.Reports.Excel
{
    public static class ExcelWriter
    {
        private static void SetBorder(ExcelPackage excel, ExcelWorksheet workSheet, string workSheetName)
        {
            workSheet = excel.Workbook.Worksheets[workSheetName];
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private static void SetWorkSheetStyle(ExcelWorksheet workSheet)
        {
            workSheet.TabColor = Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.DefaultColWidth = 20;
        }

        private static void SetRowStyle(ExcelRow row)
        {
            row.Height = 20;
            row.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            row.Style.Font.Bold = true;
        }

        private static void WriteGroupTable(SessionResultReportData dataToWrite, ExcelPackage excel, ExcelWorksheet workSheet)
        {
            for(int i = 0; i < dataToWrite.GroupTableRawViews.Count; i++)
            {
                int currentRow = 1;
                workSheet = excel.Workbook.Worksheets.Add(dataToWrite.GroupTableRawViews.ElementAt(i).Key);

                SetWorkSheetStyle(workSheet);
                SetRowStyle(workSheet.Row(currentRow));

                workSheet.Cells[currentRow, currentRow].Value = dataToWrite.SessionInfo;
                workSheet.Cells[currentRow, currentRow, currentRow, SessionResultReportData.HeadersForGroupTable.Length].Merge = true;

                SetRowStyle(workSheet.Row(++currentRow));

                workSheet.Cells[currentRow, currentRow].Value = $"Group: {dataToWrite.GroupTableRawViews.ElementAt(i).Key}";
                workSheet.Cells[currentRow, currentRow, currentRow, SessionResultReportData.HeadersForGroupTable.Length].Merge = true;

                SetRowStyle(workSheet.Row(++currentRow));

                for (int k = 0; k < SessionResultReportData.HeadersForGroupTable.Length; k++)
                {
                    workSheet.Cells[currentRow, ++k].Value = SessionResultReportData.HeadersForGroupTable[--k];
                }

                var tmp = dataToWrite.GroupTableRawViews.Values.ToList();
                for (int m = ++currentRow, j = 0; j < tmp[i].ToList().Count; j++, m++)
                {
                    workSheet.Cells[m, 1].Value = tmp[i].ToList()[j].Surname;
                    workSheet.Cells[m, 2].Value = tmp[i].ToList()[j].Name;
                    workSheet.Cells[m, 3].Value = tmp[i].ToList()[j].Patronymic;
                    workSheet.Cells[m, 4].Value = tmp[i].ToList()[j].Subject;
                    workSheet.Cells[m, 5].Value = tmp[i].ToList()[j].Form;
                    workSheet.Cells[m, 6].Value = tmp[i].ToList()[j].Date;
                    workSheet.Cells[m, 7].Value = tmp[i].ToList()[j].Assessment;
                }

                SetBorder(excel, workSheet, dataToWrite.GroupTableRawViews.ElementAt(i).Key);
            }
        }

        private static void WriteGroupSpecialtyTable(SessionResultReportData dataToWrite, ExcelPackage excel, ExcelWorksheet workSheet)
        {
            int currentRow = 1;
            workSheet = excel.Workbook.Worksheets.Add("Specialty assessments");

            SetWorkSheetStyle(workSheet);
            SetRowStyle(workSheet.Row(currentRow));

            workSheet.Cells[currentRow, currentRow].Value = "Average assessment for each specialty";
            workSheet.Cells[currentRow, currentRow, currentRow, SessionResultReportData.HeadersForGroupSpecialtyTable.Length].Merge = true;

            SetRowStyle(workSheet.Row(++currentRow));

            for (int i = 0; i < SessionResultReportData.HeadersForGroupSpecialtyTable.Length; i++)
            {
                workSheet.Cells[currentRow, ++i].Value = SessionResultReportData.HeadersForGroupSpecialtyTable[--i];
            }

            for (int i = ++currentRow, j = 0; j < dataToWrite.GroupSpecialtyTableRawViews.Count(); i++, j++)
            {
                workSheet.Cells[i, 1].Value = dataToWrite.GroupSpecialtyTableRawViews.ToList()[j].SpecialityName;
                workSheet.Cells[i, 2].Value = dataToWrite.GroupSpecialtyTableRawViews.ToList()[j].AverageAssessment;
            }

            SetBorder(excel, workSheet, "Specialty assessments");
        }

        private static void WriteExaminersTable(SessionResultReportData dataToWrite, ExcelPackage excel, ExcelWorksheet workSheet)
        {
            int currentRow = 1;
            workSheet = excel.Workbook.Worksheets.Add("Examiner assessments");

            SetWorkSheetStyle(workSheet);
            SetRowStyle(workSheet.Row(currentRow));

            workSheet.Cells[currentRow, currentRow].Value = "Average assessment for each examiner";
            workSheet.Cells[currentRow, currentRow, currentRow, SessionResultReportData.HeadersForExaminersTable.Length].Merge = true;

            SetRowStyle(workSheet.Row(++currentRow));

            for (int i = 0; i < SessionResultReportData.HeadersForExaminersTable.Length; i++)
            {
                workSheet.Cells[currentRow, ++i].Value = SessionResultReportData.HeadersForExaminersTable[--i];
            }

            for (int i = ++currentRow, j = 0; j < dataToWrite.ExaminersTableRawViews.Count(); i++, j++)
            {
                workSheet.Cells[i, 1].Value = dataToWrite.ExaminersTableRawViews.ToList()[j].ExaminerSurname;
                workSheet.Cells[i, 2].Value = dataToWrite.ExaminersTableRawViews.ToList()[j].ExaminerName;
                workSheet.Cells[i, 3].Value = dataToWrite.ExaminersTableRawViews.ToList()[j].ExaminerPatronymic;
                workSheet.Cells[i, 4].Value = dataToWrite.ExaminersTableRawViews.ToList()[j].AverageAssessment;
            }

            SetBorder(excel, workSheet, "Examiner assessments");
        }

        private static void WriteAssessmentDynamicsTable(AssessmentDynamicsReportData dataToWrite, ExcelPackage excel, ExcelWorksheet workSheet)
        {
            int currentRow = 1;
            workSheet = excel.Workbook.Worksheets.Add("Assessment dynamics");

            SetWorkSheetStyle(workSheet);
            SetRowStyle(workSheet.Row(currentRow));
            workSheet.DefaultColWidth = 25;

            workSheet.Cells[currentRow, currentRow].Value = "Dynamics of changes in the average assessment for each subject by year";
            workSheet.Cells[currentRow, currentRow, currentRow++, dataToWrite.AcademicYears.Count() + 1].Merge = true;

            for (int i = 0; i < AssessmentDynamicsReportData.HeadersForAssessmentDynamicsTable.Length; i++)
            {
                workSheet.Cells[currentRow, ++i].Value = AssessmentDynamicsReportData.HeadersForAssessmentDynamicsTable[--i];
            }

            workSheet.Cells[currentRow, currentRow - 1, currentRow + 1, currentRow - 1].Merge = true;
            workSheet.Cells[currentRow, currentRow - 1, currentRow + 1, currentRow - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[currentRow, currentRow - 1, currentRow + 1, currentRow - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[currentRow, currentRow - 1, currentRow + 1, currentRow - 1].Style.Font.Bold = true;

            workSheet.Cells[currentRow, currentRow, currentRow , currentRow + 1].Merge = true;
            workSheet.Cells[currentRow, currentRow, currentRow, currentRow + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[currentRow, currentRow, currentRow, currentRow + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[currentRow, currentRow, currentRow, currentRow].Style.Font.Bold = true;

            for (int i = currentRow, j = currentRow + 1, k = 0; k < dataToWrite.AcademicYears.Count(); i++, k++)
            {
                workSheet.Cells[j, i].Value = dataToWrite.AcademicYears.ToList()[k];
                workSheet.Cells[j, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[j, i].Style.Font.Bold = true;
            }

            for (int i = currentRow += 2, k = 0; k < dataToWrite.TableRowViews.Count(); i++, k++)
            {
                workSheet.Cells[i, 1].Value = dataToWrite.TableRowViews.ToList()[k].SubjectName;
                for (int j = 0, l = 2; j < dataToWrite.TableRowViews.ToList()[k].AvgAssessments.ToList().Count; j++, l++)
                {
                    workSheet.Cells[i, l].Value = dataToWrite.TableRowViews.ToList()[k].AvgAssessments.ToList()[j] == -1 ? "" : (object)dataToWrite.TableRowViews.ToList()[k].AvgAssessments.ToList()[j];
                    workSheet.Cells[i, l].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }

            SetBorder(excel, workSheet, "Assessment dynamics");
        }

        public static void WriteToExcel(SessionResultReportData dataToWrite, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet workSheet = null;

            WriteGroupTable(dataToWrite, excel, workSheet);
            WriteGroupSpecialtyTable(dataToWrite, excel, workSheet);
            WriteExaminersTable(dataToWrite, excel, workSheet);

            FileStream objFileStrm = File.Create(filePath);
            objFileStrm?.Close();
            objFileStrm?.Dispose();
            File.WriteAllBytes(filePath, excel.GetAsByteArray());

            excel?.Dispose();
            workSheet?.Dispose();
        }

        public static void WriteToExcel(AssessmentDynamicsReportData dataToWrite, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet workSheet = null;

            WriteAssessmentDynamicsTable(dataToWrite, excel, workSheet);

            FileStream objFileStrm = File.Create(filePath);
            objFileStrm?.Close();
            objFileStrm?.Dispose();
            File.WriteAllBytes(filePath, excel.GetAsByteArray());

            excel?.Dispose();
            workSheet?.Dispose();
        }
    }
}