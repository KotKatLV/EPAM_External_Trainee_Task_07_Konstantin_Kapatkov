﻿using DAL.DAO.Models;
using DAL.ORM.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResultOfTheSessionUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private DaoFactory daoFactory = DaoFactory.GetInstance(@"Data Source=KONSTANTINPC\SQLEXPRESS; Initial Catalog=ResultSession; Integrated Security=true;");

        [TestMethod]
        public void CreateGender_Test()
        {
            Gender gender = new Gender("Unknown");
            daoFactory.GetGender().Create(gender);
        }

        [TestMethod]
        public void ReadGender_Test()
        {
            Gender gender = daoFactory.GetGender().Read(1);
        }

        [TestMethod]
        public void UpdateGender_Test()
        {
            Gender gender = new Gender(3, "NewUnknown");
            daoFactory.GetGender().Update(gender);
        }

        [TestMethod]
        public void DeleteGender_Test()
        {
            daoFactory.GetGender().Delete(4);
        }
    }
}