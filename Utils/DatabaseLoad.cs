using IronXL;
using System.Collections.Generic;
using System.Data;
using System;


namespace Mag
{
    public struct TimeId{
        public int id;
        public int weekday;
        public int pair;
    }



    public class DataBase{

        public Dictionary<int, Audience> Audiences;
        public Dictionary<int, ClassGroup> ClassGroups;
        public Dictionary<TimeId, ConvenientTimeGroup> TimeGroups;
        public Dictionary<TimeId, ConvenientTimeTeacher> TimeTeachers;
        public Dictionary<int, Curriculum> Curricula;
        public Dictionary<int, Schedule> Schedules;
        public Dictionary<int, Teacher> Teachers;

        public DataBase(){
            Audiences = new Dictionary<int, Audience>();
            ClassGroups = new Dictionary<int, ClassGroup>();
            TimeGroups = new Dictionary<TimeId, ConvenientTimeGroup>();
            TimeTeachers = new Dictionary<TimeId, ConvenientTimeTeacher>();
            Curricula = new Dictionary<int, Curriculum>();
            Schedules = new Dictionary<int, Schedule>();
            Teachers = new Dictionary<int, Teacher>();
        }

        public void LoadAllTables(){
            IronXL.License.LicenseKey = "IRONXL.MIRONOWSLAV.27750-CACCAC8671-QZJUCPZS4ET3WTTS-7FFZXAK5SIHK-WKNSSB37UFDE-FJESYLR5HZ5G-WUI2CB6LZZ34-GDXH7N-TSJNPYMHOFGHEA-DEPLOYMENT.TRIAL-OQ3CNB.TRIAL.EXPIRES.23.AUG.2022";

            var csvFilereader = new DataTable();
            csvFilereader  = ReadExcel("Tables/audience.csv");
            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                Audiences.Add(Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                new Audience{idAudience = Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                            audienceNumber = Convert.ToInt32(csvFilereader.Rows[i][1].ToString()),
                                            capacity = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            audienceLocation = csvFilereader.Rows[i][3].ToString(),
                                            hardwareSoftware = csvFilereader.Rows[i][4].ToString()});
            }
            Console.WriteLine(Audiences.Count);

            csvFilereader = new DataTable();
            csvFilereader  = ReadExcel("Tables/classGroup.csv");

            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                ClassGroups.Add(Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                new ClassGroup{idGroup = Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                            groupName = csvFilereader.Rows[i][1].ToString(),
                                            groupSize = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            monitoreEmail = csvFilereader.Rows[i][3].ToString()});
            }
            Console.WriteLine(ClassGroups.Count);


            csvFilereader = new DataTable();
            csvFilereader  = ReadExcel("Tables/convenientTimeGroup.csv");
            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                var timeId = new TimeId(){id = Convert.ToInt32(csvFilereader.Rows[i][1].ToString()),
                                            weekday = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            pair = Convert.ToInt32(csvFilereader.Rows[i][3].ToString()) };

                TimeGroups.Add(timeId,
                            new ConvenientTimeGroup{idConvenientTime = Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                            idGroup = Convert.ToInt32(csvFilereader.Rows[i][1].ToString()),
                                            weekday = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            pairNum = Convert.ToInt32(csvFilereader.Rows[i][3].ToString()),
                                            score = Convert.ToInt32(csvFilereader.Rows[i][4].ToString())});
            }
            Console.WriteLine(TimeGroups.Count);


            csvFilereader = new DataTable();
            csvFilereader  = ReadExcel("Tables/convenientTimeTeacher.csv");
            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                var timeId = new TimeId(){id = Convert.ToInt32(csvFilereader.Rows[i][1].ToString()),
                                            weekday = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            pair = Convert.ToInt32(csvFilereader.Rows[i][3].ToString()) };

                TimeTeachers.Add(timeId,
                        new ConvenientTimeTeacher{idConvenientTime = Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                            idTeacher = Convert.ToInt32(csvFilereader.Rows[i][1].ToString()),
                                            weekday = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            pairNum = Convert.ToInt32(csvFilereader.Rows[i][3].ToString()),
                                            score = Convert.ToInt32(csvFilereader.Rows[i][4].ToString())});
            }
            Console.WriteLine(TimeTeachers.Count);


            csvFilereader = new DataTable();
            csvFilereader  = ReadExcel("Tables/curriculum.csv");

            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                try{
                    Curricula.Add(Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                new Curriculum{idCurriculum = Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                            idTeacher = Convert.ToInt32(csvFilereader.Rows[i][1].ToString()),
                                            idGroup = Convert.ToInt32(csvFilereader.Rows[i][2].ToString()),
                                            discipline = csvFilereader.Rows[i][3].ToString(),
                                            lessonType = csvFilereader.Rows[i][4].ToString(),
                                            hardwareSoftware = csvFilereader.Rows[i][5].ToString(),
                                            pairsPerWeek = Convert.ToInt32(csvFilereader.Rows[i][6].ToString())});
                }
                catch(Exception e){

                }

            }
            Console.WriteLine(Curricula.Count);

            csvFilereader = new DataTable();
            csvFilereader  = ReadExcel("Tables/teacher.csv");
            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                try{
                    Teachers.Add(Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                new Teacher{idTeacher = Convert.ToInt32(csvFilereader.Rows[i][0].ToString()),
                                                teacherName = csvFilereader.Rows[i][1].ToString(),
                                                teacherSurName = csvFilereader.Rows[i][2].ToString(),
                                                teacherMiddleName = csvFilereader.Rows[i][3].ToString(),
                                                email = csvFilereader.Rows[i][4].ToString(),
                                                convenientPlace = csvFilereader.Rows[i][5].ToString()});
                }
                catch(Exception e){

                }
            }
            Console.WriteLine(Teachers.Count);
        }


        private DataTable ReadExcel(string fileName) {
            WorkBook workbook = WorkBook.Load(fileName);
            //// Work with a single WorkSheet.
            ////you can pass static sheet name like Sheet1 to get that sheet
            ////WorkSheet sheet = workbook.GetWorkSheet("Sheet1");
            //You can also use workbook.DefaultWorkSheet to get default in case you want to get first sheet only
            WorkSheet sheet = workbook.DefaultWorkSheet;
            //Convert the worksheet to System.Data.DataTable
            //Boolean parameter sets the first row as column names of your table.
            return sheet.ToDataTable(true);
        }
    }
}