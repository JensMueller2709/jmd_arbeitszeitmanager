using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Linq;
using System.Drawing;
using JMD_Arbeitszeitmanager.Contracts.Services.ModelServices;
using System.Windows;
using System.Windows.Media;
using System;

namespace JMD_Arbeitszeitmanager.Views
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Worker> workerList { get; } = new ObservableCollection<Worker>();
        public ObservableCollection<string> workerNameList { get; } = new ObservableCollection<string>();
       

        public ObservableCollection<Schicht> schichtList { get; set; } = new ObservableCollection<Schicht>();
        public ObservableCollection<Task> taskList { get; set; } = new ObservableCollection<Task>();

        public List<string> allCostumers;

        public Dictionary<string, List<Task>> allTasks;
        public ObservableCollection<string> costumersList { get; } = new ObservableCollection<string>();

        Dictionary<string, List<Schicht>> allWorkersAndTheirSchichts;

        Dictionary<string, Dictionary<string, WorkingDayInfo>> workingTimesOfWorkers;

        DataTable table = new DataTable();
        Grid gridWorkingTimes;

        private string selectedWorker;
        private string selectedSchichtId;

        readonly IWorkerService _workerService;
        readonly ISchichtService _schichtService;
        readonly IWorkingTimeService _workingTimeService;
        readonly ITaskService _taskService;
        readonly IDatabaseConnector _databaseConnector;

        private object _listLock = new object();

        Thread getSchichts_Thread;
        Thread getTasks_Thread;

        public MainPage(IWorkerService workerService, ISchichtService schichtService, IWorkingTimeService workingTimeService, ITaskService taskService, IDatabaseConnector databaseConnector)
        {
            InitializeComponent();
            DataContext = this;

            _workerService = workerService;
            _schichtService = schichtService;
            _workingTimeService = workingTimeService;
            _taskService = taskService;
            _databaseConnector = databaseConnector;

            BindingOperations.EnableCollectionSynchronization(workerList, _listLock);

            if (_databaseConnector.checkDB_Conn())
            {
                Thread getWorkers_Thread = new Thread(new ThreadStart(getWorkers));
                getWorkers_Thread.Start();

                getSchichts_Thread = new Thread(new ThreadStart(getAllSchichtsOfWorkers));
                getSchichts_Thread.Start();

                getTasks_Thread = new Thread(new ThreadStart(getTasks));
                getTasks_Thread.Start();

                gridWorkingTimes = grid_workingTimes;

                lb_filter.SelectedIndex = 0;
                lb_filter_worker.SelectedIndex = 1;

                //createOverviewTable();

                createDataGrid();

            } else
            {
                MessageBox.Show("Keine Verbindung zur Datenbank");
            }

            today.Text = DateTime.Now.ToString("dd.MMMM yyyy");


        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }


        private void createDataGrid()
        {
            getAllCostumers();
            calcWorkingTimes();
            var selectedFilter = (lb_filter.SelectedItem as ListBoxItem).Content.ToString();
            string selectedFilterWorker;
            if(lb_filter_worker.SelectedItem == null)
            {
                selectedFilterWorker = "Alle";
            }
            else
            {
                selectedFilterWorker = (lb_filter_worker.SelectedItem as ListBoxItem).Content.ToString();
            }
            
            Dictionary<string, Dictionary<string, WorkingDayInfo>> filtered_workingTimesOfWorkers = workingTimesOfWorkers;

            //Worker Column
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridWorkingTimes.ColumnDefinitions.Add(gridCol1);
            addColumnToGrid(0, 0, "Name", true);

            //Costumer Column
            //allCostumers.Sort();
            int i = 1;
            foreach (string c in costumersList)
            {
                if (isValidCustomer(c))
                {
                    addColumnToGrid(i, 0, c, true, isCustomerHeader: true);
                    i += 1;
                }

            }

            //update list due to filter
            i = 1;
            foreach (string workerId in workingTimesOfWorkers.Keys)
            {
                              
                if (selectedFilter != "Alle")
                {
                    if (selectedFilter == "über 10")
                    {
                        filterWorkingTimes(filtered_workingTimesOfWorkers, workerId, 10, selectedFilterWorker);

                    }
                    else if (selectedFilter == "über 13")
                    {
                        filterWorkingTimes(filtered_workingTimesOfWorkers, workerId, 13,selectedFilterWorker);
                    }
                }else
                {
                    filterWorkingTimes(filtered_workingTimesOfWorkers, workerId, -1, selectedFilterWorker);
                }
            }

            //adding a row for each worker with his times
            foreach (string workerId in filtered_workingTimesOfWorkers.Keys)
            {
                var column = 1;
                for (int idx = 0; idx <= costumersList.Count; idx++)
                {
                    //adding his name in the first column
                    if (idx == 0)
                    {
                        addRow(i, 0, workerId, true);
                    }
                    else
                    {
                        string costumer = costumersList[idx - 1];
                        if (isValidCustomer(costumer))
                        {
                            if (filtered_workingTimesOfWorkers[workerId].ContainsKey(costumer))
                            {
                                var days = filtered_workingTimesOfWorkers[workerId][costumer].WorkingDaysWithoutBreak.ToString();

                                double months = (int.Parse(days) / 30.0);
                                addRow(i, column, months.ToString("0.0"));

                            }
                            else
                            {
                                addRow(i, column, "0");
                            }
                            column += 1;
                        } 
                    }

                }

                i += 1;
            }

        }

        private static bool isValidCustomer(string c)
        {
            if (c == "Sonstige" || c == "ELPS" || c.Contains("test") || c.Contains("Test"))
                return false;

            var hiddenCostumers = Services.SettingsService.GetHiddenCostumers();
            if (hiddenCostumers.Any(h => h.Equals(c, StringComparison.OrdinalIgnoreCase)))
                return false;

            return true;
        }

        private void filterWorkingTimes(Dictionary<string, Dictionary<string, WorkingDayInfo>> filtered_workingTimesOfWorkers, string workerId, double filter, string filterWorker)
        {
            Worker cur_worker=null;
            foreach(Worker worker in workerList)
            {
                if(worker.Id == workerId)
                {
                    cur_worker = worker;
                }
            }

            if(cur_worker != null)
            {
                if(filterWorker == "Aktive")
                {
                    if (!cur_worker.State)
                    {
                        filtered_workingTimesOfWorkers.Remove(workerId);
                        return;
                    }
                }else if(filterWorker == "Inaktive")
                {
                    if (cur_worker.State)
                    {
                        filtered_workingTimesOfWorkers.Remove(workerId);
                        return;
                    }
                }
            }
           
            bool delete = true;

            for (int idx = 0; idx < costumersList.Count; idx++)
            {
                string costumer = costumersList[idx];
                if (isValidCustomer(costumer))
                {
                    if (workingTimesOfWorkers[workerId].ContainsKey(costumer))
                    {
                        var days = workingTimesOfWorkers[workerId][costumer].WorkingDaysWithoutBreak.ToString();
                        double months = (int.Parse(days) / 30);
                        if (months >= filter)
                        {
                            delete = false;
                        }
                    }
                }
            }

            if (delete)
            {
                filtered_workingTimesOfWorkers.Remove(workerId);
            }
        }

        private void addRow(int row, int col, string text, bool bold = false)
        {
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(45);
            gridWorkingTimes.RowDefinitions.Add(gridRow1);

            TextBlock cellText = new TextBlock();
            cellText.Text = text;
            cellText.FontSize = 12;
            cellText.HorizontalAlignment = HorizontalAlignment.Left;
            if (bold)
            {
                cellText.FontWeight = FontWeights.Bold;
            }

            double workingTimeInMonths;
            if (double.TryParse(text, out workingTimeInMonths))
            {
                if (workingTimeInMonths >= 13)
                {
                    cellText.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (workingTimeInMonths > 10)
                {
                    cellText.Foreground = new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    cellText.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
            
            Grid.SetRow(cellText, row);
            Grid.SetColumn(cellText, col);
            gridWorkingTimes.Children.Add(cellText);
        }

        private void addColumnToGrid(int row, int col, string text, bool bold = false, bool isCustomerHeader = false)
        {
            ColumnDefinition gridCol = new ColumnDefinition();
            gridWorkingTimes.ColumnDefinitions.Add(gridCol);


            TextBlock txtBlockCostumer = new TextBlock();
            txtBlockCostumer.Text = text;
            txtBlockCostumer.FontSize = 14;
            txtBlockCostumer.VerticalAlignment = VerticalAlignment.Top;
            txtBlockCostumer.HorizontalAlignment = HorizontalAlignment.Left;
            txtBlockCostumer.Width = 90;


            if (bold)
            {
                txtBlockCostumer.FontWeight = FontWeights.Bold;
            }

            if (isCustomerHeader)
            {
                txtBlockCostumer.Cursor = System.Windows.Input.Cursors.Hand;
                txtBlockCostumer.ToolTip = "Rechtsklick um Kunde auszublenden";
                var menuItem = new MenuItem { Header = "Ausblenden" };
                menuItem.Click += (s, e) => hideCustomer(text);
                txtBlockCostumer.ContextMenu = new ContextMenu { Items = { menuItem } };
            }

            Grid.SetRow(txtBlockCostumer, col);
            Grid.SetColumn(txtBlockCostumer, row);

            gridWorkingTimes.Children.Add(txtBlockCostumer);
        }

        private void hideCustomer(string customerName)
        {
            var result = MessageBox.Show(
                $"Möchten Sie den Kunden \"{customerName}\" ausblenden?",
                "Kunde ausblenden",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var hiddenCostumers = Services.SettingsService.GetHiddenCostumers();
                if (!hiddenCostumers.Any(h => h.Equals(customerName, StringComparison.OrdinalIgnoreCase)))
                {
                    hiddenCostumers.Add(customerName);
                    Services.SettingsService.SaveHiddenCostumers(hiddenCostumers);
                }

                gridWorkingTimes.Children.Clear();
                gridWorkingTimes.RowDefinitions.Clear();
                gridWorkingTimes.ColumnDefinitions.Clear();
                createDataGrid();
            }
        }

        private void calcWorkingTimes()
        {
            getSchichts_Thread.Join();

            workingTimesOfWorkers = _workingTimeService.getWorkingDaysForWorkerAndCostumer(allWorkersAndTheirSchichts);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void getWorkers()
        {
            var workers = _workerService.getWorkers();
            SortedDictionary<string, Worker> sortedWorkers = new SortedDictionary<string, Worker>(workers);

            foreach (string workerId in sortedWorkers.Keys)
            {
                workerList.Add(workers[workerId]);
                workerNameList.Add(workerId);
            }
            
        }

        private void getTasks()
        {
            allTasks = _taskService.getTasksToSchichts();
        }

        private void getAllCostumers()
        {
            allCostumers = _schichtService.getAllCostumersCollected();
            allCostumers.Sort();
            costumersList.Clear();

            //To print these costumers first
            intoListAndDelete(allCostumers,"WRS");
            intoListAndDelete(allCostumers,"ECCO");
            intoListAndDelete(allCostumers,"HHPI"); 
            allCostumers.ForEach(c => costumersList.Add(c));

        }

        private void intoListAndDelete(List<string> source, String id)
        {
            if (costumersList.Contains(id))
            {
                costumersList.Add(source.First(x => x == id));
                source.Remove(id);
            }
           
        }

        private void getAllSchichtsOfWorkers()
        {
            allWorkersAndTheirSchichts = _schichtService.getAllSchichtsFromWorkerOrderByCostumerCollected();
        }

        private void filterSchichtsOfWorker(string workerId, string costumer)
        {
            schichtList.Clear();
            getSchichts_Thread.Join();

            foreach (Schicht s in allWorkersAndTheirSchichts[workerId])
            {
                if(costumer == "ECCO")
                {
                    if (s.Costumer == "ECCO-DE" || s.Costumer == "ECCO-A")
                    {
                        schichtList.Add(s);
                    }
                }

                if(costumer == "Nordliner")
                {
                    if (s.Costumer.ToLower().Contains("zl") &&
                    s.Costumer.ToLower().Contains("tra"))
                    {
                        schichtList.Add(s);
                    }
                }

                if (costumer == "HSL")
                {
                    if ( s.Costumer == "HSL-NL" || s.Costumer == "HSL-DE" || s.Costumer == "HSL-CB" || s.Costumer == "HSL-A")
                    {
                        schichtList.Add(s);
                    }
                }


                if (s.Costumer == costumer)
                {
                    schichtList.Add(s);
                }
            }
            
        }



        private void combo_costumer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectCostumer();
        }

        private void selectCostumer()
        {
            if (selectedWorker != null && combo_costumer.SelectedItem != null)
            {
                var selectedCostumer = combo_costumer.SelectedItem.ToString();
                if (workingTimesOfWorkers.ContainsKey(selectedWorker))
                {

                if (workingTimesOfWorkers[selectedWorker].ContainsKey(selectedCostumer))
                {
                    WorkingDayInfo workingDayInfo = workingTimesOfWorkers[selectedWorker][selectedCostumer];

                    workingDays.Text = workingDayInfo.WorkingDaysWithoutBreak.ToString() + " / " + (workingDayInfo.WorkingDaysWithoutBreak/30).ToString("0.0");
                    firstSchicht.Text = workingDayInfo.FirstRelevantSchicht.Start.ToString();
                    lastSchicht.Text = workingDayInfo.LastWorkedSchicht.Start.ToString();
                    daysToLastSchicht.Text = (DateTime.Now.Date - workingDayInfo.LastWorkedSchicht.End.Date).Days.ToString();
                     resetDate.Text = workingDayInfo.NextPossibleWorkingDayWithoutProblems.Date.ToString("dd.MMMM yyyy");
               
                }
                
                else
                {
                    firstSchicht.Text = "Keine Schicht vorhanden";
                    lastSchicht.Text = "";
                    workingDays.Text = "";
                    //nextSchicht.Text = "";
                }

                filterSchichtsOfWorker(selectedWorker, selectedCostumer);
            }
            }
        }

        private void data_grid_schichts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (schichtList.Count > 0)
            {
                var idx = data_grid_schichts.SelectedIndex;
                var selectedSchicht = schichtList[idx].Id;
                selectedSchichtId = selectedSchicht;

                taskList.Clear();
                getTasks_Thread.Join();

                if (allTasks.ContainsKey(selectedSchichtId))
                {
                    foreach (Task t in allTasks[selectedSchichtId])
                    {
                        taskList.Add(t);
                    }
                }
            }
        }

        private void combo_worker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedWorker = combo_worker.SelectedItem.ToString();

            taskList.Clear();
            schichtList.Clear();

            worker.Text = selectedWorker;
            selectCostumer();
        }

        private void lb_filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridWorkingTimes.Children.Clear();
            gridWorkingTimes.RowDefinitions.Clear();
            gridWorkingTimes.ColumnDefinitions.Clear();
            createDataGrid();
            // Debug.Print("Row Count " + gridWorkingTimes.RowDefinitions.Count);
        }

        private void lb_filter_worker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridWorkingTimes.Children.Clear();
            gridWorkingTimes.RowDefinitions.Clear();
            gridWorkingTimes.ColumnDefinitions.Clear();
            createDataGrid();
        }
    }

}
