namespace Mag{
    
    public static class Constants{

        // Вероятность мутации
        public const float MUTATE_PROB = 0.003f;

        //Количество неудачных мутаций до принудутельного назначения значений
        public const int MUTATE_TRIES = 50;

        //Вероятность размножения
        public const float CROSS_PROB = 0.5f;

        // Частота вывода статистики по эпохам обучения
        public const int STATS_PRINT_PERIOD = 10;

        // Максимальное число пар в день, возможное в расписании
        public const int MAX_PAIRS = 6;

        // Максимальное приемлемое в один день количество пар
        public const int MAX_STUDENTS_OK_PAIRS = 4;

        // Количество учебных дней в неделю
        public const int WEEK_DAYS = 6;
    }

    public static class Weights{
        
        // Базовый вес коррекно составленного гена
        public const int BASE_RIGHT_GENE_WEIGHT = 20;
        public const int STUDENT_WINDOW_WEIGHT = -15;

        public const int TEACHER_WINDOW_WEIGHT = -20;

        // Если выполняется ограничение MAX_STUDENTS_OK_PAIRS занятий в день
        public const int STUDENT_MAX_PAIRS_OK_WEIGHT = 30;

        public const int STUDENT_AUDITORIUM_MOVE_PENALTY = -2;
        public const int TEACHER_AUDITORIUM_MOVE_PENALTY = -5;

        public const int ONLY_ONE_PAIR_DAY_PENALTY = -40;


        // Множитель совпадения желаемого времени для студентов
        public const int GROUP_TIME_MULT = 1;

        // Множитель совпадения желаемого времени для преподавателей
        public const int TEACHER_TIME_MULT = 3;
    }
}