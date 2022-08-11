using Mag;
using System.Globalization;
using System.Threading;
using System;

Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");

DataBase DB = new DataBase();
DB.LoadAllTables();


Population pop = new Population(DB, Constants.POPULATION_SIZE, Constants.EPOCH_NUM);
pop.Evolve();