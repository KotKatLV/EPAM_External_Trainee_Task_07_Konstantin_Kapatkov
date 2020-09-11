﻿using BLL.Reports.Structs.ExcelTableRawViews.DynamicChangesInAverageMark;
using System.Collections.Generic;

namespace BLL.Reports.Interfaces.GroupSessionResultReport
{
    public interface IAssessmentDynamicsReportData
    {
        IEnumerable<AssessmentDynamicsTableRowView> TableRowViews { get; set; }

        IEnumerable<string> AcademicYears { get; set; }
    }
}