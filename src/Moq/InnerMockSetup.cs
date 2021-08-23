// Copyright (c) 2007, Clarius Consulting, Manas Technology Solutions, InSTEDD, and Contributors.
// All rights reserved. Licensed under the BSD 3-Clause License; see License.txt.

using System.Diagnostics;
using System.Linq.Expressions;

using Moq.Async;

namespace Moq
{
	internal sealed class InnerMockSetup : SetupWithOutParameterSupport
	{
		private readonly object returnValue;

		public InnerMockSetup(Expression originalExpression, Mock mock, MethodExpectation expectation, object returnValue)
			: base(originalExpression, mock, expectation)
		{
			Debug.Assert(Awaitable.TryGetResultRecursive(returnValue) is IMocked);

			this.returnValue = returnValue;

			this.MarkAsVerifiable();
		}

		public override Mock InnerMock => TryGetInnerMockFrom(this.returnValue);

		protected override void ExecuteCore(Invocation invocation)
		{
			invocation.ReturnValue = this.returnValue;
		}

		protected override void ResetCore()
		{
			this.InnerMock.MutableSetups.Reset();
		}

		protected override void VerifySelf()
		{
		}
	}
}
