using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer.ExceptionHandling
{
	internal class ExceptionWindowVM
	{
		public Exception Exception { get; }

		public string ExceptionType { get; }

		public ExceptionWindowVM(Exception exception)
		{
			Exception = exception;
			ExceptionType = exception.GetType().FullName;
		}
	}
}
