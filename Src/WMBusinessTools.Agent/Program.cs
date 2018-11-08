using System;
using WebMoney.Services.Contracts.BasicTypes;

namespace WMBusinessTools.Agent
{
    class Program
    {
        enum TaskType
        {
            Accounts,
            Transfers, // Для Light - игнорим!
            TransfersByAccount,
            IncomingInvoices,
            OutgoingInvoicesByAccount
        }

        enum ResultType
        {
            Success,
            Warn,
            Fail,
            Fatal // исполнение не возможно (кошелька или ID-а не существует...)
        }

        enum TaskState2
        {
            Wait,
            Started,
            Completed
        }

        public class Account
        {
            public DateTime TransfersRefreshTime { get; }
            public DateTime OutgoingInvoicesRefreshTime { get; }
        }

        class Task
        {
            public TaskType Type { get; }
            public string Value { get; }
            public int Period { get; set; }
            public TaskState2 State { get; set; }
            public DateTime WakeUpTime { get; set; }
            public DateTime RefreshTime { get; set; }
            public DateTime CreateTime { get; set; }
            public DateTime UpdateTime { get; set; }
        }

        class TaskExecution
        {
            public int Id { get; }
            public TaskType Type { get; }
            public string Value { get; }
            public ResultType ResultType { get; }
            public DateTime StartTime { get; }
            public DateTime FinishTime { get; }
        }

        class ExecutionStep
        {
            public int Id { get; }
            public int ExecutionId { get; }
            public ResultType ResultType { get; }
            public string Message { get; set; }
            public DateTime CreateTime { get; }
        }

        static void Main(string[] args)
        {
            //var unityContainer = new UnityContainer();

            //var configurationService = new ConfigurationService();
            //configurationService.RegisterServices(unityContainer);

            // 1. Все задачи Started -> Completed + TaskReport в ошибку (не завершена)

            // 2. Обновляем список кошельков мастера.

            // 3. Добавляем все кошельки и идентификаторы (Secondary) из задач. Может ли быть, что номер кошелька получить не можем???

            // 4. Получаем задачу.
            var task = ObtainTask();

            // 5. Переводим в Started
            task.State = TaskState2.Started;

            // 6. Создаем TaskExecution

            // 7. Выполняем
            var resultType = DoTask(task);

            // 8. Закрываем TaskExecution
        }

        static Task ObtainTask()
        {
            // 1. Сотрировка по типу (сначала Accounts)
            // 2. Те, у которых состояние Wait
            // 3. Те, которые должны проснуться (подошло время).
            // 4. Те, которые не Fatal


            return null;
        }

        static ResultType DoTask(Task task)
        {
            switch (task.Type)
            {
                case TaskType.Accounts:

                    // 1. Обновляем кошельки

                    // 2. Вносим ExecutionStep

                    break;
                case TaskType.Transfers:

                    // 1. Получаем список кошельков.

                    // 2. Запоминаем ID-ы транзакций.

                    // 3. Обновляем список кошельков.

                    // 4. Вносим ExecutionStep

                    // 5. Сравниваем ID-ы транзакций. Пропускаем те, где нет изменений.

                    // 6. Проходимся по всем кошелькам и получаем транзакции на основе TransfersRefreshTime (начиная от него и + день).

                    // 4. Обновляем для каждого кошелька TransfersRefreshTime + Вносим ExecutionStep

                    break;
                case TaskType.TransfersByAccount:

                    // 1. Получаем кошелек из списка, получаем TransfersRefreshTime
                    // 2. Получаем операции по кошельку.
                    // 3. Обновляем TransfersRefreshTime + Вносим ExecutionStep

                    break;
                case TaskType.IncomingInvoices:

                    // 1. Получаем начиная с RefreshTime Task
                    // 2. Вносим счета
                    // 3. Обновляем RefreshTime + Вносим ExecutionStep

                    break;
                case TaskType.OutgoingInvoicesByAccount:

                    // 1. Получаем кошелек из списка, получаем OutgoingInvoicesRefreshTime
                    // 2. Получаем счета по кошельку.
                    // 3. Обновляем OutgoingInvoicesRefreshTime + Вносим ExecutionStep

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ResultType.Success;
        }
    }
}
