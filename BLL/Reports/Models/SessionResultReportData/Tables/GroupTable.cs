﻿using BLL.Reports.Abstract;
using BLL.Reports.ExcelViews.SessionResultReport.TableView;
using BLL.Reports.Structs.ExcelTableRawViews.SessionResultReport;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Reports.Models.SessionResultReportData
{
    public class GroupTable : Report
    {
        public GroupTable(string connectionString) : base(connectionString)
        {
        }

        private IEnumerable<GroupTableRawView> GetGroupTableRowsData(int sessionId, int groupId)
        {
            return from st in Students
                   join sr in SessionResults on st.Id equals sr.StudentId
                   join s in Subjects on sr.SubjectId equals s.Id
                   join ss in SessionSchedules on st.GroupId equals ss.GroupId
                   join kaf in KnowledgeAssessmentForms on ss.KnowledgeAssessmentFormId equals kaf.Id
                   join g in Groups on st.GroupId equals g.Id
                   where ss.SubjectId == sr.SubjectId && ss.SessionId == sessionId && st.GroupId == groupId
                   select new GroupTableRawView(st.Name, st.Surname, st.Patronymic, s.Name, kaf.Form, ss.Date.ToShortDateString(), sr.Assessment);
        }

        private string GetGroupName(int groupId) => Groups.FirstOrDefault(g => g.Id == groupId)?.Name;

        private string GetSessionName(int sessionId) => Sessions.FirstOrDefault(s => s.Id == sessionId)?.Name;

        public IEnumerable<GroupTableView> GetGroupTableData(int sessionId)
        {
            List<GroupTableView> result = new List<GroupTableView>();
            foreach (int groupId in SessionSchedules.Where(ss => ss.SessionId == sessionId).Select(ss => ss.GroupId).Distinct().ToList())
            {
                result.Add(new GroupTableView(GetGroupTableRowsData(sessionId, groupId), GetGroupName(groupId), GetSessionName(sessionId)));
            }
            return result;
        }
    }
}