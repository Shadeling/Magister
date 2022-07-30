namespace Mag{
    
    public static class Constants{
        public const float MUTATE_PROB = 0.01f;

        public const int MUTATE_TRIES = 10;
        public const float CROSS_PROB = 0.3f;


        public const int STATS_PRINT_PERIOD = 5;

        public const int MAX_PAIRS = 6;

        public const int MAX_STUDENTS_OK_PAIRS = 4;

        public const int WEEK_DAYS = 6;
    }

    public static class Weights{
        
        public const int BASE_RIGHT_GENE_WEIGHT = 20;
        public const int STUDENT_WINDOW_WEIGHT = -15;

        public const int TEACHER_WINDOW_WEIGHT = -20;

        public const int STUDENT_MAX_PAIRS_OK_WEIGHT = 30;

        public const int STUDENT_AUDITORIUM_MOVE_PENALTY = -2;
        public const int TEACHER_AUDITORIUM_MOVE_PENALTY = -5;

        public const int ONLY_ONE_PAIR_DAY_PENALTY = -40;

        public const int GROUP_TIME_MULT = 1;

        public const int TEACHER_TIME_MULT = 3;
    }
}