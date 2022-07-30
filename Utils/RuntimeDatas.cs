using System.Collections.Generic;
using System.Data;
using System;



namespace Mag{


    

    public class UsedTime{
        public Dictionary<int, int[,]> GroupTimeUsed {get; private set;}
        public Dictionary<int, int[,]> TeacherTimeUsed {get; private set;}

        public Dictionary<int, int[,]> AuditoriumTimeUsed {get; private set;}

        public UsedTime(){
            GroupTimeUsed = new Dictionary<int, int[,]>();
            TeacherTimeUsed = new Dictionary<int, int[,]>();
            AuditoriumTimeUsed = new Dictionary<int, int[,]>();
        }


        public void SetTimeUsed(int groupId, int teacherId, int day, int pair, bool value){
            AddTo(GroupTimeUsed, groupId, day, pair, value);
            AddTo(TeacherTimeUsed, teacherId, day, pair, value);
        }

        private void AddTo(Dictionary<int, int[,]> dict, int id, int day, int pair, bool value){
            int extra = value? 1 : -1;

            if(dict.TryGetValue(id, out var isUsed)){
                dict[id][day, pair] += extra;
                if(dict[id][day, pair] < 0){
                    dict[id][day, pair] = 0;
                }
            }
            else{
                dict.Add(id, new int[Constants.WEEK_DAYS, Constants.MAX_PAIRS]);
                dict[id][day, pair] += extra;
                if(dict[id][day, pair] < 0){
                    dict[id][day, pair] = 0;
                }
            }
        }

        public int GetTimeUsed(int groupId, int teacherId, int day, int pair){
            int group = GetFrom(GroupTimeUsed, groupId, day, pair);
            int teacher = GetFrom(TeacherTimeUsed, teacherId, day, pair);

            return Math.Max(group, teacher);
        }


        // True if conflict
        private int GetFrom(Dictionary<int, int[,]> dict, int id, int day, int pair){
            if(dict.TryGetValue(id, out var isUsed)){
                return isUsed[day, pair];
            }
            else{
                dict.Add(id, new int[Constants.WEEK_DAYS, Constants.MAX_PAIRS]);
                return 0;
            }
        }



        public void SetAuditorium(int auditoriumId, int day, int pair, bool value){
            AddTo(AuditoriumTimeUsed, auditoriumId, day, pair, value);
        }

        public int GetAuditoriumState(int auditoriumId, int day, int pair){
            return GetFrom(AuditoriumTimeUsed, auditoriumId, day, pair);
        }
    }
}