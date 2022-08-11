
namespace Mag
{
    public class Audience{
        public int idAudience;
        public int audienceNumber;
        public int capacity;
        public string audienceLocation;
        public string hardwareSoftware;
    }

    public class ClassGroup{
        public int idGroup;
        public string groupName;
        public int groupSize;
        public string monitoreEmail;
    }

    public class ConvenientTimeGroup{
        public int idConvenientTime;
        public int idGroup;
        public int weekday;
        public int pairNum;
        public int score;
    }

    public class ConvenientTimeTeacher{
        public int idConvenientTime;
        public int idTeacher;
        public int weekday;
        public int pairNum;
        public int score;
    }

    public class Curriculum{
        public int idCurriculum;
        public int idTeacher;
        public int idGroup;
        public string discipline;
        public string lessonType;
        public string hardwareSoftware;
        public int pairsPerWeek;
    }

    public class Schedule{
        public int idSchedule;
        public int idCurriculum;
        public int idAudience;
        public int weekday;
        public int pairNum;
    }

    public class Teacher{
        public int idTeacher;
        public string teacherName;
        public string teacherSurName;
        public string teacherMiddleName;
        public string email;
        public string convenientPlace;
    }

    

}
