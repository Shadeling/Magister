using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace Mag{


    public class Individuum{
        public DataBase DBLink;

        public UsedTime UsedTime;

        private List<Chromosome> Chromosomes;

        public float fitness;

        public Individuum(DataBase DB){
            this.DBLink = DB;
            this.UsedTime = new UsedTime();
            this.Chromosomes = new List<Chromosome>();
            CreateChromosomes();
        }

        // For crossingover
        public Individuum(Individuum a, Individuum b){
            this.DBLink = a.DBLink;
            this.UsedTime = new UsedTime();
            this.Chromosomes = new List<Chromosome>();

            for(int i=0; i<a.Chromosomes.Count; i++){
                if(a.Chromosomes[i].fitness > a.Chromosomes[i].fitness){
                    this.Chromosomes.Add(new Chromosome(a.Chromosomes[i], this));
                }
                else{
                    this.Chromosomes.Add(new Chromosome(b.Chromosomes[i], this));
                }
            }

            CalculateFit();
        }

        public void CreateChromosomes(){
            var groups = DBLink.ClassGroups.Keys.ToList();
            foreach(var groupId in groups){
                Chromosome newChrom = new Chromosome(this, groupId);
                Chromosomes.Add(newChrom);
            }
        }


        public bool CheckValid(){
            foreach(var chrom in Chromosomes){
                if(!chrom.CheckValid()){
                    return false;
                }
            }

            return true;
        }

        public void Mutate(){
            foreach(var chrom in Chromosomes){
                chrom.MutateAll();
            }
        }

        public void MutateInvalidChroms(){
            foreach(var chrom in Chromosomes){
                chrom.MutateInvalidGenes();
            }
        }

        public void CalculateFit(){
            fitness = 0;
            foreach(var chrom in Chromosomes){
                fitness+= chrom.CalculateFit();
            }
            // Self calcul
            CalcTeacherWindows();

        }

        private void CalcTeacherWindows(){

            foreach(var key in UsedTime.TeacherTimeUsed.Keys){
                var myTimeUsed = UsedTime.TeacherTimeUsed[key];

                for(int i=0; i < myTimeUsed.GetLength(0); i++){
                int pairPerDay =0;
                int windows = 0;
                int winLen = 0;

                for(int j=0; j < myTimeUsed.GetLength(1); j++){
                    if(myTimeUsed[i,j]>0){
                        pairPerDay++;
                        if(winLen>0){
                            windows+=winLen;
                            winLen = 0;
                        }
                    }

                    if(pairPerDay >0 && myTimeUsed[i,j]==0){
                        winLen++;
                    }
                    
                }

                fitness+= windows * Weights.TEACHER_WINDOW_WEIGHT;
                if(pairPerDay==1){
                    fitness+= Weights.ONLY_ONE_PAIR_DAY_PENALTY;
                }
            }
            }
            
        }

        public Dictionary<int, ScheduleInformation[,]> GetSchedule(){
            var schedule = new Dictionary<int, ScheduleInformation[,]>();

            foreach(var chrom in Chromosomes){
                schedule.Add(chrom.groupId, new ScheduleInformation[Constants.WEEK_DAYS, Constants.MAX_PAIRS]);
                foreach(var gene in chrom.genes){
                    var info = gene.GetInfo(out int weekday, out int pair);
                    schedule[chrom.groupId][weekday, pair] = info;
                }
            }

            return schedule;
        }

        public string WriteToString(){
            StringBuilder currStr = new StringBuilder();
            currStr.AppendLine("\n");

            // Collect group curriculum
            foreach(var groupKey in UsedTime.GroupTimeUsed.Keys){
                currStr.AppendLine();
                var groupCurr = UsedTime.GroupTimeUsed[groupKey];
                currStr.AppendLine($"Group {groupKey}");
                
                for(int pairNum=0; pairNum<groupCurr.GetLength(1); pairNum++){
                    for(int day=0; day<groupCurr.GetLength(0); day++){
                        currStr.Append($"{groupCurr[day, pairNum]};");

                    }
                    currStr.AppendLine();
                }
            }

            // Collect teacher curriculum
            foreach(var teacherKey in UsedTime.TeacherTimeUsed.Keys){
                currStr.AppendLine();
                var teacherCurr = UsedTime.TeacherTimeUsed[teacherKey];
                currStr.AppendLine($"Teacher {teacherKey}");
                
                for(int pairNum=0; pairNum<teacherCurr.GetLength(1); pairNum++){
                    for(int day=0; day<teacherCurr.GetLength(0); day++){
                        currStr.Append($"{teacherCurr[day, pairNum]};");

                    }
                    currStr.AppendLine();
                }
            }

            return currStr.ToString();
        }
    }
}