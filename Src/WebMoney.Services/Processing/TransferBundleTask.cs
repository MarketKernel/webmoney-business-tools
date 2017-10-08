//using System;
//using System.Linq;
//using log4net;
//using Microsoft.Practices.Unity;
//using WebMoney.Services.BusinessObjects;
//using WebMoney.Services.Contracts;
//using WebMoney.Services.Contracts.BasicTypes;
//using WebMoney.Services.Contracts.BusinessObjects;
//using WebMoney.Services.DataAccess.EF;
//using WebMoney.Services.ExternalServices.Contracts;

//namespace WebMoney.Services
//{
//    internal sealed class TransferBundleTask : ITask
//    {
//        private readonly TransferBundleTaskSeed _taskSeed;

//        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransferBundleTask));
        
//        [Dependency]
//        public IUnityContainer Container { get; set; }

//        public ITaskSeed TaskSeed => _taskSeed;

//        public TaskState TaskState { get; private set; }

//        public TransferBundleTask(TransferBundleTaskSeed taskSeed)
//        {
//            _taskSeed = taskSeed ?? throw new ArgumentNullException(nameof(taskSeed));
//        }

//        public void DoWork()
//        {
//            TaskState = TaskState.Started;
//            var transferBundleService = Container.Resolve<ITransferBundleService>();
//            transferBundleService.
//        }
//    }
//}
