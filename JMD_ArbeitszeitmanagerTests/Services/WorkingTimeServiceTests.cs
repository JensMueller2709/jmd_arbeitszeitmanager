using Microsoft.VisualStudio.TestTools.UnitTesting;
using JMD_Arbeitszeitmanager.Services;
using System;
using System.Collections.Generic;
using System.Text;
using JMD_Arbeitszeitmanager.Core.Models;

namespace JMD_Arbeitszeitmanager.Services.Tests
{
    [TestClass()]
    public class WorkingTimeServiceTests
    {
        [TestMethod()]
        public void calcWorkingDaysWithoutBreakingTimeForCostumerTest()
        {
            WorkingTimeService workingTimeService = new WorkingTimeService();

            List<Schicht> schichtList = new List<Schicht>() {

            new Schicht("1", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null),
            new Schicht("1", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "1", null, null, null),
            new Schicht("1", "TestKunde1", DateTime.Parse("20.05.2020 10:00:00"), DateTime.Parse("21.05.2020 09:00:00"), "1", null, null, null),
            new Schicht("1", "TestKunde1", DateTime.Parse("22.05.2020 10:00:00"), DateTime.Parse("23.05.2020 09:00:00"), "1", null, null, null)
            };


            WorkingDayInfo workingDayInfo = workingTimeService.calcWorkingDaysWithoutBreakingTimeForCostumer(schichtList, new DateTime(2020, 06, 01));

            Assert.AreEqual(3, workingDayInfo.WorkingDaysWithoutBreak);
            Assert.AreEqual(DateTime.Parse("22.05.2020 10:00:00"), workingDayInfo.LastWorkedSchicht.Start);
            Assert.AreEqual(DateTime.Parse("20.05.2020 10:00:00"), workingDayInfo.FirstRelevantSchicht.Start);

        }

        [TestMethod()]
        public void calcWorkingDaysWithoutBreakingTimeForCostumerTest_WithNoBreak()
        {
            WorkingTimeService workingTimeService = new WorkingTimeService();

            List<Schicht> schichtList = new List<Schicht>() {

            new Schicht("1", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null),
            new Schicht("1", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "1", null, null, null),
            new Schicht("1", "TestKunde1", DateTime.Parse("20.03.2020 10:00:00"), DateTime.Parse("21.03.2020 09:00:00"), "1", null, null, null),
            new Schicht("1", "TestKunde1", DateTime.Parse("01.04.2020 10:00:00"), DateTime.Parse("23.04.2020 09:00:00"), "1", null, null, null)
            };

            WorkingDayInfo workingDayInfo = workingTimeService.calcWorkingDaysWithoutBreakingTimeForCostumer(schichtList, new DateTime(2020, 06, 01));

            Assert.AreEqual(113, workingDayInfo.WorkingDaysWithoutBreak);
            Assert.AreEqual(DateTime.Parse("01.04.2020 10:00:00"), workingDayInfo.LastWorkedSchicht.Start);
            Assert.AreEqual(DateTime.Parse("01.01.2020 10:00:00"), workingDayInfo.FirstRelevantSchicht.Start);
        }

        [TestMethod()]
        public void calcWorkingDaysWithoutBreakingTimeForCostumerTest_SingleDay()
        {
            WorkingTimeService workingTimeService = new WorkingTimeService();

            List<Schicht> schichtList = new List<Schicht>() {

            new Schicht("1", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null)

            };

            WorkingDayInfo workingDayInfo = workingTimeService.calcWorkingDaysWithoutBreakingTimeForCostumer(schichtList, new DateTime(2020, 02, 01));

            Assert.AreEqual(1, workingDayInfo.WorkingDaysWithoutBreak);
            Assert.AreEqual(DateTime.Parse("01.01.2020 10:00:00"), workingDayInfo.FirstRelevantSchicht.Start);
        }

        [TestMethod()]
        public void calcWorkingDaysWithoutBreakingTimeForCostumerTest_MultipleDaysWithEnoughBreakingTime()
        {
            WorkingTimeService workingTimeService = new WorkingTimeService();

            List<Schicht> schichtList = new List<Schicht>() {

            new Schicht("1", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null),
            new Schicht("2", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "1", null, null, null),
            new Schicht("3", "TestKunde1", DateTime.Parse("01.03.2020 10:00:00"), DateTime.Parse("02.03.2020 09:00:00"), "1", null, null, null),
            new Schicht("4", "TestKunde1", DateTime.Parse("01.04.2020 10:00:00"), DateTime.Parse("02.04.2020 09:00:00"), "1", null, null, null)

            };

            WorkingDayInfo workingDayInfo = workingTimeService.calcWorkingDaysWithoutBreakingTimeForCostumer(schichtList,DateTime.Now.Date);

            Assert.AreEqual(0, workingDayInfo.WorkingDaysWithoutBreak);
            Assert.AreEqual(DateTime.Parse("01.01.2020 10:00:00"), workingDayInfo.FirstRelevantSchicht.Start);
        }

        [TestMethod()]
        public void getWorkingDaysForWorkerAndCostumerTest()
        {
            Dictionary<string, List<Schicht>> inputList = new Dictionary<string, List<Schicht>>();
            inputList.Add("1", new List<Schicht>() {
                new Schicht("1", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "1", null, null, null),
                new Schicht("2", "TestKunde1", DateTime.Parse("20.05.2020 10:00:00"), DateTime.Parse("21.05.2020 09:00:00"), "1", null, null, null),
                new Schicht("3", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null),
                new Schicht("4", "TestKunde1", DateTime.Parse("22.05.2020 10:00:00"), DateTime.Parse("23.05.2020 09:00:00"), "1", null, null, null)});

            inputList.Add("2", new List<Schicht>() {
                new Schicht("10", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "2", null, null, null),
                new Schicht("20", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "2", null, null, null),
                new Schicht("30", "TestKunde2", DateTime.Parse("20.03.2020 10:00:00"), DateTime.Parse("21.03.2020 09:00:00"), "2", null, null, null),
                new Schicht("30", "TestKunde2", DateTime.Parse("25.03.2020 10:00:00"), DateTime.Parse("27.03.2020 09:00:00"), "2", null, null, null),
                new Schicht("40", "TestKunde3", DateTime.Parse("01.04.2020 10:00:00"), DateTime.Parse("23.04.2020 09:00:00"), "2", null, null, null),
                new Schicht("20", "TestKunde1", DateTime.Parse("25.03.2020 10:00:00"), DateTime.Parse("27.03.2020 09:00:00"), "2", null, null, null)});

            inputList.Add("3", new List<Schicht>() {
                new Schicht("11", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "3", null, null, null),
                new Schicht("12", "TestKunde1", DateTime.Parse("19.05.2020 10:00:00"), DateTime.Parse("20.05.2020 09:00:00"), "3", null, null, null),
            
            });

            WorkingTimeService workingTimeService = new WorkingTimeService();

            Dictionary<string, Dictionary<string, WorkingDayInfo>> outputList =  workingTimeService.getWorkingDaysForWorkerAndCostumer(inputList, new DateTime(2020, 06, 01));

            Assert.AreEqual(3, outputList["1"]["TestKunde1"].WorkingDaysWithoutBreak);

            Assert.AreEqual(86, outputList["2"]["TestKunde1"].WorkingDaysWithoutBreak);
            Assert.AreEqual(7, outputList["2"]["TestKunde2"].WorkingDaysWithoutBreak);
            Assert.AreEqual(22, outputList["2"]["TestKunde3"].WorkingDaysWithoutBreak);

            Assert.AreEqual(1, outputList["3"]["TestKunde1"].WorkingDaysWithoutBreak);
        }

        [TestMethod()]
        public void getWorkingDaysForWorkerAndCostumerTestWithCurrentDate()
        {
            Dictionary<string, List<Schicht>> inputList = new Dictionary<string, List<Schicht>>();
            inputList.Add("1", new List<Schicht>() {
                new Schicht("1", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "1", null, null, null),
                new Schicht("2", "TestKunde1", DateTime.Parse("20.05.2020 10:00:00"), DateTime.Parse("21.05.2020 09:00:00"), "1", null, null, null),
                new Schicht("3", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null),
                new Schicht("4", "TestKunde1", DateTime.Parse("22.05.2020 10:00:00"), DateTime.Parse("23.05.2020 09:00:00"), "1", null, null, null)});

            inputList.Add("2", new List<Schicht>() {
                new Schicht("10", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "2", null, null, null),
                new Schicht("20", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "2", null, null, null),
                new Schicht("30", "TestKunde2", DateTime.Parse("20.03.2020 10:00:00"), DateTime.Parse("21.03.2020 09:00:00"), "2", null, null, null),
                new Schicht("30", "TestKunde2", DateTime.Parse("25.03.2020 10:00:00"), DateTime.Parse("27.03.2020 09:00:00"), "2", null, null, null),
                new Schicht("40", "TestKunde3", DateTime.Parse("01.04.2020 10:00:00"), DateTime.Parse("23.04.2020 09:00:00"), "2", null, null, null),
                new Schicht("20", "TestKunde1", DateTime.Parse("25.03.2020 10:00:00"), DateTime.Parse("27.03.2020 09:00:00"), "2", null, null, null)});

            inputList.Add("3", new List<Schicht>() {
                new Schicht("11", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "3", null, null, null),
                new Schicht("12", "TestKunde1", DateTime.Parse("19.05.2020 10:00:00"), DateTime.Parse("20.05.2020 09:00:00"), "3", null, null, null),
                new Schicht("12", "TestKunde1", DateTime.Now.AddDays(-1), DateTime.Now, "3", null, null, null),

            });

            WorkingTimeService workingTimeService = new WorkingTimeService();

            Dictionary<string, Dictionary<string, WorkingDayInfo>> outputList = workingTimeService.getWorkingDaysForWorkerAndCostumer(inputList);

            Assert.AreEqual(0, outputList["1"]["TestKunde1"].WorkingDaysWithoutBreak);

            Assert.AreEqual(0, outputList["2"]["TestKunde1"].WorkingDaysWithoutBreak);
            Assert.AreEqual(0, outputList["2"]["TestKunde2"].WorkingDaysWithoutBreak);
            Assert.AreEqual(0, outputList["2"]["TestKunde3"].WorkingDaysWithoutBreak);

            Assert.AreEqual(1, outputList["3"]["TestKunde1"].WorkingDaysWithoutBreak);
        }

        [TestMethod()]
        public void calcWorkingDays_CheckNextPossibleWorkingDate()
        {
            WorkingTimeService workingTimeService = new WorkingTimeService();

            List<Schicht> schichtList = new List<Schicht>() {

            new Schicht("1", "TestKunde1", DateTime.Parse("01.01.2020 10:00:00"), DateTime.Parse("02.01.2020 09:00:00"), "1", null, null, null),
            new Schicht("2", "TestKunde1", DateTime.Parse("01.02.2020 10:00:00"), DateTime.Parse("02.02.2020 09:00:00"), "1", null, null, null),
            new Schicht("3", "TestKunde1", DateTime.Parse("01.03.2020 10:00:00"), DateTime.Parse("02.03.2020 09:00:00"), "1", null, null, null),
            new Schicht("4", "TestKunde1", DateTime.Parse("01.04.2020 10:00:00"), DateTime.Parse("02.04.2020 09:00:00"), "1", null, null, null)

            };

            WorkingDayInfo workingDayInfo = workingTimeService.calcWorkingDaysWithoutBreakingTimeForCostumer(schichtList, new DateTime(2020, 06, 01));
            Assert.AreEqual(DateTime.Parse("02.04.2020 00:00:00").AddDays(WorkingTimeService.NEEDED_BREAK_TIME_IN_DAYS), workingDayInfo.NextPossibleWorkingDayWithoutProblems);

            workingDayInfo = workingTimeService.calcWorkingDaysWithoutBreakingTimeForCostumer(schichtList, new DateTime(2020, 08, 01));
            Assert.AreEqual(new DateTime(2020, 08, 01), workingDayInfo.NextPossibleWorkingDayWithoutProblems);

        }
    }
}