//using System;

//namespace WebMoney.Services
//{
//    internal sealed class TransferBundleTaskSeed : ITaskSeed, IEquatable<TransferBundleTaskSeed>
//    {
//        public int BundleId { get; }

//        public TransferBundleTaskSeed(int bundleId)
//        {
//            BundleId = bundleId;
//        }

//        public bool Equals(TransferBundleTaskSeed other)
//        {
//            if (ReferenceEquals(null, other)) return false;
//            if (ReferenceEquals(this, other)) return true;
//            return BundleId == other.BundleId;
//        }

//        public override bool Equals(object obj)
//        {
//            if (ReferenceEquals(null, obj)) return false;
//            if (ReferenceEquals(this, obj)) return true;
//            var taskSeed = obj as TransferBundleTaskSeed;
//            return taskSeed != null && Equals(taskSeed);
//        }

//        public override int GetHashCode()
//        {
//            return BundleId;
//        }
//    }
//}
