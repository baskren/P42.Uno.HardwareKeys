﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace P42.Uno.HardwareKeys
{
    static class DependencyObjectExtensions
    {
		public static TBindable Assign<TBindable, TVariable>(this TBindable bindable, out TVariable variable)
	where TBindable : DependencyObject, TVariable
		{
			variable = bindable;
			return bindable;
		}

		public static TBindable Invoke<TBindable>(this TBindable bindable, Action<TBindable> action) where TBindable : DependencyObject
		{
			action?.Invoke(bindable);
			return bindable;
		}


		//const string bindingContextPath = Binding.SelfPath;

		/// <summary>Bind to a specified property</summary>
		public static TBindable Bind<TBindable>(
			this TBindable target,
			DependencyProperty targetProperty,
			object source,
			string path = null,
			BindingMode mode = BindingMode.OneWay,
			IValueConverter converter = null,
			object converterParameter = null,
			string converterLanguage = null,
			UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default,
			object targetNullValue = null,
			object fallbackValue = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = -1
		) where TBindable : DependencyObject
		{
			//if (targetProperty.GetMetadata(target.GetType()) is PropertyMetadata metaData 
			//	&& metaData.DefaultValue.GetType() is Type type
			//	&& type.FullName != "System.__ComObject")
			{

				var binding = new Binding
				{
					Source = source,
					Mode = mode,
					Converter = converter,
					ConverterParameter = converterParameter,
					UpdateSourceTrigger = updateSourceTrigger,
					TargetNullValue = targetNullValue,
					FallbackValue = fallbackValue
				};
				if (!string.IsNullOrWhiteSpace(converterLanguage))
					binding.ConverterLanguage = converterLanguage;
				if (!string.IsNullOrWhiteSpace(path))
					binding.Path = new PropertyPath(path);
				BindingOperations.SetBinding(target, targetProperty, binding);
			}
			//else
			//Console.WriteLine("Ignoreing Bind because cannot find PropertyMetaData for binding target["+target+"] targetProperty["+targetProperty+"] source["+source+"] and path["+path+"] at line["+lineNumber+"] in file["+filePath+"].");
			return target;
		}

		/// <summary>Bind to a specified property with inline conversion</summary>
		public static TBindable Bind<TBindable, TSource, TDest>(
			this TBindable target,
			DependencyProperty targetProperty,
			object source = null,
			string path = null,
			BindingMode mode = BindingMode.OneWay,
			Func<TSource, TDest> convert = null,
			Func<TDest, TSource> convertBack = null,
			object converterParameter = null,
			string converterLanguage = null,
			UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default,
			object targetNullValue = null,
			object fallbackValue = null
		) where TBindable : DependencyObject
		{
			var converter = new FuncConverter<TSource, TDest, object>(convert, convertBack);
			var binding = new Binding
			{
				Source = source,
				Mode = mode,
				Converter = converter,
				ConverterParameter = converterParameter,
				UpdateSourceTrigger = updateSourceTrigger,
				TargetNullValue = targetNullValue,
				FallbackValue = fallbackValue
			};
			if (!string.IsNullOrWhiteSpace(converterLanguage))
				binding.ConverterLanguage = converterLanguage;
			if (!string.IsNullOrWhiteSpace(path))
				binding.Path = new PropertyPath(path);
			BindingOperations.SetBinding(target, targetProperty, binding);
			return target;
		}

		/// <summary>Bind to a specified property with inline conversion and conversion parameter</summary>
		public static TBindable Bind<TBindable, TSource, TParam, TDest>(
			this TBindable target,
			DependencyProperty targetProperty,
			object source = null,
			string path = null,
			BindingMode mode = BindingMode.OneWay,
			Func<TSource, TParam, TDest> convert = null,
			Func<TDest, TParam, TSource> convertBack = null,
			object converterParameter = null,
			string converterLanguage = null,
			UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default,
			object targetNullValue = null,
			object fallbackValue = null
		) where TBindable : DependencyObject
		{
			var converter = new FuncConverter<TSource, TDest, TParam>(convert, convertBack);
			var binding = new Binding
			{
				Source = source,
				Mode = mode,
				Converter = converter,
				ConverterParameter = converterParameter,
				UpdateSourceTrigger = updateSourceTrigger,
				TargetNullValue = targetNullValue,
				FallbackValue = fallbackValue
			};
			if (!string.IsNullOrWhiteSpace(converterLanguage))
				binding.ConverterLanguage = converterLanguage;
			if (!string.IsNullOrWhiteSpace(path))
				binding.Path = new PropertyPath(path);
			BindingOperations.SetBinding(target, targetProperty, binding);
			return target;
		}

	}
}
