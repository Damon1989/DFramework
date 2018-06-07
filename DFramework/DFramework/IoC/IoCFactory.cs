using System;

namespace DFramework.IoC
{
    public sealed class IoCFactory
    {
        #region Singleton

        /// <summary>
        /// Get singleton instance of IoCFactory
        /// </summary>
        public static IoCFactory Instance { get; } = new IoCFactory();

        #endregion Singleton

        private static IContainer _currentContainer;

        public static bool IsInit()
        {
            return _currentContainer != null;
        }

        public IContainer CurrentContainer
        {
            get
            {
                if (_currentContainer == null)
                {
                    throw new Exception("Please call SetContainer first.");
                }
                return _currentContainer;
            }
        }

        public static IContainer SetContainer(IContainer container)
        {
            _currentContainer?.Dispose();
            _currentContainer = container;
            return _currentContainer;
        }

        public static T Resolve<T>(string name, params Parameter[] parameters)
        {
            return Instance.CurrentContainer.Resolve<T>(name, parameters);
        }

        public static T Resolve<T>(params Parameter[] parameters)
        {
            return Instance.CurrentContainer.Resolve<T>(parameters);
        }

        public static object Resolve(Type t, params Parameter[] parameters)
        {
            return Instance.CurrentContainer.Resolve(t, parameters);
        }

        public static object Resolve(Type t, string name, params Parameter[] parameters)
        {
            return Instance.CurrentContainer.Resolve(t, name, parameters);
        }
    }
}