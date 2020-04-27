using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DFramework.Config;
using DFramework.Infrastructure;
using DFramework.IoC;
using DCommon;

namespace DFramework.Message.Impl
{
    public abstract class HandlerProvider : IHandlerProvider
    {
        private readonly ConcurrentDictionary<Type, List<HandlerTypeInfo>> _HandlerTypes;
        private readonly HashSet<Type> discardKeyTypes;

        public HandlerProvider(params string[] assemblies)
        {
            Assemblies = assemblies;
            _HandlerTypes = new ConcurrentDictionary<Type, List<HandlerTypeInfo>>();
            // _HandlerConstuctParametersInfo = new Dictionary<Type, ParameterInfo[]>();
            discardKeyTypes = new HashSet<Type>();

            RegisterHandlers();
        }

        //protected abstract Type HandlerType { get; }

        private string[] Assemblies { get; }

        //private readonly Dictionary<Type, ParameterInfo[]> _HandlerConstuctParametersInfo;
        protected abstract Type[] HandlerGenericTypes { get; }

        public IList<HandlerTypeInfo> GetHandlerTypes(Type messageType)
        {
            var avaliableHandlerTypes = new List<HandlerTypeInfo>();
            if (_HandlerTypes.ContainsKey(messageType))
            {
                var handlerTypes = _HandlerTypes[messageType];
                if (handlerTypes != null && handlerTypes.Count > 0)
                    avaliableHandlerTypes = handlerTypes;
            }
            else if (!discardKeyTypes.Contains(messageType))
            {
                foreach (var handlerTypes in _HandlerTypes)
                    if (messageType.IsSubclassOf(handlerTypes.Key))
                    {
                        var messageDispatcherHandlerTypes = _HandlerTypes[handlerTypes.Key];
                        if (messageDispatcherHandlerTypes != null && messageDispatcherHandlerTypes.Count > 0)
                            avaliableHandlerTypes = avaliableHandlerTypes.Union(messageDispatcherHandlerTypes).ToList();
                    }
                if (avaliableHandlerTypes.Count == 0)
                    discardKeyTypes.Add(messageType);
                else
                    _HandlerTypes.TryAdd(messageType, avaliableHandlerTypes);
            }
            return avaliableHandlerTypes;
        }

        public object GetHandler(Type messageType)
        {
            object handler = null;
            var handlerType = GetHandlerType(messageType);
            if (handlerType != null)
                handler = IoCFactory.Resolve(handlerType.Type);
            return handler;
        }

        public void ClearRegistration()
        {
            _HandlerTypes.Clear();
        }

        private void RegisterHandlers()
        {
            var handlerElements = ConfigurationReader.Instance
                .GetConfigurationSection<FrameworkConfigurationSection>()?
                .Handlers;
            if (handlerElements != null)
                foreach (HandlerElement handlerElement in handlerElements)
                    if (Assemblies == null || Assemblies.Contains(handlerElement.Name))
                        try
                        {
                            switch (handlerElement.SourceType)
                            {
                                case HandlerSourceType.Type:
                                    var type = Type.GetType(handlerElement.Source);
                                    RegisterHandlerFromType(type);
                                    break;

                                case HandlerSourceType.Assembly:
                                    var assembly = Assembly.Load(handlerElement.Source);
                                    RegisterHandlerFromAssembly(assembly);
                                    break;
                            }
                        }
                        catch
                        {
                        }

            RegisterInheritedMessageHandlers();
        }

        private void RegisterInheritedMessageHandlers()
        {
            _HandlerTypes.Keys.ForEach(messageType =>
                _HandlerTypes.Keys.Where(type => type.IsSubclassOf(messageType))
                    .ForEach(type =>
                    {
                        var list =
                            _HandlerTypes[type].Union(_HandlerTypes[messageType]).ToList();
                        _HandlerTypes[type].Clear();
                        _HandlerTypes[type].AddRange(list);
                    }
                    ));
        }

        public void Register(Type messageType, Type handlerType)
        {
            var isAsync = false;
            var handleMethod = handlerType.GetMethods()
                .Where(m => m.GetParameters().Any(p => p.ParameterType == messageType)).FirstOrDefault();
            isAsync = typeof(Task).IsAssignableFrom(handleMethod.ReturnType);
            if (_HandlerTypes.ContainsKey(messageType))
            {
                var registeredDispatcherHandlerTypes = _HandlerTypes[messageType];
                if (registeredDispatcherHandlerTypes != null)
                {
                    if (!registeredDispatcherHandlerTypes.Exists(ht => ht.Type == handlerType && ht.IsAsync == isAsync))
                        registeredDispatcherHandlerTypes.Add(new HandlerTypeInfo(handlerType, isAsync));
                }
                else
                {
                    registeredDispatcherHandlerTypes = new List<HandlerTypeInfo>();
                    _HandlerTypes[messageType] = registeredDispatcherHandlerTypes;
                    registeredDispatcherHandlerTypes.Add(new HandlerTypeInfo(handlerType, isAsync));
                }
            }
            else
            {
                var registeredDispatcherHandlerTypes = new List<HandlerTypeInfo>();
                registeredDispatcherHandlerTypes.Add(new HandlerTypeInfo(handlerType, isAsync));
                _HandlerTypes.TryAdd(messageType, registeredDispatcherHandlerTypes);
            }
            //var parameterInfoes = handlerType.GetConstructors()
            //                                   .OrderByDescending(c => c.GetParameters().Length)
            //                                   .FirstOrDefault().GetParameters();
            //_HandlerConstuctParametersInfo[handlerType] = parameterInfoes;
        }

        public HandlerTypeInfo GetHandlerType(Type messageType)
        {
            return GetHandlerTypes(messageType).FirstOrDefault();
        }

        #region Private Methods

        private void RegisterHandlerFromAssembly(Assembly assembly)
        {
            var exportedTypes = assembly.GetExportedTypes()
                .Where(x => x.IsInterface == false && x.IsAbstract == false
                            && x.GetInterfaces()
                                .Any(y => y.IsGenericType
                                          && HandlerGenericTypes.Contains(y.GetGenericTypeDefinition())));
            foreach (var type in exportedTypes)
                RegisterHandlerFromType(type);
        }

        protected void RegisterHandlerFromType(Type handlerType)
        {
            var ihandlerTypes = handlerType.GetInterfaces().Where(x => x.IsGenericType
                                                                       && HandlerGenericTypes.Contains(x
                                                                           .GetGenericTypeDefinition()));
            foreach (var ihandlerType in ihandlerTypes)
            {
                var messageType = ihandlerType.GetGenericArguments().Single();
                //var messageHandlerWrapperType = typeof(MessageHandler<>).MakeGenericType(messageType);
                //var messageHandler = Activator.CreateInstance(handlerType);
                //var messageHandlerWrapper = Activator.CreateInstance(messageHandlerWrapperType, new object[] { messageHandler }) as IMessageHandler;
                Register(messageType, handlerType);
            }
        }

        #endregion Private Methods
    }
}