using System;
using System.Linq;


namespace Mag
{

    public class ScheduleInformation{
        public int groupId;
        public int teacherId;
        public int auditoriumId;
        public string discipline;
        public string lessontype;
    }

    public class Gene{

        private string discipline;
        private int teacherId;
        private int groupId;
        private int auditoriumId;

        private Individuum individuum;
        private string lessontype;
        private int group_cap;

        private int weekDay;
        private int pair;



        public Gene(Individuum ind, string dis, int teacher, int group, string type){
            this.individuum = ind;
            this.discipline = dis;
            this.groupId = group;
            this.teacherId = teacher;
            this.lessontype = type;

            if(individuum.DBLink.ClassGroups.TryGetValue(groupId, out var groupdata)){
                this.group_cap = groupdata.groupSize;
            }            
            RandomizeTime(true);
            RandomizeAuditorium(true);
        }

        public Gene(Gene template, Individuum ind){
            this.individuum = ind;
            this.discipline = template.discipline;
            this.groupId = template.groupId;
            this.teacherId = template.teacherId;
            this.lessontype = template.lessontype;
            this.group_cap = template.group_cap;
            this.auditoriumId = template.auditoriumId;
            this.weekDay = template.weekDay;
            this.pair = template.pair;
            individuum.UsedTime.SetAuditorium(auditoriumId, weekDay, pair, true);
            individuum.UsedTime.SetTimeUsed(groupId, teacherId, weekDay, pair, true);

            BecomeValid();
        }

        public void RandomizeTime(bool firstTime = false){

            if(!firstTime)
                individuum.UsedTime.SetTimeUsed(groupId, teacherId, weekDay, pair, false);

            int tries = 0;

            do{
                weekDay = Population.r.Next(Constants.WEEK_DAYS);
                pair = Population.r.Next(Constants.MAX_PAIRS);
                tries++;

            }while(individuum.UsedTime.GetTimeUsed(groupId, teacherId, weekDay, pair)>0 && tries<=Constants.MUTATE_TRIES);

            //Console.WriteLine($"Time tries = {tries}");
            if(tries > Constants.MUTATE_TRIES){
                var tState = individuum.UsedTime.TeacherTimeUsed[teacherId];
                var gState = individuum.UsedTime.GroupTimeUsed[groupId];
                for(int week=0; week<tState.GetLength(0); week++){
                    for(int pa=0; pa<tState.GetLength(1); pa++){
                        if(tState[week, pa]==0 && gState[week, pa]==0){
                            weekDay = week;
                            pair = pa;
                            return;
                        }
                    }
                }
            }

            individuum.UsedTime.SetTimeUsed(groupId, teacherId, weekDay, pair, true);
        }

        public void RandomizeAuditorium(bool firstTime = false){
            if(!firstTime)
                individuum.UsedTime.SetAuditorium(auditoriumId, weekDay, pair, false);

            var keys = individuum.DBLink.Audiences.Keys.ToList();
            int tries = 0;
            do{
                auditoriumId = keys[Population.r.Next(keys.Count)];
                tries++;
            }while(individuum.UsedTime.GetAuditoriumState(auditoriumId, weekDay, pair)>0 && tries<=Constants.MUTATE_TRIES);

            //Console.WriteLine($"Aud tries = {tries}");
            if(tries > Constants.MUTATE_TRIES){
                foreach(var audState in individuum.UsedTime.AuditoriumTimeUsed){
                    if(audState.Value[weekDay, pair]==0){
                        auditoriumId = audState.Key;
                        return;
                    }
                }
            }

            individuum.UsedTime.SetAuditorium(auditoriumId, weekDay, pair, true);
        }

        public void Mutate(){

            if(Population.r.NextDouble() > Constants.MUTATE_PROB)
                return;

            if(Population.r.Next(2) == 1){
                RandomizeTime();
            }
            else{
                RandomizeAuditorium();
            }
        
        }



        public bool CheckValid(){
            bool correctAuditorium = individuum.UsedTime.GetAuditoriumState(auditoriumId, weekDay, pair) <= 1;
            bool correctTime = individuum.UsedTime.GetTimeUsed(groupId, teacherId, weekDay, pair) <= 1;
            bool enouthPlaces = individuum.DBLink.Audiences[auditoriumId].capacity >= group_cap;
            
            return correctAuditorium && correctTime && enouthPlaces;
        }

        public void BecomeValid(){
            bool correctAuditorium = individuum.UsedTime.GetAuditoriumState(auditoriumId, weekDay, pair) <= 1;
            bool enouthPlaces = individuum.DBLink.Audiences[auditoriumId].capacity >= group_cap;
            if(!correctAuditorium || !enouthPlaces){
                RandomizeAuditorium();
            }

            bool correctTime = individuum.UsedTime.GetTimeUsed(groupId, teacherId, weekDay, pair) <= 1;
            if(!correctTime){
                RandomizeTime();
            }
        }

        public float CalculateFit(){
            return CalcTimeFitness() + Weights.BASE_RIGHT_GENE_WEIGHT;
        }

        private float CalcTimeFitness(){
            float fitness = 0;

            var groupTime = new TimeId(){
                id = groupId, weekday = weekDay, pair = pair
            };

            var teacherTime = new TimeId(){
                id = teacherId, weekday = weekDay, pair = pair
            };

            if(individuum.DBLink.TimeGroups.TryGetValue(groupTime, out var gTime)){
                fitness+= gTime.score * Weights.GROUP_TIME_MULT;
            }

            if(individuum.DBLink.TimeTeachers.TryGetValue(teacherTime, out var tTime)){
                fitness+= tTime.score * Weights.TEACHER_TIME_MULT;
            }

            return fitness;
        }

        public ScheduleInformation GetInfo(out int day, out int pairNum){
            day = this.weekDay;
            pairNum = this.pair;

            return new ScheduleInformation(){
                groupId = this.groupId,
                teacherId = this.teacherId,
                auditoriumId = this.auditoriumId,
                discipline = this.discipline,
                lessontype = this.lessontype
            };
        }
    }
}