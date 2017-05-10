﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Newtonsoft.Json;
using Eurofurence.Companion.DependencyResolution;
using Ninject;
using Windows.ApplicationModel;

// ReSharper disable ExplicitCallerInfoArgument

namespace Eurofurence.Companion
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        [JsonIgnore]
        public CoreDispatcher Dispatcher;

        public BindableBase()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                Dispatcher = KernelResolver.Current.Get<CoreDispatcher>();
            }
        }

        protected void InitializeDispatcherFromCurrentThread()
        {
            //Dispatcher = CoreWindow.GetForCurrentThread()?.Dispatcher;
        }

        protected void IntializeDispatcher(CoreDispatcher dispatcher)
        {
            //Dispatcher = dispatcher;
        }

        /// <summary>
        ///     Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        
        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers
        ///     that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (Dispatcher != null)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
            else
            {
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
