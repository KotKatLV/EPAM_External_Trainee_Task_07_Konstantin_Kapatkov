﻿using BLL.Reports.Excel.Views.SessionResultReport;
using System.Collections.Generic;

namespace BLL.Reports.Excel.Views.Interfaces.SessionResultReport.TableViews
{
    public interface IExaminersTableView
    {
        string[] Headers { get; }

        IEnumerable<ExaminersTableRawView> TableRawViews { get; set; }
    }
}