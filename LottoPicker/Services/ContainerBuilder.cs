using LottoPicker.Common;
using LottoPicker.Lists;
using LottoPicker.Sorting;
using TinyIoC;

namespace LottoPicker.Services
{
    public static class ContainerBuilder
    {
        private static readonly TinyIoCContainer _container;

        static ContainerBuilder()
        {
            _container = new TinyIoCContainer();
            // Singleton is the default lifetime, when the register command is called this way.
            _container.Register<IArraySort<int>, ParallelMergeArraySort<int>>();
            _container.Register<INumberPicker, LotteryNumbers>();
        }
        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }
    }
}
